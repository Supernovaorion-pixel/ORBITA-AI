using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Pipeline;
using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Quarantine;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Quarantine;

/// <summary>Teste le Quarantine Engine : décision d'isolement, conservation intégrale de la donnée originale, catégorisation.</summary>
public sealed class QuarantineEngineTests
{
    private static readonly QuarantineEngine Engine = new();
    private static readonly PipelineConfiguration DefaultConfiguration = PipelineConfiguration.Default;

    private static RawRow Row(params object?[] values) =>
        new(RowNumber: 7, Headers: ["Client", "MontantHT"], Values: values);

    private static ValidationFinding Finding(
        ValidationSeverity severity,
        ValidationCategory category = ValidationCategory.Required,
        ValidationCode? code = null,
        int? columnIndex = 0,
        string? columnCanonicalKey = "Client",
        string? rawHeader = "Client") =>
        new(
            code ?? ValidationCode.RequiredValueMissing,
            category,
            severity,
            RowNumber: 7,
            columnIndex,
            columnCanonicalKey,
            rawHeader,
            new ValidationMessage("Résumé", "Explication", "Suggestion"));

    [Fact]
    public void Classify_NoFindings_ReturnsNull()
    {
        var result = Engine.Classify(Row("Acme", 100d), [], DefaultConfiguration);

        Assert.Null(result);
    }

    [Fact]
    public void Classify_OnlyBelowThresholdFindings_ReturnsNull()
    {
        var findings = new[] { Finding(ValidationSeverity.Information), Finding(ValidationSeverity.Warning) };

        var result = Engine.Classify(Row("Acme", 100d), findings, DefaultConfiguration);

        Assert.Null(result);
    }

    [Fact]
    public void Classify_FindingAtOrAboveThreshold_ReturnsRejectedRow()
    {
        var findings = new[] { Finding(ValidationSeverity.Error) };

        var result = Engine.Classify(Row(null, 100d), findings, DefaultConfiguration);

        Assert.NotNull(result);
    }

    [Fact]
    public void Classify_RejectedRow_PreservesOriginalRowDataUnmodified()
    {
        var row = Row(null, "abc");
        var findings = new[] { Finding(ValidationSeverity.Critical) };

        var result = Engine.Classify(row, findings, DefaultConfiguration);

        Assert.NotNull(result);
        Assert.Equal(7, result!.RowNumber);
        Assert.Equal(row.Headers, result.OriginalHeaders);
        Assert.Equal(row.Values, result.OriginalValues);
    }

    [Fact]
    public void Classify_MultipleOffendingFindings_AllRetainedAsReasons()
    {
        var findings = new[]
        {
            Finding(ValidationSeverity.Error, ValidationCategory.Required, columnIndex: 0, columnCanonicalKey: "Client"),
            Finding(ValidationSeverity.Error, ValidationCategory.Type, ValidationCode.TypeNumericInvalid, columnIndex: 1, columnCanonicalKey: "MontantHT"),
        };

        var result = Engine.Classify(Row(null, "abc"), findings, DefaultConfiguration);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Reasons.Count);
    }

    [Fact]
    public void Classify_BelowThresholdFindingsIgnored_OnlyOffendingOnesBecomeReasons()
    {
        var findings = new[]
        {
            Finding(ValidationSeverity.Information, ValidationCategory.Whitespace, ValidationCode.WhitespaceSurroundingValue),
            Finding(ValidationSeverity.Error, ValidationCategory.Required),
        };

        var result = Engine.Classify(Row(null, 100d), findings, DefaultConfiguration);

        Assert.NotNull(result);
        Assert.Single(result!.Reasons);
        Assert.Equal(ValidationCode.RequiredValueMissing, result.Reasons[0].Code);
    }

    [Fact]
    public void Classify_HighestSeverityAmongReasons_DrivesHighestSeverityAndCategory()
    {
        var findings = new[]
        {
            Finding(ValidationSeverity.Error, ValidationCategory.Required),
            Finding(ValidationSeverity.Critical, ValidationCategory.Structural, ValidationCode.RequiredColumnMissing),
        };

        var result = Engine.Classify(Row(null, 100d), findings, DefaultConfiguration);

        Assert.NotNull(result);
        Assert.Equal(ValidationSeverity.Critical, result!.HighestSeverity);
        Assert.Equal(RejectedRowCategory.StructuralIssue, result.Category);
    }

    [Fact]
    public void Classify_ConfigurableThreshold_RaisingItToCritical_AcceptsErrorSeverityRows()
    {
        var configuration = PipelineConfiguration.Default with { QuarantineSeverityThreshold = ValidationSeverity.Critical };
        var findings = new[] { Finding(ValidationSeverity.Error) };

        var result = Engine.Classify(Row(null, 100d), findings, configuration);

        Assert.Null(result);
    }

    [Theory]
    [InlineData(ValidationCategory.Required, RejectedRowCategory.MissingRequiredData)]
    [InlineData(ValidationCategory.Type, RejectedRowCategory.InvalidType)]
    [InlineData(ValidationCategory.Format, RejectedRowCategory.InvalidFormat)]
    [InlineData(ValidationCategory.Structural, RejectedRowCategory.StructuralIssue)]
    [InlineData(ValidationCategory.ForbiddenValue, RejectedRowCategory.PolicyViolation)]
    [InlineData(ValidationCategory.Range, RejectedRowCategory.PolicyViolation)]
    [InlineData(ValidationCategory.Whitespace, RejectedRowCategory.DataQuality)]
    [InlineData(ValidationCategory.Encoding, RejectedRowCategory.DataQuality)]
    [InlineData(ValidationCategory.Length, RejectedRowCategory.DataQuality)]
    public void Classify_ValidationCategory_MapsToExpectedRejectedRowCategory(ValidationCategory category, RejectedRowCategory expected)
    {
        var findings = new[] { Finding(ValidationSeverity.Error, category) };

        var result = Engine.Classify(Row(null, 100d), findings, DefaultConfiguration);

        Assert.NotNull(result);
        Assert.Equal(expected, result!.Category);
    }
}
