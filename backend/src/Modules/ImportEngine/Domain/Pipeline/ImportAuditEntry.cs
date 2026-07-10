namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Une entrée individuelle et horodatée du journal d'audit d'un import (cf. <see cref="ImportAudit"/>),
/// conformément à l'exigence de traçabilité intégrale de toute décision affectant l'import
/// (architecture/ERROR_HANDLING.md §6, architecture/LOGGING_STRATEGY.md).
/// </summary>
/// <param name="Timestamp">Instant auquel cette entrée a été journalisée.</param>
/// <param name="Action">Nature de l'action journalisée (ex. "Started", "Completed", "Cancelled", "Failed").</param>
/// <param name="Description">Description factuelle et compréhensible de l'action journalisée.</param>
public sealed record ImportAuditEntry(DateTimeOffset Timestamp, string Action, string Description);
