namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Rapport intégral et traçable d'une opération de correspondance de colonnes : ce qui a été
/// reconnu, ce qui ne l'a pas été, ce qui reste ambigu, ce qui manque, et pourquoi. Aucune
/// correction automatique n'est jamais appliquée silencieusement : ce rapport est la seule
/// source de vérité sur les décisions prises (exigence de traçabilité intégrale).
/// </summary>
/// <param name="RecognizedColumns">Colonnes reconnues avec une confiance suffisante et sans conflit.</param>
/// <param name="UnknownColumns">Colonnes dont le libellé n'a pu être rapproché d'aucune colonne canonique.</param>
/// <param name="AmbiguousColumns">
/// Colonnes dont la reconnaissance reste incertaine, y compris lorsque plusieurs colonnes
/// revendiquent la même colonne canonique (colonnes dupliquées).
/// </param>
/// <param name="MissingRequiredColumns">Colonnes canoniques obligatoires n'ayant été reconnues dans aucune colonne source.</param>
/// <param name="GlobalRecognitionScore">
/// Score global de reconnaissance du fichier, de 0 à 100, pondérant davantage les colonnes
/// obligatoires que les colonnes optionnelles (cf. Infrastructure/Mapping/MappingEngine.cs).
/// </param>
/// <param name="Decisions">Trace lisible, dans l'ordre, de chaque décision prise pendant l'analyse.</param>
public sealed record MappingReport(
    IReadOnlyList<ColumnMappingOutcome> RecognizedColumns,
    IReadOnlyList<ColumnMappingOutcome> UnknownColumns,
    IReadOnlyList<ColumnMappingOutcome> AmbiguousColumns,
    IReadOnlyList<CanonicalColumnDefinition> MissingRequiredColumns,
    double GlobalRecognitionScore,
    IReadOnlyList<string> Decisions);
