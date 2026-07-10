using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Décision traçable prise par le Pipeline d'Import au cours de son exécution (ex. suspension de
/// l'import faute de colonne obligatoire, franchissement du seuil de quarantaine), au même titre
/// que <see cref="Domain.Validation.ValidationDecision"/> pour le seul moteur de validation.
/// Garantit qu'aucune décision affectant le déroulement de l'import ne reste implicite
/// (architecture/ERROR_HANDLING.md §6 — traçabilité).
/// </summary>
/// <param name="Description">Description factuelle et compréhensible de la décision prise.</param>
/// <param name="Severity">Sévérité associée à cette décision.</param>
public sealed record PipelineDecision(string Description, ValidationSeverity Severity);
