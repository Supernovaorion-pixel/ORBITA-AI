namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Normalise un libellé d'en-tête brut à des fins de comparaison structurelle (casse, accents,
/// ponctuation, espacement) — jamais à des fins de nettoyage de la donnée métier elle-même
/// (le libellé brut d'origine n'est jamais modifié dans les résultats restitués).
/// </summary>
public interface IHeaderNormalizer
{
    /// <summary>Retourne la forme normalisée de <paramref name="rawHeader"/>, utilisée pour la comparaison.</summary>
    string Normalize(string rawHeader);
}
