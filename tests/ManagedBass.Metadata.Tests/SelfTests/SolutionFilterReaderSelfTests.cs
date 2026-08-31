using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class SolutionFilterReaderSelfTests
{
    private const string Solution = """
        Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "BassFx", "AddOns\BassFx\BassFx.csproj", "{62830AC1-398E-45F6-8C77-B6346019A65D}"
        EndProject
        Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "AddOns", "AddOns", "{AAAA}"
        EndProject
        Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Bass", "Bass\Portable\Bass.csproj", "{E29CFA86}"
        EndProject
        Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "ManagedBass.Tests", "..\tests\ManagedBass.Tests\ManagedBass.Tests.csproj", "{BBBB}"
        EndProject
        """;

    [Fact]
    public void ReadSolutionProjects_OnSolution_SkipsFoldersAndSorts_WhenMixed() =>
        SolutionFilterReader.ReadSolutionProjects(Solution)
            .Should().Equal("../tests/ManagedBass.Tests/ManagedBass.Tests.csproj", "AddOns/BassFx/BassFx.csproj", "Bass/Portable/Bass.csproj");

    [Fact]
    public void ReadFilterProjects_OnFilter_NormalisesAndSorts_WhenBackslashed()
    {
        const string filter = """{ "solution": { "path": "ManagedBass.sln", "projects": [ "Bass\\Portable\\Bass.csproj", "AddOns\\BassFx\\BassFx.csproj" ] } }""";

        SolutionFilterReader.ReadFilterProjects(filter).Should().Equal("AddOns/BassFx/BassFx.csproj", "Bass/Portable/Bass.csproj");
        SolutionFilterReader.ReadFilterSolutionPath(filter).Should().Be("ManagedBass.sln");
    }
}
