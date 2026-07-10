namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Instantané de progression d'une exécution du Pipeline d'Import, restitué périodiquement à
/// l'appelant (cf. <see cref="PipelineConfiguration.ProgressReportIntervalRows"/>) via
/// <see cref="IProgress{T}"/>, sans jamais imposer d'attente au traitement lui-même.
/// </summary>
/// <param name="State">Étape courante du pipeline au moment de l'instantané.</param>
/// <param name="RowsRead">Nombre total de lignes lues jusqu'à présent.</param>
/// <param name="RowsAccepted">Nombre de lignes ayant poursuivi le pipeline jusqu'à présent.</param>
/// <param name="RowsRejected">Nombre de lignes mises en quarantaine jusqu'à présent.</param>
/// <param name="Elapsed">Durée écoulée depuis le démarrage du traitement.</param>
public sealed record PipelineProgress(
    PipelineState State,
    long RowsRead,
    long RowsAccepted,
    long RowsRejected,
    TimeSpan Elapsed);
