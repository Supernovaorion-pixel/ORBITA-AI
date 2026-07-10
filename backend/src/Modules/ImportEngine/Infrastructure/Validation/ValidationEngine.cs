using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Validation;

/// <summary>
/// Implémentation par défaut de <see cref="IValidationEngine"/> : consomme intégralement le
/// flux de constats produit par <see cref="IValidationPipeline"/>, les agrège en un
/// <see cref="ValidationReport"/> traçable (borné par <see cref="ValidationConfiguration.MaxFindingsToRetain"/>),
/// et détermine si le fichier peut être poursuivi dans le pipeline (Fusion, Analyse, ORION).
/// </summary>
public sealed class ValidationEngine : IValidationEngine
{
    private readonly IValidationPipeline _pipeline;

    public ValidationEngine(IValidationPipeline pipeline)
    {
        _pipeline = Guard.Against.Null(pipeline, nameof(pipeline));
    }

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateAsync(IAsyncEnumerable<RawRow> rows, ValidationContext context, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(rows, nameof(rows));
        Guard.Against.Null(context, nameof(context));

        var findings = new List<ValidationFinding>();
        var decisions = new List<ValidationDecision>();
        var affectedRows = new HashSet<int>();
        var totalFindings = 0;
        var informationCount = 0;
        var warningCount = 0;
        var errorCount = 0;
        var criticalCount = 0;
        var maxSeverityObserved = ValidationSeverity.Information;

        await foreach (var finding in _pipeline.ValidateAsync(rows, context, cancellationToken).WithCancellation(cancellationToken))
        {
            totalFindings++;
            affectedRows.Add(finding.RowNumber);

            switch (finding.Severity)
            {
                case ValidationSeverity.Information:
                    informationCount++;
                    break;
                case ValidationSeverity.Warning:
                    warningCount++;
                    break;
                case ValidationSeverity.Error:
                    errorCount++;
                    break;
                case ValidationSeverity.Critical:
                    criticalCount++;
                    break;
            }

            if (finding.Severity > maxSeverityObserved)
            {
                maxSeverityObserved = finding.Severity;
            }

            if (findings.Count < context.Configuration.MaxFindingsToRetain)
            {
                findings.Add(finding);
            }
        }

        var findingsTruncated = totalFindings > findings.Count;
        if (findingsTruncated)
        {
            decisions.Add(new ValidationDecision(
                $"Le nombre de constats ({totalFindings}) dépasse la limite de conservation détaillée " +
                $"({context.Configuration.MaxFindingsToRetain}) : seuls les {findings.Count} premiers sont détaillés individuellement, " +
                "le dénombrement complet reste exact dans la synthèse.",
                ValidationSeverity.Warning));
        }

        var canProceed = maxSeverityObserved < context.Configuration.BlockingSeverityThreshold;
        decisions.Add(new ValidationDecision(
            canProceed
                ? "Aucun constat n'atteint le seuil de blocage configuré : le traitement peut être poursuivi."
                : $"Au moins un constat atteint ou dépasse le seuil de blocage configuré ({context.Configuration.BlockingSeverityThreshold}) : le traitement ne doit pas être poursuivi sans intervention.",
            canProceed ? ValidationSeverity.Information : ValidationSeverity.Critical));

        var summary = new ValidationSummary(
            totalFindings,
            informationCount,
            warningCount,
            errorCount,
            criticalCount,
            affectedRows.Count,
            findingsTruncated);

        var report = new ValidationReport(findings, decisions, summary);
        return new ValidationResult(canProceed, report, context.Statistics);
    }
}
