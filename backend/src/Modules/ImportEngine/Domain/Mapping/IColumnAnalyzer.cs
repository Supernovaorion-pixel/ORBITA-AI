using OrbitaAI.Modules.ImportEngine.Domain;

namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Construit le profil de chaque colonne source (cf. <see cref="ColumnProfile"/>) à partir des
/// en-têtes et d'un échantillon borné de lignes. Ne relit jamais le fichier source : n'exploite
/// que les <see cref="RawRow"/> qui lui sont fournis, jusqu'à concurrence de
/// <see cref="MappingConfiguration.MaxContentSampleRows"/> (architecture/PERFORMANCE_GUIDELINES.md).
/// </summary>
public interface IColumnAnalyzer
{
    /// <summary>
    /// Construit un <see cref="ColumnProfile"/> par colonne présente dans <paramref name="headers"/>,
    /// en profilant le contenu à partir de <paramref name="sampleRows"/> si
    /// <paramref name="analyzeContent"/> est vrai.
    /// </summary>
    IReadOnlyList<ColumnProfile> AnalyzeColumns(
        IReadOnlyList<string> headers,
        IReadOnlyList<RawRow> sampleRows,
        MappingConfiguration configuration,
        bool analyzeContent);
}
