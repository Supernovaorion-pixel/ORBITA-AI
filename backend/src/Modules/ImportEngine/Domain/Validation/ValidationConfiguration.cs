namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Configuration par défaut du moteur de validation : seuil de blocage, garde-fous de
/// performance et bascules des vérifications génériques (par opposition à
/// <see cref="ValidationOptions"/>, propre à chaque appel).
/// </summary>
public sealed record ValidationConfiguration
{
    /// <summary>
    /// Sévérité minimale à partir de laquelle <see cref="ValidationResult.CanProceed"/> devient
    /// faux : une décision explicite et configurable, jamais un seuil figé dans l'algorithme.
    /// </summary>
    public ValidationSeverity BlockingSeverityThreshold { get; init; } = ValidationSeverity.Critical;

    /// <summary>
    /// Nombre maximal de constats individuellement conservés dans le rapport, garde-fou mémoire
    /// pour les fichiers de plusieurs millions de lignes dont le contenu serait très dégradé
    /// (architecture/PERFORMANCE_GUIDELINES.md). Au-delà, les constats supplémentaires restent
    /// comptabilisés dans <see cref="ValidationSummary"/> sans être détaillés individuellement.
    /// </summary>
    public int MaxFindingsToRetain { get; init; } = 10_000;

    /// <summary>Détecte les espaces superflus en début ou fin de valeur.</summary>
    public bool DetectSurroundingWhitespace { get; init; } = true;

    /// <summary>Détecte les espaces internes répétés (ex. « Jean   Dupont »).</summary>
    public bool DetectRepeatedInternalWhitespace { get; init; } = true;

    /// <summary>Détecte la présence de caractères de contrôle Unicode non imprimables.</summary>
    public bool DetectControlCharacters { get; init; } = true;

    /// <summary>Détecte la présence du caractère de remplacement Unicode (U+FFFD), signe d'un décodage défectueux.</summary>
    public bool DetectReplacementCharacter { get; init; } = true;

    /// <summary>
    /// Substitutions de sévérité par défaut, par <see cref="ValidationCode"/> — permet de
    /// reconfigurer la gravité d'un type de constat sans modifier la règle qui le produit.
    /// </summary>
    public IReadOnlyDictionary<string, ValidationSeverity> SeverityOverrides { get; init; } =
        new Dictionary<string, ValidationSeverity>();

    /// <summary>Configuration par défaut, adaptée à la très large majorité des cas d'usage.</summary>
    public static readonly ValidationConfiguration Default = new();
}
