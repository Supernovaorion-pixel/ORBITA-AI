using OrbitaAI.Core.Application.Contracts;
using OrbitaAI.Core.Domain;
using OrbitaAI.Core.Domain.Entities;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

/// <summary>
/// Vérifie que chacune des entités de architecture/DOMAIN_MODEL.md est correctement construite
/// sur les bases communes du Core (identité, cloisonnement par Organisation), au-delà des
/// entités déjà exercées indirectement par d'autres tests (Customer, Invoice, Organization).
/// </summary>
public sealed class DomainEntitiesStructureTests
{
    [Fact]
    public void OrganizationScopedEntities_ExposeTheirOwningOrganization()
    {
        var organizationId = Guid.NewGuid();

        IOrganizationScoped[] entities =
        {
            new License { Id = Guid.NewGuid(), OrganizationId = organizationId, Edition = LicenseEdition.Business },
            new User { Id = Guid.NewGuid(), OrganizationId = organizationId, DisplayName = "Ada", Role = UserRole.Administrator },
            new Permission { Id = Guid.NewGuid(), OrganizationId = organizationId, UserId = Guid.NewGuid() },
            new SalesRepresentative { Id = Guid.NewGuid(), OrganizationId = organizationId },
            new ProductFamily { Id = Guid.NewGuid(), OrganizationId = organizationId, Name = "Famille A" },
            new Product { Id = Guid.NewGuid(), OrganizationId = organizationId, FamilyId = Guid.NewGuid(), Name = "Produit A" },
            new Objective { Id = Guid.NewGuid(), OrganizationId = organizationId },
            new Forecast { Id = Guid.NewGuid(), OrganizationId = organizationId, ObjectiveId = Guid.NewGuid() },
            new Alert { Id = Guid.NewGuid(), OrganizationId = organizationId, Priority = AlertPriority.Critical, Status = AlertStatus.Active },
            new Report { Id = Guid.NewGuid(), OrganizationId = organizationId, GeneratedAt = DateTimeOffset.UtcNow },
            new Import { Id = Guid.NewGuid(), OrganizationId = organizationId, Mode = ImportMode.Incremental, Status = ImportStatus.Succeeded },
            new HistoryEntry { Id = Guid.NewGuid(), OrganizationId = organizationId, RecordedAt = DateTimeOffset.UtcNow },
        };

        Assert.All(entities, entity => Assert.Equal(organizationId, entity.OrganizationId));
    }

    [Fact]
    public void ProductOwner_IsNotOrganizationScoped()
    {
        // Vérifié par réflexion plutôt que par un opérateur `is` : ProductOwner étant scellé,
        // le compilateur prouverait le résultat de façon statique (CS0184) et transformerait
        // ce test en code mort. La réflexion exprime la même garantie sans ce défaut.
        var implementedInterfaces = typeof(ProductOwner).GetInterfaces();

        Assert.DoesNotContain(typeof(IOrganizationScoped), implementedInterfaces);
    }

    [Fact]
    public void Entities_AreEqual_WhenTheySharetheSameIdAndType()
    {
        var id = Guid.NewGuid();
        var first = new Objective { Id = id, OrganizationId = Guid.NewGuid() };
        var second = new Objective { Id = id, OrganizationId = Guid.NewGuid() };

        Assert.Equal(first, second);
    }

    [Fact]
    public void AuditEntry_ExposesWhoWhatWhen()
    {
        var organizationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var occurredAt = DateTimeOffset.UtcNow;

        var entry = new AuditEntry(organizationId, userId, "User.RoleChanged", nameof(User), userId.ToString(), occurredAt);

        Assert.Equal(organizationId, entry.OrganizationId);
        Assert.Equal(userId, entry.UserId);
        Assert.Equal("User.RoleChanged", entry.Action);
        Assert.Equal(occurredAt, entry.OccurredAt);
    }
}
