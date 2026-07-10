using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale une valeur dont la longueur ne respecte pas les bornes configurées
/// (« Longueur maximale », « Longueur minimale » — features/IMPORT_ENGINE.md), via
/// <see cref="ColumnValidationProfile.MinLength"/> et <see cref="ColumnValidationProfile.MaxLength"/>.
/// </summary>
public sealed class LengthRule : ValidationRuleBase
{
    // Le code effectivement employé dépend du sens de la violation (trop court / trop long) ;
    // celui-ci reflète la valeur la plus fréquemment rencontrée en pratique.
    public override ValidationCode Code => ValidationCode.LengthTooLong;

    public override ValidationCategory Category => ValidationCategory.Length;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Warning;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Profile is null || IsBlank(input.Value))
        {
            return null;
        }

        var text = AsText(input.Value) ?? string.Empty;

        if (input.Profile.MaxLength is { } maxLength && text.Length > maxLength)
        {
            return BuildFinding(
                input,
                ValidationCode.LengthTooLong,
                $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » dépasse la longueur maximale autorisée ({maxLength} caractères, {text.Length} constatés).",
                $"Réduire la longueur de la valeur à {maxLength} caractères au plus, ou ajuster la longueur maximale configurée si elle est trop restrictive.");
        }

        if (input.Profile.MinLength is { } minLength && text.Length < minLength)
        {
            return BuildFinding(
                input,
                ValidationCode.LengthTooShort,
                $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » est plus courte que la longueur minimale attendue ({minLength} caractères, {text.Length} constatés).",
                $"Compléter la valeur pour atteindre au moins {minLength} caractères, ou ajuster la longueur minimale configurée si elle est trop stricte.");
        }

        return null;
    }

    private ValidationFinding BuildFinding(ValidationRuleInput input, ValidationCode code, string summary, string resolution)
    {
        var severity = input.Configuration.SeverityOverrides.GetValueOrDefault(code.Value, DefaultSeverity);
        var rawHeader = input.ColumnIndex < input.Row.Headers.Count ? input.Row.Headers[input.ColumnIndex] : null;
        var message = new ValidationMessage(summary, summary, resolution);

        return new ValidationFinding(code, Category, severity, input.Row.RowNumber, input.ColumnIndex, input.CanonicalColumn.Key, rawHeader, message);
    }
}
