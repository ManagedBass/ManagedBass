#!/usr/bin/env bash
#
# Mutation-tests the Tier 0 / Tier 1 guards: a guard you have not watched fail is not a guard.
#
# One mutation at a time: back the file(s) up with cp, apply a deliberate defect, run ONLY the guard
# that claims to catch it, require that guard to report at least one failed test, then restore the
# backup. Never git checkout - the tree may hold uncommitted work. Mutations that change what the
# built assemblies contain rebuild the affected project both ways, so bin/ ends where it started.
#
# Usage:
#   eng/mutation-test.sh              run every mutation
#   eng/mutation-test.sh --list       list mutations and stop
#   eng/mutation-test.sh --only <n>   run one mutation (repeatable)
#
# Prerequisites: dotnet build src/ManagedBass.sln -c Release, the internal-DllImport build
# (dotnet build src/ManagedBass.Shipping.slnf -c Release -p:BassInternalDllImport=true) and, for the Tier 1
# mutations, tests/ManagedBass.Tests built (which fetches the natives on its own).
set -uo pipefail

ScriptDir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
RepoRoot="$(cd -- "$ScriptDir/.." && pwd)"

MetadataTests="tests/ManagedBass.Metadata.Tests/ManagedBass.Metadata.Tests.csproj"
FunctionalTests="tests/ManagedBass.Tests/ManagedBass.Tests.csproj"
BassProject="src/Bass/Portable/Bass.csproj"
MixProject="src/AddOns/BassMix/BassMix.csproj"
MixSource="src/AddOns/BassMix/BassMix.cs"
Props="src/Directory.Build.props"
Solution="src/ManagedBass.sln"
Targets="src/Bass/Portable/build/ManagedBass.targets"
Initialiser="src/Bass/Portable/build/ManagedBassNativeResolverInitialiser.cs"
Helper="src/Bass/Portable/CrossPlatformHelper.cs"
Resolver="src/Bass/Portable/NativeLibraryResolver.cs"
AacSource="src/AddOns/BassEnc/Portable/PInvoke/BassEnc_Aac.cs"
BuildTestWorkflow=".github/workflows/build-test.yml"
IosWorkflow=".github/workflows/ios-simulator.yml"
MixInternalDllImportDll="src/AddOns/BassMix/bin/internaldllimport/Release/netstandard2.0/ManagedBass.Mix.dll"
FunctionalOutput="tests/ManagedBass.Tests/bin/Release/net10.0"

# NOTE: split so a repo-wide grep for the old symbol stays clean; the scanner does the same.
OldSymbol="__STATIC_""LINKING__"

# Mutation registry: "<name>@@<function>". Order is the dossier's table, then the extras.
Mutations=(
    "old two-underscore STATIC symbol written into a src .cs file -> conditional-symbol scan@@mutation01"
    "__IOS__ in a non-allowlisted .cs -> conditional-symbol scan@@mutation02"
    "BassEnc_Aac.cs loses its __IOS__ -> allowlist self-expiry test@@mutation03"
    "<Platforms>AnyCPU;iOS</Platforms> back in a csproj -> platform-scaffolding scan@@mutation04"
    "a |iOS line back in src/ManagedBass.sln -> platform-scaffolding scan@@mutation05"
    "DllImport named __Internal in the normal build (BassMix #else arm, rebuilt) -> no-static-linking test@@mutation06"
    "silently changed DllName (bassmix -> bass_mix in #else arm, rebuilt) -> DllImport manifest test@@mutation07"
    "IsIOS true on every platform (rebuilt Bass + Tier 1) -> Tier 1 platform predicates@@mutation08"
    "buildTransitive Pack lines dropped from Bass.csproj -> packaging guard@@mutation09"
    "targets file stops contributing the <Compile Include> -> packaging guard@@mutation10"
    "initialiser stops calling RegisterAll() -> packaging guard@@mutation11"
    "NativeLibraryResolver made internal (rebuilt Bass) -> public-API guard@@mutation12"
    "unguarded DllName (#if/#else/#endif removed around BassMix site) -> internal-DllImport symmetry guard@@mutation13"
    "DefineConstants assigned (clobbered) in Directory.Build.props -> clobber detector@@mutation14"
    "internal-DllImport subset missing (one bin/internaldllimport dll removed) -> three-way rule hard-fails@@mutation15"
    "build-test.yml internal-DllImport step moved after the metadata test step -> CiWorkflowTests@@mutation16"
    "ios-simulator.yml 'exit 1' on missing PASS replaced by 'true' -> CiWorkflowTests@@mutation17"
    "Tier 1: libbass removed from the test output -> fixture fails, never skips@@mutation18"
    "targets: _ManagedBassApplePlatform platform decision yields 'false' -> gating guard@@mutation19"
    "targets: ItemGroup Condition compares to 'false' -> gating guard@@mutation20"
    "targets: ItemGroup Condition removed (contributes on desktop too) -> gating guard@@mutation21"
    "targets: BeforeTargets=\"AfterCompile\" -> ordering guard@@mutation22"
    "build-test.yml Pack step deleted (package-shape tests would skip) -> CiWorkflowTests@@mutation23"
    "build-test.yml 'if: false' on the internal-DllImport step -> CiWorkflowTests@@mutation24"
)

