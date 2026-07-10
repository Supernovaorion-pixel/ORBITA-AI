using OrbitaAI.Modules.ImportEngine.Domain;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure;

/// <summary>
/// Source de lecture adossée à un flux déjà ouvert, typique d'un contenu téléversé en Cloud
/// (tech/DEPLOYMENT_STRATEGY.md §2). Le flux fourni est considéré comme dédié à cette seule
/// opération de lecture : il est fermé par le lecteur à l'issue de la lecture, au même titre
/// qu'un flux ouvert par <see cref="FileReaderSource"/>. Un appelant souhaitant réutiliser le
/// flux au-delà de cette lecture doit fournir une copie dédiée.
/// </summary>
public sealed class StreamReaderSource : ReaderSource
{
    private readonly Stream _stream;

    public StreamReaderSource(Stream stream, string displayName)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        _stream = stream;
        DisplayName = displayName;
    }

    /// <inheritdoc />
    public override string DisplayName { get; }

    /// <inheritdoc />
    public override long? Length => _stream.CanSeek ? _stream.Length : null;

    /// <inheritdoc />
    public override Stream OpenRead() => _stream;
}
