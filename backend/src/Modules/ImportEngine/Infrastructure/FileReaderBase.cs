using System.Runtime.CompilerServices;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure;

/// <summary>
/// Socle commun de tout lecteur concret (patron de méthode / Template Method). Prend en charge
/// les préoccupations transversales à tout format — garde-fou de taille, suivi des statistiques,
/// cadence de notification de progression, vérification d'annulation — laissant à chaque lecteur
/// concret (<see cref="Domain.IFileReader"/>) la seule responsabilité propre à son format :
/// produire un flux de <see cref="RawRow"/> à partir du flux source ouvert.
/// </summary>
public abstract class FileReaderBase : IFileReader
{
    /// <inheritdoc />
    public abstract FileFormat Format { get; }

    /// <inheritdoc />
    public abstract bool CanRead(ReaderSource source);

    /// <inheritdoc />
    public async IAsyncEnumerable<RawRow> ReadAsync(
        ReaderContext context,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        ValidateSourceSize(context);

        var statistics = context.Statistics;
        statistics.MarkStarted();

        var rowsSinceLastReport = 0;
        try
        {
            await using var stream = context.Source.OpenRead();

            await foreach (var row in ReadRowsCoreAsync(context, stream, cancellationToken).WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                statistics.IncrementRowsRead();
                if (stream.CanSeek)
                {
                    statistics.ReportBytesRead(stream.Position);
                }

                rowsSinceLastReport++;
                if (rowsSinceLastReport >= context.Configuration.ProgressReportIntervalRows)
                {
                    ReportProgress(context);
                    rowsSinceLastReport = 0;
                }

                yield return row;

                if (context.Options.MaxRows is { } maxRows && statistics.RowsRead >= maxRows)
                {
                    yield break;
                }
            }
        }
        finally
        {
            ReportProgress(context);
            statistics.MarkCompleted();
        }
    }

    /// <summary>
    /// Produit le flux de lignes brutes propre au format concret, à partir du flux déjà ouvert
    /// de la source. Doit lever <see cref="EmptySourceException"/> si la source ne contient
    /// strictement aucun contenu structurel (cf. <see cref="ThrowIfEmpty"/>), et
    /// <see cref="CorruptedSourceException"/> si son contenu est illisible pour le format attendu.
    /// </summary>
    protected abstract IAsyncEnumerable<RawRow> ReadRowsCoreAsync(
        ReaderContext context,
        Stream stream,
        CancellationToken cancellationToken);

    /// <summary>
    /// Lève <see cref="EmptySourceException"/> lorsque <paramref name="hasAnyStructuralContent"/>
    /// est <see langword="false"/> — c'est-à-dire lorsque la source ne comporte même pas de ligne
    /// d'en-tête ou de première ligne exploitable, distinct d'un fichier structurellement valide
    /// sans ligne de données (cf. Domain/Exceptions/EmptySourceException.cs).
    /// </summary>
    protected void ThrowIfEmpty(bool hasAnyStructuralContent, ReaderContext context)
    {
        if (!hasAnyStructuralContent)
        {
            throw new EmptySourceException(context.Source.DisplayName);
        }
    }

    private static void ValidateSourceSize(ReaderContext context)
    {
        var length = context.Source.Length;
        if (length is { } knownLength && knownLength > context.Configuration.MaxSourceSizeBytes)
        {
            throw new SourceSizeExceededException(context.Source.DisplayName, knownLength, context.Configuration.MaxSourceSizeBytes);
        }
    }

    private static void ReportProgress(ReaderContext context)
    {
        var statistics = context.Statistics;
        context.Progress?.Report(new ReaderProgress(statistics.RowsRead, statistics.BytesRead, context.Source.Length, statistics.Elapsed));
    }
}
