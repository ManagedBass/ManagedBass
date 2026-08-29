#!/usr/bin/env bash
# Downloads the BASS natives listed in eng/bass-manifest.json for one platform and lays the loadable
# binaries out under artifacts/natives/<platform>/. Natives are never committed.
#
# Usage: fetch-natives.sh <windows|linux|osx|ios|catalyst|android> [output-dir]
#   --refresh   ignore the download cache
#   --list      print the archives for <platform> and exit
# Env: BASS_ARCH (uname -m style, picks the windows/linux slice), BASS_DOWNLOAD_CACHE (default artifacts/downloads).
#
# Layouts observed in the 2.4.18.3 archives:
#   windows   bass.dll (x86), x64/bass.dll
#   linux     libs/{x86,x86_64,armhf,aarch64}/libbass.so
#   osx       libbass.dylib (universal)
#   android   libs/{armeabi-v7a,arm64-v8a,x86,x86_64}/libbass.so
#   ios       bass.xcframework/{ios-arm64_armv7_armv7s,ios-arm64_i386_x86_64-simulator}/bass.framework/bass
#   catalyst  bass24-catalyst.zip only; <name>.framework/Versions/A/<name> for every add-on
set -euo pipefail

ScriptDir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
RepoRoot="$(cd -- "$ScriptDir/.." && pwd)"
Manifest="$ScriptDir/bass-manifest.json"
KnownPlatforms="windows linux osx ios catalyst android"

die()  { printf 'fetch-natives: error: %s\n' "$1" >&2; exit 1; }
info() { printf 'fetch-natives: %s\n' "$1"; }

Platform=""; OutputDir=""; Refresh=0; ListOnly=0
while [ $# -gt 0 ]; do
    case "$1" in
        -h|--help) sed -n '2,16p' "$0" | sed 's/^# \{0,1\}//'; exit 0 ;;
        --refresh) Refresh=1 ;;
        --list)    ListOnly=1 ;;
        -*)        die "unknown option '$1'" ;;
        *) if [ -z "$Platform" ]; then Platform="$1"; elif [ -z "$OutputDir" ]; then OutputDir="$1"; else die "unexpected argument '$1'"; fi ;;
    esac
    shift
done

[ -n "$Platform" ] || die "usage: fetch-natives.sh <$(echo "$KnownPlatforms" | tr ' ' '|')> [output-dir]"
case " $KnownPlatforms " in *" $Platform "*) ;; *) die "unknown platform '$Platform' (expected one of: $KnownPlatforms)" ;; esac
[ -f "$Manifest" ] || die "manifest not found at $Manifest"

[ -n "$OutputDir" ] || OutputDir="$RepoRoot/artifacts/natives/$Platform"
CacheDir="${BASS_DOWNLOAD_CACHE:-$RepoRoot/artifacts/downloads}"
command -v curl >/dev/null 2>&1 || die "curl is required"

# --- manifest: jq preferred, python3 fallback (one of them exists on every GitHub runner) ---
if command -v jq >/dev/null 2>&1; then
    manifest_rows()   { jq -r --arg p "$Platform" '.platforms[$p] // [] | .[] | [.zip, .sha256, (.libs | join(","))] | @tsv' "$Manifest"; }
    manifest_scalar() { jq -r --arg k "$1" '.[$k] // ""' "$Manifest"; }
elif command -v python3 >/dev/null 2>&1; then
    manifest_rows() {
        BASS_MANIFEST="$Manifest" BASS_PLATFORM="$Platform" python3 -c '
import json, os
m = json.load(open(os.environ["BASS_MANIFEST"], encoding="utf-8"))
for e in m.get("platforms", {}).get(os.environ["BASS_PLATFORM"], []):
    print("\t".join((e["zip"], e["sha256"], ",".join(e["libs"]))))'
    }
    manifest_scalar() {
        BASS_MANIFEST="$Manifest" BASS_KEY="$1" python3 -c '
import json, os
print(json.load(open(os.environ["BASS_MANIFEST"], encoding="utf-8")).get(os.environ["BASS_KEY"], ""))'
    }
else
    die "jq or python3 is required to read $Manifest"
fi

BassVersion="$(manifest_scalar bassVersion)"
BaseUrl="$(manifest_scalar baseUrl)"
[ -n "$BaseUrl" ] || die "manifest has no baseUrl"
Rows="$(manifest_rows)"
[ -n "$Rows" ] || die "manifest lists no archives for '$Platform'"

if [ "$ListOnly" -eq 1 ]; then
    printf '%s (BASS %s)\n' "$Platform" "$BassVersion"
    printf '%s\n' "$Rows" | while IFS="$(printf '\t')" read -r zip _ libs; do printf '  %-24s %s\n' "$zip" "$libs"; done
    exit 0
fi

# --- host slice (windows/linux only; android extracts every ABI, Apple bundles are universal) ---
HostArch="${BASS_ARCH:-$(uname -m)}"
case "$HostArch" in
    arm64|aarch64)      LinuxSlice="aarch64"; WindowsSlice="x64" ;;
    x86_64|amd64)       LinuxSlice="x86_64";  WindowsSlice="x64" ;;
    i386|i686|x86)      LinuxSlice="x86";     WindowsSlice="x86" ;;
    armv7l|armv7|armhf) LinuxSlice="armhf";   WindowsSlice="x86" ;;
    *) die "unrecognised architecture '$HostArch'; set BASS_ARCH to arm64, x86_64, i686 or armv7l" ;;
esac
# NOTE: no arm64 Windows build exists upstream; x64 runs under emulation there.

