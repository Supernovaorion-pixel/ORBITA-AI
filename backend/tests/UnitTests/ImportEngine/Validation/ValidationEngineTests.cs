using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Validation;

/// <summary>Teste le moteur de validation de bout en bout : traduction des constats structurels, agrégation par sévérité, seuil de blocage et troncature des constats.</summary>
public sealed class ValidationEngineTests
{
    private static readonly CanonicalColumnDefinition ClientColumn =
        CanonicalColumnDefinition.Create("Client", "Client", isRequired: true, expectedValueKind: ColumnValueKind.Text);

    private static readonly CanonicalColumnDefinition MontantColumn =
        CanonicalColumnDefinition.Create("MontantHT", "Montant HT", expectedValueKind: ColumnValueKind.Numeric);

    private static readonly SynonymDictionary Dictionary = new([ClientColumn, MontantColumn]);

    private static ValidationEngine CreateEngine() =>
        new(new ValidationPipeline(new ValidationRuleEngine(), ValidationRuleRegistry.CreateDefault()));

    [Fact]
    public async Task ValidateAsync_MissingRequiredColumn_ProducesCriticalStructuralFinding_AndBlocks()
    {
        var report = new MappingReport(
            RecognizedColumns: [],
            UnknownColumns: [],
            AmbiguousColumns: [],
            MissingRequiredColumns: [ClientColumn],
            GlobalRecognitionScore: 0,
            Decisions: []);
        var mapping = ValidationTestHelpers.MappingResultWithReport(new Dictionary<string, int>(), report);
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping);

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable([]), context);

        var finding = Assert.Single(result.Report.Findings);
        Assert.Equal(ValidationCode.RequiredColumnMissing, finding.Code);
        Assert.Equal(ValidationSeverity.Critical, finding.Severity);
        Assert.Equal(0, finding.RowNumber);
        Assert.False(result.CanProceed);
    }

    [Fact]
    public async Task ValidateAsync_UnknownColumn_ProducesInformationFinding_DoesNotBlock()
    {
        var report = new MappingReport(
            RecognizedColumns: [],
            UnknownColumns: [ValidationTestHelpers.Outcome(2, "ColonneMystere", ColumnRecognitionStatus.Unknown)],
            AmbiguousColumns: [],
            MissingRequiredColumns: [],
            GlobalRecognitionScore: 100,
            Decisions: []);
        var mapping = ValidationTestHelpers.MappingResultWithReport(new Dictionary<string, int>(), report);
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping);

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable([]), context);

        var finding = Assert.Single(result.Report.Findings);
        Assert.Equal(ValidationCode.UnknownColumn, finding.Code);
        Assert.Equal(ValidationSeverity.Information, finding.Severity);
        Assert.True(result.CanProceed);
    }

    [Fact]
    public async Task ValidateAsync_AmbiguousColumn_ProducesCriticalFinding_AndBlocks()
    {
        var report = new MappingReport(
            RecognizedColumns: [],
            UnknownColumns: [],
            AmbiguousColumns: [ValidationTestHelpers.Outcome(3, "Client2", ColumnRecognitionStatus.Ambiguous)],
            MissingRequiredColumns: [],
            GlobalRecognitionScore: 100,
            Decisions: []);
        var mapping = ValidationTestHelpers.MappingResultWithReport(new Dictionary<string, int>(), report);
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping);

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable([]), context);

        var finding = Assert.Single(result.Report.Findings);
        Assert.Equal(ValidationCode.DuplicateColumn, finding.Code);
        Assert.Equal(ValidationSeverity.Critical, finding.Severity);
        Assert.False(result.CanProceed);
    }

    [Fact]
    public async Task ValidateAsync_RowLevelIssues_AreAggregatedIntoSummary()
    {
        var mapping = ValidationTestHelpers.SimpleMappingResult(("Client", 0), ("MontantHT", 1));
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping);

        IReadOnlyList<string> headers = ["Client", "MontantHT"];
        var rows = new[]
        {
            ValidationTestHelpers.Row(headers, 1, "Acme", 100d),
            ValidationTestHelpers.Row(headers, 2, null, 100d),
            ValidationTestHelpers.Row(headers, 3, "Acme", "abc"),
        };

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable(rows), context);

        Assert.Equal(2, result.Report.Summary.TotalFindings);
        Assert.Equal(2, result.Report.Summary.ErrorCount);
        Assert.Equal(2, result.Report.Summary.AffectedRowCount);
        Assert.True(result.CanProceed);
    }

    [Fact]
    public async Task ValidateAsync_ConfigurableBlockingThreshold_BlocksOnLowerSeverity()
    {
        var mapping = ValidationTestHelpers.SimpleMappingResult(("Client", 0));
        var configuration = ValidationConfiguration.Default with { BlockingSeverityThreshold = ValidationSeverity.Warning };
        var options = new ValidationOptions
        {
            Profile = new ValidationRuleBuilder().ForColumn("Client").MaxLength(3).Build(),
        };
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping, options, configuration);

        IReadOnlyList<string> headers = ["Client"];
        var rows = new[] { ValidationTestHelpers.Row(headers, 1, "TropLong") };

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable(rows), context);

        var finding = Assert.Single(result.Report.Findings);
        Assert.Equal(ValidationCode.LengthTooLong, finding.Code);
        Assert.Equal(ValidationSeverity.Warning, finding.Severity);
        Assert.False(result.CanProceed);
    }

    [Fact]
    public async Task ValidateAsync_FindingsExceedingMaxToRetain_AreTruncatedButCountedInSummary()
    {
        var mapping = ValidationTestHelpers.SimpleMappingResult(("Client", 0));
        var configuration = ValidationConfiguration.Default with { MaxFindingsToRetain = 2 };
        var options = new ValidationOptions
        {
            Profile = new ValidationRuleBuilder().ForColumn("Client").Required().Build(),
        };
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping, options, configuration);

        IReadOnlyList<string> headers = ["Client"];
        var rows = Enumerable.Range(1, 5).Select(i => ValidationTestHelpers.Row(headers, i, (object?)null)).ToArray();

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable(rows), context);

        Assert.Equal(5, result.Report.Summary.TotalFindings);
        Assert.Equal(2, result.Report.Findings.Count);
        Assert.True(result.Report.Summary.FindingsTruncated);
        Assert.Contains(result.Report.Decisions, d => d.Description.Contains("dépasse la limite"));
    }

    [Fact]
    public async Task ValidateAsync_CleanData_ProducesNoFindings_AndCanProceed()
    {
        var mapping = ValidationTestHelpers.SimpleMappingResult(("Client", 0), ("MontantHT", 1));
        var context = ValidationTestHelpers.CreateContext(Dictionary, mapping);

        IReadOnlyList<string> headers = ["Client", "MontantHT"];
        var rows = new[] { ValidationTestHelpers.Row(headers, 1, "Acme", 100d) };

        var result = await CreateEngine().ValidateAsync(ValidationTestHelpers.ToAsyncEnumerable(rows), context);

        Assert.Empty(result.Report.Findings);
        Assert.True(result.CanProceed);
        Assert.True(result.Statistics.IsCompleted);
        Assert.Equal(1, result.Statistics.RowsProcessed);
    }
}
