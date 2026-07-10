using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;
using OrbitaAI.Modules.ImportEngine.Infrastructure;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine;

/// <summary>
/// Vérifie les comportements transversaux portés par <see cref="FileReaderBase"/> (garde-fou de
/// taille, statistiques, progression, annulation), communs à tout format concret.
/// </summary>
public sealed class ReaderCommonBehaviorTests
{
    [Fact]
    public async Task ReadAsync_CorruptedCsv_ThrowsCorruptedSourceException()
    {
        // Guillemet isolé au sein d'un champ non entre guillemets : invalide au sens RFC 4180.
        using var stream = TestHelpers.ToStream("A,B\n1,2\"3\n");
        var reader = new CsvFileReader();
        var context = new ReaderContext { Source = new StreamReaderSource(stream, "corrupted.csv") };

        await Assert.ThrowsAsync<CorruptedSourceException>(() => reader.ReadAsync(context).ToListAsync().AsTask());
    }

    [Fact]
    public async Task ReadAsync_CorruptedExcel_ThrowsCorruptedSourceException()
    {
        using var stream = new MemoryStream([0x00, 0x01, 0x02, 0x03, 0x04]);
        var reader = new ExcelFileReader();
        var context = new ReaderContext { Source = new StreamReaderSource(stream, "corrupted.xlsx") };

        await Assert.ThrowsAsync<CorruptedSourceException>(() => reader.ReadAsync(context).ToListAsync().AsTask());
    }

    [Fact]
    public async Task ReadAsync_SourceLargerThanConfiguredLimit_ThrowsSourceSizeExceededException()
    {
        using var stream = TestHelpers.ToStream("A,B\n1,2\n3,4\n");
        var reader = new CsvFileReader();
        var context = new ReaderContext
        {
            Source = new StreamReaderSource(stream, "export.csv"),
            Configuration = new ReaderConfiguration { MaxSourceSizeBytes = 5 },
        };

        await Assert.ThrowsAsync<SourceSizeExceededException>(() => reader.ReadAsync(context).ToListAsync().AsTask());
    }

    [Fact]
    public async Task ReadAsync_CancelledMidway_ThrowsOperationCanceledException_AndStopsBeforeCompletion()
    {
        var content = "A\n" + string.Join('\n', Enumerable.Range(1, 10_000)) + "\n";
        using var stream = TestHelpers.ToStream(content);
        var reader = new CsvFileReader();
        var context = new ReaderContext { Source = new StreamReaderSource(stream, "large.csv") };

        using var cts = new CancellationTokenSource();
        var rowsSeen = 0;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await foreach (var row in reader.ReadAsync(context, cts.Token))
            {
                rowsSeen++;
                if (rowsSeen == 100)
                {
                    await cts.CancelAsync();
                }
            }
        });

        Assert.True(rowsSeen < 10_000);
        Assert.True(context.Statistics.IsCompleted);
        Assert.True(context.Statistics.RowsRead < 10_000);
    }

    [Fact]
    public async Task ReadAsync_AlreadyCancelledToken_ThrowsImmediately_WithoutReadingAnyRow()
    {
        using var stream = TestHelpers.ToStream("A\n1\n2\n");
        var reader = new CsvFileReader();
        var context = new ReaderContext { Source = new StreamReaderSource(stream, "export.csv") };

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => reader.ReadAsync(context, cts.Token).ToListAsync().AsTask());
    }

    [Fact]
    public async Task ReadAsync_ReportsProgress_AtConfiguredInterval_AndUponCompletion()
    {
        var content = "A\n" + string.Join('\n', Enumerable.Range(1, 20)) + "\n";
        using var stream = TestHelpers.ToStream(content);
        var reader = new CsvFileReader();
        var reports = new List<ReaderProgress>();
        var progress = new Progress<ReaderProgress>(reports.Add);
        var context = new ReaderContext
        {
            Source = new StreamReaderSource(stream, "export.csv"),
            Configuration = new ReaderConfiguration { ProgressReportIntervalRows = 5 },
            Progress = progress,
        };

        await reader.ReadAsync(context).ToListAsync();
        // Progress<T> marshals callbacks via the captured SynchronizationContext/ThreadPool;
        // give queued callbacks a chance to complete before asserting.
        await Task.Delay(50);

        Assert.NotEmpty(reports);
        Assert.Contains(reports, r => r.RowsRead == 20);
        Assert.True(reports.Select(r => r.RowsRead).SequenceEqual(reports.Select(r => r.RowsRead).OrderBy(x => x)));
    }

    [Fact]
    public async Task ReadAsync_StatisticsReflectCompletedRead()
    {
        using var stream = TestHelpers.ToStream("A,B\n1,2\n3,4\n5,6\n");
        var reader = new CsvFileReader();
        var context = new ReaderContext { Source = new StreamReaderSource(stream, "export.csv") };

        Assert.False(context.Statistics.IsCompleted);

        await reader.ReadAsync(context).ToListAsync();

        Assert.True(context.Statistics.IsCompleted);
        Assert.Equal(3, context.Statistics.RowsRead);
        Assert.NotNull(context.Statistics.StartedAt);
        Assert.NotNull(context.Statistics.CompletedAt);
        Assert.True(context.Statistics.Elapsed >= TimeSpan.Zero);
        Assert.True(context.Statistics.RowsPerSecond >= 0);
    }
}
