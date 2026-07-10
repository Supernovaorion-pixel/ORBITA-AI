using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Core.Common.Pagination;

/// <summary>
/// Demande de pagination par blocs de taille constante (ux/COMPONENT_LIBRARY.md §5), commune
/// à tout tableau de données du produit (Clients, Produits, Historique, Journal, Audit...).
/// </summary>
public sealed record PaginationRequest
{
    /// <summary>Numéro de la page demandée, à partir de 1.</summary>
    public int PageNumber { get; }

    /// <summary>Nombre d'éléments par page.</summary>
    public int PageSize { get; }

    public PaginationRequest(int pageNumber, int pageSize)
    {
        PageNumber = Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));
        PageSize = Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));
    }

    /// <summary>Pagination par défaut : première page, taille standard (branding/DESIGN_PRINCIPLES.md §7).</summary>
    public static readonly PaginationRequest Default = new(pageNumber: 1, pageSize: 25);
}
