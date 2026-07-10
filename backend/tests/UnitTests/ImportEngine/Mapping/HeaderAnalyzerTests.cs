using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Mapping;

public sealed class HeaderAnalyzerTests
{
    private readonly HeaderAnalyzer _analyzer = new(new HeaderNormalizer());
    private readonly SynonymDictionary _dictionary = DefaultSynonymDictionary.Create();
    private readonly MappingConfiguration _configuration = MappingConfiguration.Default;

    [Fact]
    public void AnalyzeHeader_ExactCanonicalName_ScoresOneHundred()
    {
        var candidates = _analyzer.AnalyzeHeader("Client", _dictionary, _configuration);

        var best = Assert.Single(candidates);
        Assert.Equal("Client", best.CanonicalKey);
        Assert.Equal(100, best.ConfidenceScore);
        Assert.NotEmpty(best.Reasons);
    }

    [Theory]
    [InlineData("Nom client")]
    [InlineData("Raison sociale")]
    [InlineData("Customer")]
    [InlineData("Company")]
    [InlineData("Compte")]
    [InlineData("Code client")]
    [InlineData("Client Name")]
    public void AnalyzeHeader_KnownSynonym_RecognizesClientWithHighConfidence(string header)
    {
        var candidates = _analyzer.AnalyzeHeader(header, _dictionary, _configuration);

        var best = candidates.First();
        Assert.Equal("Client", best.CanonicalKey);
        Assert.True(best.ConfidenceScore >= _configuration.RecognizedThreshold, $"Score inattendu pour '{header}': {best.ConfidenceScore}");
    }

    [Theory]
    [InlineData("Sales Rep")]
    [InlineData("Vendeur")]
    [InlineData("Responsable")]
    public void AnalyzeHeader_KnownSynonym_RecognizesCommercial(string header)
    {
        var candidates = _analyzer.AnalyzeHeader(header, _dictionary, _configuration);

        Assert.Equal("Commercial", candidates.First().CanonicalKey);
    }

    [Theory]
    [InlineData("CA")]
    [InlineData("Net Sales")]
    [InlineData("Revenue")]
    [InlineData("Montant HT")]
    public void AnalyzeHeader_KnownSynonym_RecognizesMontantHT(string header)
    {
        var candidates = _analyzer.AnalyzeHeader(header, _dictionary, _configuration);

        Assert.Equal("MontantHT", candidates.First().CanonicalKey);
    }

    [Theory]
    [InlineData("Famille Produit")]
    [InlineData("Product Family")]
    public void AnalyzeHeader_KnownSynonym_RecognizesFamille(string header)
    {
        var candidates = _analyzer.AnalyzeHeader(header, _dictionary, _configuration);

        Assert.Equal("Famille", candidates.First().CanonicalKey);
    }

    [Theory]
    [InlineData("Date facture")]
    [InlineData("Invoice Date")]
    public void AnalyzeHeader_KnownSynonym_RecognizesDate(string header)
    {
        var candidates = _analyzer.AnalyzeHeader(header, _dictionary, _configuration);

        Assert.Equal("Date", candidates.First().CanonicalKey);
    }

    [Fact]
    public void AnalyzeHeader_TrivialVariant_ScoresNinetyFive()
    {
        // "Clients" (pluriel) est une variante triviale de "Client".
        var candidates = _analyzer.AnalyzeHeader("Clients", _dictionary, _configuration);

        var best = candidates.First();
        Assert.Equal("Client", best.CanonicalKey);
        Assert.Equal(95, best.ConfidenceScore);
    }

    [Fact]
    public void AnalyzeHeader_HeaderContainingSynonym_ScoresEighty()
    {
        // "Famille Produit Principale" contient entièrement les mots de "Famille Produit".
        var candidates = _analyzer.AnalyzeHeader("Famille Produit Principale", _dictionary, _configuration);

        var best = candidates.First();
        Assert.Equal("Famille", best.CanonicalKey);
        Assert.Equal(80, best.ConfidenceScore);
    }

    [Fact]
    public void AnalyzeHeader_CompletelyUnrelatedHeader_ReturnsNoCandidate()
    {
        var candidates = _analyzer.AnalyzeHeader("Numero de telephone", _dictionary, _configuration);

        Assert.Empty(candidates);
    }

    [Fact]
    public void AnalyzeHeader_IsCaseInsensitive()
    {
        var candidates = _analyzer.AnalyzeHeader("CLIENT", _dictionary, _configuration);

        var best = candidates.First();
        Assert.Equal("Client", best.CanonicalKey);
        Assert.Equal(100, best.ConfidenceScore);
    }

    [Fact]
    public void AnalyzeHeader_IsAccentInsensitive()
    {
        // "Année" est le libellé canonique ; "Annee" (sans accent) doit être reconnu à l'identique.
        var candidates = _analyzer.AnalyzeHeader("Annee", _dictionary, _configuration);

        var best = candidates.First();
        Assert.Equal("Annee", best.CanonicalKey);
        Assert.Equal(100, best.ConfidenceScore);
    }

    [Fact]
    public void AnalyzeHeader_ResultsAreOrderedByDescendingConfidence()
    {
        var candidates = _analyzer.AnalyzeHeader("Client", _dictionary, _configuration);

        Assert.Equal(candidates.OrderByDescending(c => c.ConfidenceScore).Select(c => c.CanonicalKey), candidates.Select(c => c.CanonicalKey));
    }
}
