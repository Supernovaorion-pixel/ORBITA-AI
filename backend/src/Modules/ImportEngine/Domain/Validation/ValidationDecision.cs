namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Trace lisible d'une décision prise par le moteur de validation (ex. « le fichier ne peut pas
/// être poursuivi : colonne obligatoire manquante »), au même titre que les décisions du Mapping
/// Engine (cf. Domain/Mapping/MappingReport.cs) — aucune décision n'est jamais implicite.
/// </summary>
public sealed record ValidationDecision(string Description, ValidationSeverity Severity);
