using OrbitaAI.Core.Domain;
using OrbitaAI.Core.Domain.Events;

namespace OrbitaAI.UnitTests.Core;

/// <summary>
/// Doublures de test minimales et réutilisables pour les tests unitaires du Core
/// (Entity, AggregateRoot, ValueObject, Domain Events). Ne représentent aucune règle
/// métier réelle : uniquement des types concrets permettant d'exercer les classes
/// abstraites du Core.
/// </summary>
internal sealed class TestDomainEvent : IDomainEvent
{
    public TestDomainEvent(Guid organizationId, string payload)
    {
        OrganizationId = organizationId;
        Payload = payload;
        OccurredAt = DateTimeOffset.UtcNow;
    }

    public Guid OrganizationId { get; }

    public DateTimeOffset OccurredAt { get; }

    public string Payload { get; }
}

internal sealed class OtherTestDomainEvent : IDomainEvent
{
    public Guid OrganizationId { get; init; }

    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
}

internal sealed class TestAggregateRoot : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;

    public void Rename(string name)
    {
        Name = name;
        RaiseRenamedEvent(name);
    }

    private void RaiseRenamedEvent(string name) =>
        AddTestEvent(new TestDomainEvent(organizationId: Guid.NewGuid(), payload: name));

    private void AddTestEvent(IDomainEvent domainEvent) => AddDomainEvent(domainEvent);
}

internal sealed class TestEntity : Entity<Guid>
{
    public int Value { get; init; }
}

internal sealed class Coordinates : ValueObject
{
    public Coordinates(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }

    public double Longitude { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}
