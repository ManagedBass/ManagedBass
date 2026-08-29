using System.Text.RegularExpressions;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record SymbolOccurrence(string RelativePath, int LineNumber, string Symbol, string Line)
{
    public override string ToString() => $"{RelativePath}:{LineNumber}: {Symbol}   |   {Line}";
}

public sealed record DefineConstantsOffence(string RelativePath, int LineNumber, string Line)
{
    public override string ToString() => $"{RelativePath}:{LineNumber}: {Line}";
}

/// <summary>Finds Xamarin-era conditional symbols and DefineConstants misuse. Comments are deliberately not stripped.</summary>
public static partial class ConditionalSymbolScanner
{
    public const string InternalDllImportSymbol = "MANAGEDBASS_INTERNAL_DLLIMPORT";

    // NOTE: the old symbol is split so a repo-wide grep for it stays clean.
    public static readonly IReadOnlyList<string> BannedSymbols =
    [
        "__STATIC_" + "LINKING__",
        "__MACOS__", "__MACCATALYST__", "__TVOS__", "__ANDROID__", "__UNIFIED__", "__MOBILE__", "__WATCHOS__"
    ];

    // NOTE: BassEnc_Aac.cs is PR #140's territory; the companion self-expiry test keeps this honest.
    public static readonly IReadOnlyDictionary<string, string> Allowlist = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["__IOS__"] = "src/AddOns/BassEnc/Portable/PInvoke/BassEnc_Aac.cs"
    };

    public static IReadOnlyList<string> AllSymbols => BannedSymbols.Concat(Allowlist.Keys).ToArray();

    public static IReadOnlyList<SymbolOccurrence> Scan(string relativePath, string content, IEnumerable<string> symbols)
    {
        var lines = Lines(content);

        // NOTE: \b is useless with underscores, so identifier boundaries are spelled out.
        return symbols
            .SelectMany(symbol => lines
                .Select((line, i) => (line, number: i + 1))
                .Where(l => Regex.IsMatch(l.line, $"(?<![A-Za-z0-9_]){Regex.Escape(symbol)}(?![A-Za-z0-9_])"))
                .Select(l => new SymbolOccurrence(relativePath, l.number, symbol, l.line.Trim())))
            .OrderBy(o => o.LineNumber)
            .ToArray();
    }

    public static IReadOnlyList<SymbolOccurrence> ApplyAllowlist(IEnumerable<SymbolOccurrence> occurrences, IReadOnlyDictionary<string, string> allowlist) =>
        occurrences
            .Where(o => !(allowlist.TryGetValue(o.Symbol, out var path) && o.RelativePath == path))
            .OrderBy(o => o.RelativePath, StringComparer.Ordinal).ThenBy(o => o.LineNumber)
            .ToArray();

    /// <summary>A DefineConstants that does not include <c>$(DefineConstants)</c> wipes every symbol set before it.</summary>
    public static IReadOnlyList<DefineConstantsOffence> FindClobbers(string relativePath, string content) =>
        DefineConstantsLines(content)
            .Where(l => !l.value.Contains("$(DefineConstants)", StringComparison.OrdinalIgnoreCase))
            .Select(l => new DefineConstantsOffence(relativePath, l.number, l.line.Trim()))
            .ToArray();

    public static IReadOnlyList<DefineConstantsOffence> FindInternalDllImportDefinitions(string relativePath, string content) =>
        DefineConstantsLines(content)
            .Where(l => l.value.Contains(InternalDllImportSymbol, StringComparison.Ordinal))
            .Select(l => new DefineConstantsOffence(relativePath, l.number, l.line.Trim()))
            .ToArray();

    // NOTE: matched over the whole file, not per line - an element split across lines is still one assignment.
    private static IEnumerable<(string line, int number, string value)> DefineConstantsLines(string content)
    {
        var normalised = content.Replace("\r\n", "\n");

        return DefineConstantsPattern().Matches(normalised)
            .Select(m => (
                line: WhitespaceRunPattern().Replace(m.Value, " "),
                number: normalised.AsSpan(0, m.Index).Count('\n') + 1,
                value: m.Groups[1].Value));
    }

    private static string[] Lines(string content) => content.Replace("\r\n", "\n").Split('\n');

    [GeneratedRegex(@"<DefineConstants(?:\s[^>]*)?>([^<]*)</DefineConstants>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex DefineConstantsPattern();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRunPattern();
}
