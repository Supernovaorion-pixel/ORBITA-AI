namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Abstraction de la provenance d'un fichier à lire : chemin sur disque (mode On-Premise,
/// tech/DEPLOYMENT_STRATEGY.md §3) ou flux déjà ouvert (ex. téléversement Cloud,
/// tech/DEPLOYMENT_STRATEGY.md §2). Le moteur de lecture ne dépend d'aucune de ces
/// origines en particulier : il ne consomme que le flux qu'expose cette abstraction.
/// </summary>
public abstract class ReaderSource
{
    /// <summary>Nom lisible de la source, destiné aux messages de diagnostic et aux statistiques.</summary>
    public abstract string DisplayName { get; }

    /// <summary>
    /// Taille totale de la source en octets, lorsqu'elle est connue à l'avance (utilisée pour
    /// exprimer la progression de lecture en pourcentage, cf. <see cref="ReaderProgress"/>).
    /// </summary>
    public abstract long? Length { get; }

    /// <summary>
    /// Ouvre un flux de lecture séquentiel sur la source. L'appelant (le lecteur) est
    /// responsable de la fermeture du flux retourné.
    /// </summary>
    public abstract Stream OpenRead();
}
