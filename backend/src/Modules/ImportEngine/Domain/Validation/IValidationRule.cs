namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Règle de validation atomique, appliquée à la valeur d'une seule colonne canonique pour une
/// seule ligne. Une règle ne modifie ni ne corrige jamais la valeur qu'elle examine : elle ne
/// fait que produire, le cas échéant, un <see cref="ValidationFinding"/> décrivant l'anomalie
/// constatée. Aucun paramètre métier (seuil, motif, valeur interdite) n'est codé en dur dans une
/// implémentation : toute donnée de configuration transite par <see cref="ValidationRuleInput.Profile"/>.
/// </summary>
public interface IValidationRule
{
    /// <summary>Identifiant technique stable du type de constat produit par cette règle.</summary>
    ValidationCode Code { get; }

    /// <summary>Nature structurelle des constats produits par cette règle.</summary>
    ValidationCategory Category { get; }

    /// <summary>Sévérité par défaut des constats produits par cette règle, avant application d'un éventuel <see cref="ValidationConfiguration.SeverityOverrides"/>.</summary>
    ValidationSeverity DefaultSeverity { get; }

    /// <summary>
    /// Évalue cette règle pour l'entrée fournie. Retourne <see langword="null"/> si aucune
    /// anomalie n'est constatée.
    /// </summary>
    ValidationFinding? Evaluate(ValidationRuleInput input);
}
