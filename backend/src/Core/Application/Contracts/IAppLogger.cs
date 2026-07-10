namespace OrbitaAI.Core.Application.Contracts;

/// <summary>
/// Contrat de journalisation technique générale (architecture/LOGGING_STRATEGY.md §2),
/// catégorisé par <typeparamref name="TCategory"/> (typiquement le type appelant), à l'image
/// des abstractions de journalisation courantes de l'écosystème .NET. Aucune implémentation
/// n'est fournie dans le Core : le choix du fournisseur de journalisation concret est une
/// décision d'Infrastructure hors du périmètre de cette mission.
/// </summary>
/// <typeparam name="TCategory">Type dont le nom qualifie l'origine des entrées journalisées.</typeparam>
public interface IAppLogger<out TCategory>
{
    /// <summary>Journalise une information de fonctionnement courant, sans caractère d'anomalie.</summary>
    void LogInformation(string message, params object?[] args);

    /// <summary>Journalise une situation à surveiller, sans impact immédiat (architecture/LOGGING_STRATEGY.md §6).</summary>
    void LogWarning(string message, params object?[] args);

    /// <summary>Journalise un échec d'opération, avec l'exception associée le cas échéant (architecture/ERROR_HANDLING.md §3).</summary>
    void LogError(Exception? exception, string message, params object?[] args);
}
