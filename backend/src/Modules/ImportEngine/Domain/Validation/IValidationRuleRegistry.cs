namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Ensemble des règles de validation (cf. <see cref="IValidationRule"/>) appliquées par le
/// pipeline. L'ajout d'une future règle se traduit uniquement par l'enregistrement d'une
/// nouvelle implémentation, sans modification du pipeline lui-même (principe ouvert/fermé,
/// architecture/CODING_PRINCIPLES.md §1).
/// </summary>
public interface IValidationRuleRegistry
{
    /// <summary>Règles de validation actuellement enregistrées.</summary>
    IReadOnlyList<IValidationRule> Rules { get; }
}
