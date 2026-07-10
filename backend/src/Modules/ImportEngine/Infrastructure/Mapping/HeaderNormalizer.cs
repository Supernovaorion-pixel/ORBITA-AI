using System.Globalization;
using System.Text;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;

namespace OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;

/// <summary>
/// Normalisation déterministe d'un libellé d'en-tête : mise en minuscule, suppression des
/// accents, remplacement de toute ponctuation ou séparateur par un espace, réduction des espaces
/// multiples. Sert uniquement à la comparaison structurelle des libellés — le libellé brut
/// d'origine n'est jamais altéré dans les résultats restitués par le moteur.
/// </summary>
public sealed class HeaderNormalizer : IHeaderNormalizer
{
    /// <inheritdoc />
    public string Normalize(string rawHeader)
    {
        ArgumentNullException.ThrowIfNull(rawHeader);

        var withoutDiacritics = RemoveDiacritics(rawHeader.Trim().ToLowerInvariant());

        var builder = new StringBuilder(withoutDiacritics.Length);
        var previousWasSpace = false;
        foreach (var c in withoutDiacritics)
        {
            if (char.IsLetterOrDigit(c))
            {
                builder.Append(c);
                previousWasSpace = false;
            }
            else if (!previousWasSpace)
            {
                builder.Append(' ');
                previousWasSpace = true;
            }
        }

        return builder.ToString().Trim();
    }

    private static string RemoveDiacritics(string value)
    {
        var decomposed = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);

        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
