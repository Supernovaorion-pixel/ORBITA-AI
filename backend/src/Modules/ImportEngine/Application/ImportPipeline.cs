using OrbitaAI.Core.Common.Guards;
using OrbitaAI.Core.Services;
using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Pipeline;
using OrbitaAI.Modules.ImportEngine.Domain.Pipeline.Exceptions;
using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;
using OrbitaAI.Modules.ImportEngine.Domain.Validation;

namespace OrbitaAI.Modules.ImportEngine.Application;

/// <summary>
/// Implémentation par défaut de <see cref="IImportPipeline"/> : relie le Reader, le Mapping
/// Engine, le Validation Engine et le Quarantine Engine en un seul passage continu sur le flux de
/// lignes (architecture/PERFORMANCE_GUIDELINES.md), sans jamais matérialiser l'ensemble du fichier
/// en mémoire. Coordonne ces moteurs déjà validés sans en réimplémenter la logique (mission 010.4) :
/// orchestration au sens d'[architecture/APPLICATION_LAYERS.md] §2, à l'image de
/// <see cref="ReaderFactory"/> pour le Reader.
///
/// Seuls un échantillon borné de lignes de tête (<see cref="PipelineConfiguration.MappingSampleSize"/>,
/// nécessaire au Mapping Engine) est retenu en mémoire ; la validation de chaque ligne est évaluée
/// au fil de l'eau via <see cref="IValidationRuleEngine"/>/<see cref="IValidationRuleRegistry"/> —
/// les mêmes briques que celles composées par <see cref="IValidationPipeline"/> — afin de pouvoir
/// immédiatement confier la ligne et ses constats au <see cref="IQuarantineEngine"/>, chose que la
/// forme agrégée de <see cref="IValidationPipeline"/>/<see cref="IValidationEngine"/> ne permet pas
/// puisqu'elle ne restitue plus la ligne d'origine. Les constats structurels (colonnes manquantes,
/// inconnues, ambiguës), indépendants de toute ligne, sont eux obtenus par réutilisation directe et
/// sans duplication de <see cref="IValidationPipeline.ValidateAsync"/> sur un flux vide.
/// </summary>
public sealed class ImportPipeline : IImportPipeline
{
    private readonly IReaderFactory _readerFactory;
    private readonly IMappingEngine _mappingEngine;
    private readonly IValidationRuleEngine _validationRuleEngine;
    private readonly IValidationRuleRegistry _validationRuleRegistry;
    private readonly IValidationPipeline _validationPipeline;
    private readonly IQuarantineEngine _quarantineEngine;
    private readonly IEventBus _eventBus;

    public ImportPipeline(
        IReaderFactory readerFactory,
        IMappingEngine mappingEngine,
        IValidationRuleEngine validationRuleEngine,
        IValidationRuleRegistry validationRuleRegistry,
        IValidationPipeline validationPipeline,
        IQuarantineEngine quarantineEngine,
        IEventBus eventBus)
    {
        _readerFactory = Guard.Against.Null(readerFactory, nameof(readerFactory));
        _mappingEngine = Guard.Against.Null(mappingEngine, nameof(mappingEngine));
        _validationRuleEngine = Guard.Against.Null(validationRuleEngine, nameof(validationRuleEngine));
        _validationRuleRegistry = Guard.Against.Null(validationRuleRegistry, nameof(validationRuleRegistry));
        _validationPipeline = Guard.Against.Null(validationPipeline, nameof(validationPipeline));
        _quarantineEngine = Guard.Against.Null(quarantineEngine, nameof(quarantineEngine));
        _eventBus = Guard.Against.Null(eventBus, nameof(eventBus));
    }

