using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.UnitTests.ImportEngine.Validation;

/// <summary>
/// Aides de test minimales pour les tests unitaires du Validation Engine : construction rapide
/// de lignes brutes, de résultats de correspondance et de contextes de validation.
/// </summary>
internal static class ValidationTestHelpers
{
    public static RawRow Row(IReadOnlyList<string> headers, int number, params object?[] values) =>
        new(number, headers, values);

    public static async IAsyncEnumerable<RawRow> ToAsyncEnumerable(IEnumerable<RawRow> rows)
    {
        foreach (var row in rows)
        {
            yield return row;
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Construit un <see cref="MappingResult"/> minimal associant chaque clé canonique fournie à
    /// l'index de colonne correspondant, sans colonne manquante, inconnue ou ambiguë.
    /// </summary>
    public static MappingResult SimpleMappingResult(params (string CanonicalKey, int ColumnIndex)[] mappings)
    {
        var index = mappings.ToDictionary(m => m.CanonicalKey, m => m.ColumnIndex, StringComparer.Ordinal);
        var report = new MappingReport(
            RecognizedColumns: Array.Empty<ColumnMappingOutcome>(),
            UnknownColumns: Array.Empty<ColumnMappingOutcome>(),
            AmbiguousColumns: Array.Empty<ColumnMappingOutcome>(),
            MissingRequiredColumns: Array.Empty<CanonicalColumnDefinition>(),
            GlobalRecognitionScore: 100,
            Decisions: Array.Empty<string>());

        return new MappingResult(index, report);
    }

    public static ValidationContext CreateContext(
        SynonymDictionary dictionary,
        MappingResult mapping,
        ValidationOptions? options = null,
        ValidationConfiguration? configuration = null) => new()
        {
            Mapping = mapping,
            Dictionary = dictionary,
            Options = options ?? ValidationOptions.Default,
            Configuration = configuration ?? ValidationConfiguration.Default,
        };

    /// <summary>Construit un <see cref="MappingResult"/> exposant directement le <see cref="MappingReport"/> fourni, pour tester la traduction des constats structurels.</summary>
    public static MappingResult MappingResultWithReport(
        IReadOnlyDictionary<string, int> columnIndexByCanonicalKey,
        MappingReport report) => new(columnIndexByCanonicalKey, report);

    /// <summary>Construit un <see cref="ColumnMappingOutcome"/> minimal, sans candidat, pour les colonnes inconnues ou ambiguës.</summary>
    public static ColumnMappingOutcome Outcome(int columnIndex, string rawHeader, ColumnRecognitionStatus status) =>
        new(columnIndex, rawHeader, status, SelectedCandidate: null, AllCandidates: Array.Empty<ColumnMappingCandidate>());
}
