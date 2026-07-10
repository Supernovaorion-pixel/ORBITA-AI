namespace OrbitaAI.Core.Application.Contracts;

/// <summary>
/// Entrée d'audit répondant aux trois questions exigées par architecture/LOGGING_STRATEGY.md §3 :
/// qui, quoi, quand. Immuable par construction, conformément à l'exigence de non-altération
/// des journaux d'audit (architecture/LOGGING_STRATEGY.md §3).
/// </summary>
/// <param name="OrganizationId">Organisation dans le périmètre de laquelle l'action a eu lieu.</param>
/// <param name="UserId">Utilisateur ayant réalisé l'action (qui).</param>
/// <param name="Action">Nature de l'action réalisée (quoi), ex. "User.RoleChanged".</param>
/// <param name="EntityType">Type de l'entité concernée par l'action, le cas échéant.</param>
/// <param name="EntityId">Identifiant de l'entité concernée par l'action, le cas échéant.</param>
/// <param name="OccurredAt">Instant auquel l'action a eu lieu (quand).</param>
public sealed record AuditEntry(
    Guid OrganizationId,
    Guid UserId,
    string Action,
    string? EntityType,
    string? EntityId,
    DateTimeOffset OccurredAt);
