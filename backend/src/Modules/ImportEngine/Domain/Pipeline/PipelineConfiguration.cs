using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Configuration par défaut du Pipeline d'Import : garde-fous de performance et seuil de
/// quarantaine, applicables à toute exécution (par opposition à <see cref="PipelineOptions"/>,
/// propre à chaque appel).
/// </summary>
public sealed record PipelineConfiguration
{
    /// <summary>
    /// Nombre de lignes de tête utilisées comme échantillon borné pour la correspondance des
    /// colonnes (cf. <c>IMappingEngine.Analyze</c>), quelle que soit la taille réelle du fichier —
    /// garde-fou garantissant des performances constantes (architecture/PERFORMANCE_GUIDELINES.md).
    /// </summary>
    public int MappingSampleSize { get; init; } = 1_000;

    /// <summary>
    /// Sévérité minimale d'un constat de validation à partir de laquelle la ligne concernée est
    /// mise en quarantaine plutôt que poursuivre le pipeline. Distincte du seuil de blocage global
    /// de l'import (cf. <see cref="ValidationConfiguration.BlockingSeverityThreshold"/>), qui
    /// détermine si l'import dans son ensemble peut se poursuivre.
    /// </summary>
    public ValidationSeverity QuarantineSeverityThreshold { get; init; } = ValidationSeverity.Error;

    /// <summary>
    /// Nombre de lignes traitées entre deux notifications de progression, afin de limiter le coût
    /// de la notification elle-même sur des fichiers de plusieurs millions de lignes.
    /// </summary>
    public int ProgressReportIntervalRows { get; init; } = 1_000;

    /// <summary>Configuration par défaut, adaptée à la très large majorité des imports.</summary>
    public static readonly PipelineConfiguration Default = new();
}
