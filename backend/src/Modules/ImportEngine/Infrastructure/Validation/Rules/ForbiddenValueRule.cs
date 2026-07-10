using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale une valeur figurant explicitement parmi les valeurs interdites configurées
/// (« Valeurs interdites » — features/IMPORT_ENGINE.md), via
/// <see cref="ColumnValidationProfile.ForbiddenValues"/>. Comparaison insensible à la casse et
/// aux espaces superflus, pour ne pas laisser passer une valeur interdite par une simple
/// variation de forme.
/// </summary>
public sealed class ForbiddenValueRule : ValidationRuleBase
{
    public override ValidationCode Code => ValidationCode.ForbiddenValue;

    public override ValidationCategory Category => ValidationCategory.ForbiddenValue;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Warning;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Profile is not { } profile || profile.ForbiddenValues.Count == 0 || IsBlank(input.Value))
        {
            return null;
        }

        var text = (AsText(input.Value) ?? string.Empty).Trim();
        var isForbidden = profile.ForbiddenValues.Any(forbidden => string.Equals(forbidden.Trim(), text, StringComparison.OrdinalIgnoreCase));

        if (!isForbidden)
        {
            return null;
        }

        return CreateFinding(
            input,
            new ValidationMessage(
                Summary: $"La valeur « {text} » est explicitement interdite pour la colonne « {input.CanonicalColumn.DisplayName} ».",
                Explanation: "Cette valeur figure dans la liste des valeurs interdites configurées pour cette colonne " +
                             "(ex. valeur d'espacement réservée telle que « N/A » ou « TBD »).",
                SuggestedResolution: "Remplacer cette valeur dans le fichier source par une donnée exploitable, ou la laisser vide si elle est réellement inconnue."));
    }
}
