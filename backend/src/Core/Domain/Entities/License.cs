using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Cadre contractuel attribué à une Organization (architecture/DOMAIN_MODEL.md §2,
/// features/LICENSE_MANAGEMENT.md). Une Organization dispose d'une licence unique active
/// à un instant donné. Squelette structurel uniquement — aucune règle métier, aucun calcul
/// de périmètre ou d'expiration.
/// </summary>
public sealed class License : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public LicenseEdition Edition { get; init; }
}

/// <summary>
/// Les quatre éditions officielles définies dans docs/BUSINESS_MODEL.md §2
/// et détaillées dans features/FEATURE_MATRIX.md.
/// </summary>
public enum LicenseEdition
{
    Community,
    Starter,
    Business,
    Enterprise
}
