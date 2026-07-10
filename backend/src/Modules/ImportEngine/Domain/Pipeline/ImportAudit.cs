namespace OrbitaAI.Modules.ImportEngine.Domain.Pipeline;

/// <summary>
/// Journal d'audit d'une exécution du Pipeline d'Import : début, fin, annulation, erreur,
/// résultat, temps et volumes (mission 010.4). Chaque décision affectant le déroulement de l'import
/// y est consignée de façon append-only, jamais modifiée ni supprimée une fois écrite
/// (architecture/ERROR_HANDLING.md §6). Distinct de <see cref="ImportHistoryEntry"/>, qui restitue
/// une synthèse de cet import pour l'historique consultable entre plusieurs imports.
/// </summary>
public sealed class ImportAudit
{
    private readonly List<ImportAuditEntry> _entries = [];

    /// <summary>Entrées du journal d'audit, dans l'ordre chronologique où elles ont été consignées.</summary>
    public IReadOnlyList<ImportAuditEntry> Entries => _entries;

    /// <summary>Consigne le démarrage de l'import.</summary>
    public void RecordStarted(string sourceName) =>
        Record("Started", $"Import démarré pour la source « {sourceName} ».");

    /// <summary>Consigne l'achèvement complet de l'import.</summary>
    public void RecordCompleted(ImportSummary summary) =>
        Record(
            "Completed",
            $"Import terminé : {summary.ImportedRows:N0} ligne(s) importée(s), {summary.RejectedRows:N0} rejetée(s) sur {summary.TotalRows:N0} au total, en {summary.ExecutionTime.TotalSeconds:N2} s.");

    /// <summary>Consigne la suspension de l'import avant tout examen ligne par ligne.</summary>
    public void RecordHalted(string reason) => Record("Halted", reason);

    /// <summary>Consigne l'annulation explicite de l'import par l'appelant.</summary>
    public void RecordCancelled(long rowsRead) =>
        Record("Cancelled", $"Import annulé après {rowsRead:N0} ligne(s) lue(s).");

    /// <summary>Consigne l'interruption de l'import par une erreur imprévue.</summary>
    public void RecordFailed(Exception exception) =>
        Record("Failed", $"Import interrompu par une erreur : {exception.Message}");

    private void Record(string action, string description) =>
        _entries.Add(new ImportAuditEntry(DateTimeOffset.UtcNow, action, description));
}
