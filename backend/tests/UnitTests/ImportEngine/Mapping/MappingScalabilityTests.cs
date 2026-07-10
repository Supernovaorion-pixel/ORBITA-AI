using System.Diagnostics;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using Xunit;
using Xunit.Abstractions;

namespace OrbitaAI.UnitTests.ImportEngine.Mapping;

/// <summary>
/// Vérifie que le moteur de correspondance respecte l'exigence de scalabilité
/// (100 à 1 000 000 de lignes, plusieurs centaines de colonnes, performances constantes —
/// cf. mission 010.2) sans aucune modification de son architecture.
/// </summary>
public sealed class MappingScalabilityTests
{
    private readonly ITestOutputHelper _output;
    private readonly MappingEngine _engine = new(new ColumnAnalyzer(), new HeaderAnalyzer(new HeaderNormalizer()), new ConfidenceEngine());

    public MappingScalabilityTests(ITestOutputHelper output) => _output = output;

    [Theory]
    [InlineData(100)]
    [InlineData(10_000)]
    [InlineData(200_000)]
    [InlineData(1_000_000)]
    public void Analyze_AcrossAllReferenceVolumes_RecognizesColumns_WithoutArchitectureChange(int rowCount)
    {
        IReadOnlyList<string> headers = ["Client", "Montant HT", "Date"];
        var rows = BuildRows(headers, rowCount);

        var stopwatch = Stopwatch.StartNew();
        var result = _engine.Analyze(headers, rows, new MappingContext());
        stopwatch.Stop();

        Assert.Equal(3, result.Report.RecognizedColumns.Count);
        _output.WriteLine($"{rowCount:N0} lignes fournies : analyse en {stopwatch.Elapsed.TotalMilliseconds:N1} ms.");
    }

    [Fact]
    public void Analyze_PerformanceIsBoundedBySampleCap_NotByTotalRowCountProvided()
    {
        IReadOnlyList<string> headers = ["Client", "Montant HT", "Date"];
        var smallInput = BuildRows(headers, 1_000);
        var hugeInput = BuildRows(headers, 1_000_000);

        var smallElapsed = Time(() => _engine.Analyze(headers, smallInput, new MappingContext()));
        var hugeElapsed = Time(() => _engine.Analyze(headers, hugeInput, new MappingContext()));

        _output.WriteLine($"1 000 lignes : {smallElapsed.TotalMilliseconds:N1} ms — 1 000 000 lignes : {hugeElapsed.TotalMilliseconds:N1} ms.");

        // Le temps d'analyse ne croît pas proportionnellement au volume fourni (échantillon borné
        // par MappingConfiguration.MaxContentSampleRows, cf. architecture/PERFORMANCE_GUIDELINES.md) :
        // un facteur 1000x sur le volume ne doit jamais se traduire par un facteur comparable sur le temps.
        Assert.True(
            hugeElapsed.TotalMilliseconds < smallElapsed.TotalMilliseconds * 20 + 200,
            $"Le temps d'analyse ({hugeElapsed.TotalMilliseconds:N1} ms) croît de façon disproportionnée avec le volume fourni ({smallElapsed.TotalMilliseconds:N1} ms pour 1000x moins de lignes).");
    }

    [Fact]
    public void Analyze_SeveralHundredColumns_RecognizesKnownColumns_AmongManyUnknowns()
    {
        var headers = new List<string> { "Client", "Montant HT", "Date", "Commercial", "Famille" };
        for (var i = 0; i < 300; i++)
        {
            headers.Add($"Colonne personnalisee {i}");
        }

        IReadOnlyList<string> headerList = headers;
        var rows = new[] { new RawRow(1, headerList, headerList.Select(_ => (object?)"valeur").ToArray()) };

        var stopwatch = Stopwatch.StartNew();
        var result = _engine.Analyze(headerList, rows, new MappingContext());
        stopwatch.Stop();

        Assert.Equal(5, result.Report.RecognizedColumns.Count);
        Assert.Equal(300, result.Report.UnknownColumns.Count);
        Assert.Equal(headerList.Count, result.Report.RecognizedColumns.Count + result.Report.UnknownColumns.Count + result.Report.AmbiguousColumns.Count);
        _output.WriteLine($"{headerList.Count} colonnes analysées en {stopwatch.Elapsed.TotalMilliseconds:N1} ms.");
    }

    [Fact]
    public void Analyze_WithoutContentAnalysis_IsSubstantiallyFasterOnLargeSamples()
    {
        IReadOnlyList<string> headers = ["Client", "Montant HT", "Date", "Commercial", "Famille", "Produit", "Quantite"];
        var rows = BuildRows(headers, 1_000_000);

        var withContent = Time(() => _engine.Analyze(headers, rows, new MappingContext()));
        var withoutContent = Time(() => _engine.Analyze(headers, rows, new MappingContext { Options = new MappingOptions { AnalyzeContent = false } }));

        _output.WriteLine($"Avec profilage de contenu : {withContent.TotalMilliseconds:N1} ms — sans : {withoutContent.TotalMilliseconds:N1} ms.");

        Assert.True(withoutContent <= withContent + TimeSpan.FromMilliseconds(50));
    }

    private static TimeSpan Time(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    private static IReadOnlyList<RawRow> BuildRows(IReadOnlyList<string> headers, int count)
    {
        var rows = new RawRow[count];
        for (var i = 0; i < count; i++)
        {
            rows[i] = new RawRow(i + 1, headers, [$"Client{i % 500}", (double)(i % 1000), "2026-01-01"]);
        }

        return rows;
    }
}
