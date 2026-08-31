using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetchNatives;

/// <summary>One upstream archive and the libraries to lift out of it.</summary>
public sealed record BassArchive(string Zip, IReadOnlyList<string> Libs);

/// <summary>The parsed contents of <c>eng/bass-manifest.json</c>.</summary>
public sealed record BassManifest(string BassVersion, string BaseUrl, IReadOnlyDictionary<string, IReadOnlyList<BassArchive>> Platforms)
{
    public const string FileName = "bass-manifest.json";

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    public static BassManifest Load(string path)
    {
        var manifest = Read(path) ?? throw new FetchException($"{path} is empty");

        if (string.IsNullOrWhiteSpace(manifest.BaseUrl))
            throw new FetchException($"{path} has no baseUrl");

        return manifest;
    }

    public IReadOnlyList<BassArchive> ArchivesFor(string platform) =>
        Platforms.TryGetValue(platform, out var archives) && archives.Count > 0
            ? archives
            : throw new FetchException($"the manifest lists no archives for '{platform}'");

    private static BassManifest? Read(string path)
    {
        try
        {
            return JsonSerializer.Deserialize<BassManifest>(File.ReadAllText(path), Options);
        }
        catch (JsonException ex)
        {
            throw new FetchException($"{path} is not valid JSON: {ex.Message}");
        }
    }
}