    /// <inheritdoc />
    public async Task<PipelineResult> RunAsync(
        PipelineContext context,
        IProgress<PipelineProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(context, nameof(context));

        if (context.Configuration.MappingSampleSize <= 0 || context.Configuration.ProgressReportIntervalRows <= 0)
        {
            throw new InvalidPipelineConfigurationException(
                "La taille d'échantillon de correspondance et l'intervalle de notification de progression doivent être strictement positifs.");
        }

        context.Statistics.MarkStarted();
        context.Audit.RecordStarted(context.SourceName);
        await _eventBus.PublishAsync(
            new ImportPipelineStartedEvent(context.OrganizationId, DateTimeOffset.UtcNow, context.ImportId, context.SourceName),
            cancellationToken);

        var decisions = new List<PipelineDecision>();
        var findingsBySeverity = new Dictionary<ValidationSeverity, long>
        {
            [ValidationSeverity.Information] = 0,
            [ValidationSeverity.Warning] = 0,
            [ValidationSeverity.Error] = 0,
            [ValidationSeverity.Critical] = 0,
        };
        var rejectedRows = new RejectedRowCollection();

        try
        {
            var reader = _readerFactory.ResolveReader(context.Source);
            var readerContext = new ReaderContext { Source = context.Source, Options = context.Options.Reader };
            var rowSource = reader.ReadAsync(readerContext, cancellationToken);

            await using var enumerator = rowSource.GetAsyncEnumerator(cancellationToken);

            var sample = new List<RawRow>(context.Configuration.MappingSampleSize);
            while (sample.Count < context.Configuration.MappingSampleSize && await enumerator.MoveNextAsync())
            {
                sample.Add(enumerator.Current);
            }

            var headers = sample.Count > 0 ? sample[0].Headers : Array.Empty<string>();

            context.Statistics.TransitionTo(PipelineState.Mapping);
            var mappingContext = new MappingContext
            {
                Options = context.Options.Mapping,
                Configuration = context.Options.MappingConfiguration,
            };
            var mapping = _mappingEngine.Analyze(headers, sample, mappingContext);

            var validationContext = new ValidationContext
            {
                Mapping = mapping,
                Dictionary = mappingContext.EffectiveDictionary,
                Options = context.Options.Validation,
                Configuration = context.Options.ValidationConfiguration,
            };

            var structuralFindings = new List<ValidationFinding>();
            await foreach (var finding in _validationPipeline.ValidateAsync(EmptyRowsAsync(), validationContext, cancellationToken))
            {
                structuralFindings.Add(finding);
                findingsBySeverity[finding.Severity]++;
                decisions.Add(new PipelineDecision(finding.Message.Summary, finding.Severity));
            }

            var maxStructuralSeverity = structuralFindings.Count > 0
                ? structuralFindings.Max(f => f.Severity)
                : ValidationSeverity.Information;

            if (maxStructuralSeverity >= validationContext.Configuration.BlockingSeverityThreshold)
            {
                context.Statistics.IncrementRowsRead(sample.Count);
                context.Statistics.MarkCompleted(PipelineState.Halted);

                var haltReason = $"Import suspendu avant tout examen ligne par ligne : au moins une anomalie structurelle " +
                                  $"atteint le seuil de blocage configuré ({validationContext.Configuration.BlockingSeverityThreshold}).";
                context.Audit.RecordHalted(haltReason);
                decisions.Add(new PipelineDecision(haltReason, maxStructuralSeverity));

                var haltedSummary = BuildSummary(context.Statistics, rejectedRows);
                var haltedMetrics = BuildMetrics(structuralFindings.Count, findingsBySeverity, mapping);
                var haltedHistory = new ImportHistoryEntry(context.ImportId, DateTimeOffset.UtcNow, context.SourceName, PipelineState.Halted, haltedSummary);

                await _eventBus.PublishAsync(
                    new ImportPipelineCompletedEvent(context.OrganizationId, DateTimeOffset.UtcNow, context.ImportId, haltedHistory),
                    cancellationToken);

                return new PipelineResult(PipelineState.Halted, mapping, haltedSummary, haltedMetrics, rejectedRows, decisions, context.Audit, haltedHistory, context.Statistics);
            }

            context.Statistics.TransitionTo(PipelineState.Processing);
            var effectiveProfile = validationContext.EffectiveProfile;

            foreach (var row in sample)
            {
                ProcessRow(row);
            }

            while (await enumerator.MoveNextAsync())
            {
                ProcessRow(enumerator.Current);
            }

            context.Statistics.MarkCompleted(PipelineState.Completed);

            var summary = BuildSummary(context.Statistics, rejectedRows);
            var metrics = BuildMetrics(structuralFindings.Count, findingsBySeverity, mapping);
            context.Audit.RecordCompleted(summary);
            decisions.Add(new PipelineDecision(
                $"Import terminé : {summary.ImportedRows:N0} ligne(s) importée(s), {summary.RejectedRows:N0} rejetée(s) sur {summary.TotalRows:N0} au total.",
                ValidationSeverity.Information));

            var history = new ImportHistoryEntry(context.ImportId, DateTimeOffset.UtcNow, context.SourceName, PipelineState.Completed, summary);
            await _eventBus.PublishAsync(
                new ImportPipelineCompletedEvent(context.OrganizationId, DateTimeOffset.UtcNow, context.ImportId, history),
                cancellationToken);

            return new PipelineResult(PipelineState.Completed, mapping, summary, metrics, rejectedRows, decisions, context.Audit, history, context.Statistics);

            void ProcessRow(RawRow row)
            {
                cancellationToken.ThrowIfCancellationRequested();

                List<ValidationFinding>? rowFindings = null;
                foreach (var (canonicalKey, columnIndex) in mapping.ColumnIndexByCanonicalKey)
                {
                    if (columnIndex >= row.Values.Count)
                    {
                        continue;
                    }

                    var canonicalColumn = FindCanonicalColumn(validationContext.Dictionary, canonicalKey);
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
                        validationContext.Configuration);

                    var cellFindings = _validationRuleEngine.EvaluateCell(input, _validationRuleRegistry);
                    if (cellFindings.Count == 0)
                    {
                        continue;
                    }

                    rowFindings ??= [];
                    rowFindings.AddRange(cellFindings);
                }

                context.Statistics.IncrementRowsRead();

                if (rowFindings is { Count: > 0 })
                {
                    foreach (var finding in rowFindings)
                    {
                        findingsBySeverity[finding.Severity]++;
                    }
                }

                var rejected = _quarantineEngine.Classify(
                    row,
                    (IReadOnlyList<ValidationFinding>?)rowFindings ?? Array.Empty<ValidationFinding>(),
                    context.Configuration);

                if (rejected is not null)
                {
                    rejectedRows.Add(rejected);
                    context.Statistics.IncrementRowsRejected();
                }
                else
                {
                    context.Statistics.IncrementRowsAccepted();
                    if (rowFindings is { Count: > 0 })
                    {
                        context.Statistics.IncrementRowsWithWarnings();
                    }
                }

                if (context.Statistics.RowsRead % context.Configuration.ProgressReportIntervalRows == 0)
                {
                    progress?.Report(new PipelineProgress(
                        context.Statistics.State,
                        context.Statistics.RowsRead,
                        context.Statistics.RowsAccepted,
                        context.Statistics.RowsRejected,
                        context.Statistics.Elapsed));
                }
            }
        }
        catch (OperationCanceledException)
        {
            context.Statistics.MarkCompleted(PipelineState.Cancelled);
            context.Audit.RecordCancelled(context.Statistics.RowsRead);
            await _eventBus.PublishAsync(
                new ImportPipelineCancelledEvent(context.OrganizationId, DateTimeOffset.UtcNow, context.ImportId, context.Statistics.RowsRead),
                CancellationToken.None);
            throw;
        }
        catch (Exception ex)
        {
            context.Statistics.MarkCompleted(PipelineState.Failed);
            context.Audit.RecordFailed(ex);
            await _eventBus.PublishAsync(
                new ImportPipelineFailedEvent(context.OrganizationId, DateTimeOffset.UtcNow, context.ImportId, ex.Message),
                CancellationToken.None);

            decisions.Add(new PipelineDecision($"Échec de l'import : {ex.Message}", ValidationSeverity.Critical));

            var failedSummary = BuildSummary(context.Statistics, rejectedRows);
            var failedMetrics = BuildMetrics(0, findingsBySeverity, mapping: null);
            var failedHistory = new ImportHistoryEntry(context.ImportId, DateTimeOffset.UtcNow, context.SourceName, PipelineState.Failed, failedSummary);

            return new PipelineResult(PipelineState.Failed, null, failedSummary, failedMetrics, rejectedRows, decisions, context.Audit, failedHistory, context.Statistics);
        }
    }

    private static ImportSummary BuildSummary(PipelineStatistics statistics, RejectedRowCollection rejectedRows)
    {
        var totalRows = statistics.RowsRead;
        var qualityScore = totalRows > 0
            ? (double)(statistics.RowsAccepted - statistics.RowsWithWarnings) / totalRows * 100
            : 100d;

        return new ImportSummary(
            totalRows,
            statistics.RowsAccepted,
            statistics.RowsRejected,
            statistics.RowsWithWarnings,
            statistics.Elapsed,
            statistics.RowsPerSecond,
            qualityScore,
            rejectedRows.CountsByCategory);
    }

    private static ImportMetrics BuildMetrics(long structuralFindingsCount, IReadOnlyDictionary<ValidationSeverity, long> findingsBySeverity, MappingResult? mapping) =>
        new(
            findingsBySeverity.Values.Sum(),
            structuralFindingsCount,
            findingsBySeverity,
            mapping?.Report.RecognizedColumns.Count ?? 0,
            mapping?.Report.UnknownColumns.Count ?? 0,
            mapping?.Report.AmbiguousColumns.Count ?? 0,
            mapping?.Report.GlobalRecognitionScore ?? 0);

    private static async IAsyncEnumerable<RawRow> EmptyRowsAsync()
    {
        await Task.CompletedTask;
        yield break;
    }

    private static CanonicalColumnDefinition? FindCanonicalColumn(SynonymDictionary dictionary, string canonicalKey) =>
        dictionary.Columns.FirstOrDefault(c => string.Equals(c.Key, canonicalKey, StringComparison.Ordinal));
}
