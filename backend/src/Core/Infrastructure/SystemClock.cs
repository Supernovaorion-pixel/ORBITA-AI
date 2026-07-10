using OrbitaAI.Core.Services;

namespace OrbitaAI.Core.Infrastructure;

/// <summary>Implémentation par défaut de <see cref="IClock"/>, adossée à l'horloge système.</summary>
public sealed class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
