namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Ligne brute lue depuis un fichier source, sans aucune interprétation, correspondance de
/// colonne, validation ou nettoyage (hors périmètre de cette mission — cf. features/IMPORT_ENGINE.md
/// §4-5, développés lors de missions ultérieures). Les valeurs conservent leur type natif tel que
/// lu dans le fichier source (ex. nombre Excel en <see cref="double"/>, texte CSV en
/// <see cref="string"/>) : aucune conversion sémantique (ex. date, devise) n'est appliquée ici,
/// conformément à l'exigence de ne faire aucune hypothèse sur le contenu des colonnes.
/// </summary>
/// <param name="RowNumber">
/// Rang de la ligne de données, à partir de 1, à l'exclusion de l'éventuelle ligne d'en-tête.
/// </param>
/// <param name="Headers">
/// Noms de colonnes tels que lus dans le fichier, ou liste vide si le fichier ne comporte pas de
/// ligne d'en-tête (cf. <see cref="ReaderOptions.HasHeaderRow"/>). Cette même instance est
/// partagée par toutes les lignes d'une même lecture, sans copie, pour limiter l'empreinte mémoire
/// sur les fichiers volumineux.
/// </param>
/// <param name="Values">Valeurs brutes de la ligne, dans l'ordre où les colonnes apparaissent dans le fichier.</param>
public sealed record RawRow(int RowNumber, IReadOnlyList<string> Headers, IReadOnlyList<object?> Values);
