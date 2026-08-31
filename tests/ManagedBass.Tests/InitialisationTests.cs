namespace ManagedBass.Tests;

/// <summary>
/// Proves the managed assembly reaches the native library at all. A green compile says nothing here:
/// every platform <c>#if</c> in this repository has two valid arms.
/// </summary>
[Collection(BassCollection.Name)]
public class InitialisationTests
{
    readonly BassFixture _bass;
    readonly ITestOutputHelper _output;

    public InitialisationTests(BassFixture bass, ITestOutputHelper output)
    {
        _bass = bass;
        _output = output;
    }

    [Fact]
    public void Bass_OnInit_Succeeds_WhenUsingNoSoundDevice()
    {
        _output.WriteLine($"Native: {NativeLibraries.PathFor("bass")}, LastError after Init: {_bass.InitError}");

        _bass.Initialised.Should().BeTrue(_bass.FailureMessage);
    }

    [Fact]
    public void Bass_OnCurrentDevice_ReturnsNoSoundDevice_WhenInitialised() =>
        Bass.CurrentDevice.Should().Be(Bass.NoSoundDevice);

    [Fact]
    public void Bass_OnChannelPlay_ReportsHandleError_WhenHandleIsInvalid()
    {
        // Asserts an enum return marshals back, not merely that a call did not crash.
        Bass.ChannelPlay(0).Should().BeFalse();
        Bass.LastError.Should().Be(Errors.Handle);
    }
}
