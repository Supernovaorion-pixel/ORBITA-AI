namespace OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

/// <summary>Aucun lecteur enregistré n'est en mesure de lire la source fournie.</summary>
public sealed class UnsupportedFileFormatException : ImportReaderException
{
    /// <summary>Nom de la source dont le format n'a pas pu être déterminé ou pris en charge.</summary>
    public string SourceDisplayName { get; }

    public UnsupportedFileFormatException(string sourceDisplayName)
        : base($"Aucun lecteur ne prend en charge le format de la source '{sourceDisplayName}'.")
    {
        SourceDisplayName = sourceDisplayName;
    }
}
