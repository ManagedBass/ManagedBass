using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class BuildOutputsSelfTests
{
    private static readonly string[] _expected = ["a.dll", "b.dll", "c.dll"];

    [Fact]
    public void Classify_OnAllPresent_ReturnsAll_WhenNothingMissing() =>
        BuildOutputs.Classify(_expected, _ => true).Should().Be(new InternalDllImportClassification(InternalDllImportPresence.All, []));

    [Fact]
    public void Classify_OnAllAbsent_ReturnsNone_WhenEverythingMissing() =>
        BuildOutputs.Classify(_expected, _ => false).Presence.Should().Be(InternalDllImportPresence.None);

    [Fact]
    public void Classify_OnSubsetAbsent_ReturnsPartialWithMissing_WhenOneMissing()
    {
        var sut = BuildOutputs.Classify(_expected, p => p != "b.dll");

        sut.Presence.Should().Be(InternalDllImportPresence.Partial);
        sut.Missing.Should().Equal("b.dll");
    }

    [Fact]
    public void ShippingProject_OnOutputPaths_UsesAssemblyNameAndInternalDllImportFolder_WhenBuilt()
    {
        var sut = new ShippingProject("src/AddOns/BassMix/BassMix.csproj", "ManagedBass.Mix");

        sut.StandardOutput("net45").Should().Be("src/AddOns/BassMix/bin/Release/net45/ManagedBass.Mix.dll");
        sut.InternalDllImportOutput().Should().Be("src/AddOns/BassMix/bin/internaldllimport/Release/netstandard2.0/ManagedBass.Mix.dll");
    }

    [Fact]
    public void ReadAssemblyName_OnCsproj_FallsBackToProjectName_WhenNotDeclared()
    {
        RepoLayout.ReadAssemblyName("<Project><PropertyGroup><AssemblyName> ManagedBass.Fx </AssemblyName></PropertyGroup></Project>", "BassFx").Should().Be("ManagedBass.Fx");
        RepoLayout.ReadAssemblyName("<Project />", "BassFx").Should().Be("BassFx");
    }

    [Fact]
    public void FindRoot_OnWalkUp_ReturnsFirstMatchingAncestor_WhenPresent()
    {
        var start = Path.Combine(Path.GetTempPath(), "a", "b", "c");
        var expected = Path.Combine(Path.GetTempPath(), "a");

        RepoLayout.FindRoot(start, d => d == expected).Should().Be(expected);
        RepoLayout.FindRoot(start, _ => false).Should().BeNull();
    }
}
