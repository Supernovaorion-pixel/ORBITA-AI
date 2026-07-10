namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Étape courante d'une exécution du Pipeline d'Import (features/IMPORT_ENGINE.md). La Validation
/// et la Quarantaine se déroulent conjointement, ligne par ligne, en un seul passage continu sur le
/// flux (architecture/PERFORMANCE_GUIDELINES.md) : <see cref="Processing"/> recouvre donc les deux.
/// </summary>
public enum PipelineState
{
    /// <summary>Aucune opération engagée.</summary>
    NotStarted,

    /// <summary>Lecture de l'échantillon initial nécessaire à la correspondance des colonnes.</summary>
    Reading,

    /// <summary>Analyse de la correspondance des colonnes (Mapping Engine).</summary>
    Mapping,

    /// <summary>Validation et quarantaine de chaque ligne, en flux continu.</summary>
    Processing,

    /// <summary>Traitement achevé, toutes les lignes ayant été lues et classées.</summary>
    Completed,

    /// <summary>
    /// Traitement suspendu avant tout examen ligne par ligne : une anomalie structurelle
    /// (ex. colonne obligatoire absente) atteint le seuil de blocage configuré
    /// (cf. <see cref="Domain.Validation.ValidationConfiguration.BlockingSeverityThreshold"/>).
    /// </summary>
    Halted,

    /// <summary>Traitement interrompu par une annulation explicite de l'appelant.</summary>
    Cancelled,

    /// <summary>Traitement interrompu par une erreur imprévue (architecture/ERROR_HANDLING.md).</summary>
    Failed,
}
