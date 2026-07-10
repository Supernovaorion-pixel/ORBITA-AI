using OrbitaAI.Modules.ImportEngine.Domain;

namespace OrbitaAI.Modules.ImportEngine.Application.Contracts;

/// <summary>
/// Point d'extension permettant aux missions ultérieures (mapping des colonnes, validation,
/// fusion, analyse — features/IMPORT_ENGINE.md §4-9) de se brancher sur le flux de lignes
/// brutes produit par un <see cref="IFileReader"/>, sans jamais modifier le moteur de lecture
/// lui-même (architecture/EXTENSIBILITY.md §11 : le noyau ignore ce qui l'étend).
///
/// Chaque étape future (un mapper de colonnes, un validateur, une fusion, un moteur d'analyse)
/// implémente ce contrat et peut être chaînée après le Reader : <c>reader.ReadAsync(context)</c>
/// produit un <see cref="IAsyncEnumerable{RawRow}"/> que n'importe quel nombre de
/// <see cref="IRawRowProcessor"/> peut transformer successivement, chacun ignorant tout des
/// autres (composition, cf. architecture/CODING_PRINCIPLES.md §6).
///
/// Squelette structurel uniquement : aucune implémentation de mapping, validation, fusion ou
/// analyse n'est fournie par cette mission, conformément à son périmètre strict (lecture seule).
/// </summary>
public interface IRawRowProcessor
{
    /// <summary>
    /// Transforme le flux de lignes brutes entrant en un flux de lignes brutes sortant. Une
    /// implémentation de validation ou de mapping peut, par exemple, enrichir, filtrer ou
    /// réordonner les lignes sans jamais altérer la source lue par le Reader.
    /// </summary>
    IAsyncEnumerable<RawRow> ProcessAsync(IAsyncEnumerable<RawRow> rows, CancellationToken cancellationToken = default);
}
