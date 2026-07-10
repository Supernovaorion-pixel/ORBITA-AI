using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale l'absence de valeur (« Valeurs obligatoires », « Valeurs nulles » —
/// features/IMPORT_ENGINE.md) pour une colonne dont le profil configuré exige une valeur sur
/// chaque ligne (cf. <see cref="ColumnValidationProfile.ValueRequired"/>).
/// </summary>
public sealed class RequiredValueRule : ValidationRuleBase
{
    public override ValidationCode Code => ValidationCode.RequiredValueMissing;

    public override ValidationCategory Category => ValidationCategory.Required;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Profile is not { ValueRequired: true } || !IsBlank(input.Value))
        {
            return null;
        }

        return CreateFinding(
            input,
            new ValidationMessage(
                Summary: $"Valeur obligatoire absente pour la colonne « {input.CanonicalColumn.DisplayName} ».",
                Explanation: $"La colonne « {input.CanonicalColumn.DisplayName} » est configurée comme exigeant une valeur sur chaque ligne, " +
                             "or celle-ci est vide ou absente pour cette ligne.",
                SuggestedResolution: "Compléter la valeur manquante dans le fichier source avant de poursuivre l'import, ou confirmer explicitement son absence."));
    }
}
