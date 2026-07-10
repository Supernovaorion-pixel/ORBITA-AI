using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping.Exceptions;

namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Ensemble configurable de <see cref="CanonicalColumnDefinition"/> que le moteur de
/// correspondance utilise pour reconnaître les colonnes d'un fichier (features/IMPORT_ENGINE.md §2).
/// Aucun synonyme n'est codé en dur dans l'algorithme de reconnaissance : ce dictionnaire est la
/// seule source de cette connaissance, et peut être intégralement remplacé (cf.
/// <see cref="MappingOptions.SynonymDictionary"/>) sans modification du moteur.
/// </summary>
public sealed class SynonymDictionary
{
    private readonly IReadOnlyList<CanonicalColumnDefinition> _columns;

    public SynonymDictionary(IEnumerable<CanonicalColumnDefinition> columns)
    {
        Guard.Against.Null(columns, nameof(columns));
        _columns = columns.ToArray();

        var duplicateKey = _columns
            .GroupBy(c => c.Key, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicateKey is not null)
        {
            throw new InvalidSynonymDictionaryException(
                $"La clé canonique '{duplicateKey.Key}' est définie plusieurs fois dans le dictionnaire.");
        }
    }

    /// <summary>Ensemble des colonnes canoniques que ce dictionnaire sait reconnaître.</summary>
    public IReadOnlyList<CanonicalColumnDefinition> Columns => _columns;

    /// <summary>Colonnes canoniques marquées comme obligatoires (cf. <see cref="CanonicalColumnDefinition.IsRequired"/>).</summary>
    public IEnumerable<CanonicalColumnDefinition> RequiredColumns => _columns.Where(c => c.IsRequired);
}
