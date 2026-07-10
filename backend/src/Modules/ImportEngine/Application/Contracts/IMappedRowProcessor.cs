using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Application.Contracts;

/// <summary>
/// Point d'extension permettant aux missions ultérieures (validation, fusion, analyse, ORION —
/// features/IMPORT_ENGINE.md §5-9, docs/AI_OVERVIEW.md) de consommer le résultat du moteur de
/// correspondance (<see cref="MappingResult"/>) conjointement au flux de lignes brutes du
/// Reader, sans jamais modifier le Mapping Engine lui-même
/// (architecture/EXTENSIBILITY.md §11 : le noyau ignore ce qui l'étend). Complète
/// <see cref="IRawRowProcessor"/> en donnant en plus accès à la correspondance de colonnes déjà
/// établie, pour que ces étapes futures résolvent une colonne canonique par sa clé
/// (<see cref="MappingResult.ColumnIndexByCanonicalKey"/>) plutôt que par une position devinée.
///
/// Squelette structurel uniquement : aucune implémentation de validation, fusion, analyse ou
/// interprétation par ORION n'est fournie par cette mission.
/// </summary>
public interface IMappedRowProcessor
{
    /// <summary>
    /// Transforme le flux de lignes brutes entrant en un flux de lignes brutes sortant, en
    /// s'appuyant sur <paramref name="mapping"/> pour interpréter la position des colonnes
    /// canoniques. Ne modifie jamais la correspondance elle-même.
    /// </summary>
    IAsyncEnumerable<RawRow> ProcessAsync(IAsyncEnumerable<RawRow> rows, MappingResult mapping, CancellationToken cancellationToken = default);
}
