namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Configuration par défaut du moteur de correspondance : seuils de décision du
/// <see cref="IConfidenceEngine"/> et garde-fous de performance applicables à toute opération
/// (par opposition à <see cref="MappingOptions"/>, propre à chaque appel). Les seuils par défaut
/// reflètent l'échelle de confiance de référence du produit (100 % certaine, 95 % très probable,
/// 80 % probable, 60 % ambiguë, moins de 60 % non reconnue).
/// </summary>
public sealed record MappingConfiguration
{
    /// <summary>
    /// Score minimal, sur 100, à partir duquel une colonne est considérée reconnue avec certitude
    /// suffisante (<see cref="ColumnRecognitionStatus.Recognized"/>).
    /// </summary>
    public double RecognizedThreshold { get; init; } = 80;

    /// <summary>
    /// Score minimal, sur 100, à partir duquel une colonne est considérée comme une correspondance
    /// plausible mais incertaine (<see cref="ColumnRecognitionStatus.Ambiguous"/>). En dessous de
    /// ce seuil, la colonne est classée <see cref="ColumnRecognitionStatus.Unknown"/>.
    /// </summary>
    public double AmbiguousThreshold { get; init; } = 60;

    /// <summary>
    /// Similarité minimale (0 à 1, fondée sur la distance d'édition normalisée) en dessous de
    /// laquelle deux libellés ne sont plus considérés comme des variantes l'un de l'autre.
    /// </summary>
    public double FuzzyMatchMinimumSimilarity { get; init; } = 0.75;

    /// <summary>
    /// Pénalité, en points, appliquée au score d'un candidat lorsque le contenu observé de la
    /// colonne contredit fortement la nature attendue (<see cref="CanonicalColumnDefinition.ExpectedValueKind"/>).
    /// </summary>
    public double ContentMismatchPenaltyPoints { get; init; } = 25;

    /// <summary>
    /// Nombre maximal de lignes d'échantillon prises en compte pour le profilage de contenu de
    /// chaque colonne, quel que soit le nombre de lignes fournies par l'appelant — garde-fou
    /// garantissant des performances constantes indépendamment de la taille réelle du fichier
    /// (architecture/PERFORMANCE_GUIDELINES.md).
    /// </summary>
    public int MaxContentSampleRows { get; init; } = 500;

    /// <summary>
    /// Écart maximal, en points, en deçà duquel deux candidats de colonnes canoniques distinctes
    /// pour une même colonne source sont considérés comme réellement à égalité (ambiguïté),
    /// plutôt que le second étant une simple correspondance secondaire sans conséquence (ex. un
    /// candidat à 100 face à un second à 80 ne constitue pas une ambiguïté réelle : l'écart de
    /// 20 points au-delà de cette marge tranche nettement en faveur du premier).
    /// </summary>
    public double TieMarginPoints { get; init; } = 10;

    /// <summary>Configuration par défaut, adaptée à la très large majorité des cas d'usage.</summary>
    public static readonly MappingConfiguration Default = new();
}
