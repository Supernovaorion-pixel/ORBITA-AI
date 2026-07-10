namespace OrbitaAI.Modules.ImportEngine.Domain.Validation.Exceptions;

/// <summary>
/// Base commune des exceptions du moteur de validation. Distincte d'un <see cref="ValidationFinding"/> :
/// une exception signale une impossibilité opérationnelle du moteur lui-même (configuration
/// invalide), jamais une anomalie constatée dans la donnée métier — celle-ci est toujours
/// restituée sous forme de constat, jamais d'exception (architecture/ERROR_HANDLING.md §2).
/// </summary>
public abstract class ValidationException : Exception
{
    protected ValidationException(string message) : base(message)
    {
    }
}
