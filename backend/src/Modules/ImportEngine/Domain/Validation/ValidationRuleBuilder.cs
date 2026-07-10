using System.Text.RegularExpressions;
using OrbitaAI.Core.Common.Guards;

namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Construit un <see cref="ValidationProfile"/> de façon fluide et lisible, sans jamais coder en
/// dur de règle métier dans un algorithme : ce constructeur ne fait qu'assembler la donnée de
/// configuration fournie par l'appelant (cf. <see cref="ColumnValidationProfile"/>).
/// </summary>
/// <example>
/// <code>
/// new ValidationRuleBuilder()
///     .ForColumn("Client").Required().MaxLength(250)
///     .ForColumn("MontantHT").Required().NumericRange(min: 0)
///     .Build();
/// </code>
/// </example>
public sealed class ValidationRuleBuilder
{
    private readonly List<ColumnValidationProfile> _profiles = [];

    /// <summary>Débute (ou reprend) la configuration de la colonne canonique <paramref name="canonicalKey"/>.</summary>
    public ColumnRuleBuilder ForColumn(string canonicalKey)
    {
        Guard.Against.NullOrWhiteSpace(canonicalKey, nameof(canonicalKey));
        return new ColumnRuleBuilder(this, canonicalKey);
    }

    /// <summary>Termine la construction et retourne le profil assemblé.</summary>
    public ValidationProfile Build() => new(_profiles);

    internal void AddProfile(ColumnValidationProfile profile) => _profiles.Add(profile);
}

/// <summary>
/// Construit de façon fluide le <see cref="ColumnValidationProfile"/> d'une colonne canonique
/// unique, dans le cadre d'un <see cref="ValidationRuleBuilder"/> englobant.
/// </summary>
public sealed class ColumnRuleBuilder
{
    private readonly ValidationRuleBuilder _parent;
    private readonly string _canonicalKey;
    private bool _committed;

    private bool _valueRequired;
    private int? _minLength;
    private int? _maxLength;
    private string? _formatPattern;
    private IReadOnlyList<string> _forbiddenValues = Array.Empty<string>();
    private double? _minNumericValue;
    private double? _maxNumericValue;

    internal ColumnRuleBuilder(ValidationRuleBuilder parent, string canonicalKey)
    {
        _parent = parent;
        _canonicalKey = canonicalKey;
    }

    /// <summary>Exige qu'une valeur non vide soit présente pour cette colonne sur chaque ligne.</summary>
    public ColumnRuleBuilder Required()
    {
        _valueRequired = true;
        return this;
    }

    /// <summary>Fixe la longueur minimale attendue d'une valeur textuelle.</summary>
    public ColumnRuleBuilder MinLength(int minLength)
    {
        _minLength = Guard.Against.Negative(minLength, nameof(minLength));
        return this;
    }

    /// <summary>Fixe la longueur maximale attendue d'une valeur textuelle.</summary>
    public ColumnRuleBuilder MaxLength(int maxLength)
    {
        _maxLength = Guard.Against.NegativeOrZero(maxLength, nameof(maxLength));
        return this;
    }

    /// <summary>Fixe une expression régulière que la valeur doit respecter.</summary>
    public ColumnRuleBuilder Format(string regularExpressionPattern)
    {
        Guard.Against.NullOrWhiteSpace(regularExpressionPattern, nameof(regularExpressionPattern));

        try
        {
            _ = new Regex(regularExpressionPattern);
        }
        catch (RegexParseException ex)
        {
            throw new Exceptions.InvalidValidationProfileException(
                $"Colonne « {_canonicalKey} » : l'expression régulière « {regularExpressionPattern} » est invalide ({ex.Message}).");
        }

        _formatPattern = regularExpressionPattern;
        return this;
    }

    /// <summary>Déclare des valeurs explicitement interdites pour cette colonne.</summary>
    public ColumnRuleBuilder Forbidden(params string[] values)
    {
        Guard.Against.Null(values, nameof(values));
        _forbiddenValues = values;
        return this;
    }

    /// <summary>Fixe la plage numérique autorisée pour une colonne de nature numérique.</summary>
    public ColumnRuleBuilder NumericRange(double? min = null, double? max = null)
    {
        _minNumericValue = min;
        _maxNumericValue = max;
        return this;
    }

    /// <summary>Termine la configuration de cette colonne et débute celle de <paramref name="canonicalKey"/>.</summary>
    public ColumnRuleBuilder ForColumn(string canonicalKey)
    {
        Commit();
        return _parent.ForColumn(canonicalKey);
    }

    /// <summary>Termine la construction et retourne le profil assemblé par le constructeur englobant.</summary>
    public ValidationProfile Build()
    {
        Commit();
        return _parent.Build();
    }

    private void Commit()
    {
        if (_committed)
        {
            return;
        }

        if (_minLength.HasValue && _maxLength.HasValue && _minLength > _maxLength)
        {
            throw new Exceptions.InvalidValidationProfileException(
                $"Colonne « {_canonicalKey} » : la longueur minimale ({_minLength}) dépasse la longueur maximale ({_maxLength}).");
        }

        if (_minNumericValue.HasValue && _maxNumericValue.HasValue && _minNumericValue > _maxNumericValue)
        {
            throw new Exceptions.InvalidValidationProfileException(
                $"Colonne « {_canonicalKey} » : la valeur numérique minimale ({_minNumericValue}) dépasse la valeur maximale ({_maxNumericValue}).");
        }

        _committed = true;
        _parent.AddProfile(new ColumnValidationProfile(
            _canonicalKey,
            _valueRequired,
            _minLength,
            _maxLength,
            _formatPattern,
            _forbiddenValues,
            _minNumericValue,
            _maxNumericValue));
    }
}
