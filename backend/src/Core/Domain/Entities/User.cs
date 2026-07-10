using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Personne accédant au système, rattachée à une Organization (architecture/DOMAIN_MODEL.md §2,
/// features/USER_MANAGEMENT.md). Squelette structurel uniquement — aucune règle métier,
/// aucune logique d'authentification ou d'autorisation.
/// </summary>
public sealed class User : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public string DisplayName { get; init; } = string.Empty;

    public UserRole Role { get; init; }
}

/// <summary>
/// Rôles officiels définis dans docs/USER_PERSONAS.md et features/USER_MANAGEMENT.md §4.
/// Le rôle Product Owner (planning/PRODUCT_OWNER_RULES.md) est délibérément exclu de cette
/// énumération : il n'est pas un rôle d'Organisation cliente (cf. Core/Domain/Entities/ProductOwner.cs).
/// </summary>
public enum UserRole
{
    GeneralManagement,
    SalesDirector,
    RegionalManager,
    SalesRepresentative,
    FinancialController,
    Administrator
}