BackupDir=""
BackedUp=()          # relative paths currently backed up (restored on cleanup)
RestoreBuild=()      # build commands to re-run after restoring, so bin/ matches the tree again
LastCommand=""
LastLog=""
ResultNumbers=()
ResultVerdicts=()
ResultNames=()

usage() { sed -n '2,/^set -uo/p' "${BASH_SOURCE[0]}" | grep '^#' | sed 's/^# \{0,1\}//'; }

log() { printf '%s\n' "$*"; }

heading() { printf '\n=== [%s] %s\n' "$1" "$2"; }

listMutations() {
    local i=1 entry
    for entry in "${Mutations[@]}"; do
        printf '%2d  %s\n' "$i" "${entry%%@@*}"
        i=$((i + 1))
    done
}

# ---- backup / restore -----------------------------------------------------------------------

backup() {
    local relative="$1"
    [ -f "$RepoRoot/$relative" ] || { log "error: cannot back up missing file $relative"; return 1; }
    mkdir -p "$BackupDir/$(dirname "$relative")"
    cp -p "$RepoRoot/$relative" "$BackupDir/$relative"
    BackedUp+=("$relative")
}

# Backs the file up and deletes it - "moved aside" mutations.
remove() {
    backup "$1" || return 1
    rm -f "$RepoRoot/$1"
}

restoreAll() {
    local relative
    for relative in ${BackedUp[@]+"${BackedUp[@]}"}; do
        # NOTE: no -p: a restored mtime older than the mutated build's outputs makes the restore
        # build an incremental no-op, and bin/ silently keeps the mutation.
        cp "$BackupDir/$relative" "$RepoRoot/$relative"
    done
    BackedUp=()
}

runRestoreBuilds() {
    local command
    for command in ${RestoreBuild[@]+"${RestoreBuild[@]}"}; do
        log "restore build: $command"
        (cd "$RepoRoot" && eval "$command" >"$BackupDir/restore-build.log" 2>&1) \
            || { log "error: restore build failed, see $BackupDir/restore-build.log"; tail -n 20 "$BackupDir/restore-build.log"; return 1; }
    done
    RestoreBuild=()
}

cleanup() {
    local code=$?
    if [ "${#BackedUp[@]}" -gt 0 ]; then
        log "interrupted: restoring ${#BackedUp[@]} backed-up file(s)"
        restoreAll
        [ "${#RestoreBuild[@]}" -gt 0 ] && runRestoreBuilds
    fi
    [ -n "$BackupDir" ] && [ "$code" -eq 0 ] && rm -rf "$BackupDir"
    exit "$code"
}
trap cleanup EXIT
trap 'exit 130' INT TERM

