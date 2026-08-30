namespace FetchNatives;

/// <param name="Source">Absolute path inside the extracted archive.</param>
/// <param name="RelativePath">Where it lands under the output directory.</param>
public sealed record PlacedLibrary(string Source, string RelativePath);

/// <summary>
/// Where each platform's archives keep their loadable binaries, and where they go in the flat output.
/// </summary>
/// <remarks>
/// Layouts observed in the 2.4.18.3 archives:
/// <code>
///   windows   bass.dll (x86), x64/bass.dll
///   linux     libs/{x86,x86_64,armhf,aarch64}/libbass.so
///   osx       libbass.dylib (universal)
///   android   libs/{armeabi-v7a,arm64-v8a,x86,x86_64}/libbass.so
///   ios       bass.xcframework/{ios-arm64_armv7_armv7s,ios-arm64_i386_x86_64-simulator}/bass.framework/bass
///   catalyst  bass24-catalyst.zip only; &lt;name&gt;.framework/Versions/A/&lt;name&gt; for every add-on
/// </code>
/// </remarks>
public static class NativeLayout
{
    public static readonly IReadOnlyList<string> KnownPlatforms = ["windows", "linux", "osx", "ios", "catalyst", "android"];

    /// <summary>Slice names for the host CPU. Only windows and linux ship per-architecture builds.</summary>
    /// <remarks>NOTE: no arm64 Windows build exists upstream; x64 runs there under emulation.</remarks>
    public static (string Windows, string Linux) SlicesFor(string architecture) =>
        architecture.ToLowerInvariant() switch
        {
            "arm64" or "aarch64" => ("x64", "aarch64"),
            "x64" or "x86_64" or "amd64" => ("x64", "x86_64"),
            "x86" or "i386" or "i686" => ("x86", "x86"),
            "arm" or "armv7" or "armv7l" or "armhf" => ("x86", "armhf"),
            _ => throw new FetchException(
                $"unrecognised architecture '{architecture}'; set BASS_ARCH to arm64, x86_64, i686 or armv7l"),
        };

    /// <summary>
    /// Everything one library contributes to the output. Apple platforms keep whole bundle directories:
    /// device and simulator are different Mach-O platforms, and <c>NativeReference</c> slices the
    /// xcframework at build time. Android keeps every ABI, each under its own subdirectory.
    /// </summary>
    public static IEnumerable<PlacedLibrary> Locate(string platform, string archiveRoot, string lib, string architecture)
    {
        var (windowsSlice, linuxSlice) = SlicesFor(architecture);

        return platform switch
        {
            "windows" => [At(archiveRoot, windowsSlice == "x64" ? $"x64/{lib}.dll" : $"{lib}.dll", $"{lib}.dll")],
            "linux" => [At(archiveRoot, $"libs/{linuxSlice}/lib{lib}.so", $"lib{lib}.so")],
            "osx" => [At(archiveRoot, $"lib{lib}.dylib", $"lib{lib}.dylib")],
            "ios" => [At(archiveRoot, $"{lib}.xcframework", $"{lib}.xcframework")],
            "catalyst" => [At(archiveRoot, $"{lib}.framework", $"{lib}.framework")],
            "android" => AndroidAbis(archiveRoot, lib),
            _ => throw new FetchException($"unknown platform '{platform}'"),
        };
    }

    private static IEnumerable<PlacedLibrary> AndroidAbis(string archiveRoot, string lib)
    {
        var abiRoot = Path.Combine(archiveRoot, "libs");

        if (!Directory.Exists(abiRoot))
            throw new FetchException("libs/ not found in archive; upstream may have changed its layout");

        return Directory.EnumerateDirectories(abiRoot)
            .Select(Path.GetFileName)
            .OfType<string>()
            .Order(StringComparer.Ordinal)
            .Select(abi => At(archiveRoot, $"libs/{abi}/lib{lib}.so", $"{abi}/lib{lib}.so"));
    }

    private static PlacedLibrary At(string archiveRoot, string relativeSource, string relativePath) =>
        new(Path.Combine(archiveRoot, relativeSource.Replace('/', Path.DirectorySeparatorChar)), relativePath);
}
