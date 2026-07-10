using System.Runtime.CompilerServices;
using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;

/// <summary>
/// Implémentation par défaut de <see cref="IValidationPipeline"/> : restitue d'abord les
/// constats structurels traduits du rapport du Mapping Engine (colonnes obligatoires manquantes,
/// inconnues, dupliquées), puis parcourt en flux continu chaque ligne fournie pour y appliquer
/// les règles de valeur applicables à chaque colonne reconnue. Ne relit jamais le fichier source :
/// n'exploite que les <see cref="RawRow"/> fournis par l'appelant.
/// </summary>
public sealed class ValidationPipeline : IValidationPipeline
{
    private readonly IValidationRuleEngine _ruleEngine;
    private readonly IValidationRuleRegistry _registry;

    public ValidationPipeline(IValidationRuleEngine ruleEngine, IValidationRuleRegistry registry)
    {
        _ruleEngine = Guard.Against.Null(ruleEngine, nameof(ruleEngine));
        _registry = Guard.Against.Null(registry, nameof(registry));
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ValidationFinding> ValidateAsync(
        IAsyncEnumerable<RawRow> rows,
        ValidationContext context,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(rows, nameof(rows));
        Guard.Against.Null(context, nameof(context));

        context.Statistics.MarkStarted();

        foreach (var finding in BuildStructuralFindings(context))
        {
            yield return finding;
        }

        var effectiveProfile = context.EffectiveProfile;

        try
        {
            await foreach (var row in rows.WithCancellation(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                context.Statistics.IncrementRowsProcessed();

                foreach (var (canonicalKey, columnIndex) in context.Mapping.ColumnIndexByCanonicalKey)
                {
                    if (columnIndex >= row.Values.Count)
                    {
                        continue;
                    }

                    var canonicalColumn = FindCanonicalColumn(context.Dictionary, canonicalKey);
                    if (canonicalColumn is null)
                    {
                        continue;
                    }

                    var input = new ValidationRuleInput(
                        row,
                        columnIndex,
                        row.Values[columnIndex],
                        canonicalColumn,
                        effectiveProfile.GetProfile(canonicalKey),
                        context.Configuration);

                    foreach (var finding in _ruleEngine.EvaluateCell(input, _registry))
                    {
                        yield return finding;
                    }
                }
            }
        }
        finally
        {
            context.Statistics.MarkCompleted();
        }
    }

    private static IEnumerable<ValidationFinding> BuildStructuralFindings(ValidationContext context)
    {
        var configuration = context.Configuration;

        foreach (var missing in context.Mapping.Report.MissingRequiredColumns)
        {
            yield return StructuralFinding(
                configuration,
                ValidationCode.RequiredColumnMissing,
                ValidationSeverity.Critical,
                columnIndex: null,
                canonicalKey: missing.Key,
                rawHeader: null,
                summary: $"Colonne obligatoire « {missing.DisplayName} » absente du fichier.",
                explanation: "Aucune colonne source n'a pu être reconnue avec certitude comme correspondant à ce champ obligatoire.",
                resolution: "Ajouter ou renommer une colonne du fichier source pour qu'elle corresponde à ce champ, puis relancer l'import.");
        }

        foreach (var unknown in context.Mapping.Report.UnknownColumns)
        {
            yield return StructuralFinding(
                configuration,
                ValidationCode.UnknownColumn,
                ValidationSeverity.Information,
                unknown.ColumnIndex,
                canonicalKey: null,
                unknown.RawHeader,
                summary: $"Colonne « {unknown.RawHeader} » non reconnue.",
                explanation: "Cette colonne ne correspond à aucun champ canonique connu ; elle sera ignorée par les traitements ultérieurs.",
                resolution: "Si cette colonne porte une donnée utile, envisager de l'ajouter au dictionnaire de correspondance.");
        }

        foreach (var ambiguous in context.Mapping.Report.AmbiguousColumns)
        {
            yield return StructuralFinding(
                configuration,
                ValidationCode.DuplicateColumn,
                ValidationSeverity.Critical,
                ambiguous.ColumnIndex,
                canonicalKey: null,
                ambiguous.RawHeader,
                summary: $"Colonne « {ambiguous.RawHeader} » ambiguë ou en conflit avec une autre colonne.",
                explanation: "Le Mapping Engine n'a pas pu retenir automatiquement de correspondance certaine pour cette colonne.",
                resolution: "Examiner le rapport de correspondance et lever l'ambiguïté manuellement avant de poursuivre.");
        }
    }

    private static ValidationFinding StructuralFinding(
        ValidationConfiguration configuration,
        ValidationCode code,
        ValidationSeverity defaultSeverity,
        int? columnIndex,
        string? canonicalKey,
        string? rawHeader,
        string summary,
        string explanation,
        string resolution)
    {
        var severity = configuration.SeverityOverrides.GetValueOrDefault(code.Value, defaultSeverity);
        return new ValidationFinding(
            code,
            ValidationCategory.Structural,
            severity,
            RowNumber: 0,
            columnIndex,
            canonicalKey,
            rawHeader,
            new ValidationMessage(summary, explanation, resolution));
    }

    private static CanonicalColumnDefinition? FindCanonicalColumn(SynonymDictionary dictionary, string canonicalKey) =>
        dictionary.Columns.FirstOrDefault(c => string.Equals(c.Key, canonicalKey, StringComparison.Ordinal));
}
