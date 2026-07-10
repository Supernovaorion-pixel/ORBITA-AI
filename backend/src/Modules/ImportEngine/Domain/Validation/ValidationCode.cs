using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Identifiant technique stable d'un type de constat de validation (ex. "VAL.REQUIRED.MISSING"),
/// permettant de le retrouver et de le configurer (cf. <see cref="ValidationConfiguration.SeverityOverrides"/>)
/// indépendamment de son message, qui peut évoluer sans casser une intégration existante.
/// </summary>
public sealed record ValidationCode
{
    public string Value { get; }

    public ValidationCode(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }

    public static implicit operator string(ValidationCode code) => code.Value;

    public override string ToString() => Value;

    // Codes officiels des règles fournies par défaut (Infrastructure/Validation/Rules/).
    public static readonly ValidationCode RequiredValueMissing = new("VAL.REQUIRED.MISSING");
    public static readonly ValidationCode TypeNumericInvalid = new("VAL.TYPE.NUMERIC_INVALID");
    public static readonly ValidationCode TypeDateInvalid = new("VAL.TYPE.DATE_INVALID");
    public static readonly ValidationCode LengthTooShort = new("VAL.LENGTH.TOO_SHORT");
    public static readonly ValidationCode LengthTooLong = new("VAL.LENGTH.TOO_LONG");
    public static readonly ValidationCode FormatMismatch = new("VAL.FORMAT.MISMATCH");
    public static readonly ValidationCode ForbiddenValue = new("VAL.FORBIDDEN.VALUE");
    public static readonly ValidationCode NumericRangeExceeded = new("VAL.RANGE.NUMERIC_EXCEEDED");
    public static readonly ValidationCode WhitespaceSurroundingValue = new("VAL.WHITESPACE.SURROUNDING");
    public static readonly ValidationCode WhitespaceInternalRepeated = new("VAL.WHITESPACE.INTERNAL_REPEATED");
    public static readonly ValidationCode ControlCharacterPresent = new("VAL.ENCODING.CONTROL_CHARACTER");
    public static readonly ValidationCode ReplacementCharacterPresent = new("VAL.ENCODING.REPLACEMENT_CHARACTER");
    public static readonly ValidationCode RequiredColumnMissing = new("VAL.STRUCTURAL.REQUIRED_COLUMN_MISSING");
    public static readonly ValidationCode UnknownColumn = new("VAL.STRUCTURAL.UNKNOWN_COLUMN");
    public static readonly ValidationCode DuplicateColumn = new("VAL.STRUCTURAL.DUPLICATE_COLUMN");
}
