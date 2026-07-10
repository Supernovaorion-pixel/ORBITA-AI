using System.Diagnostics;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace OrbitaAI.UnitTests.ImportEngine;

/// <summary>
/// Vérifie que le moteur de lecture se comporte conformément aux exigences de performance pour
/// des fichiers de taille industrielle (tech/PERFORMANCE_TARGETS.md §2, architecture/PERFORMANCE_GUIDELINES.md) :
/// lecture en flux continu, empreinte mémoire non proportionnelle à la taille du fichier,
/// annulation effective avant la fin de la lecture.
/// </summary>
public sealed class LargeFileReadingTests : IDisposable
{
    private const int RowCount = 1_000_000;
    private readonly ITestOutputHelper _output;
    private readonly string _filePath;

    public LargeFileReadingTests(ITestOutputHelper output)
    {
        _output = output;
        _filePath = Path.Combine(Path.GetTempPath(), $"orbita-ai-large-{Guid.NewGuid():N}.csv");
        GenerateCsvFile(_filePath, RowCount);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }

    [Fact]
    public async Task ReadAsync_OneMillionRowFile_ReadsEveryRow_WithBoundedMemory()
    {
        var reader = new CsvFileReader();
        var context = new ReaderContext { Source = new FileReaderSource(_filePath) };

        GC.Collect();
        GC.WaitForPendingFinalizers();
        var memoryBefore = GC.GetTotalMemory(forceFullCollection: true);

        var stopwatch = Stopwatch.StartNew();
        var rowCount = 0L;
        await foreach (var row in reader.ReadAsync(context))
        {
            rowCount++;
        }

        stopwatch.Stop();
        var memoryAfter = GC.GetTotalMemory(forceFullCollection: true);
        var fileSizeBytes = new FileInfo(_filePath).Length;
        var memoryGrowthBytes = memoryAfter - memoryBefore;

        Assert.Equal(RowCount, rowCount);
        Assert.Equal(RowCount, context.Statistics.RowsRead);
        Assert.True(context.Statistics.IsCompleted);

        // L'empreinte mémoire supplémentaire retenue après lecture reste très inférieure à la
        // taille du fichier lui-même : la preuve que le contenu n'a jamais été chargé intégralement.
        Assert.True(
            memoryGrowthBytes < fileSizeBytes / 2,
            $"Croissance mémoire ({memoryGrowthBytes:N0} octets) inattendue au regard de la taille du fichier ({fileSizeBytes:N0} octets).");

        _output.WriteLine($"Fichier : {fileSizeBytes:N0} octets, {RowCount:N0} lignes");
        _output.WriteLine($"Durée de lecture : {stopwatch.Elapsed.TotalSeconds:N2} s");
        _output.WriteLine($"Débit : {RowCount / stopwatch.Elapsed.TotalSeconds:N0} lignes/s");
        _output.WriteLine($"Croissance mémoire retenue : {memoryGrowthBytes:N0} octets");
        _output.WriteLine($"Débit statistiques internes : {context.Statistics.RowsPerSecond:N0} lignes/s");
    }

    [Fact]
    public async Task ReadAsync_CancelledEarly_OnOneMillionRowFile_StopsWithoutReadingEverything()
    {
        var reader = new CsvFileReader();
        var context = new ReaderContext { Source = new FileReaderSource(_filePath) };
        using var cts = new CancellationTokenSource();

        var stopwatch = Stopwatch.StartNew();
        var rowsSeen = 0;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await foreach (var row in reader.ReadAsync(context, cts.Token))
            {
                rowsSeen++;
                if (rowsSeen == 1_000)
                {
                    await cts.CancelAsync();
                }
            }
        });

        stopwatch.Stop();

        Assert.True(rowsSeen < RowCount);
        _output.WriteLine($"Annulation après {rowsSeen:N0} lignes en {stopwatch.Elapsed.TotalMilliseconds:N0} ms (sur {RowCount:N0} au total).");
    }

    [Fact]
    public async Task ReadAsync_WithMaxRows_OnOneMillionRowFile_StopsEarlyWithoutReadingEverything()
    {
        var reader = new CsvFileReader();
        var context = new ReaderContext
        {
            Source = new FileReaderSource(_filePath),
            Options = new ReaderOptions { MaxRows = 500 },
        };

        var stopwatch = Stopwatch.StartNew();
        var rows = await reader.ReadAsync(context).ToListAsync();
        stopwatch.Stop();

        Assert.Equal(500, rows.Count);
        _output.WriteLine($"Lecture plafonnée à 500 lignes en {stopwatch.Elapsed.TotalMilliseconds:N0} ms (sur un fichier d'un million de lignes).");
    }

    private static void GenerateCsvFile(string path, int rowCount)
    {
        using var writer = new StreamWriter(path);
        writer.WriteLine("Annee,Mois,Client,Produit,Quantite,MontantHT");
        for (var i = 1; i <= rowCount; i++)
        {
            writer.WriteLine($"2026,{(i % 12) + 1},Client{i % 500},Produit{i % 200},{i % 50},{i * 1.5:F2}");
        }
    }
}
