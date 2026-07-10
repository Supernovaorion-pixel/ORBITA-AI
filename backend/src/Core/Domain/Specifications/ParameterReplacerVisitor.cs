using System.Linq.Expressions;

namespace OrbitaAI.Core.Domain.Specifications;

/// <summary>
/// Remplace, au sein d'un arbre d'expression, toute occurrence d'un paramètre par un autre.
/// Utilisé exclusivement par les combinateurs de <see cref="Specification{TEntity}"/>
/// (<see cref="AndSpecification{TEntity}"/>, <see cref="OrSpecification{TEntity}"/>) afin que
/// les deux expressions combinées partagent un unique paramètre, condition nécessaire à la
/// construction d'une expression composée valide.
/// </summary>
internal sealed class ParameterReplacerVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _source;
    private readonly ParameterExpression _target;

    private ParameterReplacerVisitor(ParameterExpression source, ParameterExpression target)
    {
        _source = source;
        _target = target;
    }

    public static Expression Replace(Expression expression, ParameterExpression source, ParameterExpression target)
        => new ParameterReplacerVisitor(source, target).Visit(expression);

    protected override Expression VisitParameter(ParameterExpression node) => node == _source ? _target : node;
}
