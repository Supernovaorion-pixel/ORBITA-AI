using System.Text;

namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Options propres à une opération de lecture donnée (features/IMPORT_ENGINE.md §2 : détection
/// automatique). Une valeur laissée à <see langword="null"/> signifie que le lecteur doit la
/// déterminer automatiquement plutôt que de supposer une structure particulière — aucune
/// hypothèse n'est faite sur la présence, l'ordre ou le nom des colonnes.
/// </summary>
public sealed record ReaderOptions
{
    /// <summary>Indique si la première ligne du fichier contient les noms de colonnes.</summary>
    public bool HasHeaderRow { get; init; } = true;

    /// <summary>
    /// Délimiteur de champ pour un fichier CSV. Laissé à <see langword="null"/>, le délimiteur
    /// est détecté automatiquement à partir des délimiteurs candidats usuels.
    /// </summary>
    public char? Delimiter { get; init; }

    /// <summary>
    /// Encodage du fichier texte (CSV). Laissé à <see langword="null"/>, l'encodage est déterminé
    /// à partir de la marque d'ordre des octets (BOM) lorsqu'elle est présente, UTF-8 sinon.
    /// </summary>
    public Encoding? Encoding { get; init; }

    /// <summary>
    /// Nom de la feuille à lire pour un classeur Excel. Laissé à <see langword="null"/>, la
    /// première feuille du classeur est utilisée.
    /// </summary>
    public string? SheetName { get; init; }

    /// <summary>
    /// Ignore les lignes entièrement vides rencontrées pendant la lecture, plutôt que de les
    /// restituer comme des <see cref="RawRow"/> sans valeur exploitable.
    /// </summary>
    public bool SkipEmptyRows { get; init; } = true;

    /// <summary>
    /// Nombre maximal de lignes de données à lire, au-delà duquel la lecture s'arrête
    /// normalement (utile pour un aperçu partiel). <see langword="null"/> signifie l'absence de limite.
    /// </summary>
    public int? MaxRows { get; init; }

    /// <summary>Options par défaut, appliquées lorsqu'aucune personnalisation n'est requise.</summary>
    public static readonly ReaderOptions Default = new();
}