# ---- build / test helpers -------------------------------------------------------------------

build() {
    local command="dotnet build $1 -c Release ${2:-}"
    log "build: $command"
    (cd "$RepoRoot" && eval "$command" >"$BackupDir/mutation-build.log" 2>&1) \
        || { log "error: build failed, see $BackupDir/mutation-build.log"; grep -E 'error [A-Z]+[0-9]+' "$BackupDir/mutation-build.log" | head -n 5; return 1; }
}

# Runs the guard; success (for us) is at least one FAILED test - a non-zero exit alone could be a
# crashed runner, and a skip is exactly the silence the dossier warns about.
runGuard() {
    local project="$1"; shift
    LastCommand="dotnet test $project -c Release --no-build $*"
    LastLog="$BackupDir/guard.log"
    log "guard: $LastCommand"
    (cd "$RepoRoot" && dotnet test "$project" -c Release --no-build "$@" >"$LastLog" 2>&1)
    local exit=$?
    local total failed skipped
    total=$(sed -n 's/^ *total: *\([0-9][0-9]*\).*/\1/p' "$LastLog" | tail -n 1)
    failed=$(sed -n 's/^ *failed: *\([0-9][0-9]*\).*/\1/p' "$LastLog" | tail -n 1)
    skipped=$(sed -n 's/^ *skipped: *\([0-9][0-9]*\).*/\1/p' "$LastLog" | tail -n 1)
    log "guard exit=$exit total=${total:-?} failed=${failed:-?} skipped=${skipped:-?}"
    # A filter that matches nothing is not a green guard; it is a broken mutation test.
    { [ -n "$total" ] && [ "$total" -gt 0 ]; } || return 3
    [ -n "$failed" ] && [ "$failed" -gt 0 ]
}

firstFailure() {
    # MTP prints "failed <Class.Method> (<ms>)" followed by the assertion message.
    local line
    line=$(grep -m 1 -E '^failed ' "$LastLog" | sed 's/[[:space:]]*$//')
    # Skip the "from <dll>" line MTP prints before the assertion message.
    local message
    message=$(awk '/^failed / { hit = 1; next } hit && !/^[[:space:]]*from / && NF { sub(/^[[:space:]]+/, ""); print; exit }' "$LastLog")
    printf '%s :: %s' "${line:-<no failed line>}" "${message:-<no message>}"
}

# ---- mutations ------------------------------------------------------------------------------
# Each: back up, mutate, (rebuild), run guard. Return the guard's verdict.

mutation01() {
    backup "$MixSource" || return 2
    printf '\n#if %s\n#endif\n' "$OldSymbol" >>"$RepoRoot/$MixSource"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.ConditionalSymbolTests.SourceTree_OnScan_HasNoBannedSymbols_WhenAllowlistApplied
}

mutation02() {
    backup "$MixSource" || return 2
    printf '\n#if __IOS__\n#endif\n' >>"$RepoRoot/$MixSource"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.ConditionalSymbolTests.SourceTree_OnScan_HasNoBannedSymbols_WhenAllowlistApplied
}

mutation03() {
    backup "$AacSource" || return 2
    perl -pi -e 's/^#if __IOS__\s*$/#if false/' "$RepoRoot/$AacSource"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.ConditionalSymbolTests.AllowlistedFile_OnScan_StillContainsIosSymbol_WhenExemptionIsLive
}

mutation04() {
    backup "$MixProject" || return 2
    perl -0pi -e 's{(<PropertyGroup>\s*\n)}{$1    <Platforms>AnyCPU;iOS</Platforms>\n}' "$RepoRoot/$MixProject"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.PlatformScaffoldingTests.Projects_OnScan_NameNoOperatingSystemPlatform_WhenChecked
}

mutation05() {
    backup "$Solution" || return 2
    perl -pi -e 'print "\t\tDebug|iOS = Debug|iOS\n" if /^\t\tRelease\|Any CPU = Release\|Any CPU/' "$RepoRoot/$Solution"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.PlatformScaffoldingTests.Solution_OnScan_HasNoOperatingSystemConfiguration_WhenChecked
}

