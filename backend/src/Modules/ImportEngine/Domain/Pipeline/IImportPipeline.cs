namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Point d'entrée du Pipeline d'Import (features/IMPORT_ENGINE.md). Relie le Reader, le Mapping
/// Engine, le Validation Engine et le Quarantine Engine en un traitement continu, sans jamais
/// fusionner, calculer ou analyser aucune donnée métier (hors périmètre de cette mission — cf.
/// architecture/DATA_FLOW.md §3-4, développés lors de missions ultérieures).
/// </summary>
public interface IImportPipeline
{
    /// <summary>
    /// Exécute intégralement l'import décrit par <paramref name="context"/> et retourne son
    /// résultat complet, une fois la source entièrement traitée (ou l'exécution suspendue,
    /// cf. <see cref="PipelineState.Halted"/>). Notifie périodiquement <paramref name="progress"/>,
    /// si fourni, sans jamais bloquer le traitement dans l'attente de sa consommation.
    /// </summary>
    Task<PipelineResult> RunAsync(
        PipelineContext context,
        IProgress<PipelineProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
