using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class PlatformScaffoldingScannerSelfTests
{
    [Fact]
    public void ScanProjectFile_OnPlatformsElement_ReportsIos_WhenListed() =>
        PlatformScaffoldingScanner.ScanProjectFile("p.csproj", "<Platforms>AnyCPU;iOS</Platforms>")
            .Should().ContainSingle().Which.Token.Should().Be("iOS");

    [Fact]
    public void ScanProjectFile_OnPlatformsElement_ReportsNothing_WhenOnlyArchitectures() =>
        PlatformScaffoldingScanner.ScanProjectFile("p.csproj", "<Platforms>AnyCPU;x64;ARM64</Platforms>").Should().BeEmpty();

    [Fact]
    public void ScanProjectFile_OnCondition_ReportsIos_WhenPlatformComparedToOs() =>
        PlatformScaffoldingScanner.ScanProjectFile("p.csproj", "<PropertyGroup Condition=\"'$(Platform)' == 'iOS'\">")
            .Should().ContainSingle().Which.Detail.Should().Contain("$(Platform)");

    [Fact]
    public void ScanProjectFile_OnCondition_ReportsNothing_WhenPlatformComparedToArchitecture() =>
        PlatformScaffoldingScanner.ScanProjectFile("p.csproj", "<PropertyGroup Condition=\"'$(Platform)' == 'x64'\">").Should().BeEmpty();

    [Fact]
    public void ScanSolutionFile_OnConfiguration_ReportsIos_WhenPipeIos()
    {
        const string sln = "\t\tDebug|Any CPU = Debug|Any CPU\n\t\tDebug|iOS = Debug|iOS\n\t\t{GUID}.Release|iOS.ActiveCfg = Release|iOS";

        var sut = PlatformScaffoldingScanner.ScanSolutionFile("s.sln", sln);

        sut.Select(o => o.LineNumber).Should().Equal(2, 3);
    }

    [Fact]
    public void ScanSolutionFile_OnConfiguration_ReportsNothing_WhenOnlyAnyCpu() =>
        PlatformScaffoldingScanner.ScanSolutionFile("s.sln", "\t\tDebug|Any CPU = Debug|Any CPU").Should().BeEmpty();
}
