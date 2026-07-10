using OrbitaAI.Core.Domain.Events;
using OrbitaAI.Core.Infrastructure;
using OrbitaAI.Core.Services;
using OrbitaAI.Modules.ImportEngine.Application;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Pipeline;
using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Quarantine;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Pipeline;

/// <summary>
/// Teste le Pipeline d'Import de bout en bout : Reader → Mapping → Validation → Quarantine,
/// pour les lignes valides et rejetées, la suspension structurelle, l'annulation, les statistiques,
/// l'audit et les événements émis.
/// </summary>
public sealed class ImportPipelineTests
{
    private static ImportPipeline CreatePipeline(IEventBus? eventBus = null) => new(
        new ReaderFactory([new CsvFileReader(), new ExcelFileReader()]),
        new MappingEngine(new ColumnAnalyzer(), new HeaderAnalyzer(new HeaderNormalizer()), new ConfidenceEngine()),
        new ValidationRuleEngine(),
        ValidationRuleRegistry.CreateDefault(),
        new ValidationPipeline(new ValidationRuleEngine(), ValidationRuleRegistry.CreateDefault()),
        new QuarantineEngine(),
        eventBus ?? new InMemoryEventBus());

    private static PipelineContext CreateContext(ReaderSource source, PipelineConfiguration? configuration = null) => new()
    {
        Source = source,
        OrganizationId = Guid.NewGuid(),
        Configuration = configuration ?? PipelineConfiguration.Default,
    };

