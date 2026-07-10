using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Résumé quantitatif d'un import, produit automatiquement à l'issue de chaque exécution du
/// Pipeline d'Import (features/IMPORT_ENGINE.md §13-14). Destiné à la restitution à l'utilisateur
/// (rapport d'import, historique) — pour le détail technique interne, cf. <see cref="ImportMetrics"/>.
/// </summary>
/// <param name="TotalRows">Nombre total de lignes de données lues.</param>
/// <param name="ImportedRows">Nombre de lignes ayant poursuivi le pipeline.</param>
/// <param name="RejectedRows">Nombre de lignes mises en quarantaine.</param>
/// <param name="RowsWithWarnings">Nombre de lignes importées mais accompagnées d'au moins un avertissement.</param>
/// <param name="ExecutionTime">Durée totale de l'exécution.</param>
/// <param name="ThroughputRowsPerSecond">Débit moyen de traitement, en lignes par seconde.</param>
/// <param name="DataQualityScore">
/// Indicateur de qualité synthétique, sur 100 : proportion de lignes importées sans la moindre
/// anomalie (features/IMPORT_ENGINE.md §13).
/// </param>
/// <param name="ErrorBreakdown">Répartition du nombre de lignes rejetées par catégorie dominante.</param>
public sealed record ImportSummary(
    long TotalRows,
    long ImportedRows,
    long RejectedRows,
    long RowsWithWarnings,
    TimeSpan ExecutionTime,
    double ThroughputRowsPerSecond,
    double DataQualityScore,
    IReadOnlyDictionary<RejectedRowCategory, int> ErrorBreakdown);
