using System.Text.RegularExpressions;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale une valeur ne respectant pas le format attendu (« Formats » — features/IMPORT_ENGINE.md),
/// défini par une expression régulière configurable (cf. <see cref="ColumnValidationProfile.FormatPattern"/>).
/// </summary>
public sealed class FormatRule : ValidationRuleBase
{
    public override ValidationCode Code => ValidationCode.FormatMismatch;

    public override ValidationCategory Category => ValidationCategory.Format;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Warning;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Profile?.FormatPattern is not { } pattern || IsBlank(input.Value))
        {
            return null;
        }

        var text = AsText(input.Value) ?? string.Empty;
        if (Regex.IsMatch(text, pattern))
        {
            return null;
        }

        return CreateFinding(
            input,
            new ValidationMessage(
                Summary: $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » ne respecte pas le format attendu.",
                Explanation: $"La colonne « {input.CanonicalColumn.DisplayName} » exige un format conforme à « {pattern} », " +
                             $"or la valeur « {text} » ne le respecte pas.",
                SuggestedResolution: "Corriger la valeur dans le fichier source pour respecter le format attendu."));
    }
}