# --- tools that differ per OS ---
if command -v shasum >/dev/null 2>&1; then
    sha256_of() { shasum -a 256 "$1" | cut -d' ' -f1; }
elif command -v sha256sum >/dev/null 2>&1; then
    sha256_of() { sha256sum "$1" | cut -d' ' -f1; }
else
    sha256_of() { printf ''; }
    info "no shasum/sha256sum on PATH; checksum verification disabled"
fi

# NOTE: GNU tar (Git Bash on Windows) cannot read zips; only bsdtar can, so it is the last resort.
if command -v unzip >/dev/null 2>&1; then
    extract() { unzip -oq "$1" -d "$2"; }
elif command -v 7z >/dev/null 2>&1; then
    extract() { 7z x -y -o"$2" "$1" >/dev/null; }
elif command -v python3 >/dev/null 2>&1; then
    extract() { python3 -m zipfile -e "$1" "$2"; }
elif command -v python >/dev/null 2>&1; then
    extract() { python -m zipfile -e "$1" "$2"; }
elif tar --version 2>/dev/null | grep -q bsdtar; then
    extract() { tar -xf "$1" -C "$2"; }
else
    die "unzip, 7z, python or bsdtar is required to extract archives"
fi

Mismatches=0
warn_mismatch() {
    Mismatches=$((Mismatches + 1))
    cat >&2 <<WARNING

  **********************************************************************
  *  WARNING: SHA-256 MISMATCH for $1
  *  expected $2
  *  actual   $3
  *  un4seen publish at unversioned URLs, so upstream has most likely
  *  shipped a build newer than the manifest's BASS $BassVersion.
  *  Continuing (never hard-fails). Verify the new build, then update
  *  the sha256 and bassVersion in eng/bass-manifest.json.
  **********************************************************************

WARNING
}

mkdir -p "$CacheDir"

download() {
    zip="$1"; expected="$2"; target="$CacheDir/$zip"; fetched=0
    # NOTE: a mismatching cached archive is more often a stale cache than upstream drift, so re-fetch once
    # before warning; otherwise a developer silently tests against last month's binaries.
    while :; do
        if [ "$Refresh" -eq 1 ] || [ ! -s "$target" ]; then
            info "downloading $zip"
            curl --fail --silent --show-error --location --connect-timeout 30 --max-time 900 --retry 3 --retry-delay 2 \
                 -o "$target.partial" "$BaseUrl/$zip" || { rm -f "$target.partial"; die "failed to download $BaseUrl/$zip"; }
            mv -f "$target.partial" "$target"
            fetched=1
        else
            info "using cached $zip"
        fi

        actual="$(sha256_of "$target")"
        if [ -z "$actual" ] || [ "$actual" = "$expected" ]; then
            [ -n "$actual" ] && info "sha256 ok for $zip"
            return 0
        fi
        if [ "$fetched" -eq 0 ]; then
            info "cached $zip does not match the manifest; re-downloading"
            rm -f "$target"
            continue
        fi
        warn_mismatch "$zip" "$expected" "$actual"
        return 0
    done
}

WorkDir="$(mktemp -d "${TMPDIR:-/tmp}/fetch-natives.XXXXXX")"
trap 'rm -rf "$WorkDir"' EXIT INT TERM

# Copies one library out of an extracted archive root; prints each placed path relative to $OutputDir.
place() {
    root="$1"; lib="$2"
    case "$Platform" in
        windows)
            if [ "$WindowsSlice" = "x64" ]; then copy_one "$root/x64/$lib.dll" "$lib.dll"; else copy_one "$root/$lib.dll" "$lib.dll"; fi ;;
        linux)    copy_one "$root/libs/$LinuxSlice/lib$lib.so" "lib$lib.so" ;;
        osx)      copy_one "$root/lib$lib.dylib" "lib$lib.dylib" ;;
        ios)      copy_one "$root/$lib.xcframework" "$lib.xcframework" ;;   # whole xcframework: device and simulator slices differ
        catalyst) copy_one "$root/$lib.framework" "$lib.framework" ;;
        android)
            for abiDir in "$root"/libs/*/; do
                abi="$(basename "$abiDir")"
                copy_one "$abiDir/lib$lib.so" "$abi/lib$lib.so"
            done ;;
    esac
}

copy_one() {
    src="$1"; rel="$2"; dest="$OutputDir/$rel"
    [ -e "$src" ] || die "${src#"$WorkDir/"} not found in archive; upstream may have changed its layout"
    mkdir -p "$(dirname "$dest")"
    rm -rf "$dest"
    cp -R "$src" "$dest"
    printf 'fetch-natives:   -> %s/%s\n' "$Platform" "$rel"
    Placed=$((Placed + 1))
}

mkdir -p "$OutputDir"
Placed=0
RowsFile="$WorkDir/rows.tsv"
printf '%s\n' "$Rows" > "$RowsFile"

# NOTE: read from a file, not a pipe, so the counters and any die happen in this shell.
while IFS="$(printf '\t')" read -r zip expected libs; do
    [ -n "$zip" ] || continue
    download "$zip" "$expected"
    root="$WorkDir/${zip%.zip}"
    mkdir -p "$root"
    extract "$CacheDir/$zip" "$root" || die "failed to extract $CacheDir/$zip"
    for lib in $(printf '%s' "$libs" | tr ',' ' '); do
        place "$root" "$lib"
    done
done < "$RowsFile"

info "placed $Placed binaries in $OutputDir"
[ "$Mismatches" -eq 0 ] || info "completed with $Mismatches checksum warning(s); see above"
