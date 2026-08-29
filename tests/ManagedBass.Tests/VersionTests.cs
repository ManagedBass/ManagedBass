namespace ManagedBass.Tests;

[Collection(BassCollection.Name)]
public class VersionTests
{
    // 2.4.17 dropped the iOS static libraries; everything in the platform story assumes it.
    static readonly Version MinimumSupported = new(2, 4, 17, 0);

    readonly ITestOutputHelper _output;

    public VersionTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Bass_OnVersion_IsAtLeastMinimumSupported_WhenNativeIsLoaded()
    {
        var version = Bass.Version;

        // un4seen's URLs are unversioned, so the exact build only ever shows up in this log line.
        _output.WriteLine($"BASS native version: {version} (from {NativeLibraries.PathFor("bass")})");

        version.Should().BeGreaterThanOrEqualTo(MinimumSupported, $"re-run eng/fetch-natives.sh {NativeLibraries.HostPlatform} for a current build");
    }

    [Theory]
    [InlineData(0x02041203, 2, 4, 18, 3)]
    [InlineData(0x02041100, 2, 4, 17, 0)]
    [InlineData(0x02040000, 2, 4, 0, 0)]
    public void Extensions_OnGetVersion_DecodesOneByteperComponent_WhenPacked(int packed, int major, int minor, int build, int revision) =>
        Extensions.GetVersion(packed).Should().Be(new Version(major, minor, build, revision));

    [Fact]
    public void Extensions_OnGetVersion_TreatsBytesAsUnsigned_WhenTopBitIsSet() =>
        Extensions.GetVersion(unchecked((int)0xFFFFFFFF)).Should().Be(new Version(255, 255, 255, 255));
}
