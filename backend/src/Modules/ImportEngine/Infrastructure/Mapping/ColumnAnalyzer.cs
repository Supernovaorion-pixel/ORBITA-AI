using System.Globalization;
using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;

/// <summary>
/// Implémentation par défaut de <see cref="IColumnAnalyzer"/>. Parcourt une seule fois
/// l'échantillon fourni (jamais le fichier complet, jamais une seconde lecture) pour construire,
/// colonne par colonne, un <see cref="ColumnProfile"/> indépendant des autres — condition du
/// futur traitement parallèle (architecture/PERFORMANCE_GUIDELINES.md).
/// </summary>
public sealed class ColumnAnalyzer : IColumnAnalyzer
{
    /// <inheritdoc />
    public IReadOnlyList<ColumnProfile> AnalyzeColumns(
        IReadOnlyList<string> headers,
        IReadOnlyList<RawRow> sampleRows,
        MappingConfiguration configuration,
        bool analyzeContent)
    {
        Guard.Against.Null(headers, nameof(headers));
        Guard.Against.Null(sampleRows, nameof(sampleRows));
        Guard.Against.Null(configuration, nameof(configuration));

        var boundedSample = sampleRows.Count > configuration.MaxContentSampleRows
            ? sampleRows.Take(configuration.MaxContentSampleRows)
            : sampleRows;
        var sample = boundedSample as IReadOnlyList<RawRow> ?? boundedSample.ToArray();

        var profiles = new ColumnProfile[headers.Count];
        for (var columnIndex = 0; columnIndex < headers.Count; columnIndex++)
        {
            var statistics = analyzeContent
                ? BuildStatistics(sample, columnIndex)
                : ColumnStatistics.Empty;

            profiles[columnIndex] = new ColumnProfile(columnIndex, headers[columnIndex], statistics);
        }

        return profiles;
    }

    private static ColumnStatistics BuildStatistics(IReadOnlyList<RawRow> sample, int columnIndex)
    {
        var nonEmptyCount = 0;
        var numericCount = 0;
        var dateCount = 0;
        var distinctValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in sample)
        {
            if (columnIndex >= row.Values.Count)
            {
                continue;
            }

            var value = row.Values[columnIndex];
            if (value is null || (value is string s && string.IsNullOrWhiteSpace(s)))
            {
                continue;
            }

            nonEmptyCount++;

            if (IsNumeric(value))
            {
                numericCount++;
            }
            else if (IsDate(value))
            {
                dateCount++;
            }

            distinctValues.Add(value.ToString() ?? string.Empty);
        }

        return new ColumnStatistics(sample.Count, nonEmptyCount, numericCount, dateCount, distinctValues.Count);
    }

    private static bool IsNumeric(object value) => value switch
    {
        double or int or long or float or decimal => true,
        string text => double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _),
        _ => false,
    };

    private static bool IsDate(object value) => value switch
    {
        DateTime or DateTimeOffset => true,
        string text => !IsNumeric(text)
            && DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out _),
        _ => false,
    };
}
