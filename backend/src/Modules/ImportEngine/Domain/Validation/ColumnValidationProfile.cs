namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Ensemble configurable des contraintes applicables à une colonne canonique (identifiée par sa
/// clé, cf. <c>CanonicalColumnDefinition.Key</c>). Porte exclusivement de la donnée de
/// configuration : aucune contrainte n'est codée en dur dans une règle
/// (Infrastructure/Validation/Rules/) — seul ce profil, construit par un
/// <see cref="ValidationRuleBuilder"/> ou fourni directement, en porte la connaissance.
/// </summary>
/// <param name="CanonicalKey">Clé de la colonne canonique concernée.</param>
/// <param name="ValueRequired">Une valeur est attendue sur chaque ligne pour cette colonne (au-delà de la seule présence de la colonne).</param>
/// <param name="MinLength">Longueur minimale attendue d'une valeur textuelle, ou <see langword="null"/> si non contrainte.</param>
/// <param name="MaxLength">Longueur maximale attendue d'une valeur textuelle, ou <see langword="null"/> si non contrainte.</param>
/// <param name="FormatPattern">Expression régulière que la valeur doit respecter, ou <see langword="null"/> si non contrainte.</param>
/// <param name="ForbiddenValues">Valeurs explicitement interdites pour cette colonne (comparaison insensible à la casse).</param>
/// <param name="MinNumericValue">Valeur numérique minimale autorisée, ou <see langword="null"/> si non contrainte.</param>
/// <param name="MaxNumericValue">Valeur numérique maximale autorisée, ou <see langword="null"/> si non contrainte.</param>
public sealed record ColumnValidationProfile(
    string CanonicalKey,
    bool ValueRequired = false,
    int? MinLength = null,
    int? MaxLength = null,
    string? FormatPattern = null,
    IReadOnlyList<string>? ForbiddenValues = null,
    double? MinNumericValue = null,
    double? MaxNumericValue = null)
{
    /// <summary>Valeurs interdites, jamais nulles (liste vide par défaut).</summary>
    public IReadOnlyList<string> ForbiddenValues { get; init; } = ForbiddenValues ?? Array.Empty<string>();
}
