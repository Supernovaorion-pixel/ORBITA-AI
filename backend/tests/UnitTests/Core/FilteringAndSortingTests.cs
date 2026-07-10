using OrbitaAI.Core.Common.Filtering;
using OrbitaAI.Core.Common.Sorting;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class FilteringAndSortingTests
{
    [Fact]
    public void FilterDescriptor_ExposesProvidedValues()
    {
        var filter = new FilterDescriptor("Name", FilterOperator.Contains, "orbita");

        Assert.Equal("Name", filter.PropertyName);
        Assert.Equal(FilterOperator.Contains, filter.Operator);
        Assert.Equal("orbita", filter.Value);
    }

    [Fact]
    public void FilterDescriptor_AcceptsNullValue()
    {
        var filter = new FilterDescriptor("Name", FilterOperator.Equals, null);

        Assert.Null(filter.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void FilterDescriptor_WithBlankPropertyName_Throws(string? propertyName)
    {
        Assert.Throws<ArgumentException>(() => new FilterDescriptor(propertyName!, FilterOperator.Equals, "value"));
    }

    [Fact]
    public void SortDescriptor_DefaultsToAscending()
    {
        var sort = new SortDescriptor("CreatedAt");

        Assert.Equal(SortDirection.Ascending, sort.Direction);
    }

    [Fact]
    public void SortDescriptor_AcceptsExplicitDirection()
    {
        var sort = new SortDescriptor("CreatedAt", SortDirection.Descending);

        Assert.Equal(SortDirection.Descending, sort.Direction);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void SortDescriptor_WithBlankPropertyName_Throws(string? propertyName)
    {
        Assert.Throws<ArgumentException>(() => new SortDescriptor(propertyName!));
    }
}
