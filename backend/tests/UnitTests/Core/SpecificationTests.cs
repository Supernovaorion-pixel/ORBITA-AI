using System.Linq.Expressions;
using OrbitaAI.Core.Domain.Specifications;
using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class SpecificationTests
{
    private sealed class ValueGreaterThanSpecification : Specification<TestEntity>
    {
        private readonly int _threshold;

        public ValueGreaterThanSpecification(int threshold) => _threshold = threshold;

        public override Expression<Func<TestEntity, bool>> ToExpression() => entity => entity.Value > _threshold;
    }

    private sealed class ValueLessThanSpecification : Specification<TestEntity>
    {
        private readonly int _threshold;

        public ValueLessThanSpecification(int threshold) => _threshold = threshold;

        public override Expression<Func<TestEntity, bool>> ToExpression() => entity => entity.Value < _threshold;
    }

    [Fact]
    public void SingleSpecification_IsSatisfiedBy_MatchingEntity()
    {
        var specification = new ValueGreaterThanSpecification(10);
        var entity = new TestEntity { Id = Guid.NewGuid(), Value = 20 };

        Assert.True(specification.IsSatisfiedBy(entity));
    }

    [Fact]
    public void SingleSpecification_IsNotSatisfiedBy_NonMatchingEntity()
    {
        var specification = new ValueGreaterThanSpecification(10);
        var entity = new TestEntity { Id = Guid.NewGuid(), Value = 5 };

        Assert.False(specification.IsSatisfiedBy(entity));
    }

    [Fact]
    public void And_RequiresBothSpecificationsToBeSatisfied()
    {
        var combined = new ValueGreaterThanSpecification(10).And(new ValueLessThanSpecification(30));

        Assert.True(combined.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 20 }));
        Assert.False(combined.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 5 }));
        Assert.False(combined.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 35 }));
    }

    [Fact]
    public void Or_RequiresAtLeastOneSpecificationToBeSatisfied()
    {
        var combined = new ValueGreaterThanSpecification(30).Or(new ValueLessThanSpecification(10));

        Assert.True(combined.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 5 }));
        Assert.True(combined.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 35 }));
        Assert.False(combined.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 20 }));
    }

    [Fact]
    public void Not_InvertsTheSpecificationResult()
    {
        var negated = new ValueGreaterThanSpecification(10).Not();

        Assert.True(negated.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 5 }));
        Assert.False(negated.IsSatisfiedBy(new TestEntity { Id = Guid.NewGuid(), Value = 20 }));
    }

    [Fact]
    public void CombinedSpecification_ToExpression_CanBeCompiledAndReused()
    {
        var combined = new ValueGreaterThanSpecification(10).And(new ValueLessThanSpecification(30));
        var compiled = combined.ToExpression().Compile();

        Assert.True(compiled(new TestEntity { Id = Guid.NewGuid(), Value = 15 }));
        Assert.False(compiled(new TestEntity { Id = Guid.NewGuid(), Value = 50 }));
    }
}
