namespace OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

/// <summary>
/// La taille de la source dépasse la limite de sécurité configurée
/// (cf. <see cref="ReaderConfiguration.MaxSourceSizeBytes"/>), garde-fou de mémoire et de
/// performance (architecture/PERFORMANCE_GUIDELINES.md).
/// </summary>
public sealed class SourceSizeExceededException : ImportReaderException
{
    /// <summary>Nom de la source ayant dépassé la limite.</summary>
    public string SourceDisplayName { get; }

    /// <summary>Taille réelle constatée de la source, en octets.</summary>
    public long ActualSizeBytes { get; }

    /// <summary>Taille maximale autorisée, en octets.</summary>
    public long MaxAllowedSizeBytes { get; }

    public SourceSizeExceededException(string sourceDisplayName, long actualSizeBytes, long maxAllowedSizeBytes)
        : base(
            $"La source '{sourceDisplayName}' ({actualSizeBytes} octets) dépasse la taille maximale autorisée " +
            $"({maxAllowedSizeBytes} octets).")
    {
        SourceDisplayName = sourceDisplayName;
        ActualSizeBytes = actualSizeBytes;
        MaxAllowedSizeBytes = maxAllowedSizeBytes;
    }
}
