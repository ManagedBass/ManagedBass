using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 7: the opt-in build is all-or-nothing, and its source guards are symmetric.</summary>
public class InternalDllImportTests
{
    public const int ExpectedGuardedFiles = 38;

    [Fact]
    public void InternalDllImportBuild_OnInspect_ImportsOnlyInternalDllImportModule_WhenAllPresent()
    {
        var projects = RepoLayout.ShippingProjects();
        var classification = BuildOutputs.Classify(BuildOutputs.InternalDllImport(projects), BuildOutputs.Exists);

        if (classification.Presence == InternalDllImportPresence.None)
            Assert.Skip($"Internal-DllImport output absent; run: {BuildOutputs.InternalDllImportBuildCommand}");

        classification.Missing.Should().BeEmpty("a partial internal-DllImport artefact ships silently missing add-ons");

        var offenders = projects
            .SelectMany(p => AssemblyFacts.DllImports(BuildOutputs.Read(p.InternalDllImportOutput()))
                .Where(s => s.Module != "__Internal")
                .Select(s => $"{p.InternalDllImportOutput()}: {s}"))
            .ToArray();

        offenders.Should().BeEmpty();
    }

    [Fact]
    public void InternalDllImportBuild_OnInspect_MatchesStandardAssemblySet_WhenAllPresent()
    {
        var projects = RepoLayout.ShippingProjects();
        var classification = BuildOutputs.Classify(BuildOutputs.InternalDllImport(projects), BuildOutputs.Exists);

        if (classification.Presence == InternalDllImportPresence.None)
            Assert.Skip($"Internal-DllImport output absent; run: {BuildOutputs.InternalDllImportBuildCommand}");

        classification.Missing.Should().BeEmpty();

        var standard = projects.Select(p => p.StandardOutput(RepoLayout.InternalDllImportTargetFramework)).Where(BuildOutputs.Exists).Select(Path.GetFileName);
        var internalDllImport = projects.Select(p => p.InternalDllImportOutput()).Select(Path.GetFileName);

        internalDllImport.Should().BeEquivalentTo(standard).And.HaveCount(NoStaticLinkingTests.ExpectedAssemblyCount);
    }

    [Fact]
    public void Sources_OnScan_GuardEveryDllName_WhenChecked()
    {
        var facts = RepoLayout.SourceFiles(".cs")
            .Select(f => (path: f, content: RepoLayout.ReadText(f)))
            .Where(f => DllNameGuardScanner.DeclaresDllName(f.content))
            .Select(f => DllNameGuardScanner.Scan(f.path, f.content))
            .ToArray();

        facts.Should().HaveCount(ExpectedGuardedFiles);
        facts.Where(f => !f.IsGuarded).Should().BeEmpty("every DllName needs the #if MANAGEDBASS_INTERNAL_DLLIMPORT / __Internal guard");
    }

    [Fact]
    public void Props_OnScan_AppendsInternalDllImportSymbol_WhenChecked()
    {
        var content = RepoLayout.ReadText(RepoLayout.PropsRelativePath);

        ConditionalSymbolScanner.FindInternalDllImportDefinitions(RepoLayout.PropsRelativePath, content).Should().NotBeEmpty();
        ConditionalSymbolScanner.FindClobbers(RepoLayout.PropsRelativePath, content).Should().BeEmpty();
    }
}