    [Fact]
    public async Task RunAsync_CleanCsv_CompletesWithAllRowsImported_NoRejections()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,2026-01-01\nContoso,2300,2026-01-02\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Completed, result.State);
        Assert.Equal(2, result.Summary.TotalRows);
        Assert.Equal(2, result.Summary.ImportedRows);
        Assert.Equal(0, result.Summary.RejectedRows);
        Assert.Equal(0, result.RejectedRows.Count);
        Assert.Equal(100, result.Summary.DataQualityScore);
    }

    [Fact]
    public async Task RunAsync_CsvWithSomeInvalidRows_QuarantinesOnlyOffendingRows()
    {
        using var stream = TestHelpers.ToStream(
            "Client,Montant HT,Date\n" +
            "Acme,1500,2026-01-01\n" +      // valide
            ",1500,2026-01-01\n" +          // Client manquant -> rejetée
            "Contoso,pas-un-nombre,2026-01-01\n"); // MontantHT invalide -> rejetée
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Completed, result.State);
        Assert.Equal(3, result.Summary.TotalRows);
        Assert.Equal(1, result.Summary.ImportedRows);
        Assert.Equal(2, result.Summary.RejectedRows);
        Assert.Equal(2, result.RejectedRows.Count);
    }

    [Fact]
    public async Task RunAsync_RejectedRow_PreservesOriginalDataAndReasons()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\n,1500,2026-01-01\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        var rejected = Assert.Single(result.RejectedRows.Rows);
        Assert.Equal(1, rejected.RowNumber);
        Assert.Equal(["Client", "Montant HT", "Date"], rejected.OriginalHeaders);
        Assert.Equal(["", "1500", "2026-01-01"], rejected.OriginalValues);
        Assert.Contains(rejected.Reasons, r => r.Code == ValidationCode.RequiredValueMissing);
        Assert.Equal(RejectedRowCategory.MissingRequiredData, rejected.Category);
    }

    [Fact]
    public async Task RunAsync_MissingRequiredColumn_HaltsBeforeAnyRowProcessing()
    {
        using var stream = TestHelpers.ToStream("Client\nAcme\nContoso\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Halted, result.State);
        Assert.Equal(0, result.RejectedRows.Count);
        Assert.Equal(0, result.Summary.ImportedRows);
        Assert.Contains(result.Decisions, d => d.Severity == ValidationSeverity.Critical);
        Assert.Contains(result.Audit.Entries, e => e.Action == "Halted");
    }

    [Fact]
    public async Task RunAsync_DuplicateColumns_HaltsWithCriticalDecision()
    {
        using var stream = TestHelpers.ToStream("Client,Customer\nAcme,Acme Corp\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Halted, result.State);
    }

    [Fact]
    public async Task RunAsync_PublishesStartedAndCompletedEvents()
    {
        var eventBus = new InMemoryEventBus();
        ImportPipelineStartedEvent? started = null;
        ImportPipelineCompletedEvent? completed = null;
        eventBus.Subscribe<ImportPipelineStartedEvent>((e, _) => { started = e; return Task.CompletedTask; });
        eventBus.Subscribe<ImportPipelineCompletedEvent>((e, _) => { completed = e; return Task.CompletedTask; });

        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,2026-01-01\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline(eventBus).RunAsync(context);

        Assert.NotNull(started);
        Assert.Equal(context.OrganizationId, started!.OrganizationId);
        Assert.Equal(context.ImportId, started.ImportId);

        Assert.NotNull(completed);
        Assert.Equal(context.ImportId, completed!.ImportId);
        Assert.Equal(PipelineState.Completed, completed.HistoryEntry.Status);
        Assert.Equal(result.Summary.ImportedRows, completed.HistoryEntry.Summary.ImportedRows);
    }

    [Fact]
    public async Task RunAsync_Cancelled_ThrowsOperationCanceledException_AndRecordsAudit()
    {
        var rowsBuilder = new System.Text.StringBuilder("Client,Montant HT,Date\n");
        for (var i = 0; i < 50_000; i++)
        {
            rowsBuilder.Append($"Client{i},100,2026-01-01\n");
        }

        using var stream = TestHelpers.ToStream(rowsBuilder.ToString());
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));
        using var cts = new CancellationTokenSource();

        var monitorTask = Task.Run(async () =>
        {
            while (context.Statistics.RowsRead < 1_000)
            {
                await Task.Delay(1);
            }

            await cts.CancelAsync();
        });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => CreatePipeline().RunAsync(context, cancellationToken: cts.Token));
        await monitorTask;

        Assert.Equal(PipelineState.Cancelled, context.Statistics.State);
        Assert.True(context.Statistics.RowsRead < 50_000);
        Assert.Contains(context.Audit.Entries, e => e.Action == "Cancelled");
    }

    [Fact]
    public async Task RunAsync_Audit_ContainsStartedAndCompletedEntriesInOrder()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,2026-01-01\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal("Started", result.Audit.Entries[0].Action);
        Assert.Contains(result.Audit.Entries, e => e.Action == "Completed");
    }

    [Fact]
    public async Task RunAsync_Statistics_ReportThroughputAndCompletion()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,2026-01-01\nContoso,2300,2026-01-02\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.True(result.Statistics.IsCompleted);
        Assert.Equal(2, result.Statistics.RowsRead);
        Assert.True(result.Summary.ThroughputRowsPerSecond >= 0);
    }

    [Fact]
    public async Task RunAsync_ProgressReported_AtConfiguredInterval()
    {
        var rowsBuilder = new System.Text.StringBuilder("Client,Montant HT,Date\n");
        for (var i = 0; i < 10; i++)
        {
            rowsBuilder.Append($"Client{i},100,2026-01-01\n");
        }

        using var stream = TestHelpers.ToStream(rowsBuilder.ToString());
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"), PipelineConfiguration.Default with { ProgressReportIntervalRows = 2 });

        var progressReports = new List<PipelineProgress>();
        var progress = new Progress<PipelineProgress>(progressReports.Add);

        await CreatePipeline().RunAsync(context, progress);

        // Progress<T> marshals callbacks asynchronously ; laisser le temps aux rapports de s'exécuter.
        await Task.Delay(50);

        Assert.NotEmpty(progressReports);
        Assert.All(progressReports, p => Assert.Equal(0, p.RowsRead % 2));
    }

    [Fact]
    public async Task RunAsync_ExcelFile_FullChainWorks()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["Client", "Montant HT", "Date"],
            ["Acme", 1500d, "2026-01-01"],
            [null, 2300d, "2026-01-02"],
        ]);
        var context = CreateContext(new StreamReaderSource(stream, "export.xlsx"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Completed, result.State);
        Assert.Equal(2, result.Summary.TotalRows);
        Assert.Equal(1, result.Summary.ImportedRows);
        Assert.Equal(1, result.Summary.RejectedRows);
    }

    [Fact]
    public async Task RunAsync_HeaderOnlyFileWithNoDataRows_FailsGracefully_SinceColumnsCannotBeIdentified()
    {
        // Un fichier sans la moindre ligne de donnée ne permet pas au Reader d'exposer d'en-têtes
        // (RawRow.Headers n'existe que si au moins une ligne de donnée est restituée) : le Mapping
        // Engine ne peut alors identifier aucune colonne. Le pipeline doit échouer proprement,
        // plutôt que de prétendre à tort avoir importé un fichier dont il ne connaît pas la structure.
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Failed, result.State);
        Assert.Null(result.Mapping);
        Assert.Contains(result.Audit.Entries, e => e.Action == "Failed");
        Assert.Contains(result.Decisions, d => d.Severity == ValidationSeverity.Critical);
    }

    [Fact]
    public async Task RunAsync_CsvWithDataRows_CompletesWithMatchingTotalRowCount()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,2026-01-01\n");
        var context = CreateContext(new StreamReaderSource(stream, "export.csv"));

        var result = await CreatePipeline().RunAsync(context);

        Assert.Equal(PipelineState.Completed, result.State);
        Assert.Equal(1, result.Summary.TotalRows);
        Assert.Equal(100, result.Summary.DataQualityScore);
    }
}
