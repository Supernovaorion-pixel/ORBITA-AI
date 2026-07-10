using System.Diagnostics;

namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// État cumulatif du déroulement d'une opération de validation, mis à jour en continu pendant le
/// parcours du pipeline (cf. <see cref="ValidationContext.Statistics"/>), au même titre que
/// <c>ReaderStatistics</c> côté lecture — cohérence architecturale au sein du module. Porte sur
/// le déroulement du traitement (lignes parcourues, débit), jamais sur le contenu des constats
/// (cf. <see cref="ValidationSummary"/> pour ce dernier).
/// </summary>
public sealed class ValidationStatistics
{
    private long _rowsProcessed;
    private readonly Stopwatch _stopwatch = new();

    /// <summary>Nombre de lignes de données déjà parcourues.</summary>
    public long RowsProcessed => Interlocked.Read(ref _rowsProcessed);

    /// <summary>Instant de démarrage de la validation.</summary>
    public DateTimeOffset? StartedAt { get; private set; }

    /// <summary>Instant d'achèvement de la validation.</summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>Indique si la validation est arrivée à son terme.</summary>
    public bool IsCompleted => CompletedAt.HasValue;

    /// <summary>Durée écoulée depuis le démarrage de la validation.</summary>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    /// <summary>Débit moyen de validation, en lignes par seconde.</summary>
    public double RowsPerSecond => Elapsed.TotalSeconds > 0 ? RowsProcessed / Elapsed.TotalSeconds : 0;

    internal void MarkStarted()
    {
        StartedAt = DateTimeOffset.UtcNow;
        _stopwatch.Restart();
    }

    internal void IncrementRowsProcessed() => Interlocked.Increment(ref _rowsProcessed);

    internal void MarkCompleted()
    {
        _stopwatch.Stop();
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
