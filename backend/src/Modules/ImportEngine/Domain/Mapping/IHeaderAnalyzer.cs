namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Détermine, pour un libellé d'en-tête donné, les colonnes canoniques envisageables et leur
/// score de confiance fondé exclusivement sur le nom (sans considération du contenu de la
/// colonne — cf. <see cref="IConfidenceEngine"/> pour la prise en compte du contenu).
/// Repose uniquement sur des règles déterministes de comparaison de texte (aucune IA).
/// </summary>
public interface IHeaderAnalyzer
{
    /// <summary>
    /// Retourne, triés du meilleur au moins bon, les candidats dont le score atteint au moins
    /// <see cref="MappingConfiguration.AmbiguousThreshold"/>. Une liste vide signifie qu'aucune
    /// colonne canonique connue ne peut être rapprochée de ce libellé.
    /// </summary>
    IReadOnlyList<ColumnMappingCandidate> AnalyzeHeader(string rawHeader, SynonymDictionary dictionary, MappingConfiguration configuration);
}
