using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Mapping;

public sealed class ColumnAnalyzerTests
{
    private readonly ColumnAnalyzer _analyzer = new();

    private static RawRow Row(int number, IReadOnlyList<string> headers, params object?[] values) =>
        new(number, headers, values);

    [Fact]
    public void AnalyzeColumns_ProducesOneProfilePerHeader_InOrder()
    {
        IReadOnlyList<string> headers = ["Client", "Montant HT"];
        var rows = new[] { Row(1, headers, "Acme", 100d) };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: true);

        Assert.Equal(2, profiles.Count);
        Assert.Equal(0, profiles[0].ColumnIndex);
        Assert.Equal("Client", profiles[0].RawHeader);
        Assert.Equal(1, profiles[1].ColumnIndex);
        Assert.Equal("Montant HT", profiles[1].RawHeader);
    }

    [Fact]
    public void AnalyzeColumns_WithContentAnalysisDisabled_ReturnsEmptyStatistics()
    {
        IReadOnlyList<string> headers = ["Montant HT"];
        var rows = new[] { Row(1, headers, 100d), Row(2, headers, 200d) };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: false);

        Assert.Equal(ColumnStatistics.Empty, profiles[0].Statistics);
    }

    [Fact]
    public void AnalyzeColumns_ComputesFillAndNumericRatios()
    {
        IReadOnlyList<string> headers = ["Montant HT"];
        var rows = new[]
        {
            Row(1, headers, "100"),
            Row(2, headers, "200"),
            Row(3, headers, (object?)null),
            Row(4, headers, "abc"),
        };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: true);
        var statistics = profiles[0].Statistics;

        Assert.Equal(4, statistics.SampledRowCount);
        Assert.Equal(3, statistics.NonEmptyCount); // "100", "200", "abc" - le null est exclu
        Assert.Equal(2, statistics.NumericCount);  // "100", "200"
        Assert.Equal(0.75, statistics.FillRatio);
        Assert.Equal(2d / 3, statistics.NumericRatio);
    }

    [Fact]
    public void AnalyzeColumns_RecognizesNativeNumericTypes_WithoutParsing()
    {
        IReadOnlyList<string> headers = ["Montant HT"];
        var rows = new[] { Row(1, headers, 1500.5d) };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: true);

        Assert.Equal(1, profiles[0].Statistics.NumericCount);
    }

    [Fact]
    public void AnalyzeColumns_RecognizesParsableDates()
    {
        IReadOnlyList<string> headers = ["Date"];
        var rows = new[] { Row(1, headers, "2026-01-15"), Row(2, headers, "not a date") };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: true);

        Assert.Equal(1, profiles[0].Statistics.DateCount);
    }

    [Fact]
    public void AnalyzeColumns_ComputesDistinctValueCount()
    {
        IReadOnlyList<string> headers = ["Client"];
        var rows = new[] { Row(1, headers, "Acme"), Row(2, headers, "Contoso"), Row(3, headers, "Acme") };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: true);

        Assert.Equal(2, profiles[0].Statistics.DistinctValueCount);
    }

    [Fact]
    public void AnalyzeColumns_BoundsSampleSize_ToConfiguredMaximum()
    {
        IReadOnlyList<string> headers = ["Client"];
        var rows = Enumerable.Range(1, 1_000).Select(i => Row(i, headers, $"Client{i}")).ToArray();
        var configuration = MappingConfiguration.Default with { MaxContentSampleRows = 50 };

        var profiles = _analyzer.AnalyzeColumns(headers, rows, configuration, analyzeContent: true);

        Assert.Equal(50, profiles[0].Statistics.SampledRowCount);
    }

    [Fact]
    public void AnalyzeColumns_MissingCellsForShortRows_AreIgnored()
    {
        IReadOnlyList<string> headers = ["Client", "Montant HT"];
        var rows = new[] { Row(1, headers, "Acme") }; // Ligne plus courte que l'en-tête (colonnes désordonnées/absentes).

        var profiles = _analyzer.AnalyzeColumns(headers, rows, MappingConfiguration.Default, analyzeContent: true);

        Assert.Equal(1, profiles[0].Statistics.NonEmptyCount);
        Assert.Equal(0, profiles[1].Statistics.NonEmptyCount);
    }
}
