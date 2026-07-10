using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Commercial » (docs/GLOSSARY.md).
/// Porte un périmètre de vente au sein de l'Organization (architecture/DOMAIN_MODEL.md §2).
/// Squelette structurel uniquement — aucune règle métier.
///
/// NOTE (point ouvert, cf. rapport de mission) : features/SALES_ANALYTICS.md §4 introduit
/// la notion de « Territoire » comme regroupement de Commerciaux et de Clients, mais
/// architecture/DOMAIN_MODEL.md ne le liste pas comme entité de premier niveau. Aucune
/// propriété de rattachement à un territoire n'est ajoutée ici tant que cette incohérence
/// documentaire n'est pas résolue explicitement.
/// </summary>
public sealed class SalesRepresentative : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public Guid? UserId { get; init; }
}
