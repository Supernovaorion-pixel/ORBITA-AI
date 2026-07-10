namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Résultat exploitable d'une opération de validation, destiné aux futures missions (fusion,
/// analyse, ORION — cf. Application/Contracts/IValidatedRowProcessor.cs) qui doivent savoir si
/// les données peuvent être poursuivies dans le pipeline, sans avoir à réinterpréter elles-mêmes
/// le détail des constats.
/// </summary>
/// <param name="CanProceed">
/// Indique si aucun constat n'atteint <see cref="ValidationConfiguration.BlockingSeverityThreshold"/> :
/// une décision explicite, jamais une correction ou un contournement automatique des anomalies constatées.
/// </param>
/// <param name="Report">Rapport intégral et traçable de l'opération (cf. <see cref="ValidationReport"/>).</param>
/// <param name="Statistics">État du déroulement du traitement (cf. <see cref="ValidationStatistics"/>).</param>
public sealed record ValidationResult(bool CanProceed, ValidationReport Report, ValidationStatistics Statistics);
