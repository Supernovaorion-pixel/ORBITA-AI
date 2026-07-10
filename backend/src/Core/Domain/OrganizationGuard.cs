using OrbitaAI.Core.Domain.Exceptions;

namespace OrbitaAI.Core.Domain;

/// <summary>
/// Point d'application central du cloisonnement strict par Organisation
/// (architecture/DOMAIN_MODEL.md §4, architecture/ARCHITECTURE_DECISIONS.md — ADR-004).
/// Toute manipulation croisant deux entités <see cref="IOrganizationScoped"/> — ou une entité
/// et le contexte d'Organisation courant — doit être validée par cette classe plutôt que par
/// une vérification ad hoc dispersée dans chaque module.
/// </summary>
public static class OrganizationGuard
{
    /// <summary>
    /// Vérifie que <paramref name="entity"/> appartient bien à <paramref name="organizationId"/>.
    /// Lève <see cref="OrganizationMismatchException"/> dans le cas contraire.
    /// </summary>
    public static void EnsureBelongsTo(IOrganizationScoped entity, Guid organizationId)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.OrganizationId != organizationId)
        {
            throw new OrganizationMismatchException(organizationId, entity.OrganizationId);
        }
    }

    /// <summary>
    /// Vérifie que <paramref name="first"/> et <paramref name="second"/> appartiennent à la même
    /// Organisation. Lève <see cref="OrganizationMismatchException"/> dans le cas contraire.
    /// </summary>
    public static void EnsureSameOrganization(IOrganizationScoped first, IOrganizationScoped second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (first.OrganizationId != second.OrganizationId)
        {
            throw new OrganizationMismatchException(first.OrganizationId, second.OrganizationId);
        }
    }

    /// <summary>
    /// Vérifie que chacune des entités de <paramref name="entities"/> appartient à
    /// <paramref name="organizationId"/>. Lève <see cref="OrganizationMismatchException"/> à la
    /// première entité non conforme rencontrée.
    /// </summary>
    public static void EnsureAllBelongTo(IEnumerable<IOrganizationScoped> entities, Guid organizationId)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            EnsureBelongsTo(entity, organizationId);
        }
    }
}
