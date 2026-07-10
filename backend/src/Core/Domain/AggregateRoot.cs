using OrbitaAI.Core.Domain.Events;

namespace OrbitaAI.Core.Domain;

/// <summary>
/// Racine d'agrégat : entité qui constitue le point d'entrée cohérent d'un ensemble d'entités
/// et de Value Objects, et qui accumule les événements de Domaine qu'elle produit jusqu'à leur
/// distribution effective par le <c>IDomainEventDispatcher</c> (architecture/EVENT_SYSTEM.md).
/// Chacune des 16 entités de architecture/DOMAIN_MODEL.md est représentée comme une racine
/// d'agrégat indépendante dans ce squelette (aucune hiérarchie d'agrégat plus fine n'étant
/// encore spécifiée par la documentation de Domaine).
/// </summary>
/// <typeparam name="TId">Type de l'identifiant de la racine d'agrégat.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <inheritdoc />
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Enregistre un fait accompli produit par cet agrégat, destiné à être distribué après
    /// persistance (architecture/EVENT_SYSTEM.md §3 : un événement décrit un fait passé).
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc />
    public void ClearDomainEvents() => _domainEvents.Clear();
}
