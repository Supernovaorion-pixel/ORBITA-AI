namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Évalue l'ensemble des règles applicables (cf. <see cref="IValidationRuleRegistry"/>) pour une
/// seule valeur de colonne, dans une seule ligne. Ne court-circuite jamais après la première
/// anomalie constatée : toutes les règles applicables sont évaluées, garantissant qu'aucune
/// anomalie ne reste silencieuse.
/// </summary>
public interface IValidationRuleEngine
{
    /// <summary>Retourne l'ensemble des constats produits par les règles applicables à <paramref name="input"/>.</summary>
    IReadOnlyList<ValidationFinding> EvaluateCell(ValidationRuleInput input, IValidationRuleRegistry registry);
}
