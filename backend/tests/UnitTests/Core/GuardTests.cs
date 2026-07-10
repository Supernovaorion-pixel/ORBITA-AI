using OrbitaAI.Core.Common.Guards;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class GuardTests
{
    [Fact]
    public void Null_WithNonNullValue_ReturnsTheValue()
    {
        var value = "orbita-ai";

        var result = Guard.Against.Null(value, nameof(value));

        Assert.Equal(value, result);
    }

    [Fact]
    public void Null_WithNullValue_Throws()
    {
        string? value = null;

        Assert.Throws<ArgumentNullException>(() => Guard.Against.Null(value, nameof(value)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void NullOrEmpty_WithNullOrEmptyValue_Throws(string? value)
    {
        Assert.Throws<ArgumentException>(() => Guard.Against.NullOrEmpty(value, nameof(value)));
    }

    [Fact]
    public void NullOrEmpty_WithNonEmptyValue_ReturnsTheValue()
    {
        var result = Guard.Against.NullOrEmpty("orbita-ai", "value");

        Assert.Equal("orbita-ai", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NullOrWhiteSpace_WithBlankValue_Throws(string? value)
    {
        Assert.Throws<ArgumentException>(() => Guard.Against.NullOrWhiteSpace(value, nameof(value)));
    }

    [Fact]
    public void Default_WithEmptyGuid_Throws()
    {
        Assert.Throws<ArgumentException>(() => Guard.Against.Default(Guid.Empty, "value"));
    }

    [Fact]
    public void Default_WithNonEmptyGuid_ReturnsTheValue()
    {
        var id = Guid.NewGuid();

        var result = Guard.Against.Default(id, "value");

        Assert.Equal(id, result);
    }

    [Fact]
    public void Negative_WithNegativeValue_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Against.Negative(-1, "value"));
    }

    [Fact]
    public void Negative_WithZeroOrPositiveValue_ReturnsTheValue()
    {
        Assert.Equal(0, Guard.Against.Negative(0, "value"));
        Assert.Equal(5, Guard.Against.Negative(5, "value"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void NegativeOrZero_WithNonPositiveValue_Throws(int value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Against.NegativeOrZero(value, nameof(value)));
    }

    [Theory]
    [InlineData(0, 1, 10)]
    [InlineData(11, 1, 10)]
    public void OutOfRange_WithValueOutsideBounds_Throws(int value, int min, int max)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Against.OutOfRange(value, min, max, nameof(value)));
    }

    [Fact]
    public void OutOfRange_WithValueWithinBounds_ReturnsTheValue()
    {
        var result = Guard.Against.OutOfRange(5, 1, 10, "value");

        Assert.Equal(5, result);
    }

    private enum SampleEnum
    {
        First,
        Second
    }

    [Fact]
    public void OutOfEnum_WithUndefinedValue_Throws()
    {
        var undefined = (SampleEnum)99;

        Assert.Throws<ArgumentException>(() => Guard.Against.OutOfEnum(undefined, "value"));
    }

    [Fact]
    public void OutOfEnum_WithDefinedValue_ReturnsTheValue()
    {
        var result = Guard.Against.OutOfEnum(SampleEnum.Second, "value");

        Assert.Equal(SampleEnum.Second, result);
    }
}
