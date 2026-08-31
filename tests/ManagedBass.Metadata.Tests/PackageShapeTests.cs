using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 5: the core csproj packs the buildTransitive pair, and a produced package has the agreed layout.</summary>
public class PackageShapeTests
{
    private const string PackagesDirectory = "artifacts/packages";

    [Fact]
    public void CoreProject_OnInspect_PacksBuildTransitivePair_WhenChecked()
    {
        var facts = PackageShapeInspector.DescribeCoreProject(RepoLayout.ReadText(RepoLayout.CoreProjectRelativePath));

        facts.Should().Be(new CoreProjectFacts(true, true, true));
    }

    [Fact]
    public void CorePackage_OnInspect_HasFiveLibFoldersAndBuildTransitive_WhenPacked()
    {
        var package = FindCorePackage();

        // NOTE: in CI the pack step is part of the contract, so a missing package is a broken pipeline, not a dev loop without one.
        if (package is null && Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
            throw new InvalidOperationException($"No ManagedBass.<version>.nupkg under {PackagesDirectory}/ in CI; the pack step must use -o {PackagesDirectory}.");

        if (package is null)
            Assert.Skip($"No ManagedBass.<version>.nupkg under {PackagesDirectory}/; run dotnet pack -o {PackagesDirectory} to enable this check.");

        var entries = PackageShapeInspector.ReadEntries(package);

        PackageShapeInspector.LibFolders(entries).Should().BeEquivalentTo(RepoLayout.StandardTargetFrameworks);
        PackageShapeInspector.MissingPackageEntries(entries).Should().BeEmpty();
    }

    private static string? FindCorePackage()
    {
        var directory = RepoLayout.Absolute(PackagesDirectory);

        return Directory.Exists(directory)
            ? Directory.EnumerateFiles(directory, "*.nupkg", SearchOption.AllDirectories).FirstOrDefault(f => PackageShapeInspector.IsCorePackage(Path.GetFileName(f)))
            : null;
    }
}
