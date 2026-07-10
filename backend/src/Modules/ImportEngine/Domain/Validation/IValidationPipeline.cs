namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Parcourt en flux continu l'ensemble des lignes fournies par le Reader et produit, au fil de
/// l'eau, les constats de validation détectés — jamais en relisant le fichier, jamais en
/// matérialisant l'intégralité des lignes en mémoire (architecture/PERFORMANCE_GUIDELINES.md).
/// Complète les constats structurels (colonnes obligatoires manquantes, inconnues, dupliquées)
/// déjà établis par le Mapping Engine.
/// </summary>
public interface IValidationPipeline
{
    /// <summary>Valide progressivement <paramref name="rows"/> et restitue chaque constat dès sa détection.</summary>
    IAsyncEnumerable<ValidationFinding> ValidateAsync(IAsyncEnumerable<RawRow> rows, ValidationContext context, CancellationToken cancellationToken = default);
}
