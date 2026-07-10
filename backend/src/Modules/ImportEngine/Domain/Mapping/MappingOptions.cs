namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Options propres à une opération de correspondance donnée. Le dictionnaire de synonymes est
/// intégralement configurable : aucune Organisation cliente n'est contrainte au dictionnaire par
/// défaut (cf. <see cref="DefaultSynonymDictionary"/>).
/// </summary>
public sealed record MappingOptions
{
    /// <summary>
    /// Dictionnaire de colonnes canoniques et de synonymes à utiliser pour cette opération.
    /// Laissé à <see langword="null"/>, le dictionnaire par défaut (<see cref="DefaultSynonymDictionary"/>)
    /// est utilisé.
    /// </summary>
    public SynonymDictionary? SynonymDictionary { get; init; }

    /// <summary>
    /// Active le profilage de contenu (cf. <see cref="ColumnStatistics"/>) en complément de
    /// l'analyse du seul libellé d'en-tête. Désactivable pour une correspondance rapide fondée
    /// uniquement sur les en-têtes.
    /// </summary>
    public bool AnalyzeContent { get; init; } = true;

    /// <summary>Options par défaut.</summary>
    public static readonly MappingOptions Default = new();
}
