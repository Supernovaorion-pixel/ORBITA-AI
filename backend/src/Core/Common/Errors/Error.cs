namespace OrbitaAI.Core.Common.Errors;

/// <summary>
/// Représente un échec de façon explicite et typée, consommé par le Result Pattern
/// (Common/Results/Result.cs). Remplace, pour les échecs métier attendus, le recours à des
/// exceptions réservées aux situations réellement exceptionnelles (architecture/ERROR_HANDLING.md §2).
/// </summary>
/// <param name="Code">Identifiant technique stable de l'erreur (ex. "Import.InvalidFormat").</param>
/// <param name="Message">Message explicite et actionnable destiné au diagnostic ou à l'utilisateur
/// (cf. ux/ERROR_STATES.md §1, architecture/ERROR_HANDLING.md §4).</param>
/// <param name="Type">Catégorie de l'erreur (cf. <see cref="ErrorType"/>).</param>
/// <param name="ValidationErrors">Détail des anomalies de validation, renseigné uniquement lorsque
/// <paramref name="Type"/> vaut <see cref="ErrorType.Validation"/>.</param>
public sealed record Error(
    string Code,
    string Message,
    ErrorType Type = ErrorType.Failure,
    IReadOnlyCollection<ValidationError>? ValidationErrors = null)
{
    /// <summary>Absence d'erreur, utilisé exclusivement par un Result en état de succès.</summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>Construit une erreur générique (<see cref="ErrorType.Failure"/>).</summary>
    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    /// <summary>Construit une erreur signalant une ressource introuvable (<see cref="ErrorType.NotFound"/>).</summary>
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    /// <summary>Construit une erreur signalant un conflit avec un état existant (<see cref="ErrorType.Conflict"/>).</summary>
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    /// <summary>Construit une erreur d'authentification (<see cref="ErrorType.Unauthorized"/>).</summary>
    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);

    /// <summary>Construit une erreur de droits insuffisants (<see cref="ErrorType.Forbidden"/>).</summary>
    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);

    /// <summary>
    /// Construit une erreur agrégeant un ensemble d'anomalies de validation (<see cref="ErrorType.Validation"/>),
    /// conformément à architecture/ERROR_HANDLING.md §2.
    /// </summary>
    public static Error Validation(IReadOnlyCollection<ValidationError> validationErrors)
    {
        if (validationErrors is null || validationErrors.Count == 0)
        {
            throw new ArgumentException("Au moins une anomalie de validation doit être fournie.", nameof(validationErrors));
        }

        var summary = string.Join("; ", validationErrors.Select(e => $"{e.PropertyName}: {e.Message}"));
        return new Error("Validation.Failed", summary, ErrorType.Validation, validationErrors);
    }
}
