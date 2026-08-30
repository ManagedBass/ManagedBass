using System.Runtime.InteropServices;
using FetchNatives;

const string Usage = """
    usage: dotnet run --project eng/FetchNatives -- <windows|linux|osx|ios|catalyst|android> [output-dir]
      --refresh            ignore the download cache
      --list               print the archives for the platform and exit
      --manifest <path>    read a manifest other than eng/bass-manifest.json
    env: BASS_ARCH (uname -m style; picks the windows/linux slice), BASS_DOWNLOAD_CACHE (default artifacts/downloads)
    """;

try
{
    return await Run();
}
catch (FetchException ex)
{
    Report.Error(ex.Message);
    return 1;
}

async Task<int> Run()
{
    string? platform = null, outputDirectory = null, manifestPath = null;
    var refresh = false;
    var listOnly = false;

    for (var i = 0; i < args.Length; i++)
        switch (args[i])
        {
            case "-h" or "--help":
                Console.WriteLine(Usage);
                return 0;
            case "--refresh":
                refresh = true;
                break;
            case "--list":
                listOnly = true;
                break;
            case "--manifest":
                manifestPath = i + 1 < args.Length ? args[++i] : throw new FetchException("--manifest needs a path");
                break;
            case var option when option.StartsWith('-'):
                throw new FetchException($"unknown option '{option}'");
            case var value when platform is null:
                platform = value;
                break;
            case var value when outputDirectory is null:
                outputDirectory = value;
                break;
            default:
                throw new FetchException($"unexpected argument '{args[i]}'");
        }

    if (platform is null)
    {
        Report.Error($"no platform given\n{Usage}");
        return 1;
    }

    if (!NativeLayout.KnownPlatforms.Contains(platform))
        throw new FetchException($"unknown platform '{platform}' (expected one of: {string.Join(", ", NativeLayout.KnownPlatforms)})");

    manifestPath = Path.GetFullPath(manifestPath ?? LocateManifest());
    var manifest = BassManifest.Load(manifestPath);
    var repoRoot = Path.GetDirectoryName(Path.GetDirectoryName(manifestPath)!)!;

    if (listOnly)
    {
        Console.WriteLine($"{platform} (BASS {manifest.BassVersion})");

        foreach (var archive in manifest.ArchivesFor(platform))
            Console.WriteLine($"  {archive.Zip,-24} {string.Join(", ", archive.Libs)}");

        return 0;
    }

    var fetcher = new NativeFetcher(
        manifest,
        platform,
        outputDirectory ?? Path.Combine(repoRoot, "artifacts", "natives", platform),
        Environment.GetEnvironmentVariable("BASS_DOWNLOAD_CACHE") ?? Path.Combine(repoRoot, "artifacts", "downloads"),
        Environment.GetEnvironmentVariable("BASS_ARCH") ?? RuntimeInformation.OSArchitecture.ToString(),
        refresh);

    await fetcher.Run();
    return 0;
}

// NOTE: the tool runs from its own bin/ during a build and from the repo root by hand, so find the
// manifest by walking up from both rather than assuming either.
static string LocateManifest()
{
    foreach (var start in new[] { AppContext.BaseDirectory, Environment.CurrentDirectory })
        for (var directory = new DirectoryInfo(start); directory is not null; directory = directory.Parent)
        {
            var candidate = Path.Combine(directory.FullName, "eng", BassManifest.FileName);

            if (File.Exists(candidate))
                return candidate;
        }

    throw new FetchException($"could not find eng/{BassManifest.FileName} above '{Environment.CurrentDirectory}'; pass --manifest <path>");
}
