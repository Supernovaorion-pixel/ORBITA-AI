using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Historique » (docs/GLOSSARY.md).
/// État successif conservé d'une entité du Domaine (architecture/DOMAIN_MODEL.md §2,
/// architecture/DATA_FLOW.md §7). Conservation exclusivement en ajout (append-only,
/// cf. tech/DATABASE_STRATEGY.md §3). Squelette structurel uniquement.
/// </summary>
public sealed class HistoryEntry : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public DateTimeOffset RecordedAt { get; init; }
}
