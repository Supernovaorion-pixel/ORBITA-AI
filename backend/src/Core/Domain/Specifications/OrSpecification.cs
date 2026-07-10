using System.Linq.Expressions;

namespace OrbitaAI.Core.Domain.Specifications;

/// <summary>Combine deux spécifications par un OU logique (cf. <see cref="Specification{TEntity}.Or"/>).</summary>
internal sealed class OrSpecification<TEntity> : Specification<TEntity>
{
    private readonly Specification<TEntity> _left;
    private readonly Specification<TEntity> _right;

    public OrSpecification(Specification<TEntity> left, Specification<TEntity> right)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));
    }

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = leftExpression.Parameters[0];
        var rightBody = ParameterReplacerVisitor.Replace(rightExpression.Body, rightExpression.Parameters[0], parameter);

        var combinedBody = Expression.OrElse(leftExpression.Body, rightBody);
        return Expression.Lambda<Func<TEntity, bool>>(combinedBody, parameter);
    }
}
