namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Correspondance envisagée entre une colonne source et une colonne canonique, avec son score
/// de confiance et les raisons déterministes qui l'expliquent (exigence d'explicabilité —
/// aucune décision de reconnaissance n'est une boîte noire).
/// </summary>
/// <param name="CanonicalKey">Clé de la colonne canonique envisagée (cf. <see cref="CanonicalColumnDefinition.Key"/>).</param>
/// <param name="ConfidenceScore">Score de confiance, de 0 à 100.</param>
/// <param name="Reasons">Explications déterministes ayant conduit à ce score, dans l'ordre où elles ont été appliquées.</param>
public sealed record ColumnMappingCandidate(string CanonicalKey, double ConfidenceScore, IReadOnlyList<string> Reasons);
