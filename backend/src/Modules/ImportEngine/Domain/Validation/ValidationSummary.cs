namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Vue d'ensemble condensée du contenu d'un <see cref="ValidationReport"/>, destinée à une
/// consultation rapide (ex. carte de synthèse, cf. ux/DASHBOARD_SPECIFICATION.md pour le
/// principe équivalent côté restitution — aucune interface n'est construite ici). Distincte de
/// <see cref="ValidationStatistics"/>, qui porte sur le déroulement du traitement et non sur le
/// contenu des constats.
/// </summary>
/// <param name="TotalFindings">Nombre total de constats produits (avant troncature éventuelle, cf. <see cref="ValidationConfiguration.MaxFindingsToRetain"/>).</param>
/// <param name="InformationCount">Nombre de constats de sévérité <see cref="ValidationSeverity.Information"/>.</param>
/// <param name="WarningCount">Nombre de constats de sévérité <see cref="ValidationSeverity.Warning"/>.</param>
/// <param name="ErrorCount">Nombre de constats de sévérité <see cref="ValidationSeverity.Error"/>.</param>
/// <param name="CriticalCount">Nombre de constats de sévérité <see cref="ValidationSeverity.Critical"/>.</param>
/// <param name="AffectedRowCount">Nombre de lignes distinctes portant au moins un constat.</param>
/// <param name="FindingsTruncated">
/// Indique si le nombre de constats a dépassé <see cref="ValidationConfiguration.MaxFindingsToRetain"/> :
/// dans ce cas, <see cref="TotalFindings"/> reste exact, mais seul un sous-ensemble est
/// individuellement détaillé dans le rapport (garde-fou mémoire, jamais une perte d'information
/// sur l'ampleur réelle du problème).
/// </param>
public sealed record ValidationSummary(
    int TotalFindings,
    int InformationCount,
    int WarningCount,
    int ErrorCount,
    int CriticalCount,
    int AffectedRowCount,
    bool FindingsTruncated);
