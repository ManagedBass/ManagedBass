using System.Runtime.InteropServices;

namespace ManagedBass.Tests;

/// <summary>
/// Asserts the full predicate vector, trues and falses, for the OS the tests run on. This is the direct
/// guard against "fixed iOS, broke Linux".
/// </summary>
/// <remarks>
/// NOTE: the host OS comes from <see cref="RuntimeInformation"/>, never from <see cref="CrossPlatformHelper"/>,
/// otherwise a helper claiming iOS everywhere would agree with itself. Needs no natives, so it lives outside
/// the BASS collection and still runs on a machine with nothing fetched.
/// </remarks>
public class PlatformPredicateTests
{
    static readonly IReadOnlyDictionary<string, Func<bool>> Predicates = new Dictionary<string, Func<bool>>
    {
        ["IsWindows"] = () => CrossPlatformHelper.IsWindows,
        ["IsLinux"] = () => CrossPlatformHelper.IsLinux,
        ["IsMacOS"] = () => CrossPlatformHelper.IsMacOS,
        ["IsIOS"] = () => CrossPlatformHelper.IsIOS,
        ["IsTvOS"] = () => CrossPlatformHelper.IsTvOS,
        ["IsMacCatalyst"] = () => CrossPlatformHelper.IsMacCatalyst,
        ["IsAndroid"] = () => CrossPlatformHelper.IsAndroid,
        ["IsApple"] = () => CrossPlatformHelper.IsApple,
    };

    // Everything not listed for a host is expected false.
    static readonly IReadOnlyDictionary<string, string[]> ExpectedTrue = new Dictionary<string, string[]>
    {
        ["windows"] = ["IsWindows"],
        ["linux"] = ["IsLinux"],
        ["osx"] = ["IsMacOS", "IsApple"],
    };

    readonly ITestOutputHelper _output;

    public PlatformPredicateTests(ITestOutputHelper output) => _output = output;

    static string Host =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows"
        : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx"
        : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux"
        : RuntimeInformation.OSDescription;

    [Theory]
    [InlineData("IsWindows")]
    [InlineData("IsLinux")]
    [InlineData("IsMacOS")]
    [InlineData("IsIOS")]
    [InlineData("IsTvOS")]
    [InlineData("IsMacCatalyst")]
    [InlineData("IsAndroid")]
    [InlineData("IsApple")]
    public void CrossPlatformHelper_OnPredicate_MatchesHostVector_WhenRunOnDesktop(string predicate)
    {
        ExpectedTrue.Should().ContainKey(Host, "the runner OS needs a row in ExpectedTrue");

        Predicates[predicate]().Should().Be(ExpectedTrue[Host].Contains(predicate), $"{predicate} on {Host}");
    }

    [Fact]
    public void CrossPlatformHelper_OnAllPredicates_MatchesHostVector_WhenRunOnDesktop()
    {
        var actual = Predicates.ToDictionary(p => p.Key, p => p.Value());
        var expected = Predicates.ToDictionary(p => p.Key, p => ExpectedTrue[Host].Contains(p.Key));

        _output.WriteLine($"Host {Host}: " + string.Join(", ", actual.Select(p => $"{p.Key}={p.Value}")));

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CrossPlatformHelper_OnIsIOS_ExcludesMacCatalyst_WhenComparedToBcl() =>
        // Asserted against the BCL rather than as !(IsIOS && IsMacCatalyst), which is vacuous on every desktop leg.
        CrossPlatformHelper.IsIOS.Should().Be(OperatingSystem.IsIOS() && !OperatingSystem.IsMacCatalyst());

    [Fact]
    public void CrossPlatformHelper_OnIsDynamicCodeSupported_IsTrue_WhenJitted() =>
        CrossPlatformHelper.IsDynamicCodeSupported.Should().BeTrue();
}
