namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Regroupe l'ensemble des éléments nécessaires à une exécution du Pipeline d'Import : la source à
/// importer, l'Organisation concernée, les options et la configuration applicables, ainsi que
/// l'état statistique et le journal d'audit de l'opération. Un <see cref="PipelineContext"/> est
/// propre à une seule exécution ; il n'est jamais réutilisé pour un import suivant (cf.
/// <c>ReaderContext</c>, <c>MappingContext</c>, <c>ValidationContext</c> pour le principe
/// équivalent, dans un souci de cohérence architecturale au sein du module).
/// </summary>
public sealed class PipelineContext
{
    /// <summary>Source à importer (fichier ou flux, cf. <see cref="ReaderSource"/>).</summary>
    public required ReaderSource Source { get; init; }

    /// <summary>
    /// Organisation cliente pour laquelle cet import est réalisé (cloisonnement par Organisation,
    /// architecture/EVENT_SYSTEM.md §6). Portée par tout événement émis pour cet import.
    /// </summary>
    public required Guid OrganizationId { get; init; }

    /// <summary>Identifiant unique de cette exécution du pipeline.</summary>
    public Guid ImportId { get; init; } = Guid.NewGuid();

    /// <summary>Nom lisible de la source, destiné aux messages de diagnostic et à l'historique.</summary>
    public string SourceName => Source.DisplayName;

    /// <summary>Options propres à cette exécution.</summary>
    public PipelineOptions Options { get; init; } = PipelineOptions.Default;

    /// <summary>Configuration de seuils et de garde-fous applicable à cette exécution.</summary>
    public PipelineConfiguration Configuration { get; init; } = PipelineConfiguration.Default;

    /// <summary>
    /// État statistique de cette exécution, mis à jour par le pipeline au fil de l'avancement et
    /// consultable par l'appelant à tout moment, y compris après l'achèvement.
    /// </summary>
    public PipelineStatistics Statistics { get; } = new();

    /// <summary>Journal d'audit de cette exécution (cf. <see cref="ImportAudit"/>).</summary>
    public ImportAudit Audit { get; } = new();
}
