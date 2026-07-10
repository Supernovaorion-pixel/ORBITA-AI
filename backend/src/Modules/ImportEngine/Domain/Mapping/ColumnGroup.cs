namespace OrbitaAI.Modules.ImportEngine.Domain.Mapping;

/// <summary>
/// Regroupement thématique d'une <see cref="CanonicalColumnDefinition"/> (ex. « Identification »,
/// « Temporel », « Produit », « Financier »), destiné à l'organisation du rapport de
/// correspondance (features/IMPORT_ENGINE.md) et à une future restitution utilisateur. Une
/// donnée de configuration comme une autre : jamais une catégorie figée dans l'algorithme.
/// </summary>
/// <param name="Name">Nom du regroupement.</param>
public sealed record ColumnGroup(string Name)
{
    public static readonly ColumnGroup Identification = new("Identification");
    public static readonly ColumnGroup Temporal = new("Temporel");
    public static readonly ColumnGroup Product = new("Produit");
    public static readonly ColumnGroup Financial = new("Financier");
    public static readonly ColumnGroup Unclassified = new("Non classé");
}
