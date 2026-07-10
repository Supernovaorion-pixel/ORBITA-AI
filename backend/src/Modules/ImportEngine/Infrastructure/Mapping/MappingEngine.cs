using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping.Exceptions;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;

/// <summary>
/// Implémentation par défaut de <see cref="IMappingEngine"/> : orchestre
/// <see cref="IColumnAnalyzer"/>, <see cref="IHeaderAnalyzer"/> et <see cref="IConfidenceEngine"/>
/// pour produire un <see cref="MappingResult"/> intégralement traçable. N'effectue aucune
/// correction automatique cachée : toute ambiguïté (score intermédiaire, ou plusieurs colonnes
/// revendiquant le même champ canonique) est signalée dans le rapport plutôt que résolue
/// silencieusement.
/// </summary>
public sealed class MappingEngine : IMappingEngine
{
    private readonly IColumnAnalyzer _columnAnalyzer;
    private readonly IHeaderAnalyzer _headerAnalyzer;
    private readonly IConfidenceEngine _confidenceEngine;

    public MappingEngine(IColumnAnalyzer columnAnalyzer, IHeaderAnalyzer headerAnalyzer, IConfidenceEngine confidenceEngine)
    {
        _columnAnalyzer = Guard.Against.Null(columnAnalyzer, nameof(columnAnalyzer));
        _headerAnalyzer = Guard.Against.Null(headerAnalyzer, nameof(headerAnalyzer));
        _confidenceEngine = Guard.Against.Null(confidenceEngine, nameof(confidenceEngine));
    }

    /// <inheritdoc />
    public MappingResult Analyze(IReadOnlyList<string> headers, IReadOnlyList<RawRow> sampleRows, MappingContext context)
    {
        Guard.Against.Null(headers, nameof(headers));
        Guard.Against.Null(sampleRows, nameof(sampleRows));
        Guard.Against.Null(context, nameof(context));

        if (headers.Count == 0)
        {
            throw new NoHeaderRowException();
        }

        var dictionary = context.EffectiveDictionary;
        var profiles = _columnAnalyzer.AnalyzeColumns(headers, sampleRows, context.Configuration, context.Options.AnalyzeContent);

        var decisions = new List<string>();
        var outcomes = new ColumnMappingOutcome[headers.Count];

        for (var columnIndex = 0; columnIndex < headers.Count; columnIndex++)
        {
            outcomes[columnIndex] = AnalyzeSingleColumn(headers[columnIndex], columnIndex, profiles[columnIndex], dictionary, context, decisions);
        }

        ResolveDuplicateClaims(outcomes, decisions);

        var recognized = outcomes.Where(o => o.Status == ColumnRecognitionStatus.Recognized).ToArray();
        var unknown = outcomes.Where(o => o.Status == ColumnRecognitionStatus.Unknown).ToArray();
        var ambiguous = outcomes.Where(o => o.Status == ColumnRecognitionStatus.Ambiguous).ToArray();

        var recognizedKeys = new HashSet<string>(recognized.Select(o => o.SelectedCandidate!.CanonicalKey), StringComparer.Ordinal);
        var missingRequired = dictionary.RequiredColumns.Where(c => !recognizedKeys.Contains(c.Key)).ToArray();
        foreach (var missing in missingRequired)
        {
            decisions.Add($"Colonne obligatoire « {missing.DisplayName} » non trouvée dans le fichier.");
        }

        var globalScore = ComputeGlobalRecognitionScore(dictionary, recognizedKeys);
        var report = new MappingReport(recognized, unknown, ambiguous, missingRequired, globalScore, decisions);
        var columnIndexByCanonicalKey = recognized.ToDictionary(o => o.SelectedCandidate!.CanonicalKey, o => o.ColumnIndex, StringComparer.Ordinal);

        return new MappingResult(columnIndexByCanonicalKey, report);
    }

