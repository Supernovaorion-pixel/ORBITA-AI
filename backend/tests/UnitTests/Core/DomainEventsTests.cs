using OrbitaAI.Core.Domain;
using OrbitaAI.Core.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class DomainEventsTests
{
    [Fact]
    public void AggregateRoot_AccumulatesRaisedEvents()
    {
        var aggregate = new TestAggregateRoot();

        aggregate.Rename("orbita-ai");

        Assert.Single(aggregate.DomainEvents);
        Assert.IsType<TestDomainEvent>(aggregate.DomainEvents.Single());
    }

    [Fact]
    public void AggregateRoot_ClearDomainEvents_EmptiesTheCollection()
    {
        var aggregate = new TestAggregateRoot();
        aggregate.Rename("orbita-ai");

        aggregate.ClearDomainEvents();

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void NewAggregateRoot_HasNoDomainEvents()
    {
        var aggregate = new TestAggregateRoot();

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public async Task DomainEventDispatcher_PublishesAccumulatedEvents_ThroughTheEventBus()
    {
        var bus = new InMemoryEventBus();
        var dispatcher = new DomainEventDispatcher(bus);
        string? receivedPayload = null;
        bus.Subscribe<TestDomainEvent>((domainEvent, _) =>
        {
            receivedPayload = domainEvent.Payload;
            return Task.CompletedTask;
        });

        var aggregate = new TestAggregateRoot();
        aggregate.Rename("orbita-ai");

        await dispatcher.DispatchAndClearAsync(new IHasDomainEvents[] { aggregate });

        Assert.Equal("orbita-ai", receivedPayload);
    }

    [Fact]
    public async Task DomainEventDispatcher_ClearsEvents_AfterDispatching()
    {
        var bus = new InMemoryEventBus();
        var dispatcher = new DomainEventDispatcher(bus);

        var aggregate = new TestAggregateRoot();
        aggregate.Rename("orbita-ai");

        await dispatcher.DispatchAndClearAsync(new IHasDomainEvents[] { aggregate });

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public async Task DomainEventDispatcher_WithNoAccumulatedEvents_DoesNothing()
    {
        var bus = new InMemoryEventBus();
        var dispatcher = new DomainEventDispatcher(bus);
        var invoked = false;
        bus.Subscribe<TestDomainEvent>((_, _) => { invoked = true; return Task.CompletedTask; });

        var aggregate = new TestAggregateRoot();

        await dispatcher.DispatchAndClearAsync(new IHasDomainEvents[] { aggregate });

        Assert.False(invoked);
    }

    [Fact]
    public void Constructor_WithNullEventBus_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new DomainEventDispatcher(null!));
    }
}
