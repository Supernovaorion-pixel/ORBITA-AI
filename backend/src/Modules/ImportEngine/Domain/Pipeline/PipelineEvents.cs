using OrbitaAI.Core.Domain.Events;

namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Événements émis par le Pipeline d'Import via le système d'événements interne
/// (architecture/EVENT_SYSTEM.md), permettant aux modules <c>History</c> et <c>Audit</c> de réagir
/// à un import sans que le Pipeline d'Import n'ait à les connaître directement — conformément à
/// l'absence de dépendance stable du Pipeline d'Import vers ces modules
/// (architecture/MODULE_DEPENDENCIES.md). Chaque événement décrit un fait accompli, jamais une
/// instruction (architecture/EVENT_SYSTEM.md §3).
/// </summary>
/// <param name="OrganizationId">Organisation cliente concernée par cet import.</param>
/// <param name="OccurredAt">Instant auquel l'import a démarré.</param>
/// <param name="ImportId">Identifiant unique de cet import.</param>
/// <param name="SourceName">Nom lisible de la source importée.</param>
public sealed record ImportPipelineStartedEvent(
    Guid OrganizationId,
    DateTimeOffset OccurredAt,
    Guid ImportId,
    string SourceName) : IDomainEvent;

/// <summary>Émis lorsqu'un import s'est achevé, avec ou sans anomalie (cf. <see cref="ImportHistoryEntry.Status"/>).</summary>
/// <param name="OrganizationId">Organisation cliente concernée par cet import.</param>
/// <param name="OccurredAt">Instant auquel l'import s'est achevé.</param>
/// <param name="ImportId">Identifiant unique de cet import.</param>
/// <param name="HistoryEntry">Synthèse complète de cet import, destinée à l'historique.</param>
public sealed record ImportPipelineCompletedEvent(
    Guid OrganizationId,
    DateTimeOffset OccurredAt,
    Guid ImportId,
    ImportHistoryEntry HistoryEntry) : IDomainEvent;

/// <summary>Émis lorsqu'un import a été explicitement annulé par l'appelant.</summary>
/// <param name="OrganizationId">Organisation cliente concernée par cet import.</param>
/// <param name="OccurredAt">Instant auquel l'annulation a été constatée.</param>
/// <param name="ImportId">Identifiant unique de cet import.</param>
/// <param name="RowsReadBeforeCancellation">Nombre de lignes lues avant l'annulation.</param>
public sealed record ImportPipelineCancelledEvent(
    Guid OrganizationId,
    DateTimeOffset OccurredAt,
    Guid ImportId,
    long RowsReadBeforeCancellation) : IDomainEvent;

/// <summary>Émis lorsqu'un import a été interrompu par une erreur imprévue.</summary>
/// <param name="OrganizationId">Organisation cliente concernée par cet import.</param>
/// <param name="OccurredAt">Instant auquel l'erreur a été constatée.</param>
/// <param name="ImportId">Identifiant unique de cet import.</param>
/// <param name="ErrorMessage">Message factuel décrivant l'erreur, sans détail technique interne.</param>
public sealed record ImportPipelineFailedEvent(
    Guid OrganizationId,
    DateTimeOffset OccurredAt,
    Guid ImportId,
    string ErrorMessage) : IDomainEvent;
