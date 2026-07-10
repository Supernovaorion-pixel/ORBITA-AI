namespace OrbitaAI.Core.Domain;

/// <summary>
/// Base commune de tout Value Object du Domaine. Par opposition à une <see cref="Entity{TId}"/>
/// (égalité par identité), un Value Object est défini par la valeur de ses attributs : deux
/// instances sont égales si et seulement si l'ensemble de leurs composants d'égalité le sont
/// (architecture/DOMAIN_MODEL.md). Un Value Object est immuable par construction : ses
/// dérivés ne doivent exposer que des propriétés à lecture seule.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Composants sur lesquels porte l'égalité structurelle, dans un ordre stable et déterministe.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc />
    public bool Equals(ValueObject? other)
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

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ValueObject);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var component in GetEqualityComponents())
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(ValueObject? left, ValueObject? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}
