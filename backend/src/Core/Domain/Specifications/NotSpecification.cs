using System.Linq.Expressions;

namespace OrbitaAI.Core.Domain.Specifications;

/// <summary>Retourne la négation d'une spécification (cf. <see cref="Specification{TEntity}.Not"/>).</summary>
internal sealed class NotSpecification<TEntity> : Specification<TEntity>
{
    private readonly Specification<TEntity> _inner;

    public NotSpecification(Specification<TEntity> inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        var innerExpression = _inner.ToExpression();
        var negatedBody = Expression.Not(innerExpression.Body);
        return Expression.Lambda<Func<TEntity, bool>>(negatedBody, innerExpression.Parameters[0]);
    }
}
