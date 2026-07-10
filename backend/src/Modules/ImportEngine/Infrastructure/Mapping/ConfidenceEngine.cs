using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;

/// <summary>
/// Implémentation par défaut de <see cref="IConfidenceEngine"/>. N'agit jamais qu'à la baisse :
/// un contenu de colonne qui contredit fortement la nature attendue
/// (<see cref="CanonicalColumnDefinition.ExpectedValueKind"/>) réduit la confiance d'un candidat
/// déjà identifié par son nom ; un contenu cohérent la confirme sans jamais l'augmenter au-delà
/// de ce que le nom a déjà établi (le contenu ne permet jamais, à lui seul, de reconnaître une
/// colonne — seul le nom le peut, cf. <see cref="IHeaderAnalyzer"/>).
/// </summary>
public sealed class ConfidenceEngine : IConfidenceEngine
{
    /// <summary>Nombre minimal de valeurs non vides requis pour qu'un constat de contenu soit jugé significatif.</summary>
    private const int MinimumSignificantSampleSize = 5;

    /// <inheritdoc />
    public ColumnMappingCandidate ApplyContentConfirmation(
        ColumnMappingCandidate nameBasedCandidate,
        ColumnProfile profile,
        CanonicalColumnDefinition canonicalColumn,
        MappingConfiguration configuration)
    {
        Guard.Against.Null(nameBasedCandidate, nameof(nameBasedCandidate));
        Guard.Against.Null(profile, nameof(profile));
        Guard.Against.Null(canonicalColumn, nameof(canonicalColumn));
        Guard.Against.Null(configuration, nameof(configuration));

        if (canonicalColumn.ExpectedValueKind == ColumnValueKind.Any
            || profile.Statistics.NonEmptyCount < MinimumSignificantSampleSize)
        {
            return nameBasedCandidate;
        }

        var isMismatch = canonicalColumn.ExpectedValueKind switch
        {
            ColumnValueKind.Numeric => profile.Statistics.NumericRatio < 0.5,
            ColumnValueKind.Date => profile.Statistics.DateRatio < 0.5,
            ColumnValueKind.Text => profile.Statistics.NumericRatio > 0.5,
            _ => false,
        };

        if (!isMismatch)
        {
            var confirmedReasons = nameBasedCandidate.Reasons
                .Append($"Le contenu observé est cohérent avec le type attendu ({canonicalColumn.ExpectedValueKind}).")
                .ToArray();
            return nameBasedCandidate with { Reasons = confirmedReasons };
        }

        var adjustedScore = Math.Max(0, nameBasedCandidate.ConfidenceScore - configuration.ContentMismatchPenaltyPoints);
        var adjustedReasons = nameBasedCandidate.Reasons
            .Append(
                $"Confiance réduite de {configuration.ContentMismatchPenaltyPoints:N0} points : le contenu observé " +
                $"ne correspond pas au type attendu ({canonicalColumn.ExpectedValueKind}).")
            .ToArray();

        return nameBasedCandidate with { ConfidenceScore = adjustedScore, Reasons = adjustedReasons };
    }
}
