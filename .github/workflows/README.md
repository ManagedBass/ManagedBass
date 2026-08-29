# Workflows

| Workflow | Jobs | Trigger | Gate |
|---|---|---|---|
| `build-test.yml` | `metadata` (Tier 0, ubuntu), `functional (ubuntu-latest)`, `functional (windows-latest)`, `functional (macos-latest)` (Tier 1) | push, pull request | Required. |
| `ios-simulator.yml` | `ios-simulator` (Tier 2, macos) | push to master and pull requests touching `src/**`, the harness, `eng/**` or the workflow; manual | Advisory (`continue-on-error: true`) until it has a track record. |
| `natives-drift.yml` | `drift` | weekly schedule, manual | Opens a PR when `eng/bass-manifest.json` no longer matches upstream. |
| `package-and-release.yml` | `build-test`, `build-and-deploy` | published release | Packs, attaches the two release zips, pushes to NuGet. |

In `metadata`, the internal-DllImport build and the pack step run **before** the metadata tests. That ordering is what makes their "output absent, skip" branches safe; reordering turns the skip into a silent pass. `CiWorkflowTests` in Tier 0 checks the internal-DllImport ordering.

Branch-protection configuration is in `docs/required-status-checks.md`. Validate YAML locally with `ruby -ryaml -e 'YAML.load_file(ARGV[0])' <file>`; PyYAML is not installed on a typical Mac.
