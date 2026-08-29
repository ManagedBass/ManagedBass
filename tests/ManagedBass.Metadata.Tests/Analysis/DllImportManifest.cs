using System.Text.Json;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record ManifestDifference(string Assembly, IReadOnlyList<string> Expected, IReadOnlyList<string> Actual)
{
    public override string ToString() => $"{Assembly}: expected [{string.Join(", ", Expected)}] but shipped [{string.Join(", ", Actual)}]";
}

/// <summary>The checked-in map of assembly name to the native library names it imports.</summary>
public static class DllImportManifest
{
    public const string RelativePath = "tests/ManagedBass.Metadata.Tests/ExpectedDllImports.json";

    private static readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public static IReadOnlyDictionary<string, IReadOnlyList<string>> Parse(string json) =>
        (JsonSerializer.Deserialize<Dictionary<string, string[]>>(json) ?? throw new InvalidOperationException("Manifest is not a JSON object."))
        .ToDictionary(kv => kv.Key, IReadOnlyList<string> (kv) => kv.Value.OrderBy(v => v, StringComparer.Ordinal).ToArray(), StringComparer.Ordinal);

    public static string Serialise(IReadOnlyDictionary<string, IReadOnlyList<string>> manifest) =>
        JsonSerializer.Serialize(manifest.OrderBy(kv => kv.Key, StringComparer.Ordinal).ToDictionary(kv => kv.Key, kv => kv.Value), _options) + "\n";

    /// <summary>Every assembly whose imported names differ, in either direction, including ones only one side knows.</summary>
    public static IReadOnlyList<ManifestDifference> Compare(
        IReadOnlyDictionary<string, IReadOnlyList<string>> expected,
        IReadOnlyDictionary<string, IReadOnlyList<string>> actual) =>
        expected.Keys.Union(actual.Keys, StringComparer.Ordinal)
            .OrderBy(k => k, StringComparer.Ordinal)
            .Select(k => new ManifestDifference(k, expected.GetValueOrDefault(k, []), actual.GetValueOrDefault(k, [])))
            .Where(d => !d.Expected.SequenceEqual(d.Actual, StringComparer.Ordinal))
            .ToArray();
}
