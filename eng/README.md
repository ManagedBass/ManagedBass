# eng/

Build and test scripts. Nothing here ships in a package. Native BASS binaries are never committed; they are downloaded on demand into `artifacts/`, which is git-ignored.

## `FetchNatives/`

**You do not normally run this.** Building `tests/ManagedBass.Tests` or `tests/ManagedBass.Ios.Harness` fetches what it needs, so a fresh clone tests with nothing but `dotnet test`. To run it by hand:

```
dotnet run --project eng/FetchNatives -- <windows|linux|osx|ios|catalyst|android> [output-dir]
  --refresh            ignore the download cache
  --list               print the archives for the platform and exit
  --manifest <path>    read a manifest other than eng/bass-manifest.json
```

Downloads the archives listed in `bass-manifest.json` for one platform and lays the loadable binaries out flat under `artifacts/natives/<platform>/` (`bass`, `bassmix`, `bassflac`, `bassenc`). Windows and Linux pick the slice matching the host architecture (override with `BASS_ARCH`); Android extracts every ABI; Apple bundles are universal. `ios` keeps the whole `.xcframework` because device and simulator are different Mach-O platforms and `<NativeReference Kind="Framework">` slices it at build time. `catalyst` extracts four `.framework` bundles from `bass24-catalyst.zip`, the only Catalyst archive un4seen publish. `BASS_DOWNLOAD_CACHE` moves the archive cache (default `artifacts/downloads/`).

It is a C# console project rather than a shell script so that it runs the same way on Windows, where a contributor may have no bash, and so that it can be part of the build rather than a step someone has to know about. It has no `PackageReference`: its restore is the first thing that runs on a fresh clone.

### `FetchNatives.targets`

Imported by every project that needs natives. Sets `BassNativesPlatform`, and the fetch runs `BeforeTargets="PrepareForBuild"` with `Inputs`/`Outputs` against `bass-manifest.json` and an `artifacts/natives/<platform>.fetched` stamp, so it costs one download on a fresh clone and nothing on every build after that. Delete `artifacts/` to force it. Set `BassNativesCopyToOutput=true` to have the loose libraries copied next to the built assembly.

**A failed download never fails the build.** un4seen is a third party and `ManagedBass.Tests` is in `ManagedBass.sln`, so hard-failing here would stop anyone compiling the library whenever that site is unreachable. Instead the fetch warns (`BASS0001`), leaves the stamp unwritten so the next build retries, and lets whatever actually needs a native be the thing that complains: the Tier 1 fixture fails with the command to run, and the iOS harness errors on the missing `bass.xcframework`. A cached archive is used before the network is touched, so a developer who has fetched once is unaffected either way.

Opt out entirely with `-p:FetchBassNatives=false` (or the environment variable) when building offline, or in a job with no use for natives — the `metadata` job in `build-test.yml` does exactly that.

## `bass-manifest.json`

Records the expected BASS version, the base URL, and per platform the archives to fetch with the libraries to extract.

un4seen's URLs are unversioned (`bass24.zip` is always the latest build), so `bassVersion` and `verifiedOn` describe the build whose *layout* was checked, not what a fetch today returns. There are no checksums: the same unversioned URL is what anyone downloading BASS by hand would use, so a hash here is not a security boundary, and a stale one only ever produced noise. What does catch an upstream change is the layout itself — the fetcher fails loudly if `x64/bass.dll`, `libs/<abi>/libbass.so` or `bass.xcframework` is no longer where it expects.

## `pack-release-artefacts.sh`

```
eng/pack-release-artefacts.sh <version> [output-dir]
```

Collects the standard build and the internal-DllImport build (both must already exist) into `ManagedBass-<version>-dotnet.zip` and `ManagedBass-<version>-unity-ios.zip` under `artifacts/release/` by default. Run by `package-and-release.yml` on each published release.
