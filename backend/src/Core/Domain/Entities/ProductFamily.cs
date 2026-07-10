using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Famille » (docs/GLOSSARY.md).
/// Regroupement de Product partageant une caractéristique commune (architecture/DOMAIN_MODEL.md §2).
/// Squelette structurel uniquement — aucune règle métier.
/// </summary>
public sealed class ProductFamily : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public string Name { get; init; } = string.Empty;
}
