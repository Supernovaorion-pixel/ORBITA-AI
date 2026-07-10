namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline.Exceptions;

/// <summary>
/// La configuration fournie au Pipeline d'Import est structurellement invalide (ex. taille
/// d'échantillon de correspondance ou intervalle de notification de progression non strictement
/// positif).
/// </summary>
public sealed class InvalidPipelineConfigurationException : PipelineException
{
    public InvalidPipelineConfigurationException(string message) : base(message)
    {
    }
}
