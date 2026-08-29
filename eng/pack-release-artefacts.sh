#!/usr/bin/env bash
#
# Collects the built assemblies into the two zips attached to each GitHub release:
#   ManagedBass-<version>-dotnet.zip     standard assemblies, one folder per TFM, with XML docs
#   ManagedBass-<version>-unity-ios.zip  internal-DllImport netstandard2.0 assemblies + README.txt
#
# Usage: pack-release-artefacts.sh <version> [output-dir]
#
# Both builds must already exist:
#   dotnet build src/ManagedBass.sln -c Release
#   dotnet build src/ManagedBass.Shipping.slnf -c Release -p:BassInternalDllImport=true
set -euo pipefail

ScriptDir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
RepoRoot="$(cd -- "$ScriptDir/.." && pwd)"
SrcDir="$RepoRoot/src"
SolutionFilter="$SrcDir/ManagedBass.Shipping.slnf"

# Must match src/Directory.Build.props.
Frameworks=(net45 netstandard2.0 net8.0 net9.0 net10.0)
InternalDllImportFramework=netstandard2.0
ExpectedProjectCount=33

Version="${1:-}"
OutputDir="${2:-$RepoRoot/artifacts/release}"

if [ -z "$Version" ]; then
    echo "usage: $(basename "$0") <version> [output-dir]" >&2
    exit 2
fi

[ -f "$SolutionFilter" ] || { echo "error: missing $SolutionFilter" >&2; exit 1; }

# Emits "<project dir relative to src>|<assembly name>" per shipping project. Collecting each
# project's OWN assembly by name is what de-duplicates: every output dir also holds copies of
# the assemblies it references.
readProjects() {
    # JSON escapes the backslashes, hence -s to squeeze the doubled separator.
    grep -o '"[^"]*\.csproj"' "$SolutionFilter" | tr -d '"' | tr -s '\\' '/' | while IFS= read -r relative; do
        local csproj="$SrcDir/$relative"
        [ -f "$csproj" ] || { echo "error: project in filter not on disk: $relative" >&2; exit 1; }
        local name
        name="$(sed -nE 's/.*<AssemblyName>[[:space:]]*([^<]+[^< ])[[:space:]]*<\/AssemblyName>.*/\1/p' "$csproj" | head -1)"
        [ -n "$name" ] || name="$(basename "$relative" .csproj)"
        printf '%s|%s\n' "$(dirname "$relative")" "$name"
    done
}

ProjectLines="$(readProjects)"
Projects=()
while IFS= read -r line; do
    [ -n "$line" ] && Projects+=("$line")
done <<< "$ProjectLines"

if [ "${#Projects[@]}" -ne "$ExpectedProjectCount" ]; then
    echo "error: expected $ExpectedProjectCount shipping projects, filter yields ${#Projects[@]}" >&2
    exit 1
fi

Staging="$(mktemp -d "${TMPDIR:-/tmp}/managedbass-release.XXXXXX")"
trap 'rm -rf "$Staging"' EXIT
DotnetStage="$Staging/dotnet"
UnityStage="$Staging/unity-ios"
mkdir -p "$DotnetStage" "$UnityStage"

# copyAssembly <source dir> <assembly name> <destination dir>
copyAssembly() {
    local dll="$1/$2.dll" xml="$1/$2.xml"
    [ -f "$dll" ] || { echo "error: missing ${dll#"$RepoRoot"/} - has the required build run?" >&2; exit 1; }
    cp "$dll" "$3/"
    if [ -f "$xml" ]; then cp "$xml" "$3/"; else echo "warning: no XML docs beside $2.dll" >&2; fi
}

# countDlls <dir>
countDlls() {
    local count=0
    while IFS= read -r -d '' _; do count=$((count + 1)); done < <(find "$1" -name '*.dll' -print0)
    echo "$count"
}

# requireCount <dir> <expected> <label>
requireCount() {
    local actual
    actual="$(countDlls "$1")"
    if [ "$actual" -ne "$2" ]; then
        echo "error: $3: expected $2 assemblies, found $actual" >&2
        exit 1
    fi
    echo "  $3: $actual assemblies"
}

