using OrbitaAI.Core.Common.Pagination;
using OrbitaAI.Core.Domain.Specifications;

namespace OrbitaAI.Core.Domain.Repositories;

/// <summary>
/// Contrat d'accès à la persistance d'une racine d'agrégat, défini par le Domaine et implémenté
/// par l'Infrastructure d'un module (inversion de dépendance, architecture/APPLICATION_LAYERS.md §3).
/// Aucune implémentation n'est fournie dans le Core : la connexion à une base de données
/// (tech/DATABASE_STRATEGY.md) est hors du périmètre de cette mission.
/// </summary>
/// <typeparam name="TEntity">Type de la racine d'agrégat gérée par le référentiel.</typeparam>
/// <typeparam name="TId">Type de l'identifiant de <typeparamref name="TEntity"/>.</typeparam>
public interface IRepository<TEntity, TId> where TEntity : AggregateRoot<TId> where TId : notnull
{
    /// <summary>Retourne l'entité correspondant à <paramref name="id"/>, ou <see langword="null"/> si introuvable.</summary>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>Retourne l'ensemble des entités satisfaisant <paramref name="specification"/>.</summary>
    Task<IReadOnlyCollection<TEntity>> ListAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

    /// <summary>Retourne une page d'entités satisfaisant <paramref name="specification"/>, selon <paramref name="pagination"/>.</summary>
    Task<PagedResult<TEntity>> ListPagedAsync(
        Specification<TEntity> specification,
        PaginationRequest pagination,
        CancellationToken cancellationToken = default);

    /// <summary>Ajoute une nouvelle entité, effective après <c>IUnitOfWork.SaveChangesAsync</c>.</summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Marque une entité existante comme modifiée, effectif après <c>IUnitOfWork.SaveChangesAsync</c>.</summary>
    void Update(TEntity entity);

    /// <summary>Marque une entité existante pour suppression, effective après <c>IUnitOfWork.SaveChangesAsync</c>.</summary>
    void Remove(TEntity entity);
}
