using OrbitaAI.Core.Domain;
using OrbitaAI.Core.Domain.Entities;
using OrbitaAI.Core.Domain.Exceptions;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class OrganizationGuardTests
{
    [Fact]
    public void EnsureBelongsTo_WithMatchingOrganization_DoesNotThrow()
    {
        var organizationId = Guid.NewGuid();
        var customer = new Customer { Id = Guid.NewGuid(), OrganizationId = organizationId };

        OrganizationGuard.EnsureBelongsTo(customer, organizationId);
    }

    [Fact]
    public void EnsureBelongsTo_WithMismatchedOrganization_Throws()
    {
        var customer = new Customer { Id = Guid.NewGuid(), OrganizationId = Guid.NewGuid() };
        var otherOrganizationId = Guid.NewGuid();

        var exception = Assert.Throws<OrganizationMismatchException>(
            () => OrganizationGuard.EnsureBelongsTo(customer, otherOrganizationId));

        Assert.Equal(otherOrganizationId, exception.ExpectedOrganizationId);
        Assert.Equal(customer.OrganizationId, exception.ActualOrganizationId);
    }

    [Fact]
    public void EnsureSameOrganization_WithSameOrganization_DoesNotThrow()
    {
        var organizationId = Guid.NewGuid();
        var customer = new Customer { Id = Guid.NewGuid(), OrganizationId = organizationId };
        var invoice = new Invoice { Id = Guid.NewGuid(), OrganizationId = organizationId, CustomerId = customer.Id };

        OrganizationGuard.EnsureSameOrganization(customer, invoice);
    }

    [Fact]
    public void EnsureSameOrganization_WithDifferentOrganizations_Throws()
    {
        var customer = new Customer { Id = Guid.NewGuid(), OrganizationId = Guid.NewGuid() };
        var invoice = new Invoice { Id = Guid.NewGuid(), OrganizationId = Guid.NewGuid(), CustomerId = customer.Id };

        Assert.Throws<OrganizationMismatchException>(() => OrganizationGuard.EnsureSameOrganization(customer, invoice));
    }

    [Fact]
    public void EnsureAllBelongTo_WithOneMismatchedEntity_Throws()
    {
        var organizationId = Guid.NewGuid();
        var entities = new IOrganizationScoped[]
        {
            new Customer { Id = Guid.NewGuid(), OrganizationId = organizationId },
            new Customer { Id = Guid.NewGuid(), OrganizationId = Guid.NewGuid() },
        };

        Assert.Throws<OrganizationMismatchException>(() => OrganizationGuard.EnsureAllBelongTo(entities, organizationId));
    }

    [Fact]
    public void EnsureAllBelongTo_WithAllMatchingEntities_DoesNotThrow()
    {
        var organizationId = Guid.NewGuid();
        var entities = new IOrganizationScoped[]
        {
            new Customer { Id = Guid.NewGuid(), OrganizationId = organizationId },
            new Customer { Id = Guid.NewGuid(), OrganizationId = organizationId },
        };

        OrganizationGuard.EnsureAllBelongTo(entities, organizationId);
    }
}
