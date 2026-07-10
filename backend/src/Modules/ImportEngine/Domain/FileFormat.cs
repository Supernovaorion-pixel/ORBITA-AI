namespace OrbitaAI.Modules.ImportEngine.Domain;

/// <summary>
/// Format brut d'un fichier pris en charge par le moteur de lecture (features/IMPORT_ENGINE.md §2).
/// N'exprime aucune hypothèse sur le contenu métier du fichier : uniquement son encodage structurel.
/// </summary>
public enum FileFormat
{
    /// <summary>Fichier texte délimité (virgule, point-virgule, tabulation...).</summary>
    Csv,

    /// <summary>Classeur Excel au format Office Open XML (.xlsx).</summary>
    Excel
}
