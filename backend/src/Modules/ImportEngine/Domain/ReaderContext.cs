namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Regroupe l'ensemble des éléments nécessaires à une opération de lecture : la source à lire,
/// les options qui la régissent, la configuration de performance applicable, un récepteur de
/// progression optionnel, et l'état statistique de l'opération. Un <see cref="ReaderContext"/>
/// est propre à une seule opération de lecture ; il n'est jamais réutilisé pour une lecture suivante.
/// </summary>
public sealed class ReaderContext
{
    /// <summary>Source à lire (fichier ou flux, cf. <see cref="ReaderSource"/>).</summary>
    public required ReaderSource Source { get; init; }

    /// <summary>Options propres à cette lecture.</summary>
    public ReaderOptions Options { get; init; } = ReaderOptions.Default;

    /// <summary>Configuration de performance applicable à cette lecture.</summary>
    public ReaderConfiguration Configuration { get; init; } = ReaderConfiguration.Default;

    /// <summary>
    /// Récepteur optionnel des instantanés de progression périodiques
    /// (cf. <see cref="ReaderConfiguration.ProgressReportIntervalRows"/>).
    /// </summary>
    public IProgress<ReaderProgress>? Progress { get; init; }

    /// <summary>
    /// État statistique de cette opération de lecture, mis à jour par le moteur de lecture au
    /// fil de l'avancement et consultable par l'appelant à tout moment, y compris après
    /// l'achèvement de la lecture.
    /// </summary>
    public ReaderStatistics Statistics { get; } = new();
}
