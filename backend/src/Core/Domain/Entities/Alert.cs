using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Alerte » (docs/GLOSSARY.md).
/// Signalement généré lorsqu'un écart ou événement significatif est détecté
/// (architecture/DOMAIN_MODEL.md §2, features/ALERT_SYSTEM.md). Squelette structurel
/// uniquement — aucune règle de déclenchement, aucun calcul de seuil.
/// </summary>
public sealed class Alert : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public AlertPriority Priority { get; init; }

    public AlertStatus Status { get; init; }
}

/// <summary>Niveaux définis dans features/ALERT_SYSTEM.md §4.</summary>
public enum AlertPriority
{
    Informative,
    High,
    Critical
}

/// <summary>Cycle de vie défini dans features/ALERT_SYSTEM.md §6-7.</summary>
public enum AlertStatus
{
    Active,
    Acknowledged,
    Archived
}
