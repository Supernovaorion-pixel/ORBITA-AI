namespace OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

/// <summary>
/// La source ne contient strictement aucun contenu (taille nulle), ce qui empêche même la
/// détermination de sa structure (en-tête, colonnes). Distincte d'un fichier structurellement
/// valide mais sans ligne de données (cf. features/EMPTY_STATES.md côté ux/) : ce dernier cas
/// n'est pas une erreur et se traduit simplement par un flux de <see cref="RawRow"/> vide.
/// </summary>
public sealed class EmptySourceException : ImportReaderException
{
    /// <summary>Nom de la source dépourvue de tout contenu.</summary>
    public string SourceDisplayName { get; }

    public EmptySourceException(string sourceDisplayName)
        : base($"La source '{sourceDisplayName}' est vide.")
    {
        SourceDisplayName = sourceDisplayName;
    }
}
