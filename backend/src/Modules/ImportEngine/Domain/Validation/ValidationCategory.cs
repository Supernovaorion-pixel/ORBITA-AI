namespace OrbitaAI.Modules.ImportEngine.Domain.Validation;

/// <summary>Nature structurelle d'un <see cref="ValidationFinding"/>, indépendante de sa sévérité.</summary>
public enum ValidationCategory
{
    /// <summary>Concerne l'existence même d'une colonne (obligatoire manquante, inconnue, dupliquée) plutôt qu'une valeur.</summary>
    Structural,

    /// <summary>Valeur requise absente ou vide.</summary>
    Required,

    /// <summary>Valeur non interprétable dans le type attendu de la colonne (numérique, date).</summary>
    Type,

    /// <summary>Longueur de la valeur hors des bornes configurées.</summary>
    Length,

    /// <summary>Valeur ne respectant pas un format attendu (expression régulière configurée).</summary>
    Format,

    /// <summary>Valeur numérique hors de la plage configurée.</summary>
    Range,

    /// <summary>Valeur figurant explicitement parmi les valeurs interdites configurées.</summary>
    ForbiddenValue,

    /// <summary>Espacement superflu (début, fin, ou répétition interne).</summary>
    Whitespace,

    /// <summary>Caractère spécial, de contrôle, ou signe d'encodage défectueux.</summary>
    Encoding
}
