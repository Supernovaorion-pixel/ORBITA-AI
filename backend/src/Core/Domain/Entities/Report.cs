using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Rapport » (docs/GLOSSARY.md).
/// Restitution formalisée, figée à sa date de génération (architecture/DOMAIN_MODEL.md §2,
/// features/REPORTING_ENGINE.md). Squelette structurel uniquement — aucune logique
/// d'assemblage ou de génération.
/// </summary>
public sealed class Report : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public DateTimeOffset GeneratedAt { get; init; }
}
