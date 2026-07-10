using System.Globalization;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OrbitaAI.UnitTests.ImportEngine;

/// <summary>
/// Aides de test minimales pour générer des sources CSV et Excel synthétiques, sans dépendre
/// d'aucun fichier réel ni d'aucune bibliothèque d'écriture supplémentaire (DocumentFormat.OpenXml
/// est déjà référencé par le module ImportEngine pour la lecture).
/// </summary>
internal static class TestHelpers
{
    public static MemoryStream ToStream(string content, Encoding? encoding = null) =>
        new((encoding ?? Encoding.UTF8).GetBytes(content));

    /// <summary>
    /// Construit un classeur Excel minimal en mémoire, une feuille nommée <paramref name="sheetName"/>,
    /// à partir de lignes de valeurs hétérogènes (chaîne ou nombre), en s'appuyant sur une
    /// véritable table de chaînes partagées pour les valeurs textuelles (comportement fidèle à
    /// un fichier réellement produit par Excel).
    /// </summary>
    public static MemoryStream CreateXlsx(IReadOnlyList<IReadOnlyList<object?>> rows, string sheetName = "Sheet1")
    {
        var stream = new MemoryStream();
        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var sharedStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
            sharedStringPart.SharedStringTable = new SharedStringTable();
            var sharedStrings = new List<string>();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            sheets.Append(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = sheetName,
            });

            for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var rowValues = rows[rowIndex];
                var row = new Row();
                for (var columnIndex = 0; columnIndex < rowValues.Count; columnIndex++)
                {
                    var value = rowValues[columnIndex];
                    if (value is null)
                    {
                        continue; // Cellule volontairement absente, comme dans un vrai fichier Excel.
                    }

                    var cellReference = GetCellReference(columnIndex, rowIndex);
                    Cell cell;
                    if (value is double or int)
                    {
                        var numeric = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                        cell = new Cell
                        {
                            CellReference = cellReference,
                            DataType = CellValues.Number,
                            CellValue = new CellValue(numeric.ToString(CultureInfo.InvariantCulture)),
                        };
                    }
                    else if (value is bool boolean)
                    {
                        cell = new Cell
                        {
                            CellReference = cellReference,
                            DataType = CellValues.Boolean,
                            CellValue = new CellValue(boolean ? "1" : "0"),
                        };
                    }
                    else
                    {
                        var text = value.ToString() ?? string.Empty;
                        var sharedIndex = sharedStrings.IndexOf(text);
                        if (sharedIndex < 0)
                        {
                            sharedStrings.Add(text);
                            sharedIndex = sharedStrings.Count - 1;
                        }

                        cell = new Cell
                        {
                            CellReference = cellReference,
                            DataType = CellValues.SharedString,
                            CellValue = new CellValue(sharedIndex.ToString(CultureInfo.InvariantCulture)),
                        };
                    }

                    row.Append(cell);
                }

                sheetData.Append(row);
            }

            foreach (var text in sharedStrings)
            {
                sharedStringPart.SharedStringTable.Append(new SharedStringItem(new Text(text)));
            }

            workbookPart.Workbook.Save();
            worksheetPart.Worksheet.Save();
            sharedStringPart.SharedStringTable.Save();
        }

        stream.Position = 0;
        return stream;
    }

    /// <summary>Construit un classeur Excel volontairement dépourvu de toute ligne (feuille vide).</summary>
    public static MemoryStream CreateEmptyXlsx()
    {
        var stream = new MemoryStream();
        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            sheets.Append(new Sheet { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" });
            workbookPart.Workbook.Save();
            worksheetPart.Worksheet.Save();
        }

        stream.Position = 0;
        return stream;
    }

    private static string GetCellReference(int columnIndex, int rowIndex)
    {
        var column = string.Empty;
        var value = columnIndex + 1;
        while (value > 0)
        {
            var remainder = (value - 1) % 26;
            column = (char)('A' + remainder) + column;
            value = (value - 1) / 26;
        }

        return $"{column}{rowIndex + 1}";
    }
}
