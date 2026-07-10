namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Résultat exploitable d'une opération de correspondance de colonnes, destiné aux futures
/// missions (validation, fusion, analyse, ORION — cf. Application/Contracts/IMappedRowProcessor.cs)
/// qui doivent retrouver la position d'une colonne canonique dans les <c>RawRow.Values</c> du
/// Reader, sans jamais avoir à réinterpréter les libellés d'en-tête elles-mêmes.
/// </summary>
/// <param name="ColumnIndexByCanonicalKey">
/// Association entre la clé d'une colonne canonique et l'index de colonne source qui la porte,
/// restreinte aux seules colonnes au statut <see cref="ColumnRecognitionStatus.Recognized"/>
/// (une colonne ambiguë ou dupliquée n'y figure jamais, conformément à l'absence de correction
/// automatique cachée).
/// </param>
/// <param name="Report">Rapport intégral et traçable de l'opération (cf. <see cref="MappingReport"/>).</param>
public sealed record MappingResult(
    IReadOnlyDictionary<string, int> ColumnIndexByCanonicalKey,
    MappingReport Report);
