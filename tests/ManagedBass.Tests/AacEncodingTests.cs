using ManagedBass.Enc;

namespace ManagedBass.Tests;

/// <summary>
/// Drives <see cref="BassEnc_Aac"/> to a file. On Apple that runs BASSenc's Core Audio encoder for real and
/// the output is decoded back, so a wrong FourCC, bitrate or filename encoding cannot pass; elsewhere the call
/// must reach the <c>bassenc_aac</c> add-on, which un4seen do not publish for Apple and we therefore never fetch.
/// </summary>
/// <remarks>
/// NOTE: both legs assert, and they assert opposite things, so an inverted <c>IsApple</c> dispatch turns one of
/// the CI legs red whichever way it is broken. The host comes from <see cref="NativeLibraries.HostPlatform"/>
/// rather than <c>CrossPlatformHelper</c>, which is itself under test.
/// </remarks>
[Collection(BassCollection.Name)]
public class AacEncodingTests : IDisposable
{
    // LC AAC at a bitrate every Core Audio release supports, so the assertions are about our plumbing.
    const string Options = "--object-type 2 --bitrate 128000";

    readonly ITestOutputHelper _output;
    readonly List<string> _files = [];

    public AacEncodingTests(ITestOutputHelper output) => _output = output;

    public void Dispose() => _files.ForEach(File.Delete);

    [Fact]
    public void BassEnc_Aac_OnStartToFile_WritesDecodableAdts_WhenHostIsApple()
    {
        var apple = NativeLibraries.HostPlatform == "osx";
        CrossPlatformHelper.IsApple.Should().Be(apple, "the dispatch under test reads IsApple");

        if (!apple)
        {
            // Off Apple the call has to reach the add-on, whose native is deliberately not in the manifest.
            // Anything other than an unresolved bassenc_aac means the dispatch went the wrong way.
            var offApple = () => BassEnc_Aac.Start(Tone(0.1), Options, EncodeFlags.Default, OutputFile());

            offApple.Should().Throw<DllNotFoundException>().Which.Message.Should().Contain("bassenc_aac");
            return;
        }

        var wave = WaveGenerator.SineWave(seconds: 1.0);
        var path = OutputFile();
        var encoded = Encode(wave, Options, path);

        _output.WriteLine($"Encoded {wave.DataByteLength} PCM bytes to {encoded.Length} AAC bytes");
        encoded.Length.Should().BePositive("the encoder wrote no output at all");

        // ADTS: twelve sync bits, then a layer field that is always 00. A byte-swapped 'adts' would never have
        // got this far - EncodeStartCA rejects it - but this pins the container we actually asked for.
        encoded[0].Should().Be(0xFF, "an ADTS frame opens with the 0xFFF sync word");
        (encoded[1] & 0xF6).Should().Be(0xF0, "the sync word continues and the layer field is 00");

        var decoded = Bass.CreateStream(path, Flags: BassFlags.Decode);
        decoded.Should().NotBe(0, $"BASS could not decode the AAC we produced; LastError = {Bass.LastError}");

        try
        {
            var seconds = Bass.ChannelBytes2Seconds(decoded, Bass.ChannelGetLength(decoded));
            _output.WriteLine($"Round-tripped {seconds:F4} s (encoded {wave.Duration:F4} s)");

            // AAC carries encoder priming and a padded final frame, so this is a sanity band, not a match.
            seconds.Should().BeInRange(wave.Duration - 0.1, wave.Duration + 0.2);
        }
        finally
        {
            Bass.StreamFree(decoded);
        }
    }

