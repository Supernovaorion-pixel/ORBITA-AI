namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Résout le lecteur (<see cref="IFileReader"/>) approprié pour une source donnée, sans que
/// l'appelant n'ait à connaître le format concret du fichier à l'avance
/// (features/IMPORT_ENGINE.md §2 — détection automatique).
/// </summary>
public interface IReaderFactory
{
    /// <summary>
    /// Retourne le lecteur capable de traiter <paramref name="source"/>.
    /// Lève <see cref="Exceptions.UnsupportedFileFormatException"/> si aucun lecteur enregistré
    /// n'est en mesure de la lire.
    /// </summary>
    IFileReader ResolveReader(ReaderSource source);
}
