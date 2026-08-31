using System.Text.Json;
using System.Text.RegularExpressions;

namespace ManagedBass.Metadata.Tests.Analysis;

/// <summary>Project lists from a <c>.sln</c> and a <c>.slnf</c>, forward-slashed and sorted so they compare directly.</summary>
public static partial class SolutionFilterReader
{
    public static IReadOnlyList<string> ReadSolutionProjects(string solutionContent) =>
        ProjectLinePattern().Matches(solutionContent)
            .Select(m => Normalise(m.Groups[1].Value))
            .Where(p => p.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToArray();

    public static IReadOnlyList<string> ReadFilterProjects(string filterJson)
    {
        using var document = JsonDocument.Parse(filterJson);

        return document.RootElement.GetProperty("solution").GetProperty("projects")
            .EnumerateArray()
            .Select(p => Normalise(p.GetString() ?? string.Empty))
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToArray();
    }

    public static string ReadFilterSolutionPath(string filterJson)
    {
        using var document = JsonDocument.Parse(filterJson);
        return Normalise(document.RootElement.GetProperty("solution").GetProperty("path").GetString() ?? string.Empty);
    }

    private static string Normalise(string path) => path.Trim().Replace('\\', '/');

    [GeneratedRegex("""^Project\("\{[^}]*\}"\)\s*=\s*"[^"]*"\s*,\s*"([^"]*)"\s*,""", RegexOptions.Multiline)]
    private static partial Regex ProjectLinePattern();
}