    private ColumnMappingOutcome AnalyzeSingleColumn(
        string header,
        int columnIndex,
        ColumnProfile profile,
        SynonymDictionary dictionary,
        MappingContext context,
        List<string> decisions)
    {
        var nameBasedCandidates = _headerAnalyzer.AnalyzeHeader(header, dictionary, context.Configuration);

        var adjustedCandidates = nameBasedCandidates
            .Select(candidate => context.Options.AnalyzeContent
                ? _confidenceEngine.ApplyContentConfirmation(candidate, profile, FindCanonical(dictionary, candidate.CanonicalKey), context.Configuration)
                : candidate)
            .OrderByDescending(c => c.ConfidenceScore)
            .ToArray();

        var viableCandidates = adjustedCandidates.Where(c => c.ConfidenceScore >= context.Configuration.AmbiguousThreshold).ToArray();

        if (viableCandidates.Length == 0)
        {
            decisions.Add($"Colonne {columnIndex} « {header} » : aucune correspondance connue — colonne non reconnue.");
            return new ColumnMappingOutcome(columnIndex, header, ColumnRecognitionStatus.Unknown, null, adjustedCandidates);
        }

        var best = viableCandidates[0];
        // Une réelle ambiguïté entre deux colonnes canoniques ne survient que si le second
        // candidat reste à la fois au-dessus du seuil de reconnaissance ET suffisamment proche du
        // premier (cf. MappingConfiguration.TieMarginPoints) : un candidat secondaire nettement
        // distancé (ex. 80 face à 100) ne remet pas en cause le premier.
        var isTiedAtRecognizedLevel = viableCandidates.Length > 1
            && viableCandidates[1].ConfidenceScore >= context.Configuration.RecognizedThreshold
            && best.ConfidenceScore >= context.Configuration.RecognizedThreshold
            && (best.ConfidenceScore - viableCandidates[1].ConfidenceScore) <= context.Configuration.TieMarginPoints;

        if (best.ConfidenceScore >= context.Configuration.RecognizedThreshold && !isTiedAtRecognizedLevel)
        {
            decisions.Add($"Colonne {columnIndex} « {header} » → « {best.CanonicalKey} » ({best.ConfidenceScore:N0} %).");
            return new ColumnMappingOutcome(columnIndex, header, ColumnRecognitionStatus.Recognized, best, adjustedCandidates);
        }

        var candidateSummary = string.Join(", ", viableCandidates.Select(c => $"{c.CanonicalKey} ({c.ConfidenceScore:N0} %)"));
        decisions.Add($"Colonne {columnIndex} « {header} » : ambiguë — candidats envisagés : {candidateSummary}.");
        return new ColumnMappingOutcome(columnIndex, header, ColumnRecognitionStatus.Ambiguous, null, adjustedCandidates);
    }

    /// <summary>
    /// Lorsque plusieurs colonnes source revendiquent, chacune avec une confiance suffisante, le
    /// même champ canonique (colonnes dupliquées), aucune n'est retenue automatiquement : toutes
    /// basculent en ambiguës, la décision revenant à un utilisateur habilité.
    /// </summary>
    private static void ResolveDuplicateClaims(ColumnMappingOutcome[] outcomes, List<string> decisions)
    {
        var duplicateGroups = outcomes
            .Where(o => o.Status == ColumnRecognitionStatus.Recognized)
            .GroupBy(o => o.SelectedCandidate!.CanonicalKey, StringComparer.Ordinal)
            .Where(g => g.Count() > 1)
            .ToArray();

        foreach (var group in duplicateGroups)
        {
            var columnIndexes = string.Join(", ", group.Select(o => o.ColumnIndex));
            decisions.Add(
                $"Colonnes dupliquées pour « {group.Key} » (colonnes {columnIndexes}) : " +
                "aucune correction automatique n'est appliquée, une décision manuelle est requise.");

            foreach (var outcome in group)
            {
                var index = Array.IndexOf(outcomes, outcome);
                outcomes[index] = outcome with { Status = ColumnRecognitionStatus.Ambiguous, SelectedCandidate = null };
            }
        }
    }

    private static CanonicalColumnDefinition FindCanonical(SynonymDictionary dictionary, string key) =>
        dictionary.Columns.First(c => string.Equals(c.Key, key, StringComparison.Ordinal));

    /// <summary>
    /// Score global pondérant deux fois plus une colonne obligatoire reconnue qu'une colonne
    /// optionnelle, reflétant l'importance relative des deux catégories dans la fiabilité globale
    /// du fichier importé.
    /// </summary>
    private static double ComputeGlobalRecognitionScore(SynonymDictionary dictionary, HashSet<string> recognizedKeys)
    {
        var requiredKeys = dictionary.Columns.Where(c => c.IsRequired).Select(c => c.Key).ToArray();
        var optionalKeys = dictionary.Columns.Where(c => !c.IsRequired).Select(c => c.Key).ToArray();

        var recognizedRequiredCount = requiredKeys.Count(recognizedKeys.Contains);
        var recognizedOptionalCount = optionalKeys.Count(recognizedKeys.Contains);

        var denominator = (requiredKeys.Length * 2) + optionalKeys.Length;
        if (denominator == 0)
        {
            return 100;
        }

        var numerator = (recognizedRequiredCount * 2) + recognizedOptionalCount;
        return (double)numerator / denominator * 100;
    }
}
