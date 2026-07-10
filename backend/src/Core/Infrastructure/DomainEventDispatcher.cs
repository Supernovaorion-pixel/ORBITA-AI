using System.Collections.Concurrent;
using System.Reflection;
using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Core.Domain;
using OrbitaAI.Core.Domain.Events;
using OrbitaAI.Core.Services;

namespace OrbitaAI.Core.Infrastructure;

/// <summary>
/// Implémentation par défaut de <see cref="IDomainEventDispatcher"/> : relaie chaque événement
/// accumulé par les entités fournies vers le <see cref="IEventBus"/> injecté, puis purge les
/// entités. La méthode générique <see cref="IEventBus.PublishAsync{TEvent}"/> est résolue par
/// réflexion pour le type d'exécution réel de chaque événement (celui-ci n'étant connu qu'au
/// travers de l'interface non générique <see cref="IDomainEvent"/> au sein de la boucle de
/// distribution), avec mise en cache des <see cref="MethodInfo"/> déjà résolues pour limiter
/// le coût de cette résolution.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> PublishMethodByEventType = new();
    private static readonly MethodInfo PublishOpenGenericMethod = typeof(IEventBus).GetMethod(nameof(IEventBus.PublishAsync))
        ?? throw new InvalidOperationException($"{nameof(IEventBus)}.{nameof(IEventBus.PublishAsync)} est introuvable par réflexion.");

    private readonly IEventBus _eventBus;

    public DomainEventDispatcher(IEventBus eventBus)
    {
        _eventBus = Guard.Against.Null(eventBus, nameof(eventBus));
    }

    /// <inheritdoc />
    public async Task DispatchAndClearAsync(IEnumerable<IHasDomainEvents> entitiesWithEvents, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entitiesWithEvents, nameof(entitiesWithEvents));

        foreach (var entity in entitiesWithEvents)
        {
            var eventsToDispatch = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            foreach (var domainEvent in eventsToDispatch)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await PublishByRuntimeTypeAsync(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private Task PublishByRuntimeTypeAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var eventType = domainEvent.GetType();
        var closedGenericMethod = PublishMethodByEventType.GetOrAdd(
            eventType,
            static (type, openMethod) => openMethod.MakeGenericMethod(type),
            PublishOpenGenericMethod);

        var task = (Task?)closedGenericMethod.Invoke(_eventBus, new object[] { domainEvent, cancellationToken });
        return task ?? Task.CompletedTask;
    }
}
