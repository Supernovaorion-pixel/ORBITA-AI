namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Niveau de gravité d'un <see cref="ValidationFinding"/>. La sévérité par défaut de chaque
/// règle est déterministe et documentée (cf. Infrastructure/Validation/Rules/), et reste
/// intégralement modifiable via <see cref="ValidationConfiguration.SeverityOverrides"/> —
/// jamais figée dans l'algorithme d'une règle.
/// </summary>
public enum ValidationSeverity
{
    /// <summary>Constat neutre, sans conséquence attendue sur la suite du traitement.</summary>
    Information,

    /// <summary>Anomalie à surveiller, sans blocage du traitement.</summary>
    Warning,

    /// <summary>Anomalie significative affectant la fiabilité de la ligne concernée.</summary>
    Error,

    /// <summary>Anomalie structurelle ou bloquante, remettant en cause la fiabilité globale du fichier.</summary>
    Critical
}
