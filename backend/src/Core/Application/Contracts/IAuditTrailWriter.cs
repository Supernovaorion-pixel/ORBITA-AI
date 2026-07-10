namespace OrbitaAI.Core.Application.Contracts;

/// <summary>
/// Contrat d'enregistrement d'une <see cref="AuditEntry"/> dans le journal d'audit
/// (architecture/LOGGING_STRATEGY.md §3, features/AUDIT_AND_HISTORY.md §4). Toute action
/// significative affectant les données ou la configuration d'une Organisation transite par ce
/// contrat. Aucune implémentation n'est fournie dans le Core : le stockage immuable réel de
/// l'audit (tech/DATABASE_STRATEGY.md) est hors du périmètre de cette mission.
/// </summary>
public interface IAuditTrailWriter
{
    /// <summary>Enregistre une entrée d'audit de façon irréversible.</summary>
    Task RecordAsync(AuditEntry entry, CancellationToken cancellationToken = default);
}
