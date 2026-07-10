using OrbitaAI.Core.Domain.Events;

namespace OrbitaAI.Core.Services;

/// <summary>
/// Contrat du système d'événements interne (architecture/EVENT_SYSTEM.md). Permet à un module
/// d'émettre un événement sans connaître ses abonnés, et à un module de s'abonner à un
/// événement sans connaître son émetteur (architecture/EVENT_SYSTEM.md §2). Implémenté en
/// mémoire par défaut (<c>InMemoryEventBus</c>), extensible vers un courtier de messages
/// standard (AMQP) pour les déploiements à forte charge sans changement de ce contrat
/// (tech/TECHNOLOGY_STACK.md §5).
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Émet un événement représentant un fait accompli vers l'ensemble de ses abonnés actuels
    /// (architecture/EVENT_SYSTEM.md §3). Ne bloque jamais indéfiniment le module émetteur
    /// au-delà de l'exécution effective des gestionnaires abonnés (architecture/EVENT_SYSTEM.md §6).
    /// </summary>
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : class, IDomainEvent;

    /// <summary>
    /// Abonne <paramref name="handler"/> à tout événement de type <typeparamref name="TEvent"/>
    /// émis ultérieurement. Retourne un jeton d'abonnement dont la libération
    /// (<see cref="IDisposable.Dispose"/>) désabonne le gestionnaire.
    /// </summary>
    IDisposable Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> handler) where TEvent : class, IDomainEvent;
}
