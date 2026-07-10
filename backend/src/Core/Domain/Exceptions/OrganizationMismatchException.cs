namespace OrbitaAI.Core.Domain.Exceptions;

/// <summary>
/// Signale une violation du cloisonnement strict par Organisation
/// (architecture/DOMAIN_MODEL.md §4, architecture/ARCHITECTURE_DECISIONS.md — ADR-004) :
/// une entité rattachée à une Organisation a été référencée ou manipulée dans le contexte
/// d'une autre Organisation. Cette situation ne doit jamais se produire dans un système
/// correctement cloisonné ; elle constitue donc une exception, pas un échec métier attendu.
/// </summary>
public sealed class OrganizationMismatchException : DomainException
{
    /// <summary>Organisation dans le contexte de laquelle l'opération était attendue.</summary>
    public Guid ExpectedOrganizationId { get; }

    /// <summary>Organisation à laquelle l'entité manipulée est réellement rattachée.</summary>
    public Guid ActualOrganizationId { get; }

    public OrganizationMismatchException(Guid expectedOrganizationId, Guid actualOrganizationId)
        : base(
            $"L'entité appartient à l'Organisation '{actualOrganizationId}', " +
            $"mais l'opération était attendue dans le contexte de l'Organisation '{expectedOrganizationId}'.")
    {
        ExpectedOrganizationId = expectedOrganizationId;
        ActualOrganizationId = actualOrganizationId;
    }
}
