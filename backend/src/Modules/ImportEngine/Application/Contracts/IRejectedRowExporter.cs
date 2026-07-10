using OrbitaAI.Modules.ImportEngine.Domain.Quarantine;

namespace OrbitaAI.Modules.ImportEngine.Application.Contracts;

/// <summary>
/// Point d'extension permettant à une mission ultérieure (Export Engine, cf. EXPORT_ENGINE.md) de
/// restituer l'ensemble des lignes mises en quarantaine dans l'un des formats pris en charge, sans
/// jamais modifier le Quarantine Engine lui-même (architecture/EXTENSIBILITY.md §11).
///
/// Squelette structurel uniquement : aucune implémentation d'export (Excel, CSV, PDF, HTML) n'est
/// fournie par cette mission.
/// </summary>
public interface IRejectedRowExporter
{
    /// <summary>Indique si cet exportateur prend en charge <paramref name="format"/>.</summary>
    bool CanExport(RejectedRowExportFormat format);

    /// <summary>
    /// Exporte <paramref name="rejectedRows"/> dans <paramref name="format"/>, en écrivant le
    /// résultat dans <paramref name="destination"/>. Ne modifie jamais les lignes rejetées fournies.
    /// </summary>
    Task ExportAsync(
        RejectedRowCollection rejectedRows,
        RejectedRowExportFormat format,
        Stream destination,
        CancellationToken cancellationToken = default);
}
