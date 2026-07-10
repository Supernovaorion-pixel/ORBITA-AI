namespace OrbitaAI.Core.Common.Errors;

/// <summary>
/// Catégorie d'un <see cref="Error"/>, utilisée par le Result Pattern (Common/Results/Result.cs)
/// pour distinguer la nature d'un échec sans recourir à des exceptions pour le flux de contrôle
/// normal (architecture/ERROR_HANDLING.md §2).
/// </summary>
public enum ErrorType
{
    /// <summary>Échec générique, sans catégorie plus précise.</summary>
    Failure,

    /// <summary>Une donnée fournie ne respecte pas les règles attendues (architecture/ERROR_HANDLING.md §2).</summary>
    Validation,

    /// <summary>L'entité ou la ressource demandée n'existe pas dans le périmètre accessible.</summary>
    NotFound,

    /// <summary>L'opération entre en conflit avec un état existant (ex. doublon).</summary>
    Conflict,

    /// <summary>L'utilisateur n'est pas authentifié.</summary>
    Unauthorized,

    /// <summary>L'utilisateur est authentifié mais n'a pas les droits requis (architecture/ERROR_HANDLING.md §2).</summary>
    Forbidden
}
