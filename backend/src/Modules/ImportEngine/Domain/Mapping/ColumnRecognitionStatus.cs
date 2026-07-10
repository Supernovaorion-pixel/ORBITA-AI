namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Issue de la reconnaissance d'une colonne source, déterminée par les seuils de
/// <see cref="MappingConfiguration"/> appliqués au score du <see cref="IConfidenceEngine"/>.
/// </summary>
public enum ColumnRecognitionStatus
{
    /// <summary>
    /// Score au moins égal à <see cref="MappingConfiguration.RecognizedThreshold"/> et candidat
    /// unique retenu sans conflit avec une autre colonne (features/IMPORT_ENGINE.md).
    /// </summary>
    Recognized,

    /// <summary>
    /// Score compris entre <see cref="MappingConfiguration.AmbiguousThreshold"/> (inclus) et
    /// <see cref="MappingConfiguration.RecognizedThreshold"/> (exclu), ou plusieurs colonnes
    /// candidates au même champ canonique (colonnes dupliquées) : une décision humaine est requise.
    /// </summary>
    Ambiguous,

    /// <summary>Aucun candidat n'atteint <see cref="MappingConfiguration.AmbiguousThreshold"/>.</summary>
    Unknown
}
