namespace OrbitaAI.Core.Services;

/// <summary>
/// Abstraction de la génération d'identifiants uniques pour les nouvelles entités du Domaine.
/// Isoler cette responsabilité derrière un contrat permet de substituer la stratégie de
/// génération (ex. identifiants triables chronologiquement) sans modifier le Domaine ni
/// l'Application qui la consomment.
/// </summary>
public interface IIdGenerator
{
    /// <summary>Génère un nouvel identifiant unique.</summary>
    Guid NewId();
}
