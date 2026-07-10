namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Résultat complet de l'analyse d'une colonne source unique : son statut, le candidat retenu le
/// cas échéant, et l'ensemble des candidats envisagés (utile pour comprendre une ambiguïté).
/// </summary>
/// <param name="ColumnIndex">Position de la colonne dans la ligne source, à partir de 0.</param>
/// <param name="RawHeader">Libellé brut de la colonne tel qu'il apparaît dans le fichier.</param>
/// <param name="Status">Issue de la reconnaissance (cf. <see cref="ColumnRecognitionStatus"/>).</param>
/// <param name="SelectedCandidate">
/// Candidat retenu lorsque <paramref name="Status"/> vaut <see cref="ColumnRecognitionStatus.Recognized"/> ;
/// <see langword="null"/> sinon (aucune correction ou choix automatique caché en cas d'ambiguïté).
/// </param>
/// <param name="AllCandidates">Tous les candidats envisagés pour cette colonne, du meilleur au moins bon.</param>
public sealed record ColumnMappingOutcome(
    int ColumnIndex,
    string RawHeader,
    ColumnRecognitionStatus Status,
    ColumnMappingCandidate? SelectedCandidate,
    IReadOnlyList<ColumnMappingCandidate> AllCandidates);
