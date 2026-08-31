using System.Text.RegularExpressions;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record DllNameGuardFacts(string RelativePath, int Declarations, int GuardedPairs)
{
    /// <summary>Exactly one guard, exactly two declarations (the <c>__Internal</c> arm and the real one).</summary>
    public bool IsGuarded => GuardedPairs == 1 && Declarations == 2;

    public override string ToString() => $"{RelativePath}: {Declarations} declaration(s), {GuardedPairs} guard(s)";
}

/// <summary>Checks every <c>const string DllName</c> sits under the internal-DllImport guard.</summary>
public static partial class DllNameGuardScanner
{
    // NOTE: access modifiers are optional in C#, so "internal const string DllName" is still a declaration.
    private const string Declaration = @"(?:(?:public|internal|private|protected)\s+)*const\s+string\s+DllName\s*=";

    public static bool DeclaresDllName(string content) => DeclarationPattern().IsMatch(content);

    public static DllNameGuardFacts Scan(string relativePath, string content) =>
        new(relativePath, DeclarationPattern().Matches(content).Count, GuardPattern().Matches(content).Count);

    [GeneratedRegex(@"^[ \t]*" + Declaration, RegexOptions.Multiline)]
    private static partial Regex DeclarationPattern();

    [GeneratedRegex(
        @"^#if MANAGEDBASS_INTERNAL_DLLIMPORT[ \t]*\r?\n[ \t]*" + Declaration + @"\s*""__Internal"";[ \t]*\r?\n#else[ \t]*\r?\n[ \t]*" + Declaration + @"\s*""[^""]+"";[ \t]*\r?\n#endif",
        RegexOptions.Multiline)]
    private static partial Regex GuardPattern();
}
