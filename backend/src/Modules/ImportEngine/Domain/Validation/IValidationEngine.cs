namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Point d'entrée du moteur de validation (features/IMPORT_ENGINE.md §5). Intervient après le
/// Reader et le Mapping Engine, avant la Fusion, l'Analyse ou ORION. Ne modifie, ne corrige, ne
/// nettoie et ne fusionne jamais aucune donnée : il ne fait que la valider et produire un
/// rapport intégralement traçable.
/// </summary>
public interface IValidationEngine
{
    /// <summary>
    /// Valide l'intégralité de <paramref name="rows"/> et retourne le résultat complet de
    /// l'opération, une fois le flux entièrement parcouru.
    /// </summary>
    Task<ValidationResult> ValidateAsync(IAsyncEnumerable<RawRow> rows, ValidationContext context, CancellationToken cancellationToken = default);
}