mutation06() {
    backup "$MixSource" || return 2
    RestoreBuild+=("dotnet build $MixProject -c Release")
    perl -0pi -e 's/(#else\s*\n\s*const string DllName = )"bassmix";/$1"__Internal";/' "$RepoRoot/$MixSource"
    grep -q 'DllName = "__Internal";' "$RepoRoot/$MixSource" || return 2
    build "$MixProject" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NoStaticLinkingTests.StandardBuild_OnScan_ImportsNoInternalDllImportModule_WhenFrameworkBuilt
}

mutation07() {
    backup "$MixSource" || return 2
    RestoreBuild+=("dotnet build $MixProject -c Release")
    perl -0pi -e 's/(#else\s*\n\s*const string DllName = )"bassmix";/$1"bass_mix";/' "$RepoRoot/$MixSource"
    grep -q 'DllName = "bass_mix";' "$RepoRoot/$MixSource" || return 2
    build "$MixProject" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.DllImportManifestTests.BuiltAssemblies_OnCompare_MatchManifest_WhenFrameworkBuilt
}

mutation08() {
    backup "$Helper" || return 2
    # The Tier 1 project references Bass.csproj, so building it rebuilds ManagedBass.dll (net10.0) too.
    RestoreBuild+=("dotnet build $FunctionalTests -c Release")
    perl -pi -e 's/^(\s*)OperatingSystem\.IsIOS\(\) && !OperatingSystem\.IsMacCatalyst\(\);/$1true;/' "$RepoRoot/$Helper"
    grep -q '^        true;$' "$RepoRoot/$Helper" || return 2
    build "$FunctionalTests" || return 2
    runGuard "$FunctionalTests" --filter-class ManagedBass.Tests.PlatformPredicateTests
}

mutation09() {
    backup "$BassProject" || return 2
    perl -ni -e 'print unless /PackagePath="buildTransitive\/"/' "$RepoRoot/$BassProject"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.PackageShapeTests.CoreProject_OnInspect_PacksBuildTransitivePair_WhenChecked
}

mutation10() {
    backup "$Targets" || return 2
    perl -0pi -e 's{\s*<Compile Include="[^"]*ManagedBassNativeResolverInitialiser\.cs"[^>]*/>}{}' "$RepoRoot/$Targets"
    grep -q '<Compile Include' "$RepoRoot/$Targets" && return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.Targets_OnDescribe_ContributesInitialiserInsideTarget_WhenChecked
}

mutation11() {
    backup "$Initialiser" || return 2
    perl -pi -e 's/=> global::ManagedBass\.NativeLibraryResolver\.RegisterAll\(\);/{ }/' "$RepoRoot/$Initialiser"
    grep -q 'RegisterAll' "$RepoRoot/$Initialiser" && return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.Initialiser_OnDescribe_RegistersResolverFromModuleInitializer_WhenChecked
}

mutation12() {
    backup "$Resolver" || return 2
    RestoreBuild+=("dotnet build $BassProject -c Release")
    perl -pi -e 's/^public static class NativeLibraryResolver/internal static class NativeLibraryResolver/' "$RepoRoot/$Resolver"
    grep -q '^internal static class NativeLibraryResolver' "$RepoRoot/$Resolver" || return 2
    build "$BassProject" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.CoreAssembly_OnInspect_ExposesPublicResolver_WhenFrameworkBuilt
}

mutation13() {
    backup "$MixSource" || return 2
    perl -0pi -e 's/#if MANAGEDBASS_INTERNAL_DLLIMPORT\s*\n\s*const string DllName = "__Internal";\s*\n#else\s*\n(\s*const string DllName = "bassmix";)\s*\n#endif/$1/' "$RepoRoot/$MixSource"
    grep -q 'MANAGEDBASS_INTERNAL_DLLIMPORT' "$RepoRoot/$MixSource" && return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.InternalDllImportTests.Sources_OnScan_GuardEveryDllName_WhenChecked
}

