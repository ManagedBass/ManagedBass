# eng/

Build and test scripts. Nothing here ships in a package. Native BASS binaries are never committed; they are downloaded on demand into `artifacts/`, which is git-ignored.

## `fetch-natives.sh`

```
eng/fetch-natives.sh <windows|linux|osx|ios|catalyst|android> [output-dir]
  --refresh   ignore the download cache
  --list      print the archives for the platform and exit
```

Downloads the archives listed in `bass-manifest.json` for one platform, checks each SHA-256, and lays the loadable binaries out flat under `artifacts/natives/<platform>/` (`bass`, `bassmix`, `bassflac`, `bassenc`). Windows and Linux pick the slice matching `uname -m` (override with `BASS_ARCH`); Android extracts every ABI; Apple bundles are universal. `ios` keeps the whole `.xcframework` because device and simulator are different Mach-O platforms and `<NativeReference Kind="Framework">` slices it at build time. `catalyst` extracts twelve `.framework` bundles from `bass24-catalyst.zip`, the only Catalyst archive un4seen publish. `BASS_DOWNLOAD_CACHE` moves the archive cache (default `artifacts/downloads/`). Needs `curl`, `unzip` and either `jq` or `python3`.

## `bass-manifest.json`

Records the expected BASS version, the base URL, and per platform the archives to fetch with their SHA-256 and the libraries to extract.

un4seen's URLs are unversioned (`bass24.zip` is always the latest build), so the hashes are a change detector, not a security boundary. The script **warns on mismatch and continues** rather than failing: hard-failing would turn every open PR red the day upstream publishes. A mismatching *cached* archive is re-downloaded once before warning, so a stale cache is not mistaken for upstream drift. `.github/workflows/natives-drift.yml` re-hashes upstream weekly and opens a PR with the new hashes; check the new build (`bass.txt` inside the archive), update `bassVersion` and `verifiedOn`, and refresh every platform in the same commit.

## `pack-release-artefacts.sh`

```
eng/pack-release-artefacts.sh <version> [output-dir]
```

Collects the standard build and the internal-DllImport build (both must already exist) into `ManagedBass-<version>-dotnet.zip` and `ManagedBass-<version>-unity-ios.zip` under `artifacts/release/` by default. Run by `package-and-release.yml` on each published release.

## `mutation-test.sh`

Applies each defect in its built-in table (`--list` prints it) to the tree in turn, expects the guard that claims to catch it to go red, and restores the tree. Run it after touching any Tier 0 or Tier 1 guard.
