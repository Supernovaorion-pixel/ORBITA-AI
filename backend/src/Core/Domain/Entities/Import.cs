using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Opération d'intégration de données externes dans une Organization
/// (architecture/DOMAIN_MODEL.md §2, features/IMPORT_ENGINE.md). Squelette structurel
/// uniquement — aucune logique de validation, de fusion ou de remplacement.
/// </summary>
public sealed class Import : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public ImportMode Mode { get; init; }

    public ImportStatus Status { get; init; }
}

/// <summary>cf. features/IMPORT_ENGINE.md §11-12.</summary>
public enum ImportMode
{
    Incremental,
    Full
}

/// <summary>cf. features/IMPORT_ENGINE.md §14.</summary>
public enum ImportStatus
{
    InProgress,
    Succeeded,
    PartiallySucceeded,
    Failed
}
