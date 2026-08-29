using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 2: a silently changed DllName is caught by the checked-in manifest, per target framework.</summary>
public class DllImportManifestTests
{
    public static TheoryData<string> TargetFrameworks => new(RepoLayout.StandardTargetFrameworks);

    [Fact]
    public void Manifest_OnParse_HasOneEntryPerShippingAssembly_WhenCheckedIn()
    {
        var manifest = DllImportManifest.Parse(RepoLayout.ReadText(DllImportManifest.RelativePath));

        manifest.Should().HaveCount(NoStaticLinkingTests.ExpectedAssemblyCount);
        manifest.Keys.Should().BeEquivalentTo(RepoLayout.ShippingProjects().Select(p => p.AssemblyName));
    }

    [Theory, MemberData(nameof(TargetFrameworks))]
    public void BuiltAssemblies_OnCompare_MatchManifest_WhenFrameworkBuilt(string tfm)
    {
        var expected = DllImportManifest.Parse(RepoLayout.ReadText(DllImportManifest.RelativePath));

        var actual = RepoLayout.ShippingProjects()
            .Where(p => BuildOutputs.Exists(p.StandardOutput(tfm)))
            .ToDictionary(p => p.AssemblyName, p => AssemblyFacts.DistinctModules(BuildOutputs.Read(p.StandardOutput(tfm))), StringComparer.Ordinal);

        actual.Should().HaveCount(NoStaticLinkingTests.ExpectedAssemblyCount, $"every assembly must be built for {tfm}");

        var differences = DllImportManifest.Compare(expected, actual);

        differences.Should().BeEmpty(
            $"DllImport names for {tfm} must match {DllImportManifest.RelativePath}; if the change is intended, replace the file with:\n{DllImportManifest.Serialise(actual)}");
    }
}
