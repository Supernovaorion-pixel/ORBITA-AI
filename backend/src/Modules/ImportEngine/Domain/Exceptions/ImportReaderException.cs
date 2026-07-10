namespace OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

/// <summary>
/// Base commune des exceptions du moteur de lecture. Distincte des exceptions de Domaine du
/// Core (<c>OrbitaAI.Core.Domain.Exceptions.DomainException</c>) : une erreur de lecture de
/// fichier n'est pas la violation d'un invariant métier, mais un aléa d'entrée/sortie ou de
/// format de fichier (architecture/ERROR_HANDLING.md §2 — erreur d'infrastructure et erreur
/// de donnée).
/// </summary>
public abstract class ImportReaderException : Exception
{
    protected ImportReaderException(string message) : base(message)
    {
    }

    protected ImportReaderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
