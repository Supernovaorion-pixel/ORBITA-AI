namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Point d'entrée du moteur de reconnaissance de colonnes (features/IMPORT_ENGINE.md §4).
/// Totalement indépendant du Reader (Domain/IFileReader.cs) : n'opère que sur des
/// <see cref="RawRow"/> déjà fournis par l'appelant, jamais sur un fichier ou un flux. Ne
/// nettoie, ne fusionne, ne valide et ne calcule jamais aucune donnée métier — il identifie
/// uniquement la correspondance entre colonnes source et colonnes canoniques.
/// </summary>
public interface IMappingEngine
{
    /// <summary>
    /// Analyse les colonnes décrites par <paramref name="headers"/>, à l'aide de l'échantillon
    /// <paramref name="sampleRows"/> pour le profilage de contenu, et retourne le résultat complet
    /// de la correspondance. Opération synchrone, purement en mémoire, sans entrée/sortie.
    /// </summary>
    /// <exception cref="Exceptions.NoHeaderRowException"><paramref name="headers"/> est vide.</exception>
    MappingResult Analyze(IReadOnlyList<string> headers, IReadOnlyList<RawRow> sampleRows, MappingContext context);
}
