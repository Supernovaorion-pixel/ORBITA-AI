using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Application.Contracts;

/// <summary>
/// Point d'extension permettant aux missions ultérieures (fusion, analyse, ORION —
/// features/IMPORT_ENGINE.md §6-9, docs/AI_OVERVIEW.md) de consommer le résultat du moteur de
/// validation (<see cref="ValidationResult"/>) conjointement au résultat de correspondance
/// (<see cref="MappingResult"/>) et au flux de lignes brutes du Reader, sans jamais modifier le
/// Validation Engine lui-même (architecture/EXTENSIBILITY.md §11). Complète
/// <see cref="IMappedRowProcessor"/> en donnant en plus accès aux constats de validation déjà
/// établis, pour que ces étapes futures puissent décider d'agir différemment selon
/// <see cref="ValidationResult.CanProceed"/> — sans jamais avoir à revalider elles-mêmes les données.
///
/// Squelette structurel uniquement : aucune implémentation de fusion, d'analyse ou
/// d'interprétation par ORION n'est fournie par cette mission.
/// </summary>
public interface IValidatedRowProcessor
{
    /// <summary>
    /// Transforme le flux de lignes brutes entrant en un flux de lignes brutes sortant, en
    /// s'appuyant sur <paramref name="mapping"/> et <paramref name="validation"/>. Ne modifie
    /// jamais ces deux résultats.
    /// </summary>
    IAsyncEnumerable<RawRow> ProcessAsync(
        IAsyncEnumerable<RawRow> rows,
        MappingResult mapping,
        ValidationResult validation,
        CancellationToken cancellationToken = default);
}
