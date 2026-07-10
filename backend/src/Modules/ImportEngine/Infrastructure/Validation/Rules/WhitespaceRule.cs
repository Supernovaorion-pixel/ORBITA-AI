using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale un espacement superflu (« Espaces inutiles » — features/IMPORT_ENGINE.md) : espaces
/// en début ou fin de valeur, ou espaces internes répétés. Ne modifie jamais la valeur elle-même
/// (aucun nettoyage) : se contente de signaler. Les deux vérifications sont indépendamment
/// activables via <see cref="ValidationConfiguration.DetectSurroundingWhitespace"/> et
/// <see cref="ValidationConfiguration.DetectRepeatedInternalWhitespace"/>.
/// </summary>
public sealed class WhitespaceRule : ValidationRuleBase
{
    public override ValidationCode Code => ValidationCode.WhitespaceSurroundingValue;

    public override ValidationCategory Category => ValidationCategory.Whitespace;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Information;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Value is not string text || text.Length == 0)
        {
            return null;
        }

        if (input.Configuration.DetectSurroundingWhitespace && text != text.Trim())
        {
            return Build(
                input,
                ValidationCode.WhitespaceSurroundingValue,
                $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » comporte des espaces superflus en début ou fin de valeur.",
                "Retirer les espaces superflus en début et fin de valeur dans le fichier source.");
        }

        if (input.Configuration.DetectRepeatedInternalWhitespace && ContainsRepeatedWhitespace(text))
        {
            return Build(
                input,
                ValidationCode.WhitespaceInternalRepeated,
                $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » comporte des espaces internes répétés.",
                "Réduire les espaces internes répétés à un espace unique dans le fichier source.");
        }

        return null;
    }

    private static bool ContainsRepeatedWhitespace(string text)
    {
        for (var i = 1; i < text.Length; i++)
        {
            if (char.IsWhiteSpace(text[i]) && char.IsWhiteSpace(text[i - 1]))
            {
                return true;
            }
        }

        return false;
    }

    private ValidationFinding Build(ValidationRuleInput input, ValidationCode code, string summary, string resolution)
    {
        var severity = input.Configuration.SeverityOverrides.GetValueOrDefault(code.Value, DefaultSeverity);
        var rawHeader = input.ColumnIndex < input.Row.Headers.Count ? input.Row.Headers[input.ColumnIndex] : null;
        var message = new ValidationMessage(summary, summary, resolution);

        return new ValidationFinding(code, Category, severity, input.Row.RowNumber, input.ColumnIndex, input.CanonicalColumn.Key, rawHeader, message);
    }
}
