namespace OrbitaAI.Core.Domain.Events;

/// <summary>
/// Contrat marqueur de tout événement de Domaine circulant via le système d'événements
/// (architecture/EVENT_SYSTEM.md). Un événement décrit un fait passé, jamais une instruction
/// (architecture/EVENT_SYSTEM.md §3). Squelette structurel uniquement — aucune règle
/// métier, aucun contenu d'événement concret défini à ce stade.
/// </summary>
public interface IDomainEvent
{
    Guid OrganizationId { get; }

    DateTimeOffset OccurredAt { get; }
}
