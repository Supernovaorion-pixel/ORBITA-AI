namespace OrbitaAI.Core.Common.Errors;

/// <summary>
/// Anomalie de validation portant sur une propriété précise, agrégée par
/// <see cref="Error.Validation(System.Collections.Generic.IReadOnlyCollection{ValidationError})"/>
/// (architecture/ERROR_HANDLING.md §2 — erreur de donnée).
/// </summary>
/// <param name="PropertyName">Nom de la propriété ou du champ concerné.</param>
/// <param name="Message">Message explicite de l'anomalie constatée.</param>
public sealed record ValidationError(string PropertyName, string Message);
