using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Signale les anomalies d'encodage et de caractères (« Caractères spéciaux », « Encodage »,
/// « Unicode » — features/IMPORT_ENGINE.md) : présence du caractère de remplacement Unicode
/// U+FFFD (signe quasi certain d'un décodage défectueux du fichier source) et présence de
/// caractères de contrôle non imprimables. Les deux vérifications sont indépendamment activables
/// via <see cref="ValidationConfiguration.DetectReplacementCharacter"/> et
/// <see cref="ValidationConfiguration.DetectControlCharacters"/>.
/// </summary>
public sealed class SpecialCharacterRule : ValidationRuleBase
{
    private const char ReplacementCharacter = '�';

    public override ValidationCode Code => ValidationCode.ReplacementCharacterPresent;

    public override ValidationCategory Category => ValidationCategory.Encoding;

    public override ValidationSeverity DefaultSeverity => ValidationSeverity.Error;

    public override ValidationFinding? Evaluate(ValidationRuleInput input)
    {
        if (input.Value is not string text || text.Length == 0)
        {
            return null;
        }

        if (input.Configuration.DetectReplacementCharacter && text.Contains(ReplacementCharacter))
        {
            return Build(
                input,
                ValidationCode.ReplacementCharacterPresent,
                ValidationSeverity.Error,
                $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » contient le caractère de remplacement Unicode (U+FFFD).",
                "Ce caractère apparaît typiquement lorsqu'un fichier a été décodé avec un encodage différent de son encodage réel " +
                "(ex. Windows-1252 lu comme UTF-8).",
                "Réimporter le fichier en précisant explicitement son encodage d'origine.");
        }

        if (input.Configuration.DetectControlCharacters && ContainsControlCharacter(text))
        {
            return Build(
                input,
                ValidationCode.ControlCharacterPresent,
                ValidationSeverity.Warning,
                $"La valeur de la colonne « {input.CanonicalColumn.DisplayName} » contient un caractère de contrôle non imprimable.",
                "Un caractère de contrôle (ex. saut de ligne interne, tabulation) a été détecté au sein d'une valeur autrement textuelle.",
                "Vérifier l'origine de ce caractère dans le fichier source et le retirer si non intentionnel.");
        }

        return null;
    }

    private static bool ContainsControlCharacter(string text)
    {
        foreach (var c in text)
        {
            // Tabulation et espace exclus : ce sont des séparateurs usuels, non une anomalie en soi.
            if (char.IsControl(c) && c is not '\t')
            {
                return true;
            }
        }

        return false;
    }

    private ValidationFinding Build(
        ValidationRuleInput input,
        ValidationCode code,
        ValidationSeverity defaultSeverity,
        string summary,
        string explanation,
        string resolution)
    {
        var severity = input.Configuration.SeverityOverrides.GetValueOrDefault(code.Value, defaultSeverity);
        var rawHeader = input.ColumnIndex < input.Row.Headers.Count ? input.Row.Headers[input.ColumnIndex] : null;
        var message = new ValidationMessage(summary, explanation, resolution);

        return new ValidationFinding(code, Category, severity, input.Row.RowNumber, input.ColumnIndex, input.CanonicalColumn.Key, rawHeader, message);
    }
}
