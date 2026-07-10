namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Synthèse d'un import réalisé, destinée à alimenter l'historique des imports consultable depuis
/// l'écran Import (features/IMPORT_ENGINE.md §7) via le module History
/// (architecture/DATA_FLOW.md §7), auquel cette synthèse est communiquée exclusivement par
/// événement (cf. <see cref="ImportPipelineCompletedEvent"/>, architecture/EVENT_SYSTEM.md) —
/// jamais par appel direct, conformément à l'indépendance du Pipeline d'Import vis-à-vis du module
/// History (architecture/MODULE_DEPENDENCIES.md).
/// </summary>
/// <param name="ImportId">Identifiant unique de cet import.</param>
/// <param name="OccurredAt">Instant auquel cet import s'est achevé.</param>
/// <param name="SourceName">Nom lisible de la source importée.</param>
/// <param name="Status">Issue de cet import.</param>
/// <param name="Summary">Résumé quantitatif de cet import.</param>
public sealed record ImportHistoryEntry(
    Guid ImportId,
    DateTimeOffset OccurredAt,
    string SourceName,
    PipelineState Status,
    ImportSummary Summary);
