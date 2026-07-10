using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

/// <summary>
/// Ensemble des lignes mises en quarantaine au cours d'une exécution du Pipeline d'Import.
/// Contrairement au rapport de validation (cf. <c>ValidationReport.Findings</c>, borné pour des
/// raisons de mémoire), cette collection ne tronque jamais son contenu : aucune donnée rejetée ne
/// doit être perdue (mission 010.4). Son empreinte mémoire est proportionnelle au nombre de lignes
/// effectivement rejetées, jamais au volume total du fichier traité.
/// </summary>
public sealed class RejectedRowCollection
{
    private readonly List<RejectedRow> _rows = [];
    private readonly Dictionary<RejectedRowCategory, int> _countsByCategory = [];

    /// <summary>Lignes rejetées, dans l'ordre où elles ont été rencontrées.</summary>
    public IReadOnlyList<RejectedRow> Rows => _rows;

    /// <summary>Nombre total de lignes rejetées.</summary>
    public int Count => _rows.Count;

    /// <summary>Répartition du nombre de lignes rejetées par catégorie dominante.</summary>
    public IReadOnlyDictionary<RejectedRowCategory, int> CountsByCategory => _countsByCategory;

    /// <summary>Ajoute une ligne rejetée à la collection.</summary>
    public void Add(RejectedRow rejectedRow)
    {
        Guard.Against.Null(rejectedRow, nameof(rejectedRow));

        _rows.Add(rejectedRow);
        _countsByCategory[rejectedRow.Category] = _countsByCategory.GetValueOrDefault(rejectedRow.Category) + 1;
    }
}
