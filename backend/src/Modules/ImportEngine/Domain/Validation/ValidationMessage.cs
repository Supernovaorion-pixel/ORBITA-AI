namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Contenu explicatif complet d'un constat de validation. Aucun constat n'est jamais restitué
/// sans ces trois éléments : un résumé, une explication de la cause, et une piste de résolution
/// (exigence d'absence d'erreur silencieuse).
/// </summary>
/// <param name="Summary">Description brève et factuelle du constat.</param>
/// <param name="Explanation">Explication de la règle appliquée et de la raison du constat.</param>
/// <param name="SuggestedResolution">
/// Piste de résolution suggérée à l'utilisateur ou à l'Administrateur — jamais une correction
/// automatique appliquée par le moteur lui-même.
/// </param>
public sealed record ValidationMessage(string Summary, string Explanation, string SuggestedResolution);
