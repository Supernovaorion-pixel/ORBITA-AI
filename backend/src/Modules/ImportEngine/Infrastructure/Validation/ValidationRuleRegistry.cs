using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;

/// <summary>
/// Implémentation par défaut de <see cref="IValidationRuleRegistry"/>. L'ajout d'une future
/// règle se traduit uniquement par l'enregistrement d'une nouvelle implémentation de
/// <see cref="IValidationRule"/> auprès de cette classe, sans modification du pipeline.
/// </summary>
public sealed class ValidationRuleRegistry : IValidationRuleRegistry
{
    public ValidationRuleRegistry(IEnumerable<IValidationRule> rules)
    {
        Guard.Against.Null(rules, nameof(rules));
        Rules = rules.ToArray();
    }

    /// <inheritdoc />
    public IReadOnlyList<IValidationRule> Rules { get; }

    /// <summary>Registre par défaut, regroupant l'ensemble des règles fournies avec le moteur.</summary>
    public static ValidationRuleRegistry CreateDefault() => new(
    [
        new RequiredValueRule(),
        new TypeRule(),
        new LengthRule(),
        new FormatRule(),
        new ForbiddenValueRule(),
        new NumericRangeRule(),
        new WhitespaceRule(),
        new SpecialCharacterRule(),
    ]);
}
