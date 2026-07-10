namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation.Rules;

/// <summary>Inspection de valeur partagée par l'ensemble des règles fournies par défaut, pour éviter toute duplication.</summary>
internal static class ValidationValueHelpers
{
    /// <summary>Vrai si la valeur est absente ou une chaîne vide/uniquement composée d'espaces.</summary>
    public static bool IsBlank(object? value) => value switch
    {
        null => true,
        string text => string.IsNullOrWhiteSpace(text),
        _ => false,
    };

    /// <summary>Représentation textuelle de la valeur, ou <see langword="null"/> si absente.</summary>
    public static string? AsText(object? value) => value?.ToString();
}
