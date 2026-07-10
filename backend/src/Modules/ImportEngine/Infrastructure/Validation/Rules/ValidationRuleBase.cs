using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>
/// Socle commun des règles fournies par défaut : construit un <see cref="ValidationFinding"/> de
/// façon homogène (ligne, colonne, en-tête brut) et résout la sévérité effective en tenant
/// compte de <see cref="ValidationConfiguration.SeverityOverrides"/>, sans jamais figer la
/// sévérité d'un type de constat dans l'algorithme.
/// </summary>
public abstract class ValidationRuleBase : IValidationRule
{
    public abstract ValidationCode Code { get; }

    public abstract ValidationCategory Category { get; }

    public abstract ValidationSeverity DefaultSeverity { get; }

    public abstract ValidationFinding? Evaluate(ValidationRuleInput input);

    protected ValidationFinding CreateFinding(ValidationRuleInput input, ValidationMessage message)
    {
        var severity = input.Configuration.SeverityOverrides.GetValueOrDefault(Code.Value, DefaultSeverity);
        var rawHeader = input.ColumnIndex < input.Row.Headers.Count ? input.Row.Headers[input.ColumnIndex] : null;

        return new ValidationFinding(
            Code,
            Category,
            severity,
            input.Row.RowNumber,
            input.ColumnIndex,
            input.CanonicalColumn.Key,
            rawHeader,
            message);
    }

    /// <summary>Vrai si la valeur est absente ou une chaîne vide/uniquement composée d'espaces.</summary>
    protected static bool IsBlank(object? value) => ValidationValueHelpers.IsBlank(value);

    /// <summary>Représentation textuelle de la valeur, ou <see langword="null"/> si absente.</summary>
    protected static string? AsText(object? value) => ValidationValueHelpers.AsText(value);
}
