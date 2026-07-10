using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Services;

/// <summary>
/// Distribue les événements de Domaine accumulés par une ou plusieurs racines d'agrégat
/// (<see cref="IHasDomainEvents"/>) vers le <see cref="IEventBus"/>, puis les purge. Distinct
/// du <see cref="IEventBus"/> lui-même : l'<see cref="IEventBus"/> est le transport
/// (émission/abonnement), le distributeur est le point d'orchestration qui relie les
/// événements accumulés par le Domaine à ce transport, typiquement invoqué après la validation
/// transactionnelle d'un cas d'usage (<c>IUnitOfWork.SaveChangesAsync</c>).
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Distribue puis purge les événements accumulés par chacune des entités fournies.
    /// L'ordre de distribution suit l'ordre des entités puis, pour chacune, l'ordre
    /// d'enregistrement de ses événements.
    /// </summary>
    Task DispatchAndClearAsync(IEnumerable<IHasDomainEvents> entitiesWithEvents, CancellationToken cancellationToken = default);
}
