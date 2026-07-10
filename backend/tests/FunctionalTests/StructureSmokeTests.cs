using Xunit;

namespace OrbitaAI.FunctionalTests;

/// <summary>
/// Réservé aux tests fonctionnels (tech/TESTING_STRATEGY.md §4) : vérification de bout en
/// bout d'un comportement métier complet décrit dans features/. Aucun scénario fonctionnel
/// réel n'est écrit à ce stade, aucune fonctionnalité n'étant encore implémentée
/// (squelette architectural uniquement, cf. planning/DEVELOPMENT_PHASES.md).
/// </summary>
public sealed class StructureSmokeTests
{
    [Fact]
    public void Placeholder_ProjectCompilesAndRuns()
    {
        Assert.True(true);
    }
}
