namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping.Exceptions;

/// <summary>
/// Le moteur de correspondance a été sollicité sur des lignes dépourvues de toute ligne d'en-tête
/// (cf. <c>ReaderOptions.HasHeaderRow</c> côté Reader) : la reconnaissance de colonnes repose sur
/// les libellés d'en-tête et ne peut opérer de façon fiable sans eux.
/// </summary>
public sealed class NoHeaderRowException : MappingException
{
    public NoHeaderRowException()
        : base("Impossible d'identifier les colonnes : aucune ligne d'en-tête n'est disponible.")
    {
    }
}
