using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Client » (docs/GLOSSARY.md) — nommé Customer
/// pour éviter toute ambiguïté avec un client logiciel/API. Tiers avec lequel l'Organization
/// réalise ou vise une activité commerciale (architecture/DOMAIN_MODEL.md §2).
/// Squelette structurel uniquement — aucune règle métier.
/// </summary>
public sealed class Customer : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public string Name { get; init; } = string.Empty;

    public bool IsKeyAccount { get; init; }
}
