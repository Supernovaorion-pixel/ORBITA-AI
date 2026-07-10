namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Constat unique et intégralement traçable produit par une règle de validation. Ne modifie ni
/// ne corrige jamais la donnée concernée : il ne fait que la décrire (features/IMPORT_ENGINE.md §5).
/// </summary>
/// <param name="Code">Identifiant technique stable du type de constat.</param>
/// <param name="Category">Nature structurelle du constat (cf. <see cref="ValidationCategory"/>).</param>
/// <param name="Severity">Gravité du constat (cf. <see cref="ValidationSeverity"/>).</param>
/// <param name="RowNumber">
/// Ligne de données concernée, à partir de 1 ; 0 pour un constat structurel ne portant sur
/// aucune ligne en particulier (colonne obligatoire manquante, colonne dupliquée...).
/// </param>
/// <param name="ColumnIndex">Position de la colonne source concernée, ou <see langword="null"/> pour un constat sans colonne unique.</param>
/// <param name="ColumnCanonicalKey">Clé canonique de la colonne concernée, si elle a été reconnue par le Mapping Engine.</param>
/// <param name="RawHeader">Libellé brut de la colonne concernée, tel que lu par le Reader.</param>
/// <param name="Message">Contenu explicatif complet (cf. <see cref="ValidationMessage"/>).</param>
public sealed record ValidationFinding(
    ValidationCode Code,
    ValidationCategory Category,
    ValidationSeverity Severity,
    int RowNumber,
    int? ColumnIndex,
    string? ColumnCanonicalKey,
    string? RawHeader,
    ValidationMessage Message);
