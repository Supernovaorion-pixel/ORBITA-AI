using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Mesures techniques détaillées d'un import, complémentaires du résumé destiné à l'utilisateur
/// (cf. <see cref="ImportSummary"/>) : détail de la correspondance des colonnes et répartition des
/// constats de validation par sévérité, à des fins de diagnostic (architecture/LOGGING_STRATEGY.md).
/// </summary>
/// <param name="TotalFindings">Nombre total de constats de validation rencontrés (structurels et par ligne).</param>
/// <param name="StructuralFindingsCount">Nombre de constats structurels (colonnes manquantes, inconnues, ambiguës).</param>
/// <param name="FindingsBySeverity">Répartition du nombre de constats par sévérité.</param>
/// <param name="ColumnsRecognized">Nombre de colonnes reconnues avec certitude par le Mapping Engine.</param>
/// <param name="ColumnsUnknown">Nombre de colonnes non reconnues par le Mapping Engine.</param>
/// <param name="ColumnsAmbiguous">Nombre de colonnes ambiguës ou en conflit selon le Mapping Engine.</param>
/// <param name="GlobalRecognitionScore">Score global de reconnaissance des colonnes, sur 100.</param>
public sealed record ImportMetrics(
    long TotalFindings,
    long StructuralFindingsCount,
    IReadOnlyDictionary<ValidationSeverity, long> FindingsBySeverity,
    int ColumnsRecognized,
    int ColumnsUnknown,
    int ColumnsAmbiguous,
    double GlobalRecognitionScore);
