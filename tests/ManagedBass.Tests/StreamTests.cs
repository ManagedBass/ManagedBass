namespace ManagedBass.Tests;

/// <summary>
/// Drives a generated WAV through the managed-to-native boundary: bytes in, handle out, 64-bit positions
/// both ways, struct marshalled out, handle freed.
/// </summary>
[Collection(BassCollection.Name)]
public class StreamTests
{
    readonly ITestOutputHelper _output;

    public StreamTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Bass_OnCreateStream_RoundTripsLengthAndPosition_WhenGivenSineWave()
    {
        var wave = WaveGenerator.SineWave(seconds: 1.0);

        // Decode is mandatory: the no-sound device rejects playback channels.
        var handle = Bass.CreateStream(wave.Bytes, 0, wave.Bytes.Length, BassFlags.Decode);
        handle.Should().NotBe(0, $"CreateStream over a {wave.Bytes.Length}-byte WAV failed with {Bass.LastError}");

        try
        {
            var info = Bass.ChannelGetInfo(handle);
            _output.WriteLine($"Channel: {info.Frequency} Hz, {info.Channels} ch, {info.Resolution}, {info.Flags}");

            info.Frequency.Should().Be(wave.SampleRate);
            info.Channels.Should().Be(wave.Channels);

            var length = Bass.ChannelGetLength(handle);
            length.Should().BePositive($"ChannelGetLength failed with {Bass.LastError}");

            var seconds = Bass.ChannelBytes2Seconds(handle, length);
            _output.WriteLine($"Length: {length} bytes = {seconds:F4} s (generated {wave.Duration:F4} s)");
            seconds.Should().BeApproximately(wave.Duration, 0.01);

            var bytesPerFrame = info.Channels * info.Resolution switch { Resolution.Byte => 1, Resolution.Float => 4, _ => 2 };
            var target = length / 2 / bytesPerFrame * bytesPerFrame;

            Bass.ChannelSetPosition(handle, target).Should().BeTrue($"ChannelSetPosition({target}) failed with {Bass.LastError}");
            Bass.ChannelGetPosition(handle).Should().Be(target);
        }
        finally
        {
            Bass.StreamFree(handle).Should().BeTrue($"StreamFree failed with {Bass.LastError}");
        }
    }

    [Fact]
    public void Bass_OnChannelGetData_DecodesTone_WhenGivenSineWave()
    {
        var wave = WaveGenerator.SineWave(seconds: 0.25);
        var handle = Bass.CreateStream(wave.Bytes, 0, wave.Bytes.Length, BassFlags.Decode);
        handle.Should().NotBe(0, $"CreateStream failed with {Bass.LastError}");

        try
        {
            var decoded = new short[4096];
            var bytesRead = Bass.ChannelGetData(handle, decoded, decoded.Length * sizeof(short));

            bytesRead.Should().BePositive($"ChannelGetData failed with {Bass.LastError}");
            // All zeroes would mean the array marshalled empty rather than carrying the tone.
            decoded.Should().Contain(sample => Math.Abs((int)sample) > short.MaxValue / 8);
        }
        finally
        {
            Bass.StreamFree(handle);
        }
    }

    [Fact]
    public void Bass_OnCreateStream_ReportsFileFormat_WhenGivenGarbage()
    {
        var garbage = Enumerable.Range(0, 512).Select(i => (byte)(i * 37)).ToArray();

        Bass.CreateStream(garbage, 0, garbage.Length, BassFlags.Decode).Should().Be(0);
        Bass.LastError.Should().Be(Errors.FileFormat);
    }
}
