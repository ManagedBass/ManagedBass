# Workflows

| Workflow | Jobs | Trigger | Gate |
|---|---|---|---|
| `build-test.yml` | `metadata` (Tier 0, ubuntu), `functional (ubuntu-latest)`, `functional (windows-latest)`, `functional (macos-latest)` (Tier 1) | push, pull request | Required. |
| `ios-simulator.yml` | `ios-simulator` (Tier 2, macos) | push to master and pull requests touching `src/**`, the harness, `eng/**` or the workflow; manual | Advisory (`continue-on-error: true`) until it has a track record. |
| `package-and-release.yml` | `build-test`, `build-and-deploy` | published release | Packs, attaches the two release zips, pushes to NuGet. |

In `metadata`, the internal-DllImport build and the pack step run **before** the metadata tests. That ordering is what makes their "output absent, skip" branches safe; reordering turns the skip into a silent pass. `CiWorkflowTests` in Tier 0 checks the internal-DllImport ordering.

The BASS natives are not fetched by a workflow step: the projects that need them fetch them during their own build (see `eng/README.md`), so CI runs the same path a contributor does. `metadata` sets `FetchBassNatives: false` because Tier 0 never loads a native.

Make `metadata` and the three `functional (<os>)` checks required on master; `ios-simulator` stays advisory until it has run green for a while. Validate YAML locally with `ruby -ryaml -e 'YAML.load_file(ARGV[0])' <file>`; PyYAML is not installed on a typical Mac.
