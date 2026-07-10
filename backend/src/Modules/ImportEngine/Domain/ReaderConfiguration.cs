namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Configuration par défaut du moteur de lecture, applicable à l'ensemble des opérations de
/// lecture d'une instance donnée (par opposition à <see cref="ReaderOptions"/>, propre à chaque
/// opération). Fixe les garde-fous de performance et de sécurité exigés par
/// tech/PERFORMANCE_TARGETS.md et architecture/PERFORMANCE_GUIDELINES.md, notamment pour les
/// fichiers de taille industrielle (plusieurs millions de lignes).
/// </summary>
public sealed record ReaderConfiguration
{
    /// <summary>
    /// Taille de mémoire tampon utilisée pour la lecture séquentielle du flux source, en octets.
    /// Une valeur plus élevée réduit le nombre d'accès disque au prix d'une empreinte mémoire
    /// légèrement accrue.
    /// </summary>
    public int StreamBufferSizeBytes { get; init; } = 128 * 1024;

    /// <summary>
    /// Taille maximale de fichier acceptée, en octets, au-delà de laquelle la lecture est
    /// refusée avant même de commencer (cf. <see cref="Exceptions.SourceSizeExceededException"/>).
    /// Constitue un garde-fou de sécurité et de mémoire, pas une limite fonctionnelle métier.
    /// </summary>
    public long MaxSourceSizeBytes { get; init; } = 5L * 1024 * 1024 * 1024; // 5 Go

    /// <summary>
    /// Nombre de lignes lues entre deux notifications de progression
    /// (cf. <see cref="ReaderProgress"/>), afin de limiter le coût de la notification elle-même
    /// sur des fichiers de plusieurs millions de lignes.
    /// </summary>
    public int ProgressReportIntervalRows { get; init; } = 1_000;

    /// <summary>Configuration par défaut, adaptée à la très large majorité des Organisations clientes.</summary>
    public static readonly ReaderConfiguration Default = new();
}