mutation14() {
    backup "$Props" || return 2
    perl -pi -e 's{<DefineConstants>\$\(DefineConstants\);MANAGEDBASS_INTERNAL_DLLIMPORT</DefineConstants>}{<DefineConstants>MANAGEDBASS_INTERNAL_DLLIMPORT</DefineConstants>}' "$RepoRoot/$Props"
    grep -q '<DefineConstants>MANAGEDBASS_INTERNAL_DLLIMPORT</DefineConstants>' "$RepoRoot/$Props" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.ConditionalSymbolTests.BuildFiles_OnScan_NeverClobberDefineConstants_WhenAssigned
}

mutation15() {
    remove "$MixInternalDllImportDll" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.InternalDllImportTests.InternalDllImportBuild_OnInspect_ImportsOnlyInternalDllImportModule_WhenAllPresent
}

mutation16() {
    backup "$BuildTestWorkflow" || return 2
    # Lift the two-line internal-DllImport step out and drop it in front of the first "Upload test results".
    perl -0pi -e 's/^([ \t]*- name: Build \(internal DllImport[^\n]*\n[^\n]*\n)//m and ($b = $1); s/^([ \t]*- name: Upload test results)/$b$1/m' "$RepoRoot/$BuildTestWorkflow"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.CiWorkflowTests.BuildTestWorkflow_OnRead_BuildsInternalDllImportBeforeMetadataTests_WhenStepsNamed
}

mutation17() {
    backup "$IosWorkflow" || return 2
    perl -0pi -e 's/(- name: Evaluate the result.*?)(?=\n[ \t]*- name: )/ my $s = $1; $s =~ s{\bexit 1\b}{true}g; $s /se' "$RepoRoot/$IosWorkflow"
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.CiWorkflowTests.IosSimulatorWorkflow_OnRead_TreatsMissingPassAsFailure_WhenEvaluated
}

mutation18() {
    local native="" candidate
    for candidate in libbass.dylib libbass.so bass.dll; do
        [ -f "$RepoRoot/$FunctionalOutput/$candidate" ] && { native="$FunctionalOutput/$candidate"; break; }
    done
    [ -n "$native" ] || { log "error: no bass native in $FunctionalOutput (fetch natives and build Tier 1 first)"; return 2; }
    remove "$native" || return 2
    runGuard "$FunctionalTests" --filter-class ManagedBass.Tests.InitialisationTests
}

mutation19() {
    backup "$Targets" || return 2
    perl -pi -e 's{(<_ManagedBassApplePlatform Condition="[^"]*TargetPlatformIdentifier[^"]*">)true(</_ManagedBassApplePlatform>)}{$1false$2}' "$RepoRoot/$Targets"
    grep -q 'TargetPlatformIdentifier[^"]*">false</_ManagedBassApplePlatform>' "$RepoRoot/$Targets" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.Targets_OnDescribe_ContributesInitialiserInsideTarget_WhenChecked
}

mutation20() {
    backup "$Targets" || return 2
    perl -pi -e "s{(<ItemGroup Condition=\"'\\\$\\(ManagedBassRegisterNativeResolver\\)' == )'true'}{\$1'false'}" "$RepoRoot/$Targets"
    grep -q "<ItemGroup Condition=\"'\$(ManagedBassRegisterNativeResolver)' == 'false'" "$RepoRoot/$Targets" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.Targets_OnDescribe_ContributesInitialiserInsideTarget_WhenChecked
}

mutation21() {
    backup "$Targets" || return 2
    perl -pi -e 's{<ItemGroup Condition="[^"]*">}{<ItemGroup>}' "$RepoRoot/$Targets"
    grep -q '<ItemGroup>' "$RepoRoot/$Targets" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.Targets_OnDescribe_ContributesInitialiserInsideTarget_WhenChecked
}

