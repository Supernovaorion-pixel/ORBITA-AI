using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Pipeline;
using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Quarantine;

/// <summary>
/// Implémentation par défaut de <see cref="IQuarantineEngine"/> : isole une ligne dès qu'au moins
/// un constat de validation qui lui est propre atteint le seuil configuré
/// (<see cref="PipelineConfiguration.QuarantineSeverityThreshold"/>), en conservant intégralement
/// sa donnée originale. Ne modifie, ne corrige et ne fusionne jamais aucune donnée (mission 010.4).
/// </summary>
public sealed class QuarantineEngine : IQuarantineEngine
{
    /// <inheritdoc />
    public RejectedRow? Classify(RawRow row, IReadOnlyList<ValidationFinding> findings, PipelineConfiguration configuration)
    {
        Guard.Against.Null(row, nameof(row));
        Guard.Against.Null(findings, nameof(findings));
        Guard.Against.Null(configuration, nameof(configuration));

        var offendingFindings = findings.Where(f => f.Severity >= configuration.QuarantineSeverityThreshold).ToArray();
        if (offendingFindings.Length == 0)
        {
            return null;
        }

        var reasons = offendingFindings
            .Select(f => new RejectedRowReason(f.Code, f.Category, f.Severity, f.ColumnIndex, f.ColumnCanonicalKey, f.RawHeader, f.Message))
            .ToArray();

        var highestSeverity = reasons.Max(r => r.Severity);
        var dominantCategory = reasons.First(r => r.Severity == highestSeverity).Category;

        return new RejectedRow(
            row.RowNumber,
            row.Headers,
            row.Values,
            reasons,
            DetermineCategory(dominantCategory),
            highestSeverity);
    }

    private static RejectedRowCategory DetermineCategory(ValidationCategory category) => category switch
    {
        ValidationCategory.Structural => RejectedRowCategory.StructuralIssue,
        ValidationCategory.Required => RejectedRowCategory.MissingRequiredData,
        ValidationCategory.Type => RejectedRowCategory.InvalidType,
        ValidationCategory.Format => RejectedRowCategory.InvalidFormat,
        ValidationCategory.ForbiddenValue => RejectedRowCategory.PolicyViolation,
        ValidationCategory.Range => RejectedRowCategory.PolicyViolation,
        _ => RejectedRowCategory.DataQuality,
    };
}