    [Fact]
    public void BassEnc_Aac_OnStartToFile_HonoursNonAsciiPath_WhenHostIsApple()
    {
        // BASS_Encode_StartCAFile is the one filename entry point that rejects BASS_UNICODE, so it is marshalled
        // as UTF-8. A CJK character is outside Latin-1, which is where a wrong CharSet would land.
        var path = OutputFile("音-");

        if (NativeLibraries.HostPlatform != "osx")
        {
            // Same expectation as the ASCII case: the path must not change which encoder we dispatch to.
            var offApple = () => BassEnc_Aac.Start(Tone(0.1), Options, EncodeFlags.Default, path);

            offApple.Should().Throw<DllNotFoundException>().Which.Message.Should().Contain("bassenc_aac");
            return;
        }

        Encode(WaveGenerator.SineWave(seconds: 0.25), Options, path).Length.Should().BePositive();
        File.Exists(path).Should().BeTrue("the encoder wrote to a differently-encoded path");
    }

    [Fact]
    public void BassEnc_Aac_OnWrongCallbackOverload_ThrowsPlatformNotSupported_WhenCalledOnThisHost()
    {
        var handle = Tone(0.1);

        var procedure = () => BassEnc_Aac.Start(handle, Options, EncodeFlags.Default, (EncodeProcedure)((_, _, _, _, _) => { }), IntPtr.Zero);
        var procedureEx = () => BassEnc_Aac.Start(handle, Options, EncodeFlags.Default, (EncodeProcedureEx)((_, _, _, _, _, _) => { }), IntPtr.Zero);

        // Exactly one of the two is wrong for this host, and its message has to name the other one.
        if (CrossPlatformHelper.IsApple)
            procedure.Should().Throw<PlatformNotSupportedException>().WithMessage("*EncodeProcedureEx*");
        else
            procedureEx.Should().Throw<PlatformNotSupportedException>().WithMessage("*EncodeProcedure*");
    }

    [Fact]
    public void BassEnc_Aac_OnVersion_ThrowsPlatformNotSupported_WhenHostIsApple()
    {
        var version = () => BassEnc_Aac.Version;

        if (CrossPlatformHelper.IsApple)
            version.Should().Throw<PlatformNotSupportedException>().WithMessage("*EncodeStartCA*");
        else
            // Off Apple there is a native to look for; we simply never ship it, so the lookup is what fails.
            version.Should().Throw<DllNotFoundException>();
    }

    /// <summary>Encodes a whole wave and returns the file's bytes.</summary>
    byte[] Encode(WaveFile wave, string options, string path)
    {
        // Decode, so pulling the data below is what drives the encoder DSP - there is no playback device here.
        var channel = Bass.CreateStream(wave.Bytes, 0, wave.Bytes.Length, BassFlags.Decode);
        channel.Should().NotBe(0, $"CreateStream failed with {Bass.LastError}");

        var encoder = BassEnc_Aac.Start(channel, options, EncodeFlags.Default, path);
        encoder.Should().NotBe(0, $"BassEnc_Aac.Start through Core Audio failed with {Bass.LastError}");

        try
        {
            var buffer = new byte[16384];
            var pumped = 0;

            for (int read; (read = Bass.ChannelGetData(channel, buffer, buffer.Length)) > 0;)
                pumped += read;

            pumped.Should().Be(wave.DataByteLength, "the whole tone has to reach the encoder");
        }
        finally
        {
            BassEnc.EncodeStop(encoder).Should().BeTrue($"EncodeStop failed with {Bass.LastError}");
            Bass.StreamFree(channel).Should().BeTrue($"StreamFree failed with {Bass.LastError}");
        }

        return File.ReadAllBytes(path);
    }

    string OutputFile(string prefix = "")
    {
        var path = Path.Combine(Path.GetTempPath(), $"managedbass-{prefix}{Guid.NewGuid():N}.aac");
        _files.Add(path);

        return path;
    }

    /// <summary>A decoding channel to hand the API under test when the call is expected to throw before using it.</summary>
    static int Tone(double seconds)
    {
        var wave = WaveGenerator.SineWave(seconds);

        return Bass.CreateStream(wave.Bytes, 0, wave.Bytes.Length, BassFlags.Decode | BassFlags.AutoFree);
    }
}
