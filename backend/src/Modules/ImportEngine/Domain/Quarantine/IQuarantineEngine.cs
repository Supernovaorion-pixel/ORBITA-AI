using OrbitaAI.Modules.ImportEngine.Domain.Pipeline;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

/// <summary>
/// Point d'entrée du Quarantine Engine (mission 010.4). Décide, pour une ligne et les constats de
/// validation qui lui sont propres, si celle-ci poursuit le pipeline ou doit être isolée. Ne
/// modifie, ne corrige et ne fusionne jamais aucune donnée : il ne fait qu'isoler ce qui ne
/// respecte pas le seuil configuré, en conservant l'intégralité de la donnée originale.
/// </summary>
public interface IQuarantineEngine
{
    /// <summary>
    /// Classe <paramref name="row"/> à partir des constats de validation qui lui sont propres
    /// (<paramref name="findings"/>). Retourne <see langword="null"/> si la ligne peut poursuivre
    /// le pipeline, ou la <see cref="RejectedRow"/> à conserver en quarantaine dans le cas contraire.
    /// </summary>
    RejectedRow? Classify(RawRow row, IReadOnlyList<ValidationFinding> findings, PipelineConfiguration configuration);
}
