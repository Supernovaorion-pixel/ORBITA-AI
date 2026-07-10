namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Dictionnaire de synonymes officiel fourni par défaut, couvrant les champs commerciaux usuels
/// évoqués dans features/IMPORT_ENGINE.md et docs/GLOSSARY.md, tels qu'ils apparaissent dans des
/// exports d'ERP courants (ex. Sage) sans dépendre d'aucun d'entre eux en particulier.
///
/// Cette classe ne contient que de la <b>donnée</b> de configuration : aucune règle de
/// reconnaissance n'y est implémentée (celle-ci vit exclusivement dans
/// Infrastructure/Mapping/HeaderAnalyzer.cs et ConfidenceEngine.cs). Toute Organisation cliente
/// peut fournir son propre <see cref="SynonymDictionary"/> à la place de celui-ci, en totalité ou
/// par extension, sans modification du moteur (cf. <see cref="MappingOptions.SynonymDictionary"/>).
/// </summary>
public static class DefaultSynonymDictionary
{
    /// <summary>Construit le dictionnaire de synonymes par défaut.</summary>
    public static SynonymDictionary Create() => new(BuildColumns());

    private static IEnumerable<CanonicalColumnDefinition> BuildColumns()
    {
        yield return CanonicalColumnDefinition.Create(
            key: "Client",
            displayName: "Client",
            synonyms: ["Nom client", "Raison sociale", "Customer", "Company", "Compte", "Code client", "Client Name"],
            isRequired: true,
            group: ColumnGroup.Identification,
            expectedValueKind: ColumnValueKind.Text);

        yield return CanonicalColumnDefinition.Create(
            key: "Commercial",
            displayName: "Commercial",
            synonyms: ["Sales Rep", "Vendeur", "Responsable"],
            isRequired: false,
            group: ColumnGroup.Identification,
            expectedValueKind: ColumnValueKind.Text);

        yield return CanonicalColumnDefinition.Create(
            key: "MontantHT",
            displayName: "Montant HT",
            synonyms: ["CA", "Net Sales", "Revenue", "Chiffre d'affaires"],
            isRequired: true,
            group: ColumnGroup.Financial,
            expectedValueKind: ColumnValueKind.Numeric);

        yield return CanonicalColumnDefinition.Create(
            key: "Famille",
            displayName: "Famille",
            synonyms: ["Famille Produit", "Product Family"],
            isRequired: false,
            group: ColumnGroup.Product,
            expectedValueKind: ColumnValueKind.Text);

        yield return CanonicalColumnDefinition.Create(
            key: "Produit",
            displayName: "Produit",
            synonyms: ["Product", "Article", "Reference produit", "SKU"],
            isRequired: false,
            group: ColumnGroup.Product,
            expectedValueKind: ColumnValueKind.Text);

        yield return CanonicalColumnDefinition.Create(
            key: "Date",
            displayName: "Date",
            synonyms: ["Date facture", "Invoice Date"],
            isRequired: true,
            group: ColumnGroup.Temporal,
            expectedValueKind: ColumnValueKind.Date);

        yield return CanonicalColumnDefinition.Create(
            key: "Annee",
            displayName: "Année",
            synonyms: ["Year", "Annee"],
            isRequired: false,
            group: ColumnGroup.Temporal,
            expectedValueKind: ColumnValueKind.Numeric);

        yield return CanonicalColumnDefinition.Create(
            key: "Mois",
            displayName: "Mois",
            synonyms: ["Month"],
            isRequired: false,
            group: ColumnGroup.Temporal,
            expectedValueKind: ColumnValueKind.Numeric);

        yield return CanonicalColumnDefinition.Create(
            key: "Quantite",
            displayName: "Quantité",
            synonyms: ["Quantity", "Qty", "Qte"],
            isRequired: false,
            group: ColumnGroup.Financial,
            expectedValueKind: ColumnValueKind.Numeric);

        yield return CanonicalColumnDefinition.Create(
            key: "Marge",
            displayName: "Marge",
            synonyms: ["Margin", "Marge brute"],
            isRequired: false,
            group: ColumnGroup.Financial,
            expectedValueKind: ColumnValueKind.Numeric);

        yield return CanonicalColumnDefinition.Create(
            key: "MargePercent",
            displayName: "% Marge",
            synonyms: ["Margin %", "Marge %", "Taux de marge", "Margin Rate"],
            isRequired: false,
            group: ColumnGroup.Financial,
            expectedValueKind: ColumnValueKind.Numeric);
    }
}
