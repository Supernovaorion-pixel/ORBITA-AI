namespace OrbitaAI.Core.Domain;

/// <summary>
/// Marqueur des services de Domaine communs : logique métier stable qui ne trouve naturellement
/// sa place ni dans une <see cref="Entity{TId}"/> ni dans un <see cref="ValueObject"/> car elle
/// porte sur plusieurs entités ou nécessite une collaboration externe au Domaine
/// (architecture/APPLICATION_LAYERS.md §2 — Domaine). Distinct d'un service applicatif (couche
/// Application, cf. architecture/APPLICATION_LAYERS.md §2), qui orchestre un cas d'usage plutôt
/// qu'une règle métier intrinsèque.
///
/// Squelette structurel uniquement : aucun service de Domaine concret n'est défini à ce stade,
/// aucune règle métier n'étant encore développée (cf. planning/DEVELOPMENT_PHASES.md).
/// </summary>
public interface IDomainService
{
}
