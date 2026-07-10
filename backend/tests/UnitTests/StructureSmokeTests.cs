using OrbitaAI.Core.Domain.Entities;
using OrbitaAI.Modules.ImportEngine.Application;
using Xunit;

namespace OrbitaAI.UnitTests;

/// <summary>
/// Vérifie uniquement que le squelette du projet est structurellement correct et compilable
/// (planning/DEFINITION_OF_DONE.md §3), sans tester aucune règle métier — aucune n'étant
/// encore implémentée à ce stade (cf. planning/DEVELOPMENT_PHASES.md).
/// </summary>
public sealed class StructureSmokeTests
{
    [Fact]
    public void CoreDomainEntity_CanBeConstructed()
    {
        var organization = new Organization { Id = Guid.NewGuid(), Name = "Test" };

        Assert.NotEqual(Guid.Empty, organization.Id);
    }

    [Fact]
    public void ModuleService_ResolvesToDeclaredInterface()
    {
        IImportEngineService service = new ImportEngineService();

        Assert.NotNull(service);
    }
}
