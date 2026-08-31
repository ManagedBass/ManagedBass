using System.Text.RegularExpressions;

namespace ManagedBass.Metadata.Tests.Analysis;

/// <summary>Locates the repository and the shipping projects it declares.</summary>
public static partial class RepoLayout
{
    public const string SolutionRelativePath = "src/ManagedBass.sln";
    public const string SolutionFilterRelativePath = "src/ManagedBass.Shipping.slnf";
    public const string PropsRelativePath = "src/Directory.Build.props";
    public const string CoreProjectRelativePath = "src/Bass/Portable/Bass.csproj";
    public const string TargetsRelativePath = "src/Bass/Portable/build/ManagedBass.targets";
    public const string InitialiserRelativePath = "src/Bass/Portable/build/ManagedBassNativeResolverInitialiser.cs";

    public static readonly IReadOnlyList<string> StandardTargetFrameworks =
        ["net45", "netstandard2.0", "net8.0", "net9.0", "net10.0"];

    public const string InternalDllImportTargetFramework = "netstandard2.0";

    private static readonly Lazy<string> _root = new(() =>
        FindRoot(AppContext.BaseDirectory, dir => File.Exists(Path.Combine(dir, SolutionRelativePath)))
        ?? throw new InvalidOperationException($"No ancestor of {AppContext.BaseDirectory} contains {SolutionRelativePath}."));

    public static string Root => _root.Value;

    public static string Absolute(string relativePath) => Path.Combine(Root, relativePath.Replace('/', Path.DirectorySeparatorChar));

    public static string Relative(string absolutePath) => Path.GetRelativePath(Root, absolutePath).Replace('\\', '/');

    public static string ReadText(string relativePath) => File.ReadAllText(Absolute(relativePath));

    public static string? FindRoot(string start, Func<string, bool> isRoot)
    {
        for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
            if (isRoot(dir.FullName))
                return dir.FullName;

        return null;
    }

    /// <summary>Files under <c>src/</c> with any of the given extensions, repo-relative and sorted.</summary>
    public static IReadOnlyList<string> SourceFiles(params string[] extensions) =>
        Directory.EnumerateFiles(Absolute("src"), "*", SearchOption.AllDirectories)
            .Where(f => extensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
            .Select(Relative)
            .Where(f => !f.Contains("/bin/") && !f.Contains("/obj/"))
            .OrderBy(f => f, StringComparer.Ordinal)
            .ToArray();

    /// <summary>The shipping projects the solution declares, with their assembly names.</summary>
    public static IReadOnlyList<ShippingProject> ShippingProjects() =>
        SolutionFilterReader.ReadSolutionProjects(ReadText(SolutionRelativePath))
            .Where(p => !p.StartsWith("..", StringComparison.Ordinal))
            .Select(p => "src/" + p)
            .Select(p => new ShippingProject(p, ReadAssemblyName(ReadText(p), Path.GetFileNameWithoutExtension(p))))
            .OrderBy(p => p.AssemblyName, StringComparer.Ordinal)
            .ToArray();

    public static string ReadAssemblyName(string csprojContent, string fallback)
    {
        var match = AssemblyNamePattern().Match(csprojContent);
        return match.Success ? match.Groups[1].Value.Trim() : fallback;
    }

    [GeneratedRegex(@"<AssemblyName>([^<]+)</AssemblyName>")]
    private static partial Regex AssemblyNamePattern();
}

public sealed record ShippingProject(string RelativePath, string AssemblyName)
{
    public string Directory => RelativePath[..RelativePath.LastIndexOf('/')];

    public string StandardOutput(string tfm) => $"{Directory}/bin/Release/{tfm}/{AssemblyName}.dll";

    public string InternalDllImportOutput() => $"{Directory}/bin/internaldllimport/Release/{RepoLayout.InternalDllImportTargetFramework}/{AssemblyName}.dll";
}
