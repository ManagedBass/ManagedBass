using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 8: the filter the internal-DllImport build uses lists exactly the shipping projects.</summary>
public class ShippingSolutionFilterTests
{
    [Fact]
    public void Filter_OnRead_ListsExactlyTheShippingProjects_WhenCompared()
    {
        var filter = RepoLayout.ReadText(RepoLayout.SolutionFilterRelativePath);

        var shipping = SolutionFilterReader.ReadSolutionProjects(RepoLayout.ReadText(RepoLayout.SolutionRelativePath))
            .Where(p => !p.StartsWith("..", StringComparison.Ordinal))
            .ToArray();

        shipping.Should().HaveCount(NoStaticLinkingTests.ExpectedAssemblyCount);
        SolutionFilterReader.ReadFilterSolutionPath(filter).Should().Be("ManagedBass.sln");
        SolutionFilterReader.ReadFilterProjects(filter).Should().Equal(shipping);
    }

    [Fact]
    public void Filter_OnRead_ListsNoTestProject_WhenChecked()
    {
        var projects = SolutionFilterReader.ReadFilterProjects(RepoLayout.ReadText(RepoLayout.SolutionFilterRelativePath));

        projects.Where(p => p.StartsWith("..", StringComparison.Ordinal) || p.Contains("Tests", StringComparison.OrdinalIgnoreCase))
            .Should().BeEmpty("building test projects with BassInternalDllImport=true would poison Tier 1");
    }
}
