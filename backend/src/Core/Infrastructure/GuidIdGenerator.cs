using OrbitaAI.Core.Services;

namespace OrbitaAI.Core.Infrastructure;

/// <summary>Implémentation par défaut de <see cref="IIdGenerator"/>, fondée sur <see cref="Guid.NewGuid"/>.</summary>
public sealed class GuidIdGenerator : IIdGenerator
{
    /// <inheritdoc />
    public Guid NewId() => Guid.NewGuid();
}
