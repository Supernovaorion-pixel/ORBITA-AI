using OrbitaAI.Core.Common.Pagination;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class PaginationTests
{
    [Fact]
    public void PaginationRequest_Default_IsFirstPageOfStandardSize()
    {
        Assert.Equal(1, PaginationRequest.Default.PageNumber);
        Assert.Equal(25, PaginationRequest.Default.PageSize);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(-1, 10)]
    public void PaginationRequest_WithInvalidValues_Throws(int pageNumber, int pageSize)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PaginationRequest(pageNumber, pageSize));
    }

    [Fact]
    public void PagedResult_ComputesTotalPages()
    {
        var result = new PagedResult<int>(new[] { 1, 2, 3 }, pageNumber: 1, pageSize: 3, totalCount: 10);

        Assert.Equal(4, result.TotalPages);
    }

    [Fact]
    public void PagedResult_HasPreviousAndNextPage_ComputedFromPageNumber()
    {
        var middlePage = new PagedResult<int>(new[] { 4, 5, 6 }, pageNumber: 2, pageSize: 3, totalCount: 10);

        Assert.True(middlePage.HasPreviousPage);
        Assert.True(middlePage.HasNextPage);
    }

    [Fact]
    public void PagedResult_FirstPage_HasNoPreviousPage()
    {
        var firstPage = new PagedResult<int>(new[] { 1, 2, 3 }, pageNumber: 1, pageSize: 3, totalCount: 10);

        Assert.False(firstPage.HasPreviousPage);
    }

    [Fact]
    public void PagedResult_LastPage_HasNoNextPage()
    {
        var lastPage = new PagedResult<int>(new[] { 10 }, pageNumber: 4, pageSize: 3, totalCount: 10);

        Assert.False(lastPage.HasNextPage);
    }

    [Fact]
    public void PagedResult_Empty_HasNoItemsAndNoPages()
    {
        var empty = PagedResult<int>.Empty(PaginationRequest.Default);

        Assert.Empty(empty.Items);
        Assert.Equal(0, empty.TotalCount);
    }
}
