namespace OrbitaAI.Core.Application.Contracts;

/// <summary>
/// Contrat de validation transactionnelle des changements accumulés par un ou plusieurs
/// référentiels (<c>IRepository{TEntity,TId}</c>) au cours d'un même cas d'usage. Défini par
/// l'Application, implémenté par l'Infrastructure (architecture/APPLICATION_LAYERS.md §3).
/// Aucune implémentation n'est fournie dans le Core : la persistance réelle (tech/DATABASE_STRATEGY.md)
/// est hors du périmètre de cette mission.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Valide de façon atomique l'ensemble des changements accumulés depuis le dernier appel.
    /// Retourne le nombre d'entités affectées.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
