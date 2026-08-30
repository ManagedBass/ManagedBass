using System.Runtime.InteropServices;

namespace ManagedBass.Tests.Infrastructure;

/// <summary>
/// Locates the fetched BASS natives next to the test assembly and turns a missing one into an actionable failure.
/// </summary>
/// <remarks>
/// NOTE: uses <see cref="RuntimeInformation"/> only, never <c>CrossPlatformHelper</c>: the predicates are under
/// test, so the infrastructure must not agree with them by construction.
/// </remarks>
internal static class NativeLibraries
{
    public static string ProbeDirectory => AppContext.BaseDirectory;

    /// <summary>The <c>eng/FetchNatives</c> platform name for the OS running the tests.</summary>
    public static string HostPlatform =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows"
        : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx"
        : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux"
        : throw new PlatformNotSupportedException($"Unsupported test host: {RuntimeInformation.OSDescription}");

    public static string FileNameFor(string name) =>
        HostPlatform switch
        {
            "windows" => $"{name}.dll",
            "osx" => $"lib{name}.dylib",
            _ => $"lib{name}.so",
        };

    public static string PathFor(string name) => Path.Combine(ProbeDirectory, FileNameFor(name));

    /// <summary>The one place the fetch command is spelled out, so a rename of the tool moves every message at once.</summary>
    public static string FetchCommand => $"dotnet run --project eng/FetchNatives -- {HostPlatform}";

    public static void Require(string name)
    {
        if (!File.Exists(PathFor(name)))
            throw new InvalidOperationException(MissingMessage(name));
    }

    static string MissingMessage(string name) =>
        $"""
         BASS native '{FileNameFor(name)}' not found in '{ProbeDirectory}'.
         Natives are never committed. A build normally fetches them; force it and rebuild so MSBuild
         copies them into the output:

             {FetchCommand} --refresh
             dotnet build tests/ManagedBass.Tests

         BASS files present: {DescribePresent()}
         """;

    static string DescribePresent()
    {
        var present = Directory.EnumerateFiles(ProbeDirectory)
            .Select(Path.GetFileName)
            .Where(f => f!.Contains("bass", StringComparison.OrdinalIgnoreCase) && !f.StartsWith("ManagedBass", StringComparison.Ordinal))
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return present.Length == 0 ? "(none)" : string.Join(", ", present);
    }
}
