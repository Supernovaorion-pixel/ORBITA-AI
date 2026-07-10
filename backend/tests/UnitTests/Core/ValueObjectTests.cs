using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class ValueObjectTests
{
    [Fact]
    public void TwoValueObjects_WithSameComponents_AreEqual()
    {
        var first = new Coordinates(48.8566, 2.3522);
        var second = new Coordinates(48.8566, 2.3522);

        Assert.Equal(first, second);
        Assert.True(first == second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void TwoValueObjects_WithDifferentComponents_AreNotEqual()
    {
        var first = new Coordinates(48.8566, 2.3522);
        var second = new Coordinates(40.7128, -74.0060);

        Assert.NotEqual(first, second);
        Assert.True(first != second);
    }

    [Fact]
    public void ValueObject_IsNeverEqualToNull()
    {
        var value = new Coordinates(0, 0);

        Assert.False(value.Equals(null));
        Assert.False(value == null);
    }

    [Fact]
    public void ValueObject_IsEqualToItself()
    {
        var value = new Coordinates(1, 1);

        Assert.True(value.Equals(value));
    }
}
