using OrbitaAI.Modules.ImportEngine.Application;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine;

public sealed class ReaderFactoryTests
{
    private static ReaderFactory CreateFactory() => new([new CsvFileReader(), new ExcelFileReader()]);

    [Fact]
    public void ResolveReader_ForCsvSource_ReturnsCsvFileReader()
    {
        var factory = CreateFactory();
        var source = new StreamReaderSource(new MemoryStream(), "export.csv");

        var reader = factory.ResolveReader(source);

        Assert.IsType<CsvFileReader>(reader);
    }

    [Fact]
    public void ResolveReader_ForExcelSource_ReturnsExcelFileReader()
    {
        var factory = CreateFactory();
        var source = new StreamReaderSource(new MemoryStream(), "export.xlsx");

        var reader = factory.ResolveReader(source);

        Assert.IsType<ExcelFileReader>(reader);
    }

    [Fact]
    public void ResolveReader_ForUnknownExtension_ThrowsUnsupportedFileFormatException()
    {
        var factory = CreateFactory();
        var source = new StreamReaderSource(new MemoryStream(), "export.pdf");

        Assert.Throws<UnsupportedFileFormatException>(() => factory.ResolveReader(source));
    }

    [Fact]
    public void Constructor_WithNullReaders_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReaderFactory(null!));
    }

    [Fact]
    public void ResolveReader_WithNullSource_Throws()
    {
        var factory = CreateFactory();

        Assert.Throws<ArgumentNullException>(() => factory.ResolveReader(null!));
    }
}
