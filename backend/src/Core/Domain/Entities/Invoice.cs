using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Facture » (docs/GLOSSARY.md).
/// Constate une transaction commerciale entre l'Organization et un Customer
/// (architecture/DOMAIN_MODEL.md §2). Squelette structurel uniquement — aucune règle
/// métier, aucun calcul de montant ou de marge.
/// </summary>
public sealed class Invoice : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public Guid CustomerId { get; init; }

    public Guid? SalesRepresentativeId { get; init; }
}
