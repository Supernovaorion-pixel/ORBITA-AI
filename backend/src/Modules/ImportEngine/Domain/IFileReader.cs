namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Contrat commun à tout lecteur de fichier du moteur de lecture (features/IMPORT_ENGINE.md §2-6).
/// Un lecteur ne réalise strictement que la lecture : aucun mapping de colonne, aucune validation,
/// aucune fusion, aucune analyse. Le résultat est un flux progressif de <see cref="RawRow"/>,
/// jamais chargé intégralement en mémoire, permettant un traitement par lots par les missions
/// ultérieures sans modification de ce contrat.
/// </summary>
public interface IFileReader
{
    /// <summary>Format de fichier pris en charge par ce lecteur.</summary>
    FileFormat Format { get; }

    /// <summary>
    /// Indique si ce lecteur est en mesure de lire <paramref name="source"/>, typiquement à
    /// partir de son extension ou de sa signature de fichier.
    /// </summary>
    bool CanRead(ReaderSource source);

    /// <summary>
    /// Lit progressivement les lignes de données de <paramref name="context"/>.Source. La
    /// progression et les statistiques de <paramref name="context"/> sont mises à jour au fil de
    /// l'énumération. L'opération est annulable à tout moment via <paramref name="cancellationToken"/>.
    /// </summary>
    IAsyncEnumerable<RawRow> ReadAsync(ReaderContext context, CancellationToken cancellationToken = default);
}
