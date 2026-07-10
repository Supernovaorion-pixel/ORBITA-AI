using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

namespace OrbitaAI.Modules.ImportEngine.Application;

/// <summary>
/// Implémentation par défaut de <see cref="IReaderFactory"/> : sélectionne, parmi les lecteurs
/// enregistrés, le premier capable de lire la source fournie (features/IMPORT_ENGINE.md §2 —
/// détection automatique). L'ajout d'un futur format de lecture se traduit uniquement par
/// l'enregistrement d'un nouveau <see cref="IFileReader"/>, sans modification de cette classe
/// (principe ouvert/fermé, architecture/CODING_PRINCIPLES.md §1).
/// </summary>
public sealed class ReaderFactory : IReaderFactory
{
    private readonly IReadOnlyList<IFileReader> _readers;

    public ReaderFactory(IEnumerable<IFileReader> readers)
    {
        ArgumentNullException.ThrowIfNull(readers);
        _readers = readers.ToArray();
    }

    /// <inheritdoc />
    public IFileReader ResolveReader(ReaderSource source)
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var reader in _readers)
        {
            if (reader.CanRead(source))
            {
                return reader;
            }
        }

        throw new UnsupportedFileFormatException(source.DisplayName);
    }
}
