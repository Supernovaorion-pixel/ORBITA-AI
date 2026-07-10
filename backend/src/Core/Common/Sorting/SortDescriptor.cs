using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Core.Common.Sorting;

/// <summary>
/// Décrit un critère de tri unique appliqué à une colonne de tableau (ux/COMPONENT_LIBRARY.md §5 :
/// un seul critère de tri actif à la fois).
/// </summary>
public sealed record SortDescriptor
{
    /// <summary>Nom de la propriété sur laquelle porte le tri.</summary>
    public string PropertyName { get; }

    /// <summary>Sens du tri.</summary>
    public SortDirection Direction { get; }

    public SortDescriptor(string propertyName, SortDirection direction = SortDirection.Ascending)
    {
        PropertyName = Guard.Against.NullOrWhiteSpace(propertyName, nameof(propertyName));
        Direction = Guard.Against.OutOfEnum(direction, nameof(direction));
    }
}
