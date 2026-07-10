namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline.Exceptions;

/// <summary>
/// Base commune des exceptions du Pipeline d'Import. Distincte d'une <see cref="PipelineDecision"/> :
/// une exception signale une impossibilité opérationnelle du pipeline lui-même (configuration
/// invalide), jamais une anomalie constatée dans la donnée métier — celle-ci est toujours
/// restituée sous forme de résultat (<see cref="PipelineResult"/>), jamais d'exception
/// (architecture/ERROR_HANDLING.md §2).
/// </summary>
public abstract class PipelineException : Exception
{
    protected PipelineException(string message) : base(message)
    {
    }
}
