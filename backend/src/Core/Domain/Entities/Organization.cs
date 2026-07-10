using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Entité racine du Domaine (architecture/DOMAIN_MODEL.md §1). Toute autre entité,
/// à l'exception de License, existe dans le périmètre d'une Organization.
/// Squelette structurel uniquement — aucune règle métier.
/// </summary>
public sealed class Organization : AggregateRoot<Guid>
{
    public string Name { get; init; } = string.Empty;
}
