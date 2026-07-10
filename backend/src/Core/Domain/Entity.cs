namespace OrbitaAI.Core.Domain;

/// <summary>
/// Base commune de toute entité du Domaine (architecture/DOMAIN_MODEL.md). Une entité se
/// distingue d'un <see cref="ValueObject"/> par son identité (<see cref="Id"/>) : deux entités
/// sont égales si et seulement si elles partagent le même type concret et le même identifiant,
/// indépendamment de la valeur de leurs autres propriétés (« Entity Identity », par opposition
/// à l'égalité structurelle des Value Objects).
/// </summary>
/// <typeparam name="TId">Type de l'identifiant de l'entité (ex. <see cref="Guid"/>).</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    /// <summary>Identifiant unique de l'entité, stable pour toute sa durée de vie.</summary>
    public TId Id { get; init; } = default!;

    /// <inheritdoc />
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
}
