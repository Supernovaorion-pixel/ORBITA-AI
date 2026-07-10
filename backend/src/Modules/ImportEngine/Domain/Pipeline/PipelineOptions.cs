using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Options propres à une exécution donnée du Pipeline d'Import, regroupant les options et
/// configurations de chacun des moteurs qu'il relie (Reader, Mapping, Validation), sans qu'aucune
/// d'elles ne soit codée en dur (features/IMPORT_ENGINE.md).
/// </summary>
public sealed record PipelineOptions
{
    /// <summary>Options propres à la lecture du fichier source.</summary>
    public ReaderOptions Reader { get; init; } = ReaderOptions.Default;

    /// <summary>Options propres à la correspondance des colonnes.</summary>
    public MappingOptions Mapping { get; init; } = MappingOptions.Default;

    /// <summary>Configuration de seuils applicable à la correspondance des colonnes.</summary>
    public MappingConfiguration MappingConfiguration { get; init; } = MappingConfiguration.Default;

    /// <summary>Options propres à la validation de chaque ligne.</summary>
    public ValidationOptions Validation { get; init; } = ValidationOptions.Default;

    /// <summary>Configuration de seuils applicable à la validation de chaque ligne.</summary>
    public ValidationConfiguration ValidationConfiguration { get; init; } = ValidationConfiguration.Default;

    /// <summary>Options par défaut, adaptées à la très large majorité des imports.</summary>
    public static readonly PipelineOptions Default = new();
}
