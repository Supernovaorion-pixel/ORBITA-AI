namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Profil statistique du contenu d'une colonne, calculé sur un échantillon borné de lignes
/// (jamais l'intégralité du fichier — cf. architecture/PERFORMANCE_GUIDELINES.md). Utilisé
/// exclusivement pour nuancer une correspondance déjà établie par le nom de la colonne
/// (cf. <see cref="IConfidenceEngine"/>), jamais pour calculer, agréger ou modifier une donnée
/// métier : ceci reste un constat structurel sur la forme du contenu, pas son analyse
/// commerciale (hors périmètre de cette mission).
/// </summary>
/// <param name="SampledRowCount">Nombre de lignes de l'échantillon effectivement examinées.</param>
/// <param name="NonEmptyCount">Nombre de valeurs non vides parmi l'échantillon.</param>
/// <param name="NumericCount">Nombre de valeurs interprétables comme un nombre.</param>
/// <param name="DateCount">Nombre de valeurs interprétables comme une date.</param>
/// <param name="DistinctValueCount">Nombre de valeurs distinctes observées dans l'échantillon.</param>
public sealed record ColumnStatistics(
    int SampledRowCount,
    int NonEmptyCount,
    int NumericCount,
    int DateCount,
    int DistinctValueCount)
{
    /// <summary>Statistiques d'une colonne pour laquelle aucun échantillon n'a été fourni.</summary>
    public static readonly ColumnStatistics Empty = new(0, 0, 0, 0, 0);

    /// <summary>Proportion de valeurs non vides dans l'échantillon (0 si aucune ligne échantillonnée).</summary>
    public double FillRatio => SampledRowCount == 0 ? 0 : (double)NonEmptyCount / SampledRowCount;

    /// <summary>Proportion de valeurs non vides interprétables comme un nombre.</summary>
    public double NumericRatio => NonEmptyCount == 0 ? 0 : (double)NumericCount / NonEmptyCount;

    /// <summary>Proportion de valeurs non vides interprétables comme une date.</summary>
    public double DateRatio => NonEmptyCount == 0 ? 0 : (double)DateCount / NonEmptyCount;

    /// <summary>Proportion de valeurs distinctes parmi les valeurs non vides (indicateur de cardinalité).</summary>
    public double DistinctRatio => NonEmptyCount == 0 ? 0 : (double)DistinctValueCount / NonEmptyCount;
}
