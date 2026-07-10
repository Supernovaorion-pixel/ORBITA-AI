using System.Diagnostics;
using System.Globalization;
using OrbitaAI.Core.Infrastructure;
using OrbitaAI.Modules.ImportEngine.Application;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Pipeline;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Quarantine;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;
using Xunit;
using Xunit.Abstractions;

namespace OrbitaAI.UnitTests.ImportEngine.Pipeline;

/// <summary>
/// Vérifie que le Pipeline d'Import complet (Reader → Mapping → Validation → Quarantine) respecte
/// l'exigence de scalabilité (100 à 1 000 000 de lignes — mission 010.4) : traitement en flux
/// continu sans matérialisation intégrale du fichier, empreinte mémoire des lignes rejetées
/// proportionnelle au seul volume rejeté, et annulation effective avant la fin du fichier.
/// </summary>
public sealed class ImportPipelineScalabilityTests : IDisposable
{
    private readonly ITestOutputHelper _output;
    private readonly List<string> _tempFiles = [];

    public ImportPipelineScalabilityTests(ITestOutputHelper output) => _output = output;

    public void Dispose()
    {
        foreach (var path in _tempFiles.Where(File.Exists))
        {
            File.Delete(path);
        }
    }

    private static ImportPipeline CreatePipeline() => new(
        new ReaderFactory([new CsvFileReader(), new ExcelFileReader()]),
        new MappingEngine(new ColumnAnalyzer(), new HeaderAnalyzer(new HeaderNormalizer()), new ConfidenceEngine()),
        new ValidationRuleEngine(),
        ValidationRuleRegistry.CreateDefault(),
        new ValidationPipeline(new ValidationRuleEngine(), ValidationRuleRegistry.CreateDefault()),
        new QuarantineEngine(),
        new InMemoryEventBus());

    private string CreateCleanCsvFile(int rowCount)
    {
        var path = Path.Combine(Path.GetTempPath(), $"orbita-ai-pipeline-{Guid.NewGuid():N}.csv");
        _tempFiles.Add(path);

        using var writer = new StreamWriter(path);
        writer.WriteLine("Client,Montant HT,Date");
        for (var i = 1; i <= rowCount; i++)
        {
            writer.WriteLine($"Client{i % 500},{(i * 1.5).ToString("F2", CultureInfo.InvariantCulture)},2026-01-01");
        }

        return path;
    }

    private string CreateMixedCsvFile(int rowCount, int rejectEveryNth)
    {
        var path = Path.Combine(Path.GetTempPath(), $"orbita-ai-pipeline-{Guid.NewGuid():N}.csv");
        _tempFiles.Add(path);

        using var writer = new StreamWriter(path);
        writer.WriteLine("Client,Montant HT,Date");
        for (var i = 1; i <= rowCount; i++)
        {
            var client = i % rejectEveryNth == 0 ? string.Empty : $"Client{i % 500}"; // Client manquant -> rejetée
            writer.WriteLine($"{client},{(i * 1.5).ToString("F2", CultureInfo.InvariantCulture)},2026-01-01");
        }

        return path;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(10_000)]
    [InlineData(200_000)]
    [InlineData(1_000_000)]
    public async Task RunAsync_AcrossAllReferenceVolumes_ProcessesEveryRow_WithoutRegression(int rowCount)
    {
        var path = CreateCleanCsvFile(rowCount);
        var context = new PipelineContext { Source = new FileReaderSource(path), OrganizationId = Guid.NewGuid() };

        var stopwatch = Stopwatch.StartNew();
        var result = await CreatePipeline().RunAsync(context);
        stopwatch.Stop();

        Assert.Equal(PipelineState.Completed, result.State);
        Assert.Equal(rowCount, result.Summary.TotalRows);
        Assert.Equal(rowCount, result.Summary.ImportedRows);
        Assert.Equal(0, result.Summary.RejectedRows);
        Assert.Equal(100, result.Summary.DataQualityScore);

        _output.WriteLine($"{rowCount:N0} lignes propres : pipeline complet en {stopwatch.Elapsed.TotalSeconds:N2} s " +
                           $"({result.Summary.ThroughputRowsPerSecond:N0} lignes/s).");
    }

