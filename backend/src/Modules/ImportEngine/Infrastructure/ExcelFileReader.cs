using System.Globalization;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Exceptions;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure;

/// <summary>
/// Lecteur de classeurs Excel (.xlsx) (features/IMPORT_ENGINE.md §2), fondé sur DocumentFormat.OpenXml,
/// le kit officiel de Microsoft pour le format Office Open XML (tech/DEPENDENCY_POLICY.md §3 —
/// éditeur du socle .NET lui-même, licence MIT, sans ambiguïté commerciale contrairement à
/// certaines bibliothèques tierces). Utilise l'API de lecture séquentielle <see cref="OpenXmlReader"/>
/// (approche SAX) : le contenu de la feuille est parcouru élément par élément sans jamais
/// matérialiser l'arbre XML complet en mémoire, condition nécessaire aux fichiers de plusieurs
/// millions de lignes. Le format .xlsx (archive Zip) exige néanmoins un flux permettant l'accès
/// aléatoire (<see cref="Stream.CanSeek"/>) pour la lecture de sa table des matières interne :
/// ceci est distinct d'un chargement intégral en mémoire, qui reste évité pour le contenu des
/// lignes elles-mêmes.
/// </summary>
public sealed class ExcelFileReader : FileReaderBase
{
    private static readonly string[] SupportedExtensions = [".xlsx"];

    /// <inheritdoc />
    public override FileFormat Format => FileFormat.Excel;

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
        if (!stream.CanSeek)
        {
            throw new CorruptedSourceException(
                context.Source.DisplayName,
                new NotSupportedException("La lecture d'un classeur Excel requiert un flux permettant l'accès aléatoire (Stream.CanSeek)."));
        }

        SpreadsheetDocument document;
        try
        {
            document = SpreadsheetDocument.Open(stream, isEditable: false);
        }
        catch (Exception ex) when (ex is OpenXmlPackageException or InvalidDataException or FileFormatException)
        {
            throw new CorruptedSourceException(context.Source.DisplayName, ex);
        }

        using (document)
        {
            var workbookPart = document.WorkbookPart
                ?? throw new CorruptedSourceException(context.Source.DisplayName, new InvalidDataException("Classeur sans partie WorkbookPart."));

            var sharedStrings = LoadSharedStrings(workbookPart);
            var worksheetPart = ResolveWorksheetPart(workbookPart, context.Options.SheetName, context.Source.DisplayName);

            using var reader = OpenXmlReader.Create(worksheetPart);

            IReadOnlyList<string> headers = Array.Empty<string>();
            var hasAnyStructuralContent = false;
            var rowNumber = 0;
            var isFirstRow = true;

            while (reader.Read())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (reader.ElementType != typeof(Row))
                {
                    continue;
                }

                hasAnyStructuralContent = true;
                var row = (Row)reader.LoadCurrentElement()!;
                var cellsByColumnIndex = ReadCellsByColumnIndex(row, sharedStrings);

                if (isFirstRow)
                {
                    isFirstRow = false;
                    if (context.Options.HasHeaderRow)
                    {
                        headers = BuildHeaderRow(cellsByColumnIndex);
                        continue;
                    }
                }

                var width = context.Options.HasHeaderRow ? headers.Count : (cellsByColumnIndex.Count == 0 ? 0 : cellsByColumnIndex.Keys.Max() + 1);
                var values = new object?[width];
                foreach (var (columnIndex, value) in cellsByColumnIndex)
                {
                    if (columnIndex < width)
                    {
                        values[columnIndex] = value;
                    }
                }

                var isEmptyRow = values.All(value => value is null);
                if (context.Options.SkipEmptyRows && isEmptyRow)
                {
                    continue;
                }

                rowNumber++;
                yield return new RawRow(rowNumber, headers, values);

                if (context.Options.MaxRows is { } maxRows && rowNumber >= maxRows)
                {
                    yield break;
                }

                // La lecture OpenXml est intégralement synchrone (l'API ne propose aucune
                // opération asynchrone). Un point de cession coopératif est inséré à intervalle
                // régulier — plutôt qu'à chaque ligne, pour limiter le coût de la cession elle-même
                // sur un fichier de plusieurs millions de lignes — afin que l'analyse d'un gros
                // classeur ne monopolise jamais le thread appelant (exigence UX : l'interface
                // utilisateur ne doit jamais être bloquée pendant une lecture).
                if (rowNumber % context.Configuration.ProgressReportIntervalRows == 0)
                {
                    await Task.Yield();
                }
            }

