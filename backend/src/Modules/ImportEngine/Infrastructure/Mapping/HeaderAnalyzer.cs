using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;

/// <summary>
/// Implémentation par défaut de <see cref="IHeaderAnalyzer"/> : une échelle de règles
/// déterministes et explicables, appliquée dans un ordre strict, du plus certain au moins
/// certain (aucune IA, aucun synonyme codé en dur — seul <see cref="SynonymDictionary"/> porte
/// cette connaissance) :
///
/// 1. Correspondance exacte du libellé normalisé (100 %) ;
/// 2. Variante triviale — mots vides ou pluriel ignorés (95 %) ;
/// 3. Le libellé contient un synonyme connu, ou l'inverse (80 %) ;
/// 4. Ressemblance approximative par distance d'édition (60 %) ;
/// 5. Aucune règle ne s'applique : la colonne canonique n'est pas un candidat.
/// </summary>
public sealed class HeaderAnalyzer : IHeaderAnalyzer
{
    private static readonly HashSet<string> StopWords = new(StringComparer.Ordinal)
    {
        "de", "du", "des", "la", "le", "les", "l", "et", "the", "of", "a", "an",
    };

    private readonly IHeaderNormalizer _normalizer;

    public HeaderAnalyzer(IHeaderNormalizer normalizer)
    {
        _normalizer = Guard.Against.Null(normalizer, nameof(normalizer));
    }

    /// <inheritdoc />
    public IReadOnlyList<ColumnMappingCandidate> AnalyzeHeader(string rawHeader, SynonymDictionary dictionary, MappingConfiguration configuration)
    {
        Guard.Against.Null(rawHeader, nameof(rawHeader));
        Guard.Against.Null(dictionary, nameof(dictionary));
        Guard.Against.Null(configuration, nameof(configuration));

        var normalizedHeader = _normalizer.Normalize(rawHeader);
        var looseHeader = LooseNormalize(normalizedHeader);

        var candidates = new List<ColumnMappingCandidate>();

        foreach (var canonical in dictionary.Columns)
        {
            var best = FindBestMatch(normalizedHeader, looseHeader, canonical, configuration);
            if (best is not null && best.ConfidenceScore >= configuration.AmbiguousThreshold)
            {
                candidates.Add(best);
            }
        }

        return candidates.OrderByDescending(c => c.ConfidenceScore).ToArray();
    }

    private ColumnMappingCandidate? FindBestMatch(
        string normalizedHeader,
        string looseHeader,
        CanonicalColumnDefinition canonical,
        MappingConfiguration configuration)
    {
        double bestScore = 0;
        string? bestReason = null;

        foreach (var label in canonical.AllKnownLabels())
        {
            var normalizedLabel = _normalizer.Normalize(label);

            if (string.Equals(normalizedHeader, normalizedLabel, StringComparison.Ordinal))
            {
                Consider(100, $"Correspondance exacte avec « {label} ».");
                continue;
            }

            var looseLabel = LooseNormalize(normalizedLabel);
            if (string.Equals(looseHeader, looseLabel, StringComparison.Ordinal))
            {
                Consider(95, $"Variante triviale de « {label} » (mots vides ou pluriel ignorés).");
                continue;
            }

            if (SharesAllWordsWith(normalizedHeader, normalizedLabel))
            {
                Consider(80, $"Le libellé recoupe entièrement le synonyme connu « {label} ».");
                continue;
            }

            var similarity = ComputeSimilarity(normalizedHeader, normalizedLabel);
            if (similarity >= configuration.FuzzyMatchMinimumSimilarity)
            {
                Consider(60, $"Ressemblance approximative avec « {label} » (similarité {similarity:P0}).");
            }
        }

        return bestReason is null ? null : new ColumnMappingCandidate(canonical.Key, bestScore, [bestReason]);

        void Consider(double score, string reason)
        {
            if (score > bestScore)
            {
                bestScore = score;
                bestReason = reason;
            }
        }
    }

    private static string LooseNormalize(string normalizedHeader)
    {
        var tokens = normalizedHeader
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(token => !StopWords.Contains(token))
            .Select(DeSingularize);

        return string.Join(' ', tokens);
    }

    private static string DeSingularize(string token) =>
        token.Length > 3 && token.EndsWith('s') ? token[..^1] : token;

    /// <summary>
    /// Vrai si l'ensemble des mots du plus court des deux libellés se retrouve intégralement
    /// dans l'autre (ex. « famille produit » recoupe entièrement « famille »).
    /// </summary>
    private static bool SharesAllWordsWith(string first, string second)
    {
        var firstTokens = first.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var secondTokens = second.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (firstTokens.Length == 0 || secondTokens.Length == 0)
        {
            return false;
        }

        var (shorter, longer) = firstTokens.Length <= secondTokens.Length
            ? (firstTokens, secondTokens)
            : (secondTokens, firstTokens);

        var longerSet = new HashSet<string>(longer, StringComparer.Ordinal);
        return shorter.All(longerSet.Contains);
    }

    /// <summary>Similarité de 0 à 1 fondée sur la distance d'édition de Levenshtein normalisée par la longueur.</summary>
    private static double ComputeSimilarity(string first, string second)
    {
        if (first.Length == 0 && second.Length == 0)
        {
            return 1;
        }

        var distance = LevenshteinDistance(first, second);
        var maxLength = Math.Max(first.Length, second.Length);
        return maxLength == 0 ? 1 : 1 - (double)distance / maxLength;
    }

    private static int LevenshteinDistance(string first, string second)
    {
        var previousRow = new int[second.Length + 1];
        var currentRow = new int[second.Length + 1];

        for (var j = 0; j <= second.Length; j++)
        {
            previousRow[j] = j;
        }

        for (var i = 1; i <= first.Length; i++)
        {
            currentRow[0] = i;
            for (var j = 1; j <= second.Length; j++)
            {
                var cost = first[i - 1] == second[j - 1] ? 0 : 1;
                currentRow[j] = Math.Min(
                    Math.Min(currentRow[j - 1] + 1, previousRow[j] + 1),
                    previousRow[j - 1] + cost);
            }

            (previousRow, currentRow) = (currentRow, previousRow);
        }

        return previousRow[second.Length];
    }
}
