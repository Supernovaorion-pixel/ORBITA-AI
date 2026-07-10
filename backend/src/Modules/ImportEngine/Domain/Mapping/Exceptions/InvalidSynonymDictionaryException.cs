namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping.Exceptions;

/// <summary>Le dictionnaire de synonymes fourni est structurellement invalide (ex. clé canonique dupliquée).</summary>
public sealed class InvalidSynonymDictionaryException : MappingException
{
    public InvalidSynonymDictionaryException(string message) : base(message)
    {
    }
}
