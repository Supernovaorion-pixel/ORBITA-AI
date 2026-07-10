using System.Globalization;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale une valeur non interprétable dans le type attendu de sa colonne canonique
/// (<see cref="CanonicalColumnDefinition.ExpectedValueKind"/>) : « Types invalides »,
/// « Dates invalides », « Valeurs numériques invalides » (features/IMPORT_ENGINE.md). Une seule
/// règle porte ces trois vérifications, chacune produisant un code distinct, pour éviter toute
/// duplication de logique entre des règles par ailleurs identiques dans leur structure.
/// </summary>
public sealed class TypeRule : IValidationRule
{
    // Cette règle produit l'un de deux codes distincts selon le type attendu : la propriété
    // Code de l'interface reflète le cas le plus générique (numérique), le code réellement
    // employé pour chaque constat étant déterminé dans Evaluate.
    public ValidationCode Code => ValidationCode.TypeNumericInvalid;

    public ValidationCategory Category => ValidationCategory.Type;

    public ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

    public ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (ValidationValueHelpers.IsBlank(input.Value))
        {
            return null;
        }

        var text = input.Value!.ToString();

        return input.CanonicalColumn.ExpectedValueKind switch
        {
            ColumnValueKind.Numeric when input.Value is not (double or int or long or float or decimal)
                && !double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _)
                => Build(input, ValidationCode.TypeNumericInvalid, "numérique"),

            ColumnValueKind.Date when input.Value is not DateTime and not DateTimeOffset
                && !DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                => Build(input, ValidationCode.TypeDateInvalid, "date"),

            _ => null,
        };
    }

    private static ValidationFinding Build(ValidationRuleInput input, ValidationCode code, string expectedKindLabel)
    {
        var severity = input.Configuration.SeverityOverrides.GetValueOrDefault(code.Value, ValidationSeverity.Error);
        var rawHeader = input.ColumnIndex < input.Row.Headers.Count ? input.Row.Headers[input.ColumnIndex] : null;

        var message = new ValidationMessage(
            Summary: $"Valeur non interprétable comme {expectedKindLabel} pour la colonne « {input.CanonicalColumn.DisplayName} ».",
            Explanation: $"La colonne « {input.CanonicalColumn.DisplayName} » attend une valeur de type {expectedKindLabel}, " +
                         $"or la valeur « {input.Value} » ne peut être interprétée comme telle.",
            SuggestedResolution: $"Vérifier le format de la valeur dans le fichier source et le corriger en un format {expectedKindLabel} valide.");

        return new ValidationFinding(
            code,
            ValidationCategory.Type,
            severity,
            input.Row.RowNumber,
            input.ColumnIndex,
            input.CanonicalColumn.Key,
            rawHeader,
            message);
    }
}
