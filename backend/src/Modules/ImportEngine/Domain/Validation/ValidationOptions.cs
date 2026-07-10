namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>Options propres à une opération de validation donnée.</summary>
public sealed record ValidationOptions
{
    /// <summary>
    /// Profil de contraintes par colonne à appliquer. Laissé à <see langword="null"/>, le profil
    /// par défaut (<see cref="DefaultValidationProfile"/>) est utilisé.
    /// </summary>
    public ValidationProfile? Profile { get; init; }

    /// <summary>Options par défaut.</summary>
    public static readonly ValidationOptions Default = new();
}
