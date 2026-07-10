using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Validation;

/// <summary>Teste chaque règle de validation fournie par défaut de façon isolée.</summary>
public sealed class ValidationRuleTests
{
    private static readonly CanonicalColumnDefinition ClientColumn =
        CanonicalColumnDefinition.Create("Client", "Client", expectedValueKind: ColumnValueKind.Text);

    private static readonly CanonicalColumnDefinition MontantColumn =
        CanonicalColumnDefinition.Create("MontantHT", "Montant HT", expectedValueKind: ColumnValueKind.Numeric);

    private static readonly CanonicalColumnDefinition DateColumn =
        CanonicalColumnDefinition.Create("Date", "Date", expectedValueKind: ColumnValueKind.Date);

    private static ValidationRuleInput Input(
        object? value,
        CanonicalColumnDefinition? column = null,
        ColumnValidationProfile? profile = null,
        ValidationConfiguration? configuration = null)
    {
        IReadOnlyList<string> headers = ["Colonne"];
        var row = ValidationTestHelpers.Row(headers, 1, value);
        return new ValidationRuleInput(row, 0, value, column ?? ClientColumn, profile, configuration ?? ValidationConfiguration.Default);
    }

    // --- RequiredValueRule ---

    [Fact]
    public void RequiredValueRule_MissingValue_WhenRequired_ProducesFinding()
    {
        var rule = new RequiredValueRule();
        var profile = new ColumnValidationProfile("Client", ValueRequired: true);

        var finding = rule.Evaluate(Input(null, profile: profile));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.RequiredValueMissing, finding!.Code);
        Assert.Equal(ValidationSeverity.Error, finding.Severity);
    }

    [Fact]
    public void RequiredValueRule_BlankString_WhenRequired_ProducesFinding()
    {
        var rule = new RequiredValueRule();
        var profile = new ColumnValidationProfile("Client", ValueRequired: true);

        var finding = rule.Evaluate(Input("   ", profile: profile));

        Assert.NotNull(finding);
    }

    [Fact]
    public void RequiredValueRule_PresentValue_ProducesNoFinding()
    {
        var rule = new RequiredValueRule();
        var profile = new ColumnValidationProfile("Client", ValueRequired: true);

        var finding = rule.Evaluate(Input("Acme", profile: profile));

        Assert.Null(finding);
    }

    [Fact]
    public void RequiredValueRule_NotRequired_NeverProducesFinding()
    {
        var rule = new RequiredValueRule();

        var finding = rule.Evaluate(Input(null, profile: new ColumnValidationProfile("Client")));

        Assert.Null(finding);
    }

    // --- TypeRule ---

    [Fact]
    public void TypeRule_NonNumericValue_ForNumericColumn_ProducesFinding()
    {
        var rule = new TypeRule();

        var finding = rule.Evaluate(Input("abc", column: MontantColumn));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.TypeNumericInvalid, finding!.Code);
    }

    [Fact]
    public void TypeRule_NumericStringValue_ForNumericColumn_ProducesNoFinding()
    {
        var rule = new TypeRule();

        var finding = rule.Evaluate(Input("1234.56", column: MontantColumn));

        Assert.Null(finding);
    }

    [Fact]
    public void TypeRule_InvalidDate_ForDateColumn_ProducesFinding()
    {
        var rule = new TypeRule();

        var finding = rule.Evaluate(Input("pas une date", column: DateColumn));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.TypeDateInvalid, finding!.Code);
    }

    [Fact]
    public void TypeRule_ValidDate_ForDateColumn_ProducesNoFinding()
    {
        var rule = new TypeRule();

        var finding = rule.Evaluate(Input("2026-01-15", column: DateColumn));

        Assert.Null(finding);
    }

    [Fact]
    public void TypeRule_BlankValue_NeverProducesFinding()
    {
        var rule = new TypeRule();

        Assert.Null(rule.Evaluate(Input(null, column: MontantColumn)));
    }

    // --- LengthRule ---

    [Fact]
    public void LengthRule_ValueTooLong_ProducesFinding()
    {
        var rule = new LengthRule();
        var profile = new ColumnValidationProfile("Client", MaxLength: 5);

        var finding = rule.Evaluate(Input("TropLongPourCetteColonne", profile: profile));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.LengthTooLong, finding!.Code);
    }

    [Fact]
    public void LengthRule_ValueTooShort_ProducesFinding()
    {
        var rule = new LengthRule();
        var profile = new ColumnValidationProfile("Client", MinLength: 10);

        var finding = rule.Evaluate(Input("court", profile: profile));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.LengthTooShort, finding!.Code);
    }

    [Fact]
    public void LengthRule_ValueWithinBounds_ProducesNoFinding()
    {
        var rule = new LengthRule();
        var profile = new ColumnValidationProfile("Client", MinLength: 2, MaxLength: 50);

        Assert.Null(rule.Evaluate(Input("Acme", profile: profile)));
    }

    // --- FormatRule ---

    [Fact]
    public void FormatRule_ValueNotMatchingPattern_ProducesFinding()
    {
        var rule = new FormatRule();
        var profile = new ColumnValidationProfile("Client", FormatPattern: @"^[A-Z]{2}\d{4}$");

        var finding = rule.Evaluate(Input("invalide", profile: profile));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.FormatMismatch, finding!.Code);
    }

    [Fact]
    public void FormatRule_ValueMatchingPattern_ProducesNoFinding()
    {
        var rule = new FormatRule();
        var profile = new ColumnValidationProfile("Client", FormatPattern: @"^[A-Z]{2}\d{4}$");

        Assert.Null(rule.Evaluate(Input("AB1234", profile: profile)));
    }

    // --- ForbiddenValueRule ---

    [Fact]
    public void ForbiddenValueRule_ForbiddenValue_ProducesFinding_CaseInsensitive()
    {
        var rule = new ForbiddenValueRule();
        var profile = new ColumnValidationProfile("Client", ForbiddenValues: ["N/A", "TBD"]);

        var finding = rule.Evaluate(Input("n/a", profile: profile));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.ForbiddenValue, finding!.Code);
    }

    [Fact]
    public void ForbiddenValueRule_AllowedValue_ProducesNoFinding()
    {
        var rule = new ForbiddenValueRule();
        var profile = new ColumnValidationProfile("Client", ForbiddenValues: ["N/A"]);

        Assert.Null(rule.Evaluate(Input("Acme", profile: profile)));
    }

    // --- NumericRangeRule ---

    [Fact]
    public void NumericRangeRule_ValueBelowMinimum_ProducesFinding()
    {
        var rule = new NumericRangeRule();
        var profile = new ColumnValidationProfile("MontantHT", MinNumericValue: 0);

        var finding = rule.Evaluate(Input(-100d, column: MontantColumn, profile: profile));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.NumericRangeExceeded, finding!.Code);
    }

    [Fact]
    public void NumericRangeRule_ValueAboveMaximum_ProducesFinding()
    {
        var rule = new NumericRangeRule();
        var profile = new ColumnValidationProfile("MontantHT", MaxNumericValue: 1000);

        var finding = rule.Evaluate(Input(5000d, column: MontantColumn, profile: profile));

        Assert.NotNull(finding);
    }

    [Fact]
    public void NumericRangeRule_ValueWithinRange_ProducesNoFinding()
    {
        var rule = new NumericRangeRule();
        var profile = new ColumnValidationProfile("MontantHT", MinNumericValue: 0, MaxNumericValue: 1000);

        Assert.Null(rule.Evaluate(Input(500d, column: MontantColumn, profile: profile)));
    }

    // --- WhitespaceRule ---

    [Fact]
    public void WhitespaceRule_SurroundingWhitespace_ProducesFinding()
    {
        var rule = new WhitespaceRule();

        var finding = rule.Evaluate(Input("  Acme  "));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.WhitespaceSurroundingValue, finding!.Code);
    }

    [Fact]
    public void WhitespaceRule_RepeatedInternalWhitespace_ProducesFinding()
    {
        var rule = new WhitespaceRule();

        var finding = rule.Evaluate(Input("Jean   Dupont"));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.WhitespaceInternalRepeated, finding!.Code);
    }

    [Fact]
    public void WhitespaceRule_CleanValue_ProducesNoFinding()
    {
        var rule = new WhitespaceRule();

        Assert.Null(rule.Evaluate(Input("Jean Dupont")));
    }

    [Fact]
    public void WhitespaceRule_Disabled_NeverProducesFinding()
    {
        var rule = new WhitespaceRule();
        var configuration = ValidationConfiguration.Default with { DetectSurroundingWhitespace = false, DetectRepeatedInternalWhitespace = false };

        Assert.Null(rule.Evaluate(Input("  Acme  ", configuration: configuration)));
    }

    // --- SpecialCharacterRule ---

    [Fact]
    public void SpecialCharacterRule_ReplacementCharacter_ProducesFinding()
    {
        var rule = new SpecialCharacterRule();

        var finding = rule.Evaluate(Input("Acm�e"));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.ReplacementCharacterPresent, finding!.Code);
        Assert.Equal(ValidationSeverity.Error, finding.Severity);
    }

    [Fact]
    public void SpecialCharacterRule_ControlCharacter_ProducesFinding()
    {
        var rule = new SpecialCharacterRule();

        var finding = rule.Evaluate(Input("Acme" + "\u0007" + "Corp"));

        Assert.NotNull(finding);
        Assert.Equal(ValidationCode.ControlCharacterPresent, finding!.Code);
    }

    [Fact]
    public void SpecialCharacterRule_CleanUnicodeText_ProducesNoFinding()
    {
        var rule = new SpecialCharacterRule();

        Assert.Null(rule.Evaluate(Input("Société Générale — Café")));
    }
}
