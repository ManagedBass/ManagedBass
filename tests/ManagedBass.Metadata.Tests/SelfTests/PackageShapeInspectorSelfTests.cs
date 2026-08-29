using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class PackageShapeInspectorSelfTests
{
    private const string Wired = """
        <Project>
          <ItemGroup>
            <Compile Remove="build/**" />
            <None Include="build/ManagedBass.targets" Pack="true" PackagePath="buildTransitive/" />
            <None Include="build\ManagedBassNativeResolverInitialiser.cs" Pack="true" PackagePath="buildTransitive" />
          </ItemGroup>
        </Project>
        """;

    [Fact]
    public void DescribeCoreProject_OnWiredProject_ReportsAllTrue_WhenBothFilesPacked() =>
        PackageShapeInspector.DescribeCoreProject(Wired).Should().Be(new CoreProjectFacts(true, true, true));

    [Fact]
    public void DescribeCoreProject_OnProject_ReportsMissing_WhenPackDropped() =>
        PackageShapeInspector.DescribeCoreProject(Wired.Replace("Pack=\"true\" PackagePath=\"buildTransitive/\"", "")).PacksTargets.Should().BeFalse();

    [Fact]
    public void DescribeCoreProject_OnProject_ReportsMissing_WhenPackagePathWrong() =>
        PackageShapeInspector.DescribeCoreProject(Wired.Replace("buildTransitive/", "build/")).PacksTargets.Should().BeFalse();

    [Fact]
    public void DescribeCoreProject_OnProject_ReportsNoRemove_WhenCompileRemoveDropped() =>
        PackageShapeInspector.DescribeCoreProject(Wired.Replace("<Compile Remove=\"build/**\" />", "")).RemovesBuildFromCompile.Should().BeFalse();

    [Fact]
    public void LibFolders_OnEntries_ReturnsDistinctFolders_WhenMixedSeparators() =>
        PackageShapeInspector.LibFolders(["lib/net45/a.dll", "lib\\net8.0\\a.dll", "lib/net45/a.xml", "buildTransitive/x.targets"])
            .Should().Equal("net45", "net8.0");

    [Fact]
    public void MissingPackageEntries_OnEntries_ReportsInitialiser_WhenOnlyTargetsPacked() =>
        PackageShapeInspector.MissingPackageEntries(["buildTransitive/ManagedBass.targets"]).Should().Equal("buildTransitive/ManagedBassNativeResolverInitialiser.cs");

    [Theory]
    [InlineData("ManagedBass.4.0.1.nupkg", true)]
    [InlineData("ManagedBass.Fx.4.0.1.nupkg", false)]
    [InlineData("ManagedBass.4.0.1.snupkg", false)]
    public void IsCorePackage_OnFileName_DistinguishesCore_WhenAddOnsBesideIt(string fileName, bool expected) =>
        PackageShapeInspector.IsCorePackage(fileName).Should().Be(expected);
}
