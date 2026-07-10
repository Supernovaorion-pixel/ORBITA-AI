using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Droit attribué à un User, dérivé de son rôle et de son périmètre de responsabilité
/// (architecture/DOMAIN_MODEL.md §2, features/USER_MANAGEMENT.md §5).
/// Squelette structurel uniquement — aucune logique d'évaluation d'autorisation.
/// </summary>
public sealed class Permission : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public Guid UserId { get; init; }
}
