using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ManagedBass.Metadata.Tests.Analysis;

public sealed record TargetsFacts(
    bool ContributesInitialiser,
    bool ContributionInsideTarget,
    bool ContributionGatedOnApplePlatform,
    bool RunsBeforeCompile,
    bool DecidesOutsideTarget,
    IReadOnlyList<string> PlatformsDecidedInsideTarget)
{
    public IReadOnlyList<string> MissingPlatforms =>
        NativeResolverWiring.RequiredPlatforms.Where(p => !PlatformsDecidedInsideTarget.Contains(p, StringComparer.OrdinalIgnoreCase)).ToArray();
}

public sealed record InitialiserFacts(bool HasModuleInitializer, bool CallsRegisterAll);

/// <summary>Reads what the buildTransitive pair actually does. Naming the platforms is not the same as acting on them.</summary>
public static partial class NativeResolverWiring
{
    public const string ResolverTypeName = "ManagedBass.NativeLibraryResolver";
    public static readonly IReadOnlyList<string> RequiredPlatforms = ["ios", "tvos", "maccatalyst"];
    public static readonly IReadOnlyList<string> RequiredResolverMembers = ["RegisterAll()", "Register(System.Reflection.Assembly)"];

    private static readonly string[] _compileOrderingTargets = ["BeforeCompile", "CoreCompile", "Compile", "BeforeBuild"];

    public static TargetsFacts DescribeTargets(string content)
    {
        XDocument document;

        try
        {
            document = XDocument.Parse(content);
        }
        catch (XmlException)
        {
            return new(false, false, false, false, false, []);
        }

        var all = document.Descendants().ToArray();

        var contributions = all
            .Where(e => e.Name.LocalName == "Compile" && IncludesInitialiser(e.Attribute("Include")?.Value))
            .ToArray();

        var owningTargets = contributions.Select(TargetOf).OfType<XElement>().ToArray();

        var decisions = all
            .Select(e => (element: e, condition: ConditionOf(e)))
            .Where(d => d.condition.Contains("$(TargetPlatformIdentifier)", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        var insideTarget = decisions.Where(d => TargetOf(d.element) is not null)
            .SelectMany(d => PlatformLiteralPattern().Matches(d.condition).Select(m => m.Groups[1].Value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new(
            contributions.Length > 0,
            owningTargets.Length > 0,
            contributions.Length > 0 && contributions.All(c => GatedOnApplePlatform(c, all)),
            owningTargets.Any(BeforeCompile),
            decisions.Any(d => TargetOf(d.element) is null),
            insideTarget);
    }

    /// <summary>Comments are stripped first: an attribute that survives only in a comment registers nothing.</summary>
    public static InitialiserFacts DescribeInitialiser(string content)
    {
        var code = BlockCommentPattern().Replace(LineCommentPattern().Replace(content, string.Empty), string.Empty);
        return new(ModuleInitializerPattern().IsMatch(code), RegisterAllPattern().IsMatch(code));
    }

    /// <summary>
    /// A Condition on the contribution's own ancestry must, once the property chain is followed, come down to
    /// <c>$(TargetPlatformIdentifier)</c> being one of the required platforms. A decision that exists but is not
    /// what the ItemGroup keys on gates nothing.
    /// </summary>
    private static bool GatedOnApplePlatform(XElement contribution, XElement[] all) =>
        contribution.AncestorsAndSelf()
            .Select(ConditionOf)
            .Where(c => c.Length > 0)
            .Any(c => ConditionSelectsApple(c, all, []));

    private static bool ConditionSelectsApple(string condition, XElement[] all, HashSet<string> visited) =>
        NamesAllRequiredPlatforms(condition)
        || TruePropertyPattern().Matches(condition).Any(m => PropertyIsTrueOnlyOnApple(m.Groups[1].Value, all, visited));

    private static bool PropertyIsTrueOnlyOnApple(string property, XElement[] all, HashSet<string> visited)
    {
        if (!visited.Add(property))
            return false;

        var definitions = all
            .Where(e => e.Name.LocalName == property && e.Parent?.Name.LocalName == "PropertyGroup")
            .Select(e => (value: e.Value.Trim(), condition: ConditionOf(e)))
            .ToArray();

        var trueDefinitions = definitions.Where(d => d.value.Equals("true", StringComparison.OrdinalIgnoreCase)).ToArray();

        // A 'true' the platform did not decide (unconditional, or conditioned on something else) is a leak.
        if (trueDefinitions.Length > 0)
            return trueDefinitions.All(d => NamesAllRequiredPlatforms(d.condition));

        // No literal 'true': the property defaults to another property, so follow that one.
        return definitions
            .Select(d => PropertyReferencePattern().Match(d.value))
            .Where(m => m.Success)
            .Any(m => PropertyIsTrueOnlyOnApple(m.Groups[1].Value, all, visited));
    }

    private static bool NamesAllRequiredPlatforms(string condition)
    {
        var named = PlatformEqualityPattern().Matches(condition).Select(m => m.Groups[1].Value).ToArray();
        return RequiredPlatforms.All(p => named.Contains(p, StringComparer.OrdinalIgnoreCase));
    }

    private static string ConditionOf(XElement element) => element.Attribute("Condition")?.Value ?? string.Empty;

    private static bool IncludesInitialiser(string? include) =>
        include is not null
        && include.Contains("$(MSBuildThisFileDirectory)", StringComparison.Ordinal)
        && include.Replace('\\', '/').EndsWith(PackageShapeInspector.InitialiserFileName, StringComparison.OrdinalIgnoreCase);

    private static XElement? TargetOf(XElement element) => element.Ancestors().FirstOrDefault(a => a.Name.LocalName == "Target");

    // NOTE: exact tokens - "AfterCompile" contains "Compile" and runs far too late.
    private static bool BeforeCompile(XElement target) =>
        (target.Attribute("BeforeTargets")?.Value ?? string.Empty)
        .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Any(t => _compileOrderingTargets.Contains(t, StringComparer.OrdinalIgnoreCase));

    [GeneratedRegex(@"\$\(TargetPlatformIdentifier\)'\s*[=!]=\s*'([^']*)'", RegexOptions.IgnoreCase)]
    private static partial Regex PlatformLiteralPattern();

    [GeneratedRegex(@"\$\(TargetPlatformIdentifier\)'\s*==\s*'([^']*)'", RegexOptions.IgnoreCase)]
    private static partial Regex PlatformEqualityPattern();

    [GeneratedRegex(@"'\$\(([A-Za-z_][A-Za-z0-9_]*)\)'\s*==\s*'true'", RegexOptions.IgnoreCase)]
    private static partial Regex TruePropertyPattern();

    [GeneratedRegex(@"^\$\(([A-Za-z_][A-Za-z0-9_]*)\)$")]
    private static partial Regex PropertyReferencePattern();

    [GeneratedRegex(@"\[\s*(?:System\.Runtime\.CompilerServices\.|CompilerServices\.)?ModuleInitializer(?:Attribute)?\s*\]")]
    private static partial Regex ModuleInitializerPattern();

    [GeneratedRegex(@"NativeLibraryResolver\s*\.\s*RegisterAll\s*\(\s*\)")]
    private static partial Regex RegisterAllPattern();

    [GeneratedRegex(@"//[^\r\n]*")]
    private static partial Regex LineCommentPattern();

    [GeneratedRegex(@"/\*.*?\*/", RegexOptions.Singleline)]
    private static partial Regex BlockCommentPattern();
}
