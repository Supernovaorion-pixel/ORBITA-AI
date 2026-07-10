using System.Diagnostics;

namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// État cumulatif d'une opération de lecture, mis à jour en continu pendant la lecture et
/// consultable aussi bien en cours (supervision) qu'une fois la lecture terminée (rapport final,
/// features/IMPORT_ENGINE.md §14). Une même instance, exposée par <see cref="ReaderContext"/>,
/// sert les deux usages sans dupliquer l'état entre un objet de progression et un objet de bilan.
/// Les mutations sont réservées au moteur de lecture lui-même (accesseurs internes) ; l'appelant
/// ne fait que consulter l'état.
/// </summary>
public sealed class ReaderStatistics
{
    private long _rowsRead;
    private readonly Stopwatch _stopwatch = new();

    /// <summary>Nombre de lignes de données lues jusqu'à présent.</summary>
    public long RowsRead => Interlocked.Read(ref _rowsRead);

    /// <summary>Nombre d'octets déjà consommés dans le flux source, si mesurable.</summary>
    public long? BytesRead { get; private set; }

    /// <summary>Instant de démarrage de la lecture, une fois celle-ci engagée.</summary>
    public DateTimeOffset? StartedAt { get; private set; }

    /// <summary>Instant d'achèvement de la lecture, une fois celle-ci terminée (avec ou sans erreur).</summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>Indique si la lecture est arrivée à son terme (avec ou sans erreur).</summary>
    public bool IsCompleted => CompletedAt.HasValue;

    /// <summary>Durée écoulée depuis le démarrage de la lecture (figée une fois celle-ci terminée).</summary>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    /// <summary>Débit moyen de lecture, en lignes par seconde.</summary>
    public double RowsPerSecond => Elapsed.TotalSeconds > 0 ? RowsRead / Elapsed.TotalSeconds : 0;

    internal void MarkStarted()
    {
        StartedAt = DateTimeOffset.UtcNow;
        _stopwatch.Restart();
    }

    internal void IncrementRowsRead(long count = 1) => Interlocked.Add(ref _rowsRead, count);

    internal void ReportBytesRead(long bytesRead) => BytesRead = bytesRead;

    internal void MarkCompleted()
    {
        _stopwatch.Stop();
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
