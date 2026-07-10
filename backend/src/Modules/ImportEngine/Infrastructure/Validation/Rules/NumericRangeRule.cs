using System.Globalization;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale une valeur numérique hors de la plage configurée (« Valeurs incohérentes » —
/// features/IMPORT_ENGINE.md), via <see cref="ColumnValidationProfile.MinNumericValue"/> et
/// <see cref="ColumnValidationProfile.MaxNumericValue"/>. Une plage bornée reste une règle
/// statique et déterministe, jamais une analyse ou une inférence statistique de la donnée.
/// </summary>
public sealed class NumericRangeRule : ValidationRuleBase
{
    public override ValidationCode Code => ValidationCode.NumericRangeExceeded;

    public override ValidationCategory Category => ValidationCategory.Range;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Warning;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Profile is not { } profile
            || (profile.MinNumericValue is null && profile.MaxNumericValue is null)
            || IsBlank(input.Value)
            || !TryGetNumericValue(input.Value!, out var numericValue))
        {
            return null;
        }

        if (profile.MinNumericValue is { } min && numericValue < min)
        {
            return CreateFinding(
                input,
                new ValidationMessage(
                    Summary: $"La valeur {numericValue} de la colonne « {input.CanonicalColumn.DisplayName} » est inférieure au minimum autorisé ({min}).",
                    Explanation: $"La colonne « {input.CanonicalColumn.DisplayName} » est configurée avec une valeur minimale de {min}.",
                    SuggestedResolution: "Vérifier la valeur dans le fichier source, ou ajuster la plage configurée si elle est trop stricte."));
        }

        if (profile.MaxNumericValue is { } max && numericValue > max)
        {
            return CreateFinding(
                input,
                new ValidationMessage(
                    Summary: $"La valeur {numericValue} de la colonne « {input.CanonicalColumn.DisplayName} » dépasse le maximum autorisé ({max}).",
                    Explanation: $"La colonne « {input.CanonicalColumn.DisplayName} » est configurée avec une valeur maximale de {max}.",
                    SuggestedResolution: "Vérifier la valeur dans le fichier source, ou ajuster la plage configurée si elle est trop stricte."));
        }

        return null;
    }

    private static bool TryGetNumericValue(object value, out double numericValue)
    {
        switch (value)
        {
            case double d:
                numericValue = d;
                return true;
            case int or long or float or decimal:
                numericValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return true;
            case string text:
                return double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out numericValue);
            default:
                numericValue = 0;
                return false;
        }
    }
}
