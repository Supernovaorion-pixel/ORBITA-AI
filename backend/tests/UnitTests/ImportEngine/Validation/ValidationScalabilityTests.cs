using System.Diagnostics;
using System.Runtime.CompilerServices;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;
using Xunit;
using Xunit.Abstractions;

namespace OrbitaAI.UnitTests.ImportEngine.Validation;

/// <summary>
/// Vérifie que le moteur de validation respecte l'exigence de scalabilité (100 à 1 000 000 de
/// lignes — cf. mission 010.3) : traitement en flux continu sans jamais matérialiser l'ensemble
/// des lignes, empreinte mémoire des constats bornée par <see cref="ValidationConfiguration.MaxFindingsToRetain"/>
/// quel que soit le volume traité, et arrêt effectif avant la fin du flux lors d'une annulation.
/// </summary>
public sealed class ValidationScalabilityTests
{
    private static readonly CanonicalColumnDefinition ClientColumn =
        CanonicalColumnDefinition.Create("Client", "Client", isRequired: true, expectedValueKind: ColumnValueKind.Text);

    private static readonly CanonicalColumnDefinition MontantColumn =
        CanonicalColumnDefinition.Create("MontantHT", "Montant HT", expectedValueKind: ColumnValueKind.Numeric);

    private static readonly SynonymDictionary Dictionary = new([ClientColumn, MontantColumn]);
    private static readonly IReadOnlyList<string> Headers = ["Client", "MontantHT"];

    private readonly ITestOutputHelper _output;
    private readonly ValidationEngine _engine = new(new ValidationPipeline(new ValidationRuleEngine(), ValidationRuleRegistry.CreateDefault()));

    public ValidationScalabilityTests(ITestOutputHelper output) => _output = output;

    private static ValidationContext CreateContext(ValidationConfiguration? configuration = null) => new()
    {
        Mapping = new MappingResult(new Dictionary<string, int> { ["Client"] = 0, ["MontantHT"] = 1 }, EmptyReport()),
        Dictionary = Dictionary,
        Configuration = configuration ?? ValidationConfiguration.Default,
    };

    private static MappingReport EmptyReport() => new(
        RecognizedColumns: [],
        UnknownColumns: [],
        AmbiguousColumns: [],
        MissingRequiredColumns: [],
        GlobalRecognitionScore: 100,
        Decisions: []);

    private static async IAsyncEnumerable<RawRow> GenerateCleanRowsAsync(int count)
    {
        for (var i = 1; i <= count; i++)
        {
            yield return new RawRow(i, Headers, ["Acme", 100d]);
        }

        await Task.CompletedTask;
    }

    private static async IAsyncEnumerable<RawRow> GenerateDirtyRowsAsync(int count)
    {
        for (var i = 1; i <= count; i++)
        {
            // Client manquant sur chaque ligne : une anomalie (Erreur) garantie par ligne.
            yield return new RawRow(i, Headers, [null, 100d]);
        }

        await Task.CompletedTask;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(10_000)]
    [InlineData(200_000)]
    [InlineData(1_000_000)]
    public async Task ValidateAsync_AcrossAllReferenceVolumes_ProcessesEveryRow_WithoutFindings(int rowCount)
    {
        var context = CreateContext();

        var stopwatch = Stopwatch.StartNew();
        var result = await _engine.ValidateAsync(GenerateCleanRowsAsync(rowCount), context);
        stopwatch.Stop();

        Assert.Empty(result.Report.Findings);
        Assert.True(result.CanProceed);
        Assert.Equal(rowCount, result.Statistics.RowsProcessed);
        Assert.True(result.Statistics.IsCompleted);

        _output.WriteLine($"{rowCount:N0} lignes propres : validées en {stopwatch.Elapsed.TotalMilliseconds:N1} ms " +
                           $"({result.Statistics.RowsPerSecond:N0} lignes/s).");
    }

    [Fact]
    public async Task ValidateAsync_OneMillionDirtyRows_FindingsBoundedByMaxFindingsToRetain_ButSummaryStaysExact()
    {
        const int rowCount = 1_000_000;
        var configuration = ValidationConfiguration.Default with { MaxFindingsToRetain = 10_000 };
        var context = CreateContext(configuration);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        var memoryBefore = GC.GetTotalMemory(forceFullCollection: true);

        var stopwatch = Stopwatch.StartNew();
        var result = await _engine.ValidateAsync(GenerateDirtyRowsAsync(rowCount), context);
        stopwatch.Stop();

        var memoryAfter = GC.GetTotalMemory(forceFullCollection: true);
        var memoryGrowthBytes = memoryAfter - memoryBefore;

        Assert.Equal(rowCount, result.Statistics.RowsProcessed);
        Assert.Equal(rowCount, result.Report.Summary.TotalFindings);
        Assert.Equal(rowCount, result.Report.Summary.ErrorCount);
        Assert.Equal(10_000, result.Report.Findings.Count);
        Assert.True(result.Report.Summary.FindingsTruncated);
        // Sévérité Erreur (pas Critique) : ne franchit pas le seuil de blocage par défaut.
        Assert.True(result.CanProceed);

        // Retenir seulement 10 000 constats détaillés sur 1 000 000 de lignes en anomalie : l'empreinte
        // mémoire des constats ne doit jamais croître proportionnellement au volume total traité.
        Assert.True(
            memoryGrowthBytes < 200_000_000,
            $"Croissance mémoire ({memoryGrowthBytes:N0} octets) incompatible avec un plafond de {configuration.MaxFindingsToRetain:N0} constats retenus.");

        _output.WriteLine($"{rowCount:N0} lignes en anomalie : validées en {stopwatch.Elapsed.TotalSeconds:N2} s " +
                           $"({result.Statistics.RowsPerSecond:N0} lignes/s), {result.Report.Findings.Count:N0} constats détaillés " +
                           $"sur {result.Report.Summary.TotalFindings:N0} au total, croissance mémoire {memoryGrowthBytes:N0} octets.");
    }

    [Fact]
    public async Task ValidateAsync_CancelledEarly_OnOneMillionRowStream_StopsWithoutGeneratingEverything()
    {
        var context = CreateContext();
        var rowsGenerated = 0;

        async IAsyncEnumerable<RawRow> GenerateAndCountAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            for (var i = 1; i <= 1_000_000; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                rowsGenerated++;
                yield return new RawRow(i, Headers, ["Acme", 100d]);
                await Task.Yield();
            }
        }

        using var cts = new CancellationTokenSource();
        var monitorTask = Task.Run(async () =>
        {
            while (context.Statistics.RowsProcessed < 1_000)
            {
                await Task.Delay(1);
            }

            await cts.CancelAsync();
        });

        var stopwatch = Stopwatch.StartNew();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => _engine.ValidateAsync(GenerateAndCountAsync(cts.Token), context, cts.Token));
        stopwatch.Stop();

        await monitorTask;

        Assert.True(rowsGenerated < 1_000_000);
        _output.WriteLine($"Annulation après {rowsGenerated:N0} lignes générées en {stopwatch.Elapsed.TotalMilliseconds:N0} ms (sur 1 000 000 au total).");
    }
}
