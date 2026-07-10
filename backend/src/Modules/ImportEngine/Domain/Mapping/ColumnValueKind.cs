namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Nature de contenu attendue pour une colonne canonique, utilisée uniquement à titre indicatif
/// par le <see cref="IConfidenceEngine"/> pour confirmer ou nuancer une correspondance déjà
/// établie par le nom de la colonne (jamais pour reconnaître une colonne par son seul contenu —
/// cf. Domain/Mapping/IConfidenceEngine.cs). Une règle déterministe, jamais une inférence
/// statistique ou une IA.
/// </summary>
public enum ColumnValueKind
{
    /// <summary>Aucune attente particulière : le contenu n'influence jamais la confiance.</summary>
    Any,

    /// <summary>Texte libre (ex. nom de client, libellé produit).</summary>
    Text,

    /// <summary>Valeur numérique (ex. montant, quantité).</summary>
    Numeric,

    /// <summary>Date ou horodatage.</summary>
    Date
}
