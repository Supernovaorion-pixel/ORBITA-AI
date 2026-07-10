using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Résultat complet d'une exécution du Pipeline d'Import, restitué une fois le flux entièrement
/// parcouru (ou l'exécution suspendue, cf. <see cref="PipelineState.Halted"/>). Regroupe tout ce
/// qui doit rester traçable d'un import : correspondance des colonnes, résumé et mesures,
/// lignes mises en quarantaine, décisions prises et journal d'audit.
/// </summary>
/// <param name="State">Issue de cette exécution.</param>
/// <param name="Mapping">
/// Résultat de correspondance des colonnes, si celui-ci a pu être établi (absent uniquement en
/// cas d'échec avant même l'analyse de correspondance, cf. <see cref="PipelineState.Failed"/>).
/// </param>
/// <param name="Summary">Résumé quantitatif de l'import, destiné à l'utilisateur.</param>
/// <param name="Metrics">Mesures techniques détaillées de l'import.</param>
/// <param name="RejectedRows">Ensemble des lignes mises en quarantaine.</param>
/// <param name="Decisions">Décisions traçables prises au cours de l'exécution.</param>
/// <param name="Audit">Journal d'audit complet de cette exécution.</param>
/// <param name="History">Synthèse de cet import destinée à l'historique.</param>
/// <param name="Statistics">État statistique final de cette exécution.</param>
public sealed record PipelineResult(
    PipelineState State,
    MappingResult? Mapping,
    ImportSummary Summary,
    ImportMetrics Metrics,
    RejectedRowCollection RejectedRows,
    IReadOnlyList<PipelineDecision> Decisions,
    ImportAudit Audit,
    ImportHistoryEntry History,
    PipelineStatistics Statistics);
