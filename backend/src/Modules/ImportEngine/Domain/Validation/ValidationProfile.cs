using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Ensemble configurable de <see cref="ColumnValidationProfile"/>, un par colonne canonique
/// contrainte. Une colonne canonique sans profil explicite n'est soumise à aucune contrainte
/// propre à sa valeur au-delà des vérifications génériques toujours actives (type attendu,
/// espacement, encodage — cf. Infrastructure/Validation/Rules/).
/// </summary>
public sealed class ValidationProfile
{
    private readonly IReadOnlyDictionary<string, ColumnValidationProfile> _columnProfiles;

    public ValidationProfile(IEnumerable<ColumnValidationProfile> columnProfiles)
    {
        Guard.Against.Null(columnProfiles, nameof(columnProfiles));
        _columnProfiles = columnProfiles.ToDictionary(p => p.CanonicalKey, StringComparer.Ordinal);
    }

    /// <summary>Profil configuré pour <paramref name="canonicalKey"/>, ou <see langword="null"/> si aucune contrainte propre n'est définie.</summary>
    public ColumnValidationProfile? GetProfile(string canonicalKey) =>
        _columnProfiles.GetValueOrDefault(canonicalKey);

    /// <summary>Profil vide : aucune colonne canonique n'est soumise à une contrainte propre.</summary>
    public static readonly ValidationProfile Empty = new(Array.Empty<ColumnValidationProfile>());
}
