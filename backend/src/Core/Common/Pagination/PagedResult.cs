using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Core.Common.Pagination;

/// <summary>
/// Résultat paginé d'une liste de données, conforme au comportement de pagination commun
/// défini dans ux/COMPONENT_LIBRARY.md §5.
/// </summary>
/// <typeparam name="TItem">Type des éléments de la page.</typeparam>
public sealed class PagedResult<TItem>
{
    /// <summary>Éléments de la page courante.</summary>
    public IReadOnlyCollection<TItem> Items { get; }

    /// <summary>Numéro de la page courante, à partir de 1.</summary>
    public int PageNumber { get; }

    /// <summary>Nombre d'éléments par page.</summary>
    public int PageSize { get; }

    /// <summary>Nombre total d'éléments disponibles, toutes pages confondues.</summary>
    public int TotalCount { get; }

    /// <summary>Nombre total de pages, calculé à partir de <see cref="TotalCount"/> et <see cref="PageSize"/>.</summary>
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>Indique si une page précédente existe.</summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>Indique si une page suivante existe.</summary>
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedResult(IReadOnlyCollection<TItem> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = Guard.Against.Null(items, nameof(items));
        PageNumber = Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));
        PageSize = Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));
        TotalCount = Guard.Against.Negative(totalCount, nameof(totalCount));
    }

    /// <summary>Construit un résultat paginé vide, pour un périmètre sans donnée disponible (cf. ux/EMPTY_STATES.md).</summary>
    public static PagedResult<TItem> Empty(PaginationRequest request) =>
        new(Array.Empty<TItem>(), request.PageNumber, request.PageSize, totalCount: 0);
}
