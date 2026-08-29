using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class ConditionalSymbolScannerSelfTests
{
    private static readonly string _staticSymbol = ConditionalSymbolScanner.BannedSymbols[0];

    [Fact]
    public void Scan_OnBannedSymbol_ReportsLine_WhenInDirective()
    {
        var sut = ConditionalSymbolScanner.Scan("x.cs", $"using System;\n#if {_staticSymbol}\n#endif", ConditionalSymbolScanner.AllSymbols);

        sut.Should().ContainSingle().Which.Should().BeEquivalentTo(new { LineNumber = 2, Symbol = _staticSymbol });
    }

    [Fact]
    public void Scan_OnBannedSymbol_ReportsLine_WhenInComment() =>
        ConditionalSymbolScanner.Scan("x.cs", $"// mentions {_staticSymbol} in prose", ConditionalSymbolScanner.AllSymbols).Should().ContainSingle();

    [Fact]
    public void Scan_OnLongerIdentifier_ReportsNothing_WhenSymbolIsEmbedded() =>
        ConditionalSymbolScanner.Scan("x.cs", "#if __IOS__EXTRA || X__IOS__", ["__IOS__"]).Should().BeEmpty();

    [Fact]
    public void ApplyAllowlist_OnIosSymbol_KeepsOccurrence_WhenOutsideAllowlistedFile()
    {
        var allowed = ConditionalSymbolScanner.Allowlist["__IOS__"];
        var occurrences = new[]
        {
            new SymbolOccurrence(allowed, 1, "__IOS__", "#if __IOS__"),
            new SymbolOccurrence("src/Other.cs", 1, "__IOS__", "#if __IOS__"),
            new SymbolOccurrence(allowed, 2, _staticSymbol, "x")
        };

        var sut = ConditionalSymbolScanner.ApplyAllowlist(occurrences, ConditionalSymbolScanner.Allowlist);

        sut.Should().HaveCount(2).And.NotContain(o => o.RelativePath == allowed && o.Symbol == "__IOS__");
    }

    [Fact]
    public void FindClobbers_OnAssignment_ReportsLine_WhenDefineConstantsNotReferenced()
    {
        const string csproj = "<PropertyGroup>\n<DefineConstants>__IOS__</DefineConstants>\n<DefineConstants>$(DefineConstants);X</DefineConstants>\n</PropertyGroup>";

        ConditionalSymbolScanner.FindClobbers("p.csproj", csproj).Should().ContainSingle().Which.LineNumber.Should().Be(2);
    }

    [Fact]
    public void FindClobbers_OnMultiLineAssignment_ReportsOpeningLine_WhenDefineConstantsNotReferenced()
    {
        const string csproj = "<PropertyGroup>\n  <DefineConstants>\n    __IOS__;\n    X\n  </DefineConstants>\n</PropertyGroup>";

        ConditionalSymbolScanner.FindClobbers("p.csproj", csproj).Should().ContainSingle().Which.LineNumber.Should().Be(2);
    }

    [Fact]
    public void FindClobbers_OnMultiLineAppend_ReportsNothing_WhenDefineConstantsReferenced() =>
        ConditionalSymbolScanner.FindClobbers("p.csproj", "<DefineConstants>\n  $(DefineConstants);\n  X\n</DefineConstants>").Should().BeEmpty();

    [Fact]
    public void FindInternalDllImportDefinitions_OnSymbol_ReportsOnlyDefineConstantsLines_WhenMentionedElsewhere()
    {
        const string csproj = "<!-- MANAGEDBASS_INTERNAL_DLLIMPORT -->\n<DefineConstants>$(DefineConstants);MANAGEDBASS_INTERNAL_DLLIMPORT</DefineConstants>";

        ConditionalSymbolScanner.FindInternalDllImportDefinitions("p.csproj", csproj).Should().ContainSingle().Which.LineNumber.Should().Be(2);
    }

    [Fact]
    public void BannedSymbols_OnList_CoverXamarinFamily_WhenChecked() =>
        ConditionalSymbolScanner.BannedSymbols.Should().Contain(["__MACOS__", "__MACCATALYST__", "__TVOS__", "__ANDROID__", "__UNIFIED__", "__MOBILE__", "__WATCHOS__"])
            .And.Contain(s => s.StartsWith("__STATIC_") && s.EndsWith("LINKING__"));
}
