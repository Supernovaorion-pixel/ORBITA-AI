using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Regroupe les éléments nécessaires à une opération de validation : le résultat de
/// correspondance de colonnes déjà établi par le Mapping Engine, les options et la configuration
/// applicables, et l'état statistique du traitement (cf. <c>ReaderContext</c> et
/// <c>MappingContext</c> pour le principe équivalent, dans un souci de cohérence architecturale
/// au sein du module).
/// </summary>
public sealed class ValidationContext
{
    /// <summary>Résultat de correspondance de colonnes produit par le Mapping Engine, préalable à toute validation.</summary>
    public required MappingResult Mapping { get; init; }

    /// <summary>
    /// Dictionnaire de colonnes canoniques utilisé lors de la correspondance ayant produit
    /// <see cref="Mapping"/> (cf. <c>MappingContext.EffectiveDictionary</c>) — nécessaire pour
    /// retrouver, pour chaque colonne reconnue, sa définition complète (type attendu, libellé).
    /// Fournir systématiquement le même dictionnaire que celui utilisé pour la correspondance,
    /// afin que les clés canoniques de <see cref="Mapping"/> y soient toutes résolubles.
    /// </summary>
    public required SynonymDictionary Dictionary { get; init; }

    /// <summary>Options propres à cette opération.</summary>
    public ValidationOptions Options { get; init; } = ValidationOptions.Default;

    /// <summary>Configuration de seuils et de garde-fous applicable à cette opération.</summary>
    public ValidationConfiguration Configuration { get; init; } = ValidationConfiguration.Default;

    /// <summary>Profil effectivement utilisé : celui des <see cref="Options"/>, ou le profil par défaut.</summary>
    public ValidationProfile EffectiveProfile => Options.Profile ?? DefaultValidationProfile.Create();

    /// <summary>
    /// État statistique de cette opération de validation, mis à jour par le pipeline au fil de
    /// l'avancement et consultable par l'appelant à tout moment, y compris après l'achèvement.
    /// </summary>
    public ValidationStatistics Statistics { get; } = new();
}