for framework in "${Frameworks[@]}"; do
    mkdir -p "$DotnetStage/$framework"
    for project in "${Projects[@]}"; do
        copyAssembly "$SrcDir/${project%%|*}/bin/Release/$framework" "${project##*|}" "$DotnetStage/$framework"
    done
    requireCount "$DotnetStage/$framework" "$ExpectedProjectCount" "$framework"
done

for project in "${Projects[@]}"; do
    copyAssembly "$SrcDir/${project%%|*}/bin/internaldllimport/Release/$InternalDllImportFramework" "${project##*|}" "$UnityStage"
done
requireCount "$UnityStage" "$ExpectedProjectCount" "internal-DllImport $InternalDllImportFramework"

cat > "$UnityStage/README.txt" <<README
ManagedBass $Version - internal-DllImport build for Unity on iOS
=========================================================

WHAT THIS IS

The standard ManagedBass assemblies ($ExpectedProjectCount, netstandard2.0) recompiled with
MANAGEDBASS_INTERNAL_DLLIMPORT defined, so every DllImport names "__Internal": the native symbol is
resolved from the app executable itself (and anything linked into it) instead of by loading a library at run time.

This is a symbol lookup scope, not static linking. BASS has shipped no static iOS libraries
since 2.4.17; "__Internal" works with the dynamic bass.xcframework embedded in your app.

WHY UNITY ON iOS NEEDS IT

Unity builds iOS with IL2CPP, which binds P/Invoke at link time. There is no runtime library
load to intercept, so ManagedBass.NativeLibraryResolver (which handles .NET iOS apps) cannot
help. Unity's own documentation requires DllImport("__Internal") on iOS. IL2CPP remains the
only iOS backend after Unity's CoreCLR move, so this is permanent.

INSTALLING

1. Copy these DLLs into their own folder, e.g. Assets/Plugins/ManagedBass-iOS/.
2. Copy the standard netstandard2.0/ DLLs from ManagedBass-$Version-dotnet.zip into a
   separate folder, e.g. Assets/Plugins/ManagedBass/.
3. In the Plugin Inspector set the platform filters (select each DLL, then Apply):
       internal-DllImport DLLs  ->  iOS only
       standard DLLs    ->  Editor, Standalone, Android; iOS UNTICKED
   The two filters MUST be mutually exclusive. Both sets share an assembly identity, so any
   target with both enabled fails with a duplicate-assembly error.
4. Put bass.xcframework (and the xcframework of every add-on you use) in Assets/Plugins/iOS.
   In the generated Xcode project they must be "Embed & Sign", not merely linked.

ALTERNATIVE: VENDOR THE SOURCE

Copy the repository's src/ tree into Assets/ and add MANAGEDBASS_INTERNAL_DLLIMPORT to
Player Settings > iOS > Other Settings > Scripting Define Symbols. Those symbols are per
build target, so Unity compiles the right variant per platform and no plugin filtering is
needed.

EVERYWHERE ELSE, USE THE STANDARD BUILD

Desktop, Android, the Unity Editor and every non-Unity .NET app want the standard
assemblies. The "__Internal" assemblies in this zip only work inside an app that has the BASS
frameworks linked in, so the symbols are reachable from the executable itself.

https://github.com/ManagedBass/ManagedBass
README

mkdir -p "$OutputDir"
OutputDir="$(cd -- "$OutputDir" && pwd)"  # zip runs from inside the staging dir
DotnetZip="$OutputDir/ManagedBass-$Version-dotnet.zip"
UnityZip="$OutputDir/ManagedBass-$Version-unity-ios.zip"
rm -f "$DotnetZip" "$UnityZip"

(cd "$DotnetStage" && zip -q -r -X "$DotnetZip" .)
(cd "$UnityStage" && zip -q -r -X "$UnityZip" .)

echo "Wrote $DotnetZip"
echo "Wrote $UnityZip"
