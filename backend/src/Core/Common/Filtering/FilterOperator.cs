namespace OrbitaAI.Core.Common.Filtering;

/// <summary>Opérateur de comparaison appliqué par un <see cref="FilterDescriptor"/> (ux/COMPONENT_LIBRARY.md §5).</summary>
public enum FilterOperator
{
    Equals,
    NotEquals,
    Contains,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}
