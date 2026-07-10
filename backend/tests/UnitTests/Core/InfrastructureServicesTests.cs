using OrbitaAI.Core.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class InfrastructureServicesTests
{
    [Fact]
    public void SystemClock_UtcNow_IsCloseToRealTime()
    {
        var clock = new SystemClock();

        var before = DateTimeOffset.UtcNow;
        var reading = clock.UtcNow;
        var after = DateTimeOffset.UtcNow;

        Assert.InRange(reading, before.AddSeconds(-1), after.AddSeconds(1));
    }

    [Fact]
    public void GuidIdGenerator_NewId_ReturnsNonEmptyUniqueValues()
    {
        var generator = new GuidIdGenerator();

        var first = generator.NewId();
        var second = generator.NewId();

        Assert.NotEqual(Guid.Empty, first);
        Assert.NotEqual(first, second);
    }
}
