using System.IO.Compression;

namespace FetchNatives;

/// <summary>
/// Zip extraction that keeps Unix symbolic links.
/// </summary>
/// <remarks>
/// NOTE: <see cref="ZipFile.ExtractToDirectory(string, string)"/> writes a symlink entry as an ordinary file
/// holding the target path, which silently breaks the Catalyst bundles - their
/// <c>&lt;name&gt;.framework/&lt;name&gt;</c> is a link into <c>Versions/Current/</c>.
/// </remarks>
public static class ArchiveExtractor
{
    private const int SymbolicLink = 0xA000;
    private const int FileTypeMask = 0xF000;

    public static void ExtractTo(string archivePath, string destinationRoot)
    {
        var root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(destinationRoot)) + Path.DirectorySeparatorChar;
        Directory.CreateDirectory(root);

        using var archive = ZipFile.OpenRead(archivePath);

        foreach (var entry in archive.Entries)
        {
            var destination = ResolveEntryPath(root, entry.FullName);

            if (entry.FullName.EndsWith('/'))
            {
                Directory.CreateDirectory(destination);
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);

            if (!TryExtractSymbolicLink(entry, destination))
                entry.ExtractToFile(destination, overwrite: true);
        }
    }

    /// <summary>Copies a file or a whole directory tree, recreating any symbolic links rather than following them.</summary>
    public static void Copy(string source, string destination)
    {
        if (new FileInfo(source).LinkTarget is { } linkTarget)
        {
            File.CreateSymbolicLink(destination, linkTarget);
            return;
        }

        if (!Directory.Exists(source))
        {
            File.Copy(source, destination, overwrite: true);
            return;
        }

        Directory.CreateDirectory(destination);

        foreach (var child in Directory.EnumerateFileSystemEntries(source))
            Copy(child, Path.Combine(destination, Path.GetFileName(child)));
    }

    private static bool TryExtractSymbolicLink(ZipArchiveEntry entry, string destination)
    {
        if (OperatingSystem.IsWindows() || ((entry.ExternalAttributes >> 16) & FileTypeMask) != SymbolicLink)
            return false;

        using var reader = new StreamReader(entry.Open());
        var linkTarget = reader.ReadToEnd().Trim();

        if (linkTarget.Length == 0)
            return false;

        File.Delete(destination);
        File.CreateSymbolicLink(destination, linkTarget);
        return true;
    }

    private static string ResolveEntryPath(string root, string entryName)
    {
        var destination = Path.GetFullPath(Path.Combine(root, entryName.Replace('/', Path.DirectorySeparatorChar)));

        return destination.StartsWith(root, StringComparison.Ordinal)
            ? destination
            : throw new FetchException($"archive entry '{entryName}' points outside the extraction directory");
    }
}
