using OrbitaAI.Modules.ImportEngine.Domain;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping;
using OrbitaAI.Modules.ImportEngine.Domain.Mapping.Exceptions;
using OrbitaAI.Modules.ImportEngine.Infrastructure.Mapping;
using Xunit;

namespace OrbitaAI.UnitTests.ImportEngine.Mapping;

public sealed class MappingEngineTests
{
    private readonly MappingEngine _engine = new(new ColumnAnalyzer(), new HeaderAnalyzer(new HeaderNormalizer()), new ConfidenceEngine());

    private static RawRow Row(IReadOnlyList<string> headers, int number, params object?[] values) => new(number, headers, values);

    private static IReadOnlyList<RawRow> SampleRows(IReadOnlyList<string> headers, params object?[][] rows) =>
        rows.Select((values, i) => Row(headers, i + 1, values)).ToArray();

    [Fact]
    public void Analyze_ExactCanonicalHeaders_RecognizesAllColumns()
    {
        IReadOnlyList<string> headers = ["Client", "Montant HT", "Date"];
        var rows = SampleRows(headers, ["Acme", 1000d, "2026-01-01"], ["Contoso", 2000d, "2026-01-02"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.Equal(3, result.Report.RecognizedColumns.Count);
        Assert.Equal(0, result.ColumnIndexByCanonicalKey["Client"]);
        Assert.Equal(1, result.ColumnIndexByCanonicalKey["MontantHT"]);
        Assert.Equal(2, result.ColumnIndexByCanonicalKey["Date"]);
        Assert.All(result.Report.RecognizedColumns, o => Assert.Equal(100, o.SelectedCandidate!.ConfidenceScore));
    }

    [Theory]
    [InlineData("Nom client")]
    [InlineData("Raison sociale")]
    [InlineData("Code client")]
    public void Analyze_FrenchSynonyms_RecognizesClient(string header)
    {
        IReadOnlyList<string> headers = [header];
        var rows = SampleRows(headers, ["Acme"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.True(result.ColumnIndexByCanonicalKey.ContainsKey("Client"));
    }

    [Theory]
    [InlineData("Customer")]
    [InlineData("Company")]
    [InlineData("Client Name")]
    public void Analyze_EnglishSynonyms_RecognizesClient(string header)
    {
        IReadOnlyList<string> headers = [header];
        var rows = SampleRows(headers, ["Acme"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.True(result.ColumnIndexByCanonicalKey.ContainsKey("Client"));
    }

    [Fact]
    public void Analyze_MixedEnglishAndFrenchHeaders_RecognizesAll()
    {
        IReadOnlyList<string> headers = ["Customer", "Net Sales", "Vendeur", "Product Family"];
        var rows = SampleRows(headers, ["Acme", 100d, "Jean Dupont", "Meubles"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.Equal(4, result.Report.RecognizedColumns.Count);
        Assert.Equal(["Client", "MontantHT", "Commercial", "Famille"], result.ColumnIndexByCanonicalKey.Keys.OrderBy(k => result.ColumnIndexByCanonicalKey[k]));
    }

    [Fact]
    public void Analyze_ReorderedColumns_StillRecognizedByName_NotPosition()
    {
        IReadOnlyList<string> headersOriginal = ["Client", "Montant HT", "Date"];
        IReadOnlyList<string> headersReordered = ["Date", "Client", "Montant HT"];

        var resultOriginal = _engine.Analyze(headersOriginal, SampleRows(headersOriginal, ["Acme", 100d, "2026-01-01"]), new MappingContext());
        var resultReordered = _engine.Analyze(headersReordered, SampleRows(headersReordered, ["2026-01-01", "Acme", 100d]), new MappingContext());

        Assert.Equal(0, resultOriginal.ColumnIndexByCanonicalKey["Client"]);
        Assert.Equal(1, resultReordered.ColumnIndexByCanonicalKey["Client"]);
        Assert.Equal(0, resultReordered.ColumnIndexByCanonicalKey["Date"]);
    }

    [Fact]
    public void Analyze_UnknownColumn_IsReportedAsUnknown_NotRecognized()
    {
        IReadOnlyList<string> headers = ["Client", "Numero de telephone"];
        var rows = SampleRows(headers, ["Acme", "0102030405"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.Single(result.Report.UnknownColumns);
        Assert.Equal("Numero de telephone", result.Report.UnknownColumns[0].RawHeader);
        Assert.False(result.ColumnIndexByCanonicalKey.ContainsKey("Numero de telephone"));
    }

    [Fact]
    public void Analyze_MissingRequiredColumn_IsReportedExplicitly()
    {
        // "Client", "Date" et "MontantHT" sont obligatoires par défaut ; seul "Client" est fourni ici.
        IReadOnlyList<string> headers = ["Client"];
        var rows = SampleRows(headers, ["Acme"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.Contains(result.Report.MissingRequiredColumns, c => c.Key == "Date");
        Assert.Contains(result.Report.MissingRequiredColumns, c => c.Key == "MontantHT");
        Assert.Contains(result.Report.Decisions, d => d.Contains("Date") && d.Contains("non trouvée"));
    }

    [Fact]
    public void Analyze_DuplicateColumnsClaimingSameCanonicalKey_AreBothMarkedAmbiguous()
    {
        IReadOnlyList<string> headers = ["Client", "Customer"]; // deux colonnes revendiquant "Client".
        var rows = SampleRows(headers, ["Acme", "Acme Corp"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.False(result.ColumnIndexByCanonicalKey.ContainsKey("Client"));
        Assert.Equal(2, result.Report.AmbiguousColumns.Count);
        Assert.All(result.Report.AmbiguousColumns, o => Assert.Null(o.SelectedCandidate));
        Assert.Contains(result.Report.Decisions, d => d.Contains("dupliquées"));
    }

    [Fact]
    public void Analyze_AmbiguousScoreBand_IsReportedAsAmbiguous_WithoutAutomaticCorrection()
    {
        // "Famille Produit Principale" ne recoupe que partiellement "Famille Produit" (80%),
        // ce qui reste en deçà du seuil de reconnaissance par défaut (80% est le seuil, donc ici on
        // force un score ambigu via une configuration de seuil de reconnaissance plus stricte).
        IReadOnlyList<string> headers = ["Famille Produit Principale"];
        var rows = SampleRows(headers, ["Meubles"]);
        var context = new MappingContext { Configuration = MappingConfiguration.Default with { RecognizedThreshold = 90 } };

        var result = _engine.Analyze(headers, rows, context);

        Assert.Single(result.Report.AmbiguousColumns);
        Assert.Null(result.Report.AmbiguousColumns[0].SelectedCandidate);
        Assert.False(result.ColumnIndexByCanonicalKey.ContainsKey("Famille"));
    }

    [Fact]
    public void Analyze_GlobalRecognitionScore_ReflectsRequiredAndOptionalColumnsFound()
    {
        // Dictionnaire par défaut : 3 obligatoires (Client, MontantHT, Date), 8 optionnelles.
        IReadOnlyList<string> headers = ["Client", "Montant HT", "Date"]; // les 3 obligatoires, aucune optionnelle.
        var rows = SampleRows(headers, ["Acme", 100d, "2026-01-01"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        // (3*2 + 0) / (3*2 + 8) * 100 = 6/14*100 ≈ 42.86
        Assert.Equal(6d / 14 * 100, result.Report.GlobalRecognitionScore, precision: 2);
    }

    [Fact]
    public void Analyze_AllColumnsRecognized_GlobalScoreIsOneHundred()
    {
        var dictionary = new SynonymDictionary([CanonicalColumnDefinition.Create("Client", "Client", isRequired: true)]);
        IReadOnlyList<string> headers = ["Client"];
        var rows = SampleRows(headers, ["Acme"]);
        var context = new MappingContext { Options = new MappingOptions { SynonymDictionary = dictionary } };

        var result = _engine.Analyze(headers, rows, context);

        Assert.Equal(100, result.Report.GlobalRecognitionScore);
    }

    [Fact]
    public void Analyze_ReportDecisions_AreNeverEmpty_ForAnyColumn()
    {
        IReadOnlyList<string> headers = ["Client", "Colonne Inconnue"];
        var rows = SampleRows(headers, ["Acme", "xyz"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.True(result.Report.Decisions.Count >= 2);
    }

    [Fact]
    public void Analyze_NoHeaders_ThrowsNoHeaderRowException()
    {
        Assert.Throws<NoHeaderRowException>(() => _engine.Analyze(Array.Empty<string>(), Array.Empty<RawRow>(), new MappingContext()));
    }

    [Fact]
    public void Analyze_CustomSynonymDictionary_OverridesDefaultCompletely()
    {
        var customDictionary = new SynonymDictionary(
        [
            CanonicalColumnDefinition.Create("Magasin", "Magasin", synonyms: ["Store", "Boutique"], isRequired: true),
        ]);
        IReadOnlyList<string> headers = ["Boutique"];
        var rows = SampleRows(headers, ["Paris Centre"]);
        var context = new MappingContext { Options = new MappingOptions { SynonymDictionary = customDictionary } };

        var result = _engine.Analyze(headers, rows, context);

        Assert.True(result.ColumnIndexByCanonicalKey.ContainsKey("Magasin"));
        // "Client" ne fait plus partie du dictionnaire personnalisé : jamais reconnu ici.
        Assert.False(result.ColumnIndexByCanonicalKey.ContainsKey("Client"));
    }

    [Fact]
    public void Analyze_ContentMismatch_DowngradesRecognizedColumnToAmbiguous()
    {
        // L'en-tête "CA" correspond exactement à MontantHT (100%), mais le contenu est
        // entièrement textuel : la confiance est réduite et la colonne devient ambiguë plutôt que reconnue.
        IReadOnlyList<string> headers = ["CA"];
        var rows = SampleRows(headers, ["indisponible"], ["indisponible"], ["indisponible"], ["indisponible"], ["indisponible"]);

        var result = _engine.Analyze(headers, rows, new MappingContext());

        Assert.False(result.ColumnIndexByCanonicalKey.ContainsKey("MontantHT"));
        Assert.Single(result.Report.AmbiguousColumns);
    }

    [Fact]
    public void Analyze_WithContentAnalysisDisabled_DoesNotPenalizeContentMismatch()
    {
        IReadOnlyList<string> headers = ["CA"];
        var rows = SampleRows(headers, ["indisponible"], ["indisponible"], ["indisponible"], ["indisponible"], ["indisponible"]);
        var context = new MappingContext { Options = new MappingOptions { AnalyzeContent = false } };

        var result = _engine.Analyze(headers, rows, context);

        Assert.True(result.ColumnIndexByCanonicalKey.ContainsKey("MontantHT"));
    }
}
