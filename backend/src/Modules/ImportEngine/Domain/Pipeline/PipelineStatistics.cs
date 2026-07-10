using System.Diagnostics;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// État cumulatif d'une exécution du Pipeline d'Import, mis à jour en continu pendant le
/// traitement et consultable aussi bien en cours (supervision, <see cref="PipelineProgress"/>)
/// qu'une fois le traitement terminé (rapport final, cf. <see cref="ImportSummary"/>). Les
/// compteurs sont thread-safe (architecture/PERFORMANCE_GUIDELINES.md — compatibilité avec une
/// parallélisation future du traitement des lignes) ; les mutations sont réservées au Pipeline
/// lui-même (accesseurs internes).
/// </summary>
public sealed class PipelineStatistics
{
    private long _rowsRead;
    private long _rowsAccepted;
    private long _rowsRejected;
    private long _rowsWithWarnings;
    private readonly Stopwatch _stopwatch = new();

    /// <summary>Étape courante du pipeline.</summary>
    public PipelineState State { get; private set; } = PipelineState.NotStarted;

    /// <summary>Nombre total de lignes de données lues jusqu'à présent.</summary>
    public long RowsRead => Interlocked.Read(ref _rowsRead);

    /// <summary>Nombre de lignes ayant poursuivi le pipeline (non mises en quarantaine).</summary>
    public long RowsAccepted => Interlocked.Read(ref _rowsAccepted);

    /// <summary>Nombre de lignes mises en quarantaine.</summary>
    public long RowsRejected => Interlocked.Read(ref _rowsRejected);

    /// <summary>Nombre de lignes acceptées mais accompagnées d'au moins un avertissement.</summary>
    public long RowsWithWarnings => Interlocked.Read(ref _rowsWithWarnings);

    /// <summary>Instant de démarrage du traitement, une fois celui-ci engagé.</summary>
    public DateTimeOffset? StartedAt { get; private set; }

    /// <summary>Instant d'achèvement du traitement, une fois celui-ci terminé (quelle qu'en soit l'issue).</summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>Indique si le traitement est arrivé à son terme (quelle qu'en soit l'issue).</summary>
    public bool IsCompleted => CompletedAt.HasValue;

    /// <summary>Durée écoulée depuis le démarrage du traitement (figée une fois celui-ci terminé).</summary>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    /// <summary>Débit moyen de traitement, en lignes par seconde.</summary>
    public double RowsPerSecond => Elapsed.TotalSeconds > 0 ? RowsRead / Elapsed.TotalSeconds : 0;

    internal void MarkStarted()
    {
        StartedAt = DateTimeOffset.UtcNow;
        _stopwatch.Restart();
        State = PipelineState.Reading;
    }

    internal void TransitionTo(PipelineState state) => State = state;

    internal void IncrementRowsRead(long count = 1) => Interlocked.Add(ref _rowsRead, count);

    internal void IncrementRowsAccepted() => Interlocked.Increment(ref _rowsAccepted);

    internal void IncrementRowsRejected() => Interlocked.Increment(ref _rowsRejected);

    internal void IncrementRowsWithWarnings() => Interlocked.Increment(ref _rowsWithWarnings);

    internal void MarkCompleted(PipelineState finalState)
    {
        _stopwatch.Stop();
        CompletedAt = DateTimeOffset.UtcNow;
        State = finalState;
    }
}
