namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Portrait complet d'une colonne source à un instant donné de l'analyse : sa position, son
/// libellé brut tel que lu par le Reader, et son profil de contenu (cf. <see cref="ColumnStatistics"/>).
/// Chaque instance est indépendante des autres colonnes du même fichier, ce qui permet leur
/// analyse en parallèle par une future mission sans modification de ce type
/// (architecture/PERFORMANCE_GUIDELINES.md — préparation du traitement par lots/parallèle).
/// </summary>
/// <param name="ColumnIndex">Position de la colonne dans la ligne source, à partir de 0.</param>
/// <param name="RawHeader">Libellé brut de la colonne tel qu'il apparaît dans le fichier.</param>
/// <param name="Statistics">Profil de contenu de la colonne (cf. <see cref="ColumnStatistics"/>).</param>
public sealed record ColumnProfile(int ColumnIndex, string RawHeader, ColumnStatistics Statistics);
