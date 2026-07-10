namespace OrbitaAI.Core.Services;

/// <summary>
/// Abstraction du temps système (Clock / DateTime Provider). Toute lecture de l'heure courante
/// dans le Domaine ou l'Application transite par ce contrat plutôt que par un appel direct à
/// <see cref="DateTimeOffset.UtcNow"/>, condition nécessaire pour rendre le comportement
/// dépendant du temps entièrement testable (architecture/CODING_PRINCIPLES.md §7 — lisibilité
/// et testabilité).
/// </summary>
public interface IClock
{
    /// <summary>Instant courant, exprimé en temps universel coordonné (UTC).</summary>
    DateTimeOffset UtcNow { get; }
}
