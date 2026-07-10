using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

/// <summary>
/// Une raison individuelle pour laquelle une ligne a été mise en quarantaine, reprenant
/// intégralement le détail d'un <see cref="ValidationFinding"/> (hors numéro de ligne, déjà porté
/// par <see cref="RejectedRow"/>) : règle violée, sévérité, colonne concernée, message,
/// explication et suggestion de résolution. Une ligne rejetée peut porter plusieurs raisons.
/// </summary>
/// <param name="Code">Code de la règle de validation violée.</param>
/// <param name="Category">Catégorie de la règle de validation violée.</param>
/// <param name="Severity">Sévérité du constat à l'origine de cette raison.</param>
/// <param name="ColumnIndex">Index de la colonne concernée, si applicable.</param>
/// <param name="ColumnCanonicalKey">Clé canonique de la colonne concernée, si reconnue.</param>
/// <param name="RawHeader">En-tête brut de la colonne concernée, tel que lu dans le fichier source.</param>
/// <param name="Message">Message, explication et suggestion de résolution associés.</param>
public sealed record RejectedRowReason(
    ValidationCode Code,
    ValidationCategory Category,
    ValidationSeverity Severity,
    int? ColumnIndex,
    string? ColumnCanonicalKey,
    string? RawHeader,
    ValidationMessage Message);
