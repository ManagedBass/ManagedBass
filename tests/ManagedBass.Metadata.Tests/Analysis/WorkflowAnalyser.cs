using System.Text.RegularExpressions;

namespace ManagedBass.Metadata.Tests.Analysis;

/// <param name="Job">The job the step belongs to, or empty when no <c>jobs:</c> block encloses it.</param>
/// <param name="IsConditional">Whether the step carries an <c>if:</c> key, so it can be skipped at run time.</param>
public sealed record WorkflowStep(string Name, string Body, string Job, int Index, bool IsConditional);

public sealed record OutcomeHandling(bool ChecksForPass, bool FailsExplicitly);

/// <summary>The three steps whose placement makes the Tier 0 skips safe: two producers and the test that consumes them.</summary>
public sealed record MetadataPipeline(WorkflowStep? InternalDllImportBuild, WorkflowStep? Pack, WorkflowStep? MetadataTest)
{
    public bool AllPresent => InternalDllImportBuild is not null && Pack is not null && MetadataTest is not null;

    public bool InSameJob => AllPresent && InternalDllImportBuild!.Job == MetadataTest!.Job && Pack!.Job == MetadataTest.Job;

    /// <summary>Both producers precede the consumer; their order relative to each other does not matter.</summary>
    public bool ProducersBeforeTest => AllPresent && InternalDllImportBuild!.Index < MetadataTest!.Index && Pack!.Index < MetadataTest.Index;

    public IReadOnlyList<string> ConditionalSteps =>
        new[] { InternalDllImportBuild, Pack, MetadataTest }.OfType<WorkflowStep>().Where(s => s.IsConditional).Select(s => s.Name).ToArray();
}

/// <summary>Line-level reading of GitHub workflow files. Whole-line comments are dropped so prose cannot stand in for a command.</summary>
public static partial class WorkflowAnalyser
{
    public const string WorkflowDirectory = ".github/workflows";
    public const string BuildTestWorkflow = "build-test.yml";
    public const string PackageAndReleaseWorkflow = "package-and-release.yml";
    public const string IosSimulatorWorkflow = "ios-simulator.yml";
    public const string InternalDllImportStepName = "Build (internal DllImport, Unity/iOS variant)";
    public const string MetadataTestStepName = "Test (metadata and package shape)";
    public const string PackCommand = "dotnet pack";
    public const string PackOutputOption = "-o artifacts/packages";
    public const string PassMarker = "outcome=PASS";

    public static string StripCommentLines(string content) =>
        string.Join('\n', content.Replace("\r\n", "\n").Split('\n').Where(l => !l.TrimStart().StartsWith('#')));

    public static IReadOnlyList<WorkflowStep> Steps(string content)
    {
        var lines = StripCommentLines(content).Split('\n');
        var jobsStart = Array.FindIndex(lines, l => JobsPattern().IsMatch(l));
        var jobStarts = lines
            .Select((l, i) => (match: JobNamePattern().Match(l), i))
            .Where(j => jobsStart >= 0 && j.i > jobsStart && j.match.Success)
            .ToArray();
        var starts = lines.Select((l, i) => (match: StepNamePattern().Match(l), i)).Where(s => s.match.Success).ToArray();

        return starts
            .Select((s, n) =>
            {
                var nextStep = n + 1 < starts.Length ? starts[n + 1].i : lines.Length;
                var nextJob = jobStarts.Select(j => j.i).Where(i => i > s.i).DefaultIfEmpty(lines.Length).First();
                var body = lines[s.i..Math.Min(nextStep, nextJob)];
                var job = jobStarts.Where(j => j.i < s.i).Select(j => j.match.Groups[1].Value).LastOrDefault() ?? string.Empty;
                return new WorkflowStep(Unquote(s.match.Groups[1].Value), string.Join('\n', body), job, n, body.Any(l => IfKeyPattern().IsMatch(l)));
            })
            .ToArray();
    }

    public static int IndexOfStep(IReadOnlyList<WorkflowStep> steps, string name) =>
        steps.ToList().FindIndex(s => string.Equals(s.Name, name, StringComparison.Ordinal));

    /// <summary>Anchored on the metadata test step: producers are looked for in its job first, then anywhere.</summary>
    public static MetadataPipeline DescribeMetadataPipeline(string content)
    {
        var steps = Steps(content);
        var test = steps.FirstOrDefault(s => s.Name == MetadataTestStepName);

        return new(
            Prefer(steps.Where(s => s.Name == InternalDllImportStepName && s.Body.Contains(BuildOutputs.InternalDllImportBuildCommand, StringComparison.Ordinal)), test),
            Prefer(steps.Where(s => s.Body.Contains(PackCommand, StringComparison.Ordinal) && s.Body.Contains(PackOutputOption, StringComparison.Ordinal)), test),
            test);
    }

    /// <summary>Whether the step that looks for the pass marker also exits non-zero after its last look.</summary>
    public static OutcomeHandling DescribeOutcomeHandling(string content)
    {
        var step = Steps(content).FirstOrDefault(s => s.Body.Contains(PassMarker, StringComparison.Ordinal));

        if (step is null)
            return new(false, false);

        // An 'exit 1' before the check is some other failure; only one after the marker's last mention answers silence.
        var afterLastCheck = step.Body[(step.Body.LastIndexOf(PassMarker, StringComparison.Ordinal) + PassMarker.Length)..];
        return new(true, ExitNonZeroPattern().IsMatch(afterLastCheck));
    }

    private static WorkflowStep? Prefer(IEnumerable<WorkflowStep> candidates, WorkflowStep? anchor)
    {
        var all = candidates.ToArray();
        return all.FirstOrDefault(s => anchor is not null && s.Job == anchor.Job) ?? all.FirstOrDefault();
    }

    private static string Unquote(string value)
    {
        var trimmed = value.Trim();
        return trimmed.Length >= 2 && (trimmed[0] == '"' || trimmed[0] == '\'') && trimmed[^1] == trimmed[0] ? trimmed[1..^1] : trimmed;
    }

    [GeneratedRegex(@"^\s*-\s*name:\s*(.+?)\s*$")]
    private static partial Regex StepNamePattern();

    [GeneratedRegex(@"^jobs:\s*$")]
    private static partial Regex JobsPattern();

    [GeneratedRegex(@"^  ([A-Za-z0-9_-]+):\s*$")]
    private static partial Regex JobNamePattern();

    [GeneratedRegex(@"^\s*if\s*:")]
    private static partial Regex IfKeyPattern();

    [GeneratedRegex(@"\bexit\s+[1-9]\d*\b")]
    private static partial Regex ExitNonZeroPattern();
}
