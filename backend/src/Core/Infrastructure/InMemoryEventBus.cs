using System.Collections.Concurrent;
using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Core.Domain.Events;
using OrbitaAI.Core.Services;

namespace OrbitaAI.Core.Infrastructure;

/// <summary>
/// Implémentation par défaut du bus d'événements, en mémoire et en processus, conforme au
/// choix documenté dans tech/TECHNOLOGY_STACK.md §5. Thread-safe : les abonnements et
/// émissions peuvent survenir concurremment depuis plusieurs modules. Une implémentation
/// adossée à un courtier de messages standard (AMQP) pourra être substituée pour les
/// déploiements à forte charge, sans modification des modules consommateurs
/// (architecture/EVENT_SYSTEM.md).
/// </summary>
public sealed class InMemoryEventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<Func<IDomainEvent, CancellationToken, Task>>> _handlersByEventType = new();
    private readonly object _mutationLock = new();

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : class, IDomainEvent
    {
        Guard.Against.Null(domainEvent, nameof(domainEvent));

        var handlersSnapshot = GetHandlersSnapshot(typeof(TEvent));
        if (handlersSnapshot.Count == 0)
        {
            return;
        }

        foreach (var handler in handlersSnapshot)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await handler(domainEvent, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public IDisposable Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> handler) where TEvent : class, IDomainEvent
    {
        Guard.Against.Null(handler, nameof(handler));

        Task WrappedHandler(IDomainEvent domainEvent, CancellationToken cancellationToken) =>
            handler((TEvent)domainEvent, cancellationToken);

        Func<IDomainEvent, CancellationToken, Task> wrapped = WrappedHandler;

        lock (_mutationLock)
        {
            var handlers = _handlersByEventType.GetOrAdd(
                typeof(TEvent),
                static _ => new List<Func<IDomainEvent, CancellationToken, Task>>());
            handlers.Add(wrapped);
        }

        return new Subscription(() => Unsubscribe(typeof(TEvent), wrapped));
    }

    private List<Func<IDomainEvent, CancellationToken, Task>> GetHandlersSnapshot(Type eventType)
    {
        lock (_mutationLock)
        {
            return _handlersByEventType.TryGetValue(eventType, out var handlers)
                ? new List<Func<IDomainEvent, CancellationToken, Task>>(handlers)
                : new List<Func<IDomainEvent, CancellationToken, Task>>();
        }
    }

    private void Unsubscribe(Type eventType, Func<IDomainEvent, CancellationToken, Task> wrapped)
    {
        lock (_mutationLock)
        {
            if (_handlersByEventType.TryGetValue(eventType, out var handlers))
            {
                handlers.Remove(wrapped);
            }
        }
    }

    /// <summary>Jeton d'abonnement retourné par <see cref="Subscribe{TEvent}"/>.</summary>
    private sealed class Subscription : IDisposable
    {
        private readonly Action _onDispose;
        private bool _disposed;

        public Subscription(Action onDispose) => _onDispose = onDispose;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _onDispose();
        }
    }
}
