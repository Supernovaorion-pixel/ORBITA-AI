namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping.Exceptions;

/// <summary>
/// Base commune des exceptions du moteur de correspondance de colonnes. Distincte des
/// exceptions du Reader (<c>Domain.Exceptions.ImportReaderException</c>) : une erreur de
/// correspondance ne relève ni de la lecture d'un fichier, ni d'un invariant du Domaine partagé
/// (Core), mais d'une impossibilité à interpréter ou à configurer la reconnaissance des colonnes.
/// </summary>
public abstract class MappingException : Exception
{
    protected MappingException(string message) : base(message)
    {
    }
}
