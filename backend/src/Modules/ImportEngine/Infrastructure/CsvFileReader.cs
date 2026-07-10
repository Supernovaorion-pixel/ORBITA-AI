using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure;

/// <summary>
/// Lecteur de fichiers CSV (features/IMPORT_ENGINE.md §2), fondé sur CsvHelper (tech/DEPENDENCY_POLICY.md
/// §3 — bibliothèque mature, largement adoptée, licence compatible). Lit le fichier ligne par
/// ligne en flux continu (streaming), sans jamais charger son contenu intégral en mémoire, et
/// restitue les champs bruts de chaque ligne sans aucune conversion de type ni interprétation
/// (aucun appel aux accesseurs typés de CsvHelper qui déclencheraient une validation de
/// structure) — conformément à l'exigence de ne faire aucune hypothèse sur les colonnes.
/// </summary>
public sealed class CsvFileReader : FileReaderBase
{
    private static readonly string[] SupportedExtensions = [".csv", ".txt"];

    /// <inheritdoc />
    public override FileFormat Format => FileFormat.Csv;

    /// <inheritdoc />
    public override bool CanRead(ReaderSource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var extension = Path.GetExtension(source.DisplayName);
        return SupportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    protected override async IAsyncEnumerable<RawRow> ReadRowsCoreAsync(
        ReaderContext context,
        Stream stream,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var encoding = context.Options.Encoding ?? Encoding.UTF8;
        using var textReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = context.Options.HasHeaderRow,
            TrimOptions = TrimOptions.None,
            DetectDelimiter = context.Options.Delimiter is null,
            Delimiter = context.Options.Delimiter?.ToString() ?? ",",
        };

        using var csv = new CsvReader(textReader, configuration);

        bool hasAnyContent;
        try
        {
            hasAnyContent = await csv.ReadAsync().ConfigureAwait(false);
        }
        catch (CsvHelperException ex)
        {
            throw new CorruptedSourceException(context.Source.DisplayName, ex);
        }

        ThrowIfEmpty(hasAnyContent, context);

        IReadOnlyList<string> headers = Array.Empty<string>();
        if (context.Options.HasHeaderRow)
        {
            csv.ReadHeader();
            headers = csv.HeaderRecord ?? Array.Empty<string>();

            try
            {
                hasAnyContent = await csv.ReadAsync().ConfigureAwait(false);
            }
            catch (CsvHelperException ex)
            {
                throw new CorruptedSourceException(context.Source.DisplayName, ex);
            }
        }

        var rowNumber = 0;
        while (hasAnyContent)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string[]? record;
            try
            {
                // CsvHelper valide et matérialise les champs de la ligne au moment de cet accès
                // (et non lors de l'appel précédent à ReadAsync) : une donnée mal formée
                // (ex. guillemet isolé hors RFC 4180) lève ici CsvHelperException.
                record = csv.Parser.Record;
            }
            catch (CsvHelperException ex)
            {
                throw new CorruptedSourceException(context.Source.DisplayName, ex);
            }

            var isEmptyRow = record is null || record.All(string.IsNullOrEmpty);

            if (record is not null && !(context.Options.SkipEmptyRows && isEmptyRow))
            {
                rowNumber++;
                // Le tampon interne de CsvHelper est réutilisé à chaque lecture : une copie
                // défensive est nécessaire pour que la ligne restituée reste valable après que
                // la lecture ait progressé (nécessaire à la sécurité, non une copie superflue).
                yield return new RawRow(rowNumber, headers, (string[])record.Clone());
            }

            try
            {
                hasAnyContent = await csv.ReadAsync().ConfigureAwait(false);
            }
            catch (CsvHelperException ex)
            {
                throw new CorruptedSourceException(context.Source.DisplayName, ex);
            }
        }
    }
}
