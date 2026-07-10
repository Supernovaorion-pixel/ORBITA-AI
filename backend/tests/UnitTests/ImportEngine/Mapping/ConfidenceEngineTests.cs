using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Mapping;

public sealed class ConfidenceEngineTests
{
    private readonly ConfidenceEngine _engine = new();
    private readonly MappingConfiguration _configuration = MappingConfiguration.Default;

    private static readonly CanonicalColumnDefinition NumericColumn =
        CanonicalColumnDefinition.Create("MontantHT", "Montant HT", expectedValueKind: ColumnValueKind.Numeric);

    private static readonly CanonicalColumnDefinition AnyColumn =
        CanonicalColumnDefinition.Create("Client", "Client", expectedValueKind: ColumnValueKind.Any);

    [Fact]
    public void ApplyContentConfirmation_ContentMatchesExpectedKind_KeepsScoreUnchanged()
    {
        var candidate = new ColumnMappingCandidate("MontantHT", 100, ["Correspondance exacte."]);
        var profile = new ColumnProfile(0, "Montant HT", new ColumnStatistics(10, 10, 10, 0, 10));

        var result = _engine.ApplyContentConfirmation(candidate, profile, NumericColumn, _configuration);

        Assert.Equal(100, result.ConfidenceScore);
        Assert.True(result.Reasons.Count > candidate.Reasons.Count);
    }

    [Fact]
    public void ApplyContentConfirmation_ContentContradictsExpectedKind_ReducesScore()
    {
        var candidate = new ColumnMappingCandidate("MontantHT", 80, ["Correspondance partielle."]);
        // Colonne cense contenir des montants mais entièrement composée de texte.
        var profile = new ColumnProfile(0, "Montant", new ColumnStatistics(10, 10, 0, 0, 10));

        var result = _engine.ApplyContentConfirmation(candidate, profile, NumericColumn, _configuration);

        Assert.Equal(80 - _configuration.ContentMismatchPenaltyPoints, result.ConfidenceScore);
        Assert.Contains(result.Reasons, r => r.Contains("réduite"));
    }

    [Fact]
    public void ApplyContentConfirmation_ScoreNeverGoesBelowZero()
    {
        var candidate = new ColumnMappingCandidate("MontantHT", 10, ["Ressemblance faible."]);
        var profile = new ColumnProfile(0, "Montant", new ColumnStatistics(10, 10, 0, 0, 10));
        var configuration = _configuration with { ContentMismatchPenaltyPoints = 50 };

        var result = _engine.ApplyContentConfirmation(candidate, profile, NumericColumn, configuration);

        Assert.Equal(0, result.ConfidenceScore);
    }

    [Fact]
    public void ApplyContentConfirmation_ExpectedKindAny_NeverAdjustsScore()
    {
        var candidate = new ColumnMappingCandidate("Client", 100, ["Correspondance exacte."]);
        var profile = new ColumnProfile(0, "Client", new ColumnStatistics(10, 10, 10, 0, 10)); // tout numérique, pourtant "Any".

        var result = _engine.ApplyContentConfirmation(candidate, profile, AnyColumn, _configuration);

        Assert.Same(candidate, result);
    }

    [Fact]
    public void ApplyContentConfirmation_InsufficientSample_NeverAdjustsScore()
    {
        var candidate = new ColumnMappingCandidate("MontantHT", 80, ["Correspondance partielle."]);
        var profile = new ColumnProfile(0, "Montant", new ColumnStatistics(2, 2, 0, 0, 2)); // échantillon trop petit.

        var result = _engine.ApplyContentConfirmation(candidate, profile, NumericColumn, _configuration);

        Assert.Same(candidate, result);
    }

    [Fact]
    public void ApplyContentConfirmation_NeverIncreasesScore()
    {
        var candidate = new ColumnMappingCandidate("MontantHT", 60, ["Ressemblance approximative."]);
        var profile = new ColumnProfile(0, "Montant HT", new ColumnStatistics(10, 10, 10, 0, 10)); // contenu parfaitement cohérent.

        var result = _engine.ApplyContentConfirmation(candidate, profile, NumericColumn, _configuration);

        Assert.Equal(60, result.ConfidenceScore); // jamais > au score initial fondé sur le nom.
    }
}
