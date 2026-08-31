using System.Text.RegularExpressions;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record ScaffoldingOffence(string RelativePath, int LineNumber, string Token, string Detail, string Line)
{
    public override string ToString() => $"{RelativePath}:{LineNumber}: {Detail} '{Token}'   |   {Line}";
}

/// <summary>Detects operating systems used as MSBuild Platform values. Platform means CPU architecture.</summary>
public static partial class PlatformScaffoldingScanner
{
    public static readonly IReadOnlySet<string> OperatingSystemNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "iOS", "iPhone", "iPhoneSimulator", "tvOS", "watchOS", "macOS", "OSX", "MacCatalyst", "Mac Catalyst",
        "Xamarin.iOS", "Xamarin.Mac", "Android", "MonoAndroid", "Linux", "Windows", "UWP", "UAP", "Browser", "WASM"
    };

    public static IReadOnlyList<ScaffoldingOffence> ScanProjectFile(string relativePath, string content)
    {
        var offences = new List<ScaffoldingOffence>();

        foreach (var (line, number) in Lines(content))
        {
            foreach (Match match in PlatformElementPattern().Matches(line))
                offences.AddRange(Split(match.Groups[2].Value)
                    .Where(OperatingSystemNames.Contains)
                    .Select(t => new ScaffoldingOffence(relativePath, number, t, $"<{match.Groups[1].Value}> names an operating system", line.Trim())));

            if (line.Contains("$(Platform)", StringComparison.OrdinalIgnoreCase))
                offences.AddRange(QuotedLiteralPattern().Matches(line)
                    .SelectMany(m => Split(m.Groups[1].Value))
                    .Where(OperatingSystemNames.Contains)
                    .Select(t => new ScaffoldingOffence(relativePath, number, t, "$(Platform) condition names an operating system", line.Trim())));
        }

        return Deduplicate(offences);
    }

    /// <summary>Solution configurations are <c>Debug|iOS</c>; the platform is whatever follows the pipe.</summary>
    public static IReadOnlyList<ScaffoldingOffence> ScanSolutionFile(string relativePath, string content) =>
        Deduplicate(Lines(content)
            .SelectMany(l => SolutionPlatformPattern().Matches(l.line)
                .Select(m => m.Groups[1].Value.Trim())
                .Where(OperatingSystemNames.Contains)
                .Select(t => new ScaffoldingOffence(relativePath, l.number, t, "solution configuration names an operating system", l.line.Trim()))));

    private static IReadOnlyList<ScaffoldingOffence> Deduplicate(IEnumerable<ScaffoldingOffence> offences) =>
        offences.DistinctBy(o => (o.RelativePath, o.LineNumber, o.Token, o.Detail))
            .OrderBy(o => o.RelativePath, StringComparer.Ordinal).ThenBy(o => o.LineNumber)
            .ToArray();

    private static IEnumerable<(string line, int number)> Lines(string content) =>
        content.Replace("\r\n", "\n").Split('\n').Select((line, i) => (line, i + 1));

    private static IEnumerable<string> Split(string value) =>
        value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    [GeneratedRegex(@"<(Platforms|Platform)>([^<]*)</\1>", RegexOptions.IgnoreCase)]
    private static partial Regex PlatformElementPattern();

    [GeneratedRegex(@"'([^']*)'")]
    private static partial Regex QuotedLiteralPattern();

    [GeneratedRegex(@"\|([A-Za-z0-9_ +\-]+)")]
    private static partial Regex SolutionPlatformPattern();
}