    [Fact]
    public async Task RunAsync_OneMillionRowsWithTenPercentRejected_KeepsEveryRejectedRow_WithBoundedMemory()
    {
        const int rowCount = 1_000_000;
        var path = CreateMixedCsvFile(rowCount, rejectEveryNth: 10);
        var context = new PipelineContext { Source = new FileReaderSource(path), OrganizationId = Guid.NewGuid() };

        GC.Collect();
        GC.WaitForPendingFinalizers();
        var memoryBefore = GC.GetTotalMemory(forceFullCollection: true);

        var stopwatch = Stopwatch.StartNew();
        var result = await CreatePipeline().RunAsync(context);
        stopwatch.Stop();

        var memoryAfter = GC.GetTotalMemory(forceFullCollection: true);
        var memoryGrowthBytes = memoryAfter - memoryBefore;
        var fileSizeBytes = new FileInfo(path).Length;

        Assert.Equal(PipelineState.Completed, result.State);
        Assert.Equal(rowCount, result.Summary.TotalRows);
        Assert.Equal(rowCount / 10, result.Summary.RejectedRows);
        Assert.Equal(rowCount / 10, result.RejectedRows.Count);
        Assert.Equal(rowCount - (rowCount / 10), result.Summary.ImportedRows);

        // La mémoire retenue croît avec le nombre de lignes réellement REJETÉES (10 % ici), jamais
        // avec le volume total du fichier : une marge généreuse par ligne rejetée (structure riche —
        // données originales, raisons, messages) suffit à distinguer ce comportement d'une
        // matérialisation intégrale du fichier (qui coûterait un ordre de grandeur de plus).
        var maxExpectedBytes = result.RejectedRows.Count * 2_000L;
        Assert.True(
            memoryGrowthBytes < maxExpectedBytes,
            $"Croissance mémoire ({memoryGrowthBytes:N0} octets) incompatible avec une rétention proportionnelle " +
            $"aux seules {result.RejectedRows.Count:N0} lignes rejetées (fichier total : {fileSizeBytes:N0} octets).");

        _output.WriteLine($"{rowCount:N0} lignes (10 % rejetées) : pipeline complet en {stopwatch.Elapsed.TotalSeconds:N2} s " +
                           $"({result.Summary.ThroughputRowsPerSecond:N0} lignes/s), " +
                           $"{result.RejectedRows.Count:N0} lignes rejetées conservées intégralement, " +
                           $"croissance mémoire {memoryGrowthBytes:N0} octets pour un fichier de {fileSizeBytes:N0} octets.");
    }

    [Fact]
    public async Task RunAsync_CancelledEarly_OnOneMillionRowFile_StopsWithoutReadingEverything()
    {
        var path = CreateCleanCsvFile(1_000_000);
        var context = new PipelineContext { Source = new FileReaderSource(path), OrganizationId = Guid.NewGuid() };
        using var cts = new CancellationTokenSource();

        var monitorTask = Task.Run(async () =>
        {
            while (context.Statistics.RowsRead < 5_000)
            {
                await Task.Delay(1);
            }

            await cts.CancelAsync();
        });

        var stopwatch = Stopwatch.StartNew();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => CreatePipeline().RunAsync(context, cancellationToken: cts.Token));
        stopwatch.Stop();
        await monitorTask;

        Assert.True(context.Statistics.RowsRead < 1_000_000);
        _output.WriteLine($"Annulation après {context.Statistics.RowsRead:N0} lignes en {stopwatch.Elapsed.TotalMilliseconds:N0} ms (sur 1 000 000 au total).");
    }
}