            ThrowIfEmpty(hasAnyStructuralContent, context);
        }
    }

    private static IReadOnlyList<string> BuildHeaderRow(SortedDictionary<int, object?> cellsByColumnIndex)
    {
        if (cellsByColumnIndex.Count == 0)
        {
            return Array.Empty<string>();
        }

        var width = cellsByColumnIndex.Keys.Max() + 1;
        var headers = new string[width];
        for (var i = 0; i < width; i++)
        {
            headers[i] = string.Empty;
        }

        foreach (var (columnIndex, value) in cellsByColumnIndex)
        {
            headers[columnIndex] = value?.ToString() ?? string.Empty;
        }

        return headers;
    }

    private static SortedDictionary<int, object?> ReadCellsByColumnIndex(Row row, IReadOnlyList<string> sharedStrings)
    {
        var result = new SortedDictionary<int, object?>();
        foreach (var cell in row.Elements<Cell>())
        {
            var reference = cell.CellReference?.Value;
            if (reference is null)
            {
                continue;
            }

            var columnIndex = GetColumnIndex(reference);
            result[columnIndex] = GetCellValue(cell, sharedStrings);
        }

        return result;
    }

    private static object? GetCellValue(Cell cell, IReadOnlyList<string> sharedStrings)
    {
        var rawText = cell.CellValue?.Text;

        if (cell.DataType?.Value == CellValues.SharedString)
        {
            return int.TryParse(rawText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index) && index >= 0 && index < sharedStrings.Count
                ? sharedStrings[index]
                : null;
        }

        if (cell.DataType?.Value == CellValues.InlineString)
        {
            return cell.InlineString?.Text?.Text;
        }

        if (cell.DataType?.Value == CellValues.Boolean)
        {
            return rawText == "1";
        }

        if (rawText is null)
        {
            return null;
        }

        if (cell.DataType is null || cell.DataType.Value == CellValues.Number)
        {
            return double.TryParse(rawText, NumberStyles.Float, CultureInfo.InvariantCulture, out var number)
                ? number
                : rawText;
        }

        // Erreur de formule (#DIV/0!, etc.) ou type non anticipé : restitué tel quel, sans
        // interprétation, conformément à l'absence d'hypothèse sur le contenu des colonnes.
        return rawText;
    }

    private static int GetColumnIndex(string cellReference)
    {
        var index = 0;
        foreach (var c in cellReference)
        {
            if (!char.IsLetter(c))
            {
                break;
            }

            index = (index * 26) + (char.ToUpperInvariant(c) - 'A' + 1);
        }

        return index - 1;
    }

    private static IReadOnlyList<string> LoadSharedStrings(WorkbookPart workbookPart)
    {
        var sharedStringPart = workbookPart.SharedStringTablePart;
        if (sharedStringPart?.SharedStringTable is null)
        {
            return Array.Empty<string>();
        }

        return sharedStringPart.SharedStringTable.Elements<SharedStringItem>()
            .Select(item => item.InnerText)
            .ToArray();
    }

    private static WorksheetPart ResolveWorksheetPart(WorkbookPart workbookPart, string? sheetName, string sourceDisplayName)
    {
        var sheets = workbookPart.Workbook?.Sheets?.Elements<Sheet>().ToList() ?? [];
        var sheet = sheetName is null
            ? sheets.FirstOrDefault()
            : sheets.FirstOrDefault(s => string.Equals(s.Name?.Value, sheetName, StringComparison.OrdinalIgnoreCase));

        if (sheet?.Id?.Value is null)
        {
            throw new CorruptedSourceException(sourceDisplayName, new InvalidDataException("Aucune feuille exploitable n'a été trouvée dans le classeur."));
        }

        return (WorksheetPart)workbookPart.GetPartById(sheet.Id.Value);
    }
}
