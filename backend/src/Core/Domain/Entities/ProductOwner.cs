using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Rôle unique et global réservé au créateur d'ORBITA AI (planning/PRODUCT_OWNER_RULES.md).
/// Distinct de <see cref="User"/> : n'est rattaché à aucune Organization et n'est jamais
/// soumis au cloisonnement par Organisation (architecture/DOMAIN_MODEL.md §4).
/// Un seul enregistrement de ce type existe dans le système (planning/PRODUCT_OWNER_RULES.md §11).
/// Squelette structurel uniquement — aucune règle métier, aucune logique d'octroi d'accès.
/// </summary>
public sealed class ProductOwner : AggregateRoot<Guid>
{
    public string DisplayName { get; init; } = string.Empty;
}
