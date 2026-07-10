using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;

/// <summary>
/// Implémentation par défaut de <see cref="IValidationRuleEngine"/> : évalue systématiquement
/// l'intégralité des règles enregistrées pour une valeur donnée, sans jamais s'arrêter à la
/// première anomalie constatée (aucune erreur silencieuse).
/// </summary>
public sealed class ValidationRuleEngine : IValidationRuleEngine
{
    /// <inheritdoc />
    public IReadOnlyList<ValidationFinding> EvaluateCell(ValidationRuleInput input, IValidationRuleRegistry registry)
    {
        Guard.Against.Null(input, nameof(input));
        Guard.Against.Null(registry, nameof(registry));

        List<ValidationFinding>? findings = null;

        foreach (var rule in registry.Rules)
        {
            var finding = rule.Evaluate(input);
            if (finding is null)
            {
                continue;
            }

            findings ??= [];
            findings.Add(finding);
        }

        return findings ?? (IReadOnlyList<ValidationFinding>)Array.Empty<ValidationFinding>();
    }
}
