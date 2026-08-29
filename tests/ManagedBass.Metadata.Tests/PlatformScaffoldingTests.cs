using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 4: no operating system used as an MSBuild Platform, in projects or the solution.</summary>
public class PlatformScaffoldingTests
{
    [Fact]
    public void Projects_OnScan_NameNoOperatingSystemPlatform_WhenChecked()
    {
        var projects = RepoLayout.SourceFiles(".csproj", ".props", ".targets");
        projects.Should().NotBeEmpty();

        var offences = projects.SelectMany(p => PlatformScaffoldingScanner.ScanProjectFile(p, RepoLayout.ReadText(p))).ToArray();

        offences.Should().BeEmpty("an OS is a runtime decision (CrossPlatformHelper), never a build Platform");
    }

    [Fact]
    public void Solution_OnScan_HasNoOperatingSystemConfiguration_WhenChecked()
    {
        var offences = PlatformScaffoldingScanner.ScanSolutionFile(RepoLayout.SolutionRelativePath, RepoLayout.ReadText(RepoLayout.SolutionRelativePath));

        offences.Should().BeEmpty();
    }
}
