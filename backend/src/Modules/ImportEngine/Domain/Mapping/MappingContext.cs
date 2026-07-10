namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Regroupe les options et la configuration applicables à une opération de correspondance de
/// colonnes (cf. <see cref="ReaderContext"/> pour le principe équivalent côté lecture, dans un
/// souci de cohérence architecturale au sein du module).
/// </summary>
public sealed class MappingContext
{
    /// <summary>Options propres à cette opération.</summary>
    public MappingOptions Options { get; init; } = MappingOptions.Default;

    /// <summary>Configuration de seuils et de garde-fous applicable à cette opération.</summary>
    public MappingConfiguration Configuration { get; init; } = MappingConfiguration.Default;

    /// <summary>Dictionnaire effectivement utilisé : celui des <see cref="Options"/>, ou le dictionnaire par défaut.</summary>
    public SynonymDictionary EffectiveDictionary => Options.SynonymDictionary ?? DefaultSynonymDictionary.Create();
}
