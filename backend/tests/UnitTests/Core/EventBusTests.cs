using OrbitaAI.Core.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class EventBusTests
{
    [Fact]
    public async Task PublishAsync_InvokesSubscribedHandler()
    {
        var bus = new InMemoryEventBus();
        TestDomainEvent? received = null;

        bus.Subscribe<TestDomainEvent>((domainEvent, _) =>
        {
            received = domainEvent;
            return Task.CompletedTask;
        });

        var published = new TestDomainEvent(Guid.NewGuid(), "hello");
        await bus.PublishAsync(published);

        Assert.Same(published, received);
    }

    [Fact]
    public async Task PublishAsync_InvokesAllHandlersSubscribedToTheSameEventType()
    {
        var bus = new InMemoryEventBus();
        var invocationCount = 0;

        bus.Subscribe<TestDomainEvent>((_, _) => { invocationCount++; return Task.CompletedTask; });
        bus.Subscribe<TestDomainEvent>((_, _) => { invocationCount++; return Task.CompletedTask; });

        await bus.PublishAsync(new TestDomainEvent(Guid.NewGuid(), "payload"));

        Assert.Equal(2, invocationCount);
    }

    [Fact]
    public async Task PublishAsync_DoesNotInvokeHandlersOfADifferentEventType()
    {
        var bus = new InMemoryEventBus();
        var otherEventInvoked = false;

        bus.Subscribe<OtherTestDomainEvent>((_, _) => { otherEventInvoked = true; return Task.CompletedTask; });

        await bus.PublishAsync(new TestDomainEvent(Guid.NewGuid(), "payload"));

        Assert.False(otherEventInvoked);
    }

    [Fact]
    public async Task PublishAsync_WithNoSubscribers_CompletesWithoutError()
    {
        var bus = new InMemoryEventBus();

        await bus.PublishAsync(new TestDomainEvent(Guid.NewGuid(), "payload"));
    }

    [Fact]
    public async Task Subscribe_DisposingTheSubscription_StopsFurtherInvocations()
    {
        var bus = new InMemoryEventBus();
        var invocationCount = 0;

        var subscription = bus.Subscribe<TestDomainEvent>((_, _) => { invocationCount++; return Task.CompletedTask; });

        await bus.PublishAsync(new TestDomainEvent(Guid.NewGuid(), "first"));
        subscription.Dispose();
        await bus.PublishAsync(new TestDomainEvent(Guid.NewGuid(), "second"));

        Assert.Equal(1, invocationCount);
    }

    [Fact]
    public async Task PublishAsync_WithCancelledToken_ThrowsBeforeInvokingHandler()
    {
        var bus = new InMemoryEventBus();
        var invoked = false;
        bus.Subscribe<TestDomainEvent>((_, _) => { invoked = true; return Task.CompletedTask; });

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => bus.PublishAsync(new TestDomainEvent(Guid.NewGuid(), "payload"), cts.Token));

        Assert.False(invoked);
    }

    [Fact]
    public void Subscribe_WithNullHandler_Throws()
    {
        var bus = new InMemoryEventBus();

        Assert.Throws<ArgumentNullException>(() => bus.Subscribe<TestDomainEvent>(null!));
    }
}
