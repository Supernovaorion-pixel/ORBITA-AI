using OrbitaAI.Core.Domain.Events;

namespace OrbitaAI.Core.Domain;

/// <summary>
/// Contrat non générique satisfait par toute <see cref="AggregateRoot{TId}"/>, permettant à
/// l'infrastructure (ex. <c>IDomainEventDispatcher</c>) de manipuler des racines d'agrégat de
/// types d'identifiant différents de façon uniforme (architecture/EVENT_SYSTEM.md).
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>Événements de Domaine accumulés depuis la dernière purge.</summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>Purge les événements accumulés, typiquement après leur distribution effective.</summary>
    void ClearDomainEvents();
}