mutation22() {
    backup "$Targets" || return 2
    perl -pi -e 's{BeforeTargets="BeforeCompile;CoreCompile"}{BeforeTargets="AfterCompile"}' "$RepoRoot/$Targets"
    grep -q 'BeforeTargets="AfterCompile"' "$RepoRoot/$Targets" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.NativeResolverWiringTests.Targets_OnDescribe_ContributesInitialiserInsideTarget_WhenChecked
}

mutation23() {
    backup "$BuildTestWorkflow" || return 2
    perl -0pi -e 's/^[ \t]*- name: Pack[^\n]*\n[^\n]*dotnet pack[^\n]*\n//m' "$RepoRoot/$BuildTestWorkflow"
    grep -q 'dotnet pack' "$RepoRoot/$BuildTestWorkflow" && return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.CiWorkflowTests.BuildTestWorkflow_OnRead_BuildsInternalDllImportBeforeMetadataTests_WhenStepsNamed
}

mutation24() {
    backup "$BuildTestWorkflow" || return 2
    perl -pi -e 's/^([ \t]*)- name: Build \(internal DllImport[^\n]*\n/$&$1  if: false\n/' "$RepoRoot/$BuildTestWorkflow"
    grep -q '^ *if: false$' "$RepoRoot/$BuildTestWorkflow" || return 2
    runGuard "$MetadataTests" --filter-method ManagedBass.Metadata.Tests.CiWorkflowTests.BuildTestWorkflow_OnRead_BuildsInternalDllImportBeforeMetadataTests_WhenStepsNamed
}

# ---- driver ---------------------------------------------------------------------------------

runMutation() {
    local n="$1" entry="${Mutations[$((n - 1))]}"
    local name="${entry%%@@*}" function="${entry##*@@}"
    heading "$n" "$name"

    "$function"
    local verdict=$?
    local detail

    case "$verdict" in
        0) detail="PASS: guard went red - $(firstFailure)";;
        2) detail="ERROR: mutation could not be applied or built";;
        3) detail="ERROR: the filter matched zero tests - $LastCommand";;
        *) detail="FAIL: guard stayed green (failed=0) - $LastCommand";;
    esac

    restoreAll
    runRestoreBuilds || detail="$detail [restore build FAILED]"

    log "$detail"
    ResultNumbers+=("$n"); ResultVerdicts+=("${detail%%:*}"); ResultNames+=("$name")
    [ "$verdict" -eq 0 ]
}

main() {
    local selected=()
    while [ $# -gt 0 ]; do
        case "$1" in
            --list) listMutations; BackedUp=(); trap - EXIT; exit 0;;
            --only) [ -n "${2:-}" ] || { usage; exit 2; }; selected+=("$2"); shift;;
            -h|--help) usage; trap - EXIT; exit 0;;
            *) usage; exit 2;;
        esac
        shift
    done

    [ "${#selected[@]}" -eq 0 ] && selected=($(seq 1 "${#Mutations[@]}"))

    BackupDir="$(mktemp -d "${TMPDIR:-/tmp}/managedbass-mutation.XXXXXX")"
    log "repo: $RepoRoot"
    log "backups: $BackupDir"

    local n failures=0
    for n in ${selected[@]+"${selected[@]}"}; do
        if ! [ "$n" -ge 1 ] 2>/dev/null || [ "$n" -gt "${#Mutations[@]}" ]; then
            log "error: no mutation $n (1..${#Mutations[@]})"; exit 2
        fi
        runMutation "$n" || failures=$((failures + 1))
    done

    printf '\n%-4s %-6s %s\n' '#' 'result' 'mutation'
    local i
    for i in ${ResultNumbers[@]+"${!ResultNumbers[@]}"}; do
        printf "%-4s %-6s %s\n" "${ResultNumbers[$i]}" "${ResultVerdicts[$i]}" "${ResultNames[$i]}"
    done
    printf '\n%d mutation(s), %d not caught\n' "${#ResultNumbers[@]}" "$failures"

    [ "$failures" -eq 0 ]
}

main "$@"
