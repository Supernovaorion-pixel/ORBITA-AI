using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Core.Common.Filtering;

/// <summary>
/// Décrit un critère de filtre combinable appliqué à une colonne de tableau
/// (ux/COMPONENT_LIBRARY.md §5 : filtres combinables entre eux).
/// </summary>
public sealed record FilterDescriptor
{
    /// <summary>Nom de la propriété sur laquelle porte le filtre.</summary>
    public string PropertyName { get; }

    /// <summary>Opérateur de comparaison appliqué.</summary>
    public FilterOperator Operator { get; }

    /// <summary>Valeur de comparaison, éventuellement nulle (ex. filtre « non renseigné »).</summary>
    public object? Value { get; }

    public FilterDescriptor(string propertyName, FilterOperator @operator, object? value)
    {
        PropertyName = Guard.Against.NullOrWhiteSpace(propertyName, nameof(propertyName));
        Operator = Guard.Against.OutOfEnum(@operator, nameof(@operator));
        Value = value;
    }
}
