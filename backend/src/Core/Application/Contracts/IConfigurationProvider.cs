namespace OrbitaAI.Core.Application.Contracts;

/// <summary>
/// Contrat de lecture des trois niveaux de configuration définis dans
/// tech/CONFIGURATION_MANAGEMENT.md : système, Organisation, utilisateur — dans cet ordre de
/// précédence croissante (tech/CONFIGURATION_MANAGEMENT.md §5 : une valeur utilisateur prévaut
/// sur une valeur d'Organisation, qui prévaut sur une valeur système). Aucune implémentation
/// n'est fournie dans le Core : le stockage réel de la configuration (tech/DATABASE_STRATEGY.md)
/// est hors du périmètre de cette mission.
/// </summary>
public interface IConfigurationProvider
{
    /// <summary>Lit un paramètre de configuration système (tech/CONFIGURATION_MANAGEMENT.md §2).</summary>
    Task<string?> GetSystemSettingAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Lit un paramètre de configuration propre à une Organisation (tech/CONFIGURATION_MANAGEMENT.md §3).</summary>
    Task<string?> GetOrganizationSettingAsync(Guid organizationId, string key, CancellationToken cancellationToken = default);

    /// <summary>Lit un paramètre de configuration propre à un utilisateur (tech/CONFIGURATION_MANAGEMENT.md §4).</summary>
    Task<string?> GetUserSettingAsync(Guid userId, string key, CancellationToken cancellationToken = default);
}
