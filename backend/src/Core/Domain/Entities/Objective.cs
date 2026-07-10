using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Objectif » (docs/GLOSSARY.md).
/// Cible de performance définie pour un périmètre et une période (architecture/DOMAIN_MODEL.md §2).
/// Squelette structurel uniquement — aucune règle métier, aucun calcul de cascade.
/// </summary>
public sealed class Objective : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }
}
