namespace ManagedBass.Metadata.Tests.Analysis;

public enum InternalDllImportPresence { None, Partial, All }

public sealed record InternalDllImportClassification(InternalDllImportPresence Presence, IReadOnlyList<string> Missing);

/// <summary>Where each shipping assembly should be on disk, and which are actually there.</summary>
public static class BuildOutputs
{
    public const string InternalDllImportBuildCommand = "dotnet build src/ManagedBass.Shipping.slnf -c Release -p:BassInternalDllImport=true";

    public static IReadOnlyList<string> Standard(IEnumerable<ShippingProject> projects, string tfm) =>
        projects.Select(p => p.StandardOutput(tfm)).ToArray();

    public static IReadOnlyList<string> InternalDllImport(IEnumerable<ShippingProject> projects) =>
        projects.Select(p => p.InternalDllImportOutput()).ToArray();

    /// <summary>None absent → assert, all absent → skip, anything in between → a partial artefact, which is a defect.</summary>
    public static InternalDllImportClassification Classify(IReadOnlyList<string> expected, Func<string, bool> exists)
    {
        var missing = expected.Where(p => !exists(p)).OrderBy(p => p, StringComparer.Ordinal).ToArray();

        var presence = missing.Length == 0 ? InternalDllImportPresence.All
            : missing.Length == expected.Count ? InternalDllImportPresence.None
            : InternalDllImportPresence.Partial;

        return new(presence, missing);
    }

    public static byte[] Read(string relativePath) => File.ReadAllBytes(RepoLayout.Absolute(relativePath));

    public static bool Exists(string relativePath) => File.Exists(RepoLayout.Absolute(relativePath));
}
