namespace OrbitaAI.Modules.ImportEngine.Application.Contracts;

/// <summary>
/// Formats d'export pris en charge pour les lignes mises en quarantaine (cf. <see cref="IRejectedRowExporter"/>).
/// Squelette structurel uniquement — aucune implémentation d'export n'est fournie par cette mission
/// (cf. EXPORT_ENGINE.md, développé lors d'une mission ultérieure).
/// </summary>
public enum RejectedRowExportFormat
{
    /// <summary>Fichier texte à valeurs séparées.</summary>
    Csv,

    /// <summary>Classeur Excel.</summary>
    Excel,

    /// <summary>Rapport PDF.</summary>
    Pdf,

    /// <summary>Rapport HTML.</summary>
    Html,
}
