namespace OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

/// <summary>
/// Regroupement thématique, à des fins de restitution (répartition par type d'erreur —
/// features/IMPORT_ENGINE.md §14), de la raison pour laquelle une ligne a été mise en quarantaine.
/// Plus grossier que <see cref="Domain.Validation.ValidationCategory"/> : classe le motif dominant
/// d'une ligne rejetée pour un rapport d'import lisible, sans exposer le détail interne des règles
/// de validation.
/// </summary>
public enum RejectedRowCategory
{
    /// <summary>Une donnée obligatoire est absente.</summary>
    MissingRequiredData,

    /// <summary>Une valeur ne correspond pas au type attendu (numérique, date).</summary>
    InvalidType,

    /// <summary>Une valeur ne respecte pas le format attendu.</summary>
    InvalidFormat,

    /// <summary>Une valeur enfreint une politique explicite (valeur interdite, plage numérique).</summary>
    PolicyViolation,

    /// <summary>Une anomalie structurelle du fichier (colonne obligatoire absente, colonnes dupliquées).</summary>
    StructuralIssue,

    /// <summary>Toute autre anomalie de qualité de donnée (espaces superflus, encodage, longueur).</summary>
    DataQuality,
}
