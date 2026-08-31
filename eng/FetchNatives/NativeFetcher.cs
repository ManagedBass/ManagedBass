namespace FetchNatives;

/// <summary>
/// Downloads one platform's archives into a cache and lays the loadable binaries out flat under the
/// output directory. Natives are never committed, so this is what makes a fresh clone testable.
/// </summary>
public sealed class NativeFetcher
{
    private static readonly TimeSpan[] RetryDelays = [TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5)];

    private readonly BassManifest _manifest;
    private readonly string _platform;
    private readonly string _outputDirectory;
    private readonly string _cacheDirectory;
    private readonly string _architecture;
    private readonly bool _refresh;

    public NativeFetcher(BassManifest manifest, string platform, string outputDirectory, string cacheDirectory, string architecture, bool refresh)
    {
        _manifest = manifest;
        _platform = platform;
        _outputDirectory = outputDirectory;
        _cacheDirectory = cacheDirectory;
        _architecture = architecture;
        _refresh = refresh;
    }

    public async Task Run()
    {
        Directory.CreateDirectory(_cacheDirectory);
        Directory.CreateDirectory(_outputDirectory);

        var work = Directory.CreateTempSubdirectory("fetch-natives.");
        var placed = 0;

        try
        {
            foreach (var archive in _manifest.ArchivesFor(_platform))
            {
                var extracted = Path.Combine(work.FullName, Path.GetFileNameWithoutExtension(archive.Zip));
                ArchiveExtractor.ExtractTo(await Download(archive), extracted);
                placed += archive.Libs.Sum(lib => Place(extracted, lib));
            }
        }
        finally
        {
            work.Delete(recursive: true);
        }

        Report.Info($"placed {placed} binaries in {_outputDirectory}");
    }

    private async Task<string> Download(BassArchive archive)
    {
        var target = Path.Combine(_cacheDirectory, archive.Zip);

        if (!_refresh && File.Exists(target) && new FileInfo(target).Length > 0)
        {
            Report.Info($"using cached {archive.Zip}");
            return target;
        }

        var url = $"{_manifest.BaseUrl.TrimEnd('/')}/{archive.Zip}";
        Report.Info($"downloading {url}");

        using var http = new HttpClient { Timeout = TimeSpan.FromMinutes(15) };
        var partial = $"{target}.partial";

        for (var attempt = 0; ; attempt++)
        {
            try
            {
                await using (var response = await http.GetStreamAsync(url))
                await using (var file = File.Create(partial))
                    await response.CopyToAsync(file);

                File.Move(partial, target, overwrite: true);
                return target;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException)
            {
                File.Delete(partial);

                if (attempt >= RetryDelays.Length)
                    throw new FetchException($"failed to download {url}: {ex.Message}");

                Report.Info($"download failed ({ex.Message}); retrying");
                await Task.Delay(RetryDelays[attempt]);
            }
        }
    }

    private int Place(string archiveRoot, string lib)
    {
        var libraries = NativeLayout.Locate(_platform, archiveRoot, lib, _architecture).ToArray();

        foreach (var library in libraries)
        {
            if (!Path.Exists(library.Source))
                throw new FetchException(
                    $"{Path.GetRelativePath(archiveRoot, library.Source)} not found in the archive; upstream may have changed its layout");

            var destination = Path.Combine(_outputDirectory, library.RelativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            Delete(destination);
            ArchiveExtractor.Copy(library.Source, destination);
            Report.Info($"  -> {_platform}/{library.RelativePath}");
        }

        return libraries.Length;
    }

    private static void Delete(string path)
    {
        if (Directory.Exists(path) && new FileInfo(path).LinkTarget is null)
            Directory.Delete(path, recursive: true);
        else
            File.Delete(path);
    }
}
