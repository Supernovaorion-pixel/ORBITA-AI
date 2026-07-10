namespace OrbitaAI.Core.Application.Contracts;

/// <summary>
/// Contrat d'accès à l'identité de l'appelant courant, point d'appui de toute vérification
/// d'autorisation centralisée (tech/SECURITY_REQUIREMENTS.md §2) et du cloisonnement par
/// Organisation (architecture/DOMAIN_MODEL.md §4). Distingue explicitement le rôle Product
/// Owner (planning/PRODUCT_OWNER_RULES.md), qui n'est rattaché à aucune Organisation et n'est
/// jamais soumis au cloisonnement standard. Aucune implémentation n'est fournie dans le Core :
/// la résolution réelle de l'identité (authentification, tech/SECURITY_REQUIREMENTS.md §1) est
/// hors du périmètre de cette mission.
/// </summary>
public interface ICurrentUserContext
{
    /// <summary>Identifiant de l'utilisateur courant, ou <see langword="null"/> si non authentifié.</summary>
    Guid? UserId { get; }

    /// <summary>
    /// Organisation dans le périmètre de laquelle l'utilisateur courant opère, ou
    /// <see langword="null"/> pour le Product Owner (planning/PRODUCT_OWNER_RULES.md §1),
    /// qui n'est rattaché à aucune Organisation.
    /// </summary>
    Guid? OrganizationId { get; }

    /// <summary>
    /// Indique si l'appelant courant est le Product Owner (planning/PRODUCT_OWNER_RULES.md) :
    /// un accès permanent à toutes les fonctionnalités, éditions et modules, jamais restreint
    /// par une licence d'Organisation cliente (planning/PRODUCT_OWNER_RULES.md §2-4).
    /// </summary>
    bool IsProductOwner { get; }

    /// <summary>Permissions accordées à l'appelant courant (features/USER_MANAGEMENT.md §5).</summary>
    IReadOnlyCollection<string> Permissions { get; }
}
