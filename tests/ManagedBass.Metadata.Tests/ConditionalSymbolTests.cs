using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 3: no dead Xamarin-era symbols under <c>src/</c>, and no DefineConstants misuse.</summary>
public class ConditionalSymbolTests
{
    private static readonly string[] _scannedExtensions = [".cs", ".csproj", ".props", ".targets", ".sln", ".slnf"];
    private static readonly string[] _buildFileExtensions = [".csproj", ".props", ".targets"];

    [Fact]
    public void SourceTree_OnScan_HasNoBannedSymbols_WhenScanned()
    {
        var files = RepoLayout.SourceFiles(_scannedExtensions);
        files.Should().NotBeEmpty();

        var violations = files
            .SelectMany(f => ConditionalSymbolScanner.Scan(f, RepoLayout.ReadText(f), ConditionalSymbolScanner.BannedSymbols))
            .OrderBy(o => o.RelativePath, StringComparer.Ordinal).ThenBy(o => o.LineNumber)
            .ToArray();

        violations.Should().BeEmpty("comments count too - keep history out of src/");
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
