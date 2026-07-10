using Xunit;

namespace OrbitaAI.UnitTests.Core;

public sealed class EntityTests
{
    [Fact]
    public void TwoEntities_WithSameIdAndType_AreEqual_EvenWithDifferentAttributes()
    {
        var id = Guid.NewGuid();
        var first = new TestEntity { Id = id, Value = 1 };
        var second = new TestEntity { Id = id, Value = 2 };

        Assert.Equal(first, second);
        Assert.True(first == second);
    }

    [Fact]
    public void TwoEntities_WithDifferentIds_AreNotEqual_EvenWithSameAttributes()
    {
        var first = new TestEntity { Id = Guid.NewGuid(), Value = 1 };
        var second = new TestEntity { Id = Guid.NewGuid(), Value = 1 };

        Assert.NotEqual(first, second);
        Assert.True(first != second);
    }

    [Fact]
    public void Entity_IsNeverEqualToNull()
    {
        var entity = new TestEntity { Id = Guid.NewGuid() };

        Assert.False(entity.Equals(null));
    }

    [Fact]
    public void Entity_HashCode_IsStableForSameIdAndType()
    {
        var id = Guid.NewGuid();
        var first = new TestEntity { Id = id, Value = 1 };
        var second = new TestEntity { Id = id, Value = 2 };

        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
