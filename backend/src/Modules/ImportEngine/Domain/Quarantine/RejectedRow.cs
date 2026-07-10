using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

/// <summary>
/// Une ligne mise en quarantaine par le Quarantine Engine : jamais modifiée, jamais fusionnée,
/// jamais perdue (mission 010.4). Conserve intégralement la donnée originale de la ligne ainsi que
/// l'ensemble des raisons de son rejet, afin de permettre sa correction et sa réintégration
/// ultérieure par l'utilisateur (features/IMPORT_ENGINE.md §8).
/// </summary>
/// <param name="RowNumber">Numéro de la ligne rejetée, tel que produit par le Reader.</param>
/// <param name="OriginalHeaders">En-têtes du fichier source, inchangés.</param>
/// <param name="OriginalValues">Valeurs originales de la ligne, dans l'ordre du fichier source, inchangées.</param>
/// <param name="Reasons">Ensemble des raisons ayant motivé la mise en quarantaine de cette ligne.</param>
/// <param name="Category">Catégorie dominante de rejet, dérivée de la raison la plus sévère.</param>
/// <param name="HighestSeverity">Sévérité la plus élevée parmi <see cref="Reasons"/>.</param>
public sealed record RejectedRow(
    int RowNumber,
    IReadOnlyList<string> OriginalHeaders,
    IReadOnlyList<object?> OriginalValues,
    IReadOnlyList<RejectedRowReason> Reasons,
    RejectedRowCategory Category,
    ValidationSeverity HighestSeverity);
