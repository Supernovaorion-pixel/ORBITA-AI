namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Rapport intégral et traçable d'une opération de validation. Aucune correction n'est jamais
/// appliquée : ce rapport est la seule source de vérité sur les constats et décisions prises
/// (exigence d'absence d'erreur silencieuse et de traçabilité intégrale).
/// </summary>
/// <param name="Findings">
/// Constats individuels, dans l'ordre où ils ont été détectés, limités à
/// <see cref="ValidationConfiguration.MaxFindingsToRetain"/> (cf. <see cref="Summary"/> pour le
/// dénombrement exact au-delà de cette limite).
/// </param>
/// <param name="Decisions">Trace lisible, dans l'ordre, des décisions prises pendant l'analyse.</param>
/// <param name="Summary">Vue d'ensemble condensée du contenu du rapport.</param>
public sealed record ValidationReport(
    IReadOnlyList<ValidationFinding> Findings,
    IReadOnlyList<ValidationDecision> Decisions,
    ValidationSummary Summary);
