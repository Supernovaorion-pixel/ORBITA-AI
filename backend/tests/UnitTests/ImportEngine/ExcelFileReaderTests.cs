using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine;

public sealed class ExcelFileReaderTests
{
    private static ReaderContext CreateContext(MemoryStream stream, ReaderOptions? options = null) =>
        new()
        {
            Source = new StreamReaderSource(stream, "test.xlsx"),
            Options = options ?? ReaderOptions.Default,
        };

    private static async Task<List<RawRow>> ReadAllAsync(ExcelFileReader reader, ReaderContext context) =>
        await reader.ReadAsync(context).ToListAsync();

    [Fact]
    public void CanRead_RecognizesXlsxExtensionOnly()
    {
        var reader = new ExcelFileReader();

        Assert.True(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.xlsx")));
        Assert.True(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.XLSX")));
        Assert.False(reader.CanRead(new StreamReaderSource(new MemoryStream(), "export.csv")));
    }

    [Fact]
    public async Task ReadAsync_WithHeaderRow_ExposesHeadersAndNativeTypedValues()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["Client", "Montant HT"],
            ["Acme", 1500.5],
            ["Contoso", 2300d],
        ]);
        var reader = new ExcelFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(2, rows.Count);
        Assert.Equal(["Client", "Montant HT"], rows[0].Headers);
        Assert.Equal("Acme", rows[0].Values[0]);
        Assert.Equal(1500.5, rows[0].Values[1]);
        Assert.IsType<double>(rows[0].Values[1]); // Aucune conversion : type natif Excel préservé.
    }

    [Fact]
    public async Task ReadAsync_PreservesSparseCells_AsExplicitNulls()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["A", "B", "C"],
            ["1", null, "3"],
        ]);
        var reader = new ExcelFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(3, rows[0].Values.Count);
        Assert.Equal("1", rows[0].Values[0]);
        Assert.Null(rows[0].Values[1]);
        Assert.Equal("3", rows[0].Values[2]);
    }

    [Fact]
    public async Task ReadAsync_WithoutHeaderRow_ProducesEmptyHeaders()
    {
        using var stream = TestHelpers.CreateXlsx([["Acme", 1500d]]);
        var reader = new ExcelFileReader();
        var context = CreateContext(stream, new ReaderOptions { HasHeaderRow = false });

        var rows = await ReadAllAsync(reader, context);

        Assert.Single(rows);
        Assert.Empty(rows[0].Headers);
    }

    [Fact]
    public async Task ReadAsync_ReusesRepeatedSharedStrings_Correctly()
    {
        using var stream = TestHelpers.CreateXlsx(
        [
            ["Client"],
            ["Acme"],
            ["Contoso"],
            ["Acme"],
        ]);
        var reader = new ExcelFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Equal(["Acme"], rows[0].Values);
        Assert.Equal(["Contoso"], rows[1].Values);
        Assert.Equal(["Acme"], rows[2].Values);
    }

    [Fact]
    public async Task ReadAsync_SelectsSheetByName()
    {
        using var stream = TestHelpers.CreateXlsx([["Client"], ["Acme"]], sheetName: "Ventes 2026");
        var reader = new ExcelFileReader();
        var context = CreateContext(stream, new ReaderOptions { SheetName = "Ventes 2026" });

        var rows = await ReadAllAsync(reader, context);

        Assert.Single(rows);
    }

    [Fact]
    public async Task ReadAsync_HeaderOnlyWorkbook_YieldsNoRows_WithoutThrowing()
    {
        using var stream = TestHelpers.CreateXlsx([["Client", "Montant HT"]]);
        var reader = new ExcelFileReader();
        var context = CreateContext(stream);

        var rows = await ReadAllAsync(reader, context);

        Assert.Empty(rows);
        Assert.True(context.Statistics.IsCompleted);
    }

    [Fact]
    public async Task ReadAsync_WorkbookWithNoRowsAtAll_ThrowsEmptySourceException()
    {
        using var stream = TestHelpers.CreateEmptyXlsx();
        var reader = new ExcelFileReader();
        var context = CreateContext(stream);

        await Assert.ThrowsAsync<EmptySourceException>(() => ReadAllAsync(reader, context));
    }

    [Fact]
    public async Task ReadAsync_NonSeekableStream_ThrowsCorruptedSourceException()
    {
        await using var nonSeekable = new NonSeekableStream(TestHelpers.CreateXlsx([["A"]]));
        var reader = new ExcelFileReader();
        var context = new ReaderContext { Source = new StreamReaderSource(nonSeekable, "test.xlsx") };

        await Assert.ThrowsAsync<CorruptedSourceException>(() => ReadAllAsync(reader, context));
    }

    /// <summary>Enveloppe forçant <see cref="Stream.CanSeek"/> à <see langword="false"/>, pour simuler un flux réseau non-adressable.</summary>
    private sealed class NonSeekableStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
