using OrbitaAI.Modules.ImportEngine.Domain;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure;

/// <summary>
/// Source de lecture adossée à un chemin de fichier sur disque, typique d'un déploiement
/// On-Premise (tech/DEPLOYMENT_STRATEGY.md §3). Ouvre le fichier en accès séquentiel exclusif à
/// la lecture, sans le charger en mémoire.
/// </summary>
public sealed class FileReaderSource : ReaderSource
{
    private readonly string _filePath;
    private readonly int _bufferSizeBytes;

    public FileReaderSource(string filePath, int bufferSizeBytes = 128 * 1024)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
        _bufferSizeBytes = bufferSizeBytes;
    }

    /// <inheritdoc />
    public override string DisplayName => Path.GetFileName(_filePath);

    /// <inheritdoc />
    public override long? Length => File.Exists(_filePath) ? new FileInfo(_filePath).Length : null;

    /// <inheritdoc />
    public override Stream OpenRead() =>
        new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            _bufferSizeBytes,
            FileOptions.SequentialScan | FileOptions.Asynchronous);
}
