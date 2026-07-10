namespace OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

/// <summary>
/// Le contenu de la source est illisible ou structurellement invalide pour son format déclaré
/// (ex. classeur Excel tronqué, CSV mal formé), au-delà de ce qu'une tolérance raisonnable de
/// lecture peut absorber.
/// </summary>
public sealed class CorruptedSourceException : ImportReaderException
{
    /// <summary>Nom de la source dont le contenu s'est révélé illisible.</summary>
    public string SourceDisplayName { get; }

    public CorruptedSourceException(string sourceDisplayName, Exception innerException)
        : base($"Le contenu de la source '{sourceDisplayName}' est illisible ou corrompu.", innerException)
    {
        SourceDisplayName = sourceDisplayName;
    }
}
