namespace OrbitaAI.Core.Domain;

/// <summary>
/// Marqueur structurel appliqué à toute entité rattachée à une Organisation.
/// Traduit le principe de cloisonnement défini dans architecture/DOMAIN_MODEL.md §4 :
/// aucune entité, hors Organization et License, n'existe indépendamment d'une Organisation.
/// Aucune règle métier n'est implémentée ici (squelette architectural uniquement).
/// </summary>
public interface IOrganizationScoped
{
    Guid OrganizationId { get; }
}
