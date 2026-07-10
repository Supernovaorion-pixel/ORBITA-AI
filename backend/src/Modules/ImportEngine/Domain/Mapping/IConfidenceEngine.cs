namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Calcule le score de confiance final d'un candidat de correspondance, en combinant le score
/// fondé sur le nom (cf. <see cref="IHeaderAnalyzer"/>) avec, lorsqu'il est disponible, le
/// contenu observé de la colonne (cf. <see cref="ColumnStatistics"/>) — exclusivement pour
/// confirmer ou nuancer un candidat déjà identifié par son nom, jamais pour en découvrir un
/// nouveau. Chaque ajustement est expliqué (cf. <see cref="ColumnMappingCandidate.Reasons"/>).
/// Aucune IA : uniquement des règles déterministes et documentées.
/// </summary>
public interface IConfidenceEngine
{
    /// <summary>
    /// Retourne le candidat final, dont le score peut avoir été ajusté à la baisse par rapport à
    /// <paramref name="nameBasedCandidate"/> si le contenu de <paramref name="profile"/> contredit
    /// la nature attendue de la colonne canonique visée.
    /// </summary>
    ColumnMappingCandidate ApplyContentConfirmation(
        ColumnMappingCandidate nameBasedCandidate,
        ColumnProfile profile,
        CanonicalColumnDefinition canonicalColumn,
        MappingConfiguration configuration);
}
