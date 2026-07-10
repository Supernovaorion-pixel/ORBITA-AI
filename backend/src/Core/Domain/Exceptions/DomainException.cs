namespace OrbitaAI.Core.Domain.Exceptions;

/// <summary>
/// Base commune de toute exception représentant la violation d'un invariant du Domaine.
/// Réservée aux situations réellement exceptionnelles (invariant structurel rompu) ; un échec
/// métier attendu (donnée invalide, règle non satisfaite dans le flux normal) est représenté
/// par le Result Pattern (Common/Results/Result.cs), pas par une exception
/// (architecture/ERROR_HANDLING.md §2).
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
