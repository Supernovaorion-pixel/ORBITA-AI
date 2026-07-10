using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine;

public sealed class CsvFileReaderTests
{
    private static ReaderContext CreateContext(MemoryStream stream, ReaderOptions? options = null) =>
        new()
        {
            Source = new StreamReaderSource(stream, "test.csv"),
            Options = options ?? ReaderOptions.Default,
        };

    private static async Task<List<RawRow>> ReadAllAsync(CsvFileReader reader, ReaderContext context) =>
        await reader.ReadAsync(context).ToListAsync();

    [Fact]
    public void CanRead_RecognizesCsvAndTxtExtensions()
    {
        var reader = new CsvFileReader();

        Assert.True(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.csv")));
        Assert.True(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.CSV")));
        Assert.True(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.txt")));
        Assert.False(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.xlsx")));
    }

    [Fact]
    public async Task ReadAsync_WithHeaderRow_ExposesHeadersAndRawStringValues()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT\nAcme,1500\nContoso,2300\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(2, rows.Count);
        Assert.Equal(["Client", "Montant HT"], rows[0].Headers);
        Assert.Equal(["Acme", "1500"], rows[0].Values);
        Assert.Equal(["Contoso", "2300"], rows[1].Values);
        Assert.IsType<string>(rows[0].Values[1]); // Aucune conversion numérique : valeur brute.
    }

    [Fact]
    public async Task ReadAsync_AssignsSequentialOneBasedRowNumbers_ExcludingHeader()
    {
        using var stream = TestHelpers.ToStream("A,B\n1,2\n3,4\n5,6\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal([1, 2, 3], rows.Select(r => r.RowNumber));
    }

    [Fact]
    public async Task ReadAsync_WithoutHeaderRow_ProducesEmptyHeadersAndTreatsFirstLineAsData()
    {
        using var stream = TestHelpers.ToStream("Acme,1500\nContoso,2300\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream, new ReaderOptions { HasHeaderRow = false });

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(2, rows.Count);
        Assert.Empty(rows[0].Headers);
        Assert.Equal(["Acme", "1500"], rows[0].Values);
    }

    [Fact]
    public async Task ReadAsync_WithSemicolonDelimiter_AutoDetectsDelimiter()
    {
        using var stream = TestHelpers.ToStream("Client;Montant HT\nAcme;1500\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Single(rows);
        Assert.Equal(["Acme", "1500"], rows[0].Values);
    }

    [Fact]
    public async Task ReadAsync_WithExplicitDelimiter_OverridesAutoDetection()
    {
        using var stream = TestHelpers.ToStream("Client|Montant HT\nAcme|1500\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream, new ReaderOptions { Delimiter = '|' });

        var rows = await ReadAllAsync(reader, context);

        Assert.Single(rows);
        Assert.Equal(["Acme", "1500"], rows[0].Values);
    }

    [Fact]
    public async Task ReadAsync_PreservesQuotedFieldsContainingTheDelimiter()
    {
        using var stream = TestHelpers.ToStream("Client,Ville\n\"Acme, Inc.\",Paris\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal("Acme, Inc.", rows[0].Values[0]);
    }

    [Fact]
    public async Task ReadAsync_SkipsEmptyRows_WhenConfiguredTo()
    {
        using var stream = TestHelpers.ToStream("A,B\n1,2\n,\n3,4\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream, new ReaderOptions { SkipEmptyRows = true });

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public async Task ReadAsync_KeepsEmptyRows_WhenNotConfiguredToSkip()
    {
        using var stream = TestHelpers.ToStream("A,B\n1,2\n,\n3,4\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream, new ReaderOptions { SkipEmptyRows = false });

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public async Task ReadAsync_RespectsMaxRows()
    {
        using var stream = TestHelpers.ToStream("A\n1\n2\n3\n4\n5\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream, new ReaderOptions { MaxRows = 2 });

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public async Task ReadAsync_HeaderOnlyFile_YieldsNoRows_WithoutThrowing()
    {
        using var stream = TestHelpers.ToStream("Client,Montant HT\n");
        var reader = new CsvFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Empty(rows);
        Assert.True(context.Statistics.IsCompleted);
    }

    [Fact]
    public async Task ReadAsync_CompletelyEmptyFile_ThrowsEmptySourceException()
    {
        using var stream = TestHelpers.ToStream(string.Empty);
        var reader = new CsvFileReader();
        var context = CreateContext(stream);

        await Assert.ThrowsAsync<EmptySourceException>(() => ReadAllAsync(reader, context));
    }
}
