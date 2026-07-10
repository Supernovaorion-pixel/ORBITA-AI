namespace OrbitaAI.Modules.ImportEngine.Domain.Validation.Exceptions;

/// <summary>
/// Le profil de validation fourni est structurellement invalide (ex. longueur minimale
/// supérieure à la longueur maximale pour une même colonne).
/// </summary>
public sealed class InvalidValidationProfileException : ValidationException
{
    public InvalidValidationProfileException(string message) : base(message)
    {
    }
}
