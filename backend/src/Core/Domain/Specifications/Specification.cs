using System.Linq.Expressions;

namespace OrbitaAI.Core.Domain.Specifications;

/// <summary>
/// Encapsule un critère de sélection ou une règle métier réutilisable sous forme d'expression
/// composable (Specification Pattern), consommée notamment par <c>IRepository{TEntity,TId}</c>
/// (Domain/Repositories/IRepository.cs). Permet d'exprimer une règle une seule fois et de la
/// combiner avec d'autres (<see cref="And"/>, <see cref="Or"/>, <see cref="Not"/>) plutôt que
/// de la dupliquer (architecture/CODING_PRINCIPLES.md §3 — DRY).
/// </summary>
/// <typeparam name="TEntity">Type de l'entité évaluée par la spécification.</typeparam>
public abstract class Specification<TEntity>
{
    /// <summary>Expression représentant le critère de la spécification.</summary>
    public abstract Expression<Func<TEntity, bool>> ToExpression();

    /// <summary>Évalue la spécification en mémoire pour une instance donnée.</summary>
    public bool IsSatisfiedBy(TEntity entity) => ToExpression().Compile().Invoke(entity);

    /// <summary>Combine cette spécification avec <paramref name="other"/> par un ET logique.</summary>
    public Specification<TEntity> And(Specification<TEntity> other) => new AndSpecification<TEntity>(this, other);

    /// <summary>Combine cette spécification avec <paramref name="other"/> par un OU logique.</summary>
    public Specification<TEntity> Or(Specification<TEntity> other) => new OrSpecification<TEntity>(this, other);

    /// <summary>Retourne la négation de cette spécification.</summary>
    public Specification<TEntity> Not() => new NotSpecification<TEntity>(this);
}
