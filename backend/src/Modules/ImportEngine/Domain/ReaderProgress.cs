namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Instantané de l'avancement d'une lecture en cours, notifié périodiquement via
/// <c>IProgress&lt;ReaderProgress&gt;</c> (cf. <see cref="ReaderContext.Progress"/>) afin que
/// l'interface utilisateur ne soit jamais bloquée pendant la lecture d'un fichier volumineux
/// (ux/LOADING_STATES.md §3).
/// </summary>
/// <param name="RowsRead">Nombre de lignes de données déjà lues au moment de cet instantané.</param>
/// <param name="BytesRead">Nombre d'octets déjà consommés dans le flux source, si mesurable.</param>
/// <param name="TotalBytes">Taille totale de la source en octets, si connue à l'avance.</param>
/// <param name="Elapsed">Durée écoulée depuis le début de la lecture.</param>
public sealed record ReaderProgress(long RowsRead, long? BytesRead, long? TotalBytes, TimeSpan Elapsed)
{
    /// <summary>
    /// Pourcentage d'avancement estimé, fondé sur la proportion d'octets déjà lus, lorsque
    /// <see cref="BytesRead"/> et <see cref="TotalBytes"/> sont tous deux disponibles ;
    /// <see langword="null"/> sinon (ex. taille totale non déterminable à l'avance).
    /// </summary>
    public double? PercentComplete =>
        BytesRead.HasValue && TotalBytes is > 0
            ? Math.Clamp(BytesRead.Value * 100d / TotalBytes.Value, 0, 100)
            : null;
}
