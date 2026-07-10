using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine;

/// <summary>
/// Teste la chaîne complète Reader → Mapping Engine → Validation Engine, telle qu'elle
/// s'exécutera réellement en production, pour les deux formats de fichier pris en charge
/// (CSV et Excel).
/// </summary>
public sealed class ImportEngineIntegrationTests
{
    private static readonly MappingEngine MappingEngine = new(new ColumnAnalyzer(), new HeaderAnalyzer(new HeaderNormalizer()), new ConfidenceEngine());
    private static readonly ValidationEngine ValidationEngine = new(new ValidationPipeline(new ValidationRuleEngine(), ValidationRuleRegistry.CreateDefault()));

    private static async Task<(MappingResult Mapping, ValidationResult Validation)> RunFullChainAsync(CsvFileReader reader, ReaderContext readerContext)
    {
        var rows = await reader.ReadAsync(readerContext).ToListAsync();
        var headers = rows.Count > 0 ? rows[0].Headers : Array.Empty<string>();

        var mappingContext = new MappingContext();
        var mapping = MappingEngine.Analyze(headers, rows, mappingContext);

        var validationContext = new ValidationContext { Mapping = mapping, Dictionary = mappingContext.EffectiveDictionary };
        var validation = await ValidationEngine.ValidateAsync(ToAsyncEnumerable(rows), validationContext);

        return (mapping, validation);
    }

    private static async Task<(MappingResult Mapping, ValidationResult Validation)> RunFullChainAsync(ExcelFileReader reader, ReaderContext readerContext)
    {
        var rows = await reader.ReadAsync(readerContext).ToListAsync();
        var headers = rows.Count > 0 ? rows[0].Headers : Array.Empty<string>();

        var mappingContext = new MappingContext();
        var mapping = MappingEngine.Analyze(headers, rows, mappingContext);

        var validationContext = new ValidationContext { Mapping = mapping, Dictionary = mappingContext.EffectiveDictionary };
        var validation = await ValidationEngine.ValidateAsync(ToAsyncEnumerable(rows), validationContext);

        return (mapping, validation);
    }

    private static async IAsyncEnumerable<RawRow> ToAsyncEnumerable(IEnumerable<RawRow> rows)
    {
        foreach (var row in rows)
        {
            yield return row;
        }

        await Task.CompletedTask;
    }

    private static ReaderContext CreateContext(MemoryStream stream, string fileName) =>
        new() { Source = new StreamReaderSource(stream, fileName), Options = ReaderOptions.Default };

    [Fact]
    public async Task FullChain_CsvWithCleanData_RecognizesColumns_AndProducesNoValidationFindings()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,2026-01-01\nContoso,2300,2026-01-02\n");
        var (mapping, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Equal(3, mapping.Report.RecognizedColumns.Count);
        Assert.Empty(validation.Report.Findings);
        Assert.True(validation.CanProceed);
    }

    [Fact]
    public async Task FullChain_CsvWithMissingRequiredColumn_BlocksProceeding_WithTraceableFinding()
    {
        using var stream = TestHelpers.ToStream("Client\nAcme\nContoso\n");
        var (mapping, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Contains(mapping.Report.MissingRequiredColumns, c => c.Key == "MontantHT");
        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.RequiredColumnMissing && f.ColumnCanonicalKey == "MontantHT");
        Assert.False(validation.CanProceed);
    }

    [Fact]
    public async Task FullChain_CsvWithBlankRequiredValue_ProducesRowLevelErrorFinding()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\n,1500,2026-01-01\n");
        var (_, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        var finding = Assert.Single(validation.Report.Findings);
        Assert.Equal(ValidationCode.RequiredValueMissing, finding.Code);
        Assert.Equal(1, finding.RowNumber);
        Assert.Equal(ValidationSeverity.Error, finding.Severity);
    }

    [Fact]
    public async Task FullChain_CsvWithInvalidNumericValue_ProducesTypeFinding()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,pas-un-nombre,2026-01-01\n");
        var (_, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.TypeNumericInvalid);
    }

    [Fact]
    public async Task FullChain_CsvWithInvalidDateValue_ProducesTypeFinding()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\nAcme,1500,pas-une-date\n");
        var (_, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.TypeDateInvalid);
    }

    [Fact]
    public async Task FullChain_CsvWithDuplicateColumnsClaimingSameField_ProducesCriticalStructuralFinding()
    {
        using var stream = TestHelpers.ToStream("Client,Customer\nAcme,Acme Corp\n");
        var (mapping, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Equal(2, mapping.Report.AmbiguousColumns.Count);
        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.DuplicateColumn && f.Severity == ValidationSeverity.Critical);
        Assert.False(validation.CanProceed);
    }

    [Fact]
    public async Task FullChain_CsvWithUnknownColumn_ProducesInformationFinding_WithoutBlocking()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date,Colonne Mystere\nAcme,1500,2026-01-01,xyz\n");
        var (_, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.UnknownColumn && f.Severity == ValidationSeverity.Information);
        Assert.True(validation.CanProceed);
    }

    [Fact]
    public async Task FullChain_CsvWithUnicodeAndWhitespaceNoise_ProducesInformationFindings_WithoutBlocking()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT,Date\n  Société Générale — Café  ,1500,2026-01-01\n");
        var (_, validation) = await RunFullChainAsync(new CsvFileReader(), CreateContext(stream, "export.csv"));

        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.WhitespaceSurroundingValue);
        Assert.True(validation.CanProceed);
    }

    [Fact]
    public async Task FullChain_ExcelWithCleanData_RecognizesColumns_AndProducesNoValidationFindings()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["Client", "Montant HT", "Date"],
            ["Acme", 1500d, "2026-01-01"],
            ["Contoso", 2300d, "2026-01-02"],
        ]);
        var (mapping, validation) = await RunFullChainAsync(new ExcelFileReader(), CreateContext(stream, "export.xlsx"));

        Assert.Equal(3, mapping.Report.RecognizedColumns.Count);
        Assert.Empty(validation.Report.Findings);
        Assert.True(validation.CanProceed);
    }

    [Fact]
    public async Task FullChain_ExcelWithMissingRequiredValue_ProducesRowLevelErrorFinding()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["Client", "Montant HT", "Date"],
            [null, 1500d, "2026-01-01"],
        ]);
        var (_, validation) = await RunFullChainAsync(new ExcelFileReader(), CreateContext(stream, "export.xlsx"));

        var finding = Assert.Single(validation.Report.Findings);
        Assert.Equal(ValidationCode.RequiredValueMissing, finding.Code);
        Assert.Equal(ValidationSeverity.Error, finding.Severity);
    }

    [Fact]
    public async Task FullChain_ExcelWithInvalidNumericValue_ProducesTypeFinding()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["Client", "Montant HT", "Date"],
            ["Acme", "pas-un-nombre", "2026-01-01"],
        ]);
        var (_, validation) = await RunFullChainAsync(new ExcelFileReader(), CreateContext(stream, "export.xlsx"));

        Assert.Contains(validation.Report.Findings, f => f.Code == ValidationCode.TypeNumericInvalid);
    }
}
