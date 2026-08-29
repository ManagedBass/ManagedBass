using System.IO.Compression;
using System.Xml.Linq;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record CoreProjectFacts(bool RemovesBuildFromCompile, bool PacksTargets, bool PacksInitialiser);

/// <summary>What the core csproj packs, and what a produced package actually contains.</summary>
public static class PackageShapeInspector
{
    public const string TargetsFileName = "ManagedBass.targets";
    public const string InitialiserFileName = "ManagedBassNativeResolverInitialiser.cs";
    public const string BuildTransitiveFolder = "buildTransitive/";

    public static readonly IReadOnlyList<string> RequiredPackageEntries =
        [BuildTransitiveFolder + TargetsFileName, BuildTransitiveFolder + InitialiserFileName];

    public static CoreProjectFacts DescribeCoreProject(string csprojContent)
    {
        var document = XDocument.Parse(csprojContent);
        var elements = document.Descendants().ToArray();

        var removesBuild = elements.Any(e => e.Name.LocalName == "Compile" && Normalise(e.Attribute("Remove")?.Value) == "build/**");

        return new(removesBuild, PacksToBuildTransitive(elements, TargetsFileName), PacksToBuildTransitive(elements, InitialiserFileName));
    }

    public static IReadOnlyList<string> LibFolders(IEnumerable<string> entries) =>
        entries.Select(Normalise)
            .Where(e => e.StartsWith("lib/", StringComparison.OrdinalIgnoreCase))
            .Select(e => e["lib/".Length..].Split('/', 2)[0])
            .Where(f => f.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(f => f, StringComparer.Ordinal)
            .ToArray();

    public static IReadOnlyList<string> MissingPackageEntries(IEnumerable<string> entries)
    {
        var present = entries.Select(Normalise).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return RequiredPackageEntries.Where(e => !present.Contains(e)).ToArray();
    }

    public static IReadOnlyList<string> ReadEntries(string packagePath)
    {
        using var archive = ZipFile.OpenRead(packagePath);
        return archive.Entries.Select(e => Normalise(e.FullName)).ToArray();
    }

    /// <summary>The core package among <c>ManagedBass.Fx.3.0.0.nupkg</c> and friends: name then version digits.</summary>
    public static bool IsCorePackage(string fileName) =>
        fileName.StartsWith("ManagedBass.", StringComparison.OrdinalIgnoreCase)
        && fileName.Length > "ManagedBass.".Length
        && char.IsDigit(fileName["ManagedBass.".Length])
        && fileName.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase);

    private static bool PacksToBuildTransitive(IEnumerable<XElement> elements, string fileName) =>
        elements.Any(e =>
            e.Name.LocalName == "None"
            && Normalise(e.Attribute("Include")?.Value).EndsWith("build/" + fileName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(e.Attribute("Pack")?.Value, "true", StringComparison.OrdinalIgnoreCase)
            && Normalise(e.Attribute("PackagePath")?.Value).TrimEnd('/') == BuildTransitiveFolder.TrimEnd('/'));

    private static string Normalise(string? path) => (path ?? string.Empty).Trim().Replace('\\', '/');
}
