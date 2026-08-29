using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 1: the normal build never imports <c>__Internal</c>, on any of the five target frameworks.</summary>
public class NoStaticLinkingTests
{
    public const int ExpectedAssemblyCount = 33;

    public static TheoryData<string> TargetFrameworks => new(RepoLayout.StandardTargetFrameworks);

    [Theory, MemberData(nameof(TargetFrameworks))]
    public void StandardBuild_OnScan_HasAllAssemblies_WhenFrameworkBuilt(string tfm)
    {
        var expected = BuildOutputs.Standard(RepoLayout.ShippingProjects(), tfm);
        var missing = expected.Where(p => !BuildOutputs.Exists(p)).ToArray();

        // NOTE: fails rather than skips - an unbuilt framework would otherwise pass vacuously.
        expected.Should().HaveCount(ExpectedAssemblyCount);
        missing.Should().BeEmpty($"every shipping assembly must be built for {tfm} (run dotnet build src/ManagedBass.sln -c Release)");
    }

    [Theory, MemberData(nameof(TargetFrameworks))]
    public void StandardBuild_OnScan_ImportsNoInternalDllImportModule_WhenFrameworkBuilt(string tfm)
    {
        var offenders = BuildOutputs.Standard(RepoLayout.ShippingProjects(), tfm)
            .Where(BuildOutputs.Exists)
            .SelectMany(p => AssemblyFacts.DllImports(BuildOutputs.Read(p)).Where(s => s.Module == "__Internal").Select(s => $"{p}: {s}"))
            .ToArray();

        offenders.Should().BeEmpty("the standard build must resolve natives dynamically");
    }
}
