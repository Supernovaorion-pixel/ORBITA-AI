using OrbitaAI.Core.Domain;

namespace OrbitaAI.Core.Domain.Entities;

/// <summary>
/// Traduction technique du terme métier « Prévision » (docs/GLOSSARY.md).
/// Estimation de performance future dérivée de l'historique et référençant un Objective
/// (architecture/DOMAIN_MODEL.md §2, features/FORECAST_ENGINE.md). Squelette structurel
/// uniquement — aucun calcul, aucun algorithme de projection.
/// </summary>
public sealed class Forecast : AggregateRoot<Guid>, IOrganizationScoped
{
    public Guid OrganizationId { get; init; }

    public Guid ObjectiveId { get; init; }
}
