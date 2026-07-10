namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>
/// Profil de validation par défaut, délibérément prudent : seules les colonnes obligatoires du
/// dictionnaire de correspondance par défaut (cf. <c>DefaultSynonymDictionary</c>) exigent une
/// valeur, avec une longueur maximale raisonnable pour les champs textuels. Toute contrainte plus
/// stricte (plage numérique, format, valeurs interdites) relève d'un choix explicite de
/// l'Organisation cliente via <see cref="ValidationRuleBuilder"/>, jamais d'une hypothèse
/// implicite du moteur sur une règle de gestion qui lui échappe.
/// </summary>
public static class DefaultValidationProfile
{
    /// <summary>Construit le profil de validation par défaut.</summary>
    public static ValidationProfile Create() => new ValidationRuleBuilder()
        .ForColumn("Client").Required().MaxLength(250)
        .ForColumn("Commercial").MaxLength(250)
        .ForColumn("MontantHT").Required()
        .ForColumn("Famille").MaxLength(250)
        .ForColumn("Produit").MaxLength(250)
        .ForColumn("Date").Required()
        .Build();
}
