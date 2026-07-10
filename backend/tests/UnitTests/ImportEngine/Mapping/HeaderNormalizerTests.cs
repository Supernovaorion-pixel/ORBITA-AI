using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Mapping;

public sealed class HeaderNormalizerTests
{
    private readonly HeaderNormalizer _normalizer = new();

    [Theory]
    [InlineData("Client", "client")]
    [InlineData("  Client  ", "client")]
    [InlineData("CLIENT", "client")]
    [InlineData("Nom_Client", "nom client")]
    [InlineData("Nom-Client", "nom client")]
    [InlineData("Raison Sociale", "raison sociale")]
    [InlineData("Année", "annee")]
    [InlineData("% Marge", "marge")]
    [InlineData("Date facture", "date facture")]
    public void Normalize_ProducesExpectedForm(string input, string expected)
    {
        Assert.Equal(expected, _normalizer.Normalize(input));
    }

    [Fact]
    public void Normalize_CollapsesMultipleSeparators()
    {
        Assert.Equal("nom client", _normalizer.Normalize("Nom   ___   Client"));
    }

    [Fact]
    public void Normalize_OfEmptyString_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, _normalizer.Normalize(string.Empty));
    }

    [Fact]
    public void Normalize_WithNullInput_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _normalizer.Normalize(null!));
    }
}
