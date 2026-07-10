using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Description d'une colonne métier canonique que le moteur de correspondance sait reconnaître
/// (features/IMPORT_ENGINE.md §2 : détection automatique, sans hypothèse sur les colonnes
/// réellement présentes dans un fichier donné). Entièrement porté par la donnée : aucun nom de
/// colonne, canonique ou synonyme, n'est codé en dur dans un algorithme — seule cette définition,
/// assemblée par un <see cref="SynonymDictionary"/> configurable, en porte la connaissance.
/// </summary>
/// <param name="Key">Identifiant technique stable et unique de la colonne canonique (ex. "Client").</param>
/// <param name="DisplayName">Libellé de référence, lisible, de la colonne canonique.</param>
/// <param name="Synonyms">
/// Ensemble des libellés bruts connus comme désignant cette même colonne canonique (ex. "Nom client",
/// "Raison sociale", "Customer"...). Ne contient jamais <see cref="DisplayName"/> lui-même.
/// </param>
/// <param name="IsRequired">
/// Indique si cette colonne est attendue dans tout fichier d'import (cf. features/IMPORT_ENGINE.md) ;
/// son absence est alors signalée comme colonne obligatoire manquante dans le rapport.
/// </param>
/// <param name="Group">Regroupement thématique de cette colonne, à des fins de restitution.</param>
/// <param name="ExpectedValueKind">
/// Nature de contenu attendue, utilisée uniquement pour nuancer une correspondance déjà établie
/// par le nom (cf. <see cref="ColumnValueKind"/>).
/// </param>
public sealed record CanonicalColumnDefinition(
    string Key,
    string DisplayName,
    IReadOnlyList<string> Synonyms,
    bool IsRequired,
    ColumnGroup Group,
    ColumnValueKind ExpectedValueKind = ColumnValueKind.Any)
{
    /// <summary>Valide qu'une définition est structurellement cohérente (clé et libellé renseignés).</summary>
    public static CanonicalColumnDefinition Create(
        string key,
        string displayName,
        IReadOnlyList<string>? synonyms = null,
        bool isRequired = false,
        ColumnGroup? group = null,
        ColumnValueKind expectedValueKind = ColumnValueKind.Any)
    {
        Guard.Against.NullOrWhiteSpace(key, nameof(key));
        Guard.Against.NullOrWhiteSpace(displayName, nameof(displayName));

        return new CanonicalColumnDefinition(
            key,
            displayName,
            synonyms ?? Array.Empty<string>(),
            isRequired,
            group ?? ColumnGroup.Unclassified,
            expectedValueKind);
    }

    /// <summary>Tous les libellés reconnus pour cette colonne canonique : le nom de référence puis ses synonymes.</summary>
    public IEnumerable<string> AllKnownLabels()
    {
        yield return DisplayName;
        foreach (var synonym in Synonyms)
        {
            yield return synonym;
        }
    }
}
