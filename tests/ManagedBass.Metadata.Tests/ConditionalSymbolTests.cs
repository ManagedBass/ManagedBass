using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 3: no dead Xamarin-era symbols under <c>src/</c>, and no DefineConstants misuse.</summary>
public class ConditionalSymbolTests
{
    private static readonly string[] _scannedExtensions = [".cs", ".csproj", ".props", ".targets", ".sln", ".slnf"];
    private static readonly string[] _buildFileExtensions = [".csproj", ".props", ".targets"];

    [Fact]
    public void SourceTree_OnScan_HasNoBannedSymbols_WhenAllowlistApplied()
    {
        var files = RepoLayout.SourceFiles(_scannedExtensions);
        files.Should().NotBeEmpty();

        var occurrences = files.SelectMany(f => ConditionalSymbolScanner.Scan(f, RepoLayout.ReadText(f), ConditionalSymbolScanner.AllSymbols));
        var violations = ConditionalSymbolScanner.ApplyAllowlist(occurrences, ConditionalSymbolScanner.Allowlist);

        violations.Should().BeEmpty("comments count too - put history in CONTRIBUTING.md, not under src/");
    }

    [Fact]
    public void AllowlistedFile_OnScan_StillContainsIosSymbol_WhenExemptionIsLive()
    {
        var path = ConditionalSymbolScanner.Allowlist["__IOS__"];

        File.Exists(RepoLayout.Absolute(path)).Should().BeTrue($"{path} was deleted, so the __IOS__ exemption is stale - remove it from the allowlist");

        ConditionalSymbolScanner.Scan(path, RepoLayout.ReadText(path), ["__IOS__"])
            .Should().NotBeEmpty($"{path} no longer uses __IOS__, so the exemption is stale - remove it from the allowlist");
    }

    [Fact]
    public void BuildFiles_OnScan_NeverClobberDefineConstants_WhenAssigned()
    {
        var clobbers = RepoLayout.SourceFiles(_buildFileExtensions)
            .SelectMany(f => ConditionalSymbolScanner.FindClobbers(f, RepoLayout.ReadText(f)))
            .ToArray();

        clobbers.Should().BeEmpty("DefineConstants must append to $(DefineConstants)");
    }

    [Fact]
    public void BuildFiles_OnScan_DefineInternalDllImportSymbolOnlyInProps_WhenDefined()
    {
        var strays = RepoLayout.SourceFiles(_buildFileExtensions)
            .Where(f => f != RepoLayout.PropsRelativePath)
            .SelectMany(f => ConditionalSymbolScanner.FindInternalDllImportDefinitions(f, RepoLayout.ReadText(f)))
            .ToArray();

        strays.Should().BeEmpty($"{ConditionalSymbolScanner.InternalDllImportSymbol} is defined once, in {RepoLayout.PropsRelativePath}");
    }
}
