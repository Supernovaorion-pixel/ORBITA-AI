using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Contexte complet fourni à un <see cref="IValidationRule"/> pour évaluer la valeur d'une
/// colonne canonique reconnue, pour une ligne donnée.
/// </summary>
/// <param name="Row">Ligne brute complète, telle que fournie par le Reader.</param>
/// <param name="ColumnIndex">Position de la colonne source concernée.</param>
/// <param name="Value">Valeur brute de la colonne pour cette ligne, ou <see langword="null"/> si absente.</param>
/// <param name="CanonicalColumn">Définition de la colonne canonique reconnue par le Mapping Engine.</param>
/// <param name="Profile">Contraintes configurées pour cette colonne canonique, si elles existent.</param>
/// <param name="Configuration">Configuration globale applicable à cette opération de validation.</param>
public sealed record ValidationRuleInput(
    RawRow Row,
    int ColumnIndex,
    object? Value,
    CanonicalColumnDefinition CanonicalColumn,
    ColumnValidationProfile? Profile,
    ValidationConfiguration Configuration);
