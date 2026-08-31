using System.Text;

namespace ManagedBass.Tests.Infrastructure;

/// <summary>An in-memory RIFF/WAVE file and the parameters it was generated from.</summary>
public record WaveFile(byte[] Bytes, int SampleRate, int Channels, int BitsPerSample, int DataByteLength)
{
    public int BlockAlign => Channels * BitsPerSample / 8;

    public double Duration => (double)DataByteLength / (SampleRate * BlockAlign);
}

/// <summary>Generates deterministic 16-bit PCM sine waves so no fixture audio file is needed on disk.</summary>
public static class WaveGenerator
{
    const int HeaderLength = 44;
    const int BitsPerSample = 16;

    public static WaveFile SineWave(double seconds = 1.0, double frequency = 440.0, int sampleRate = 44100, int channels = 1)
    {
        var blockAlign = channels * BitsPerSample / 8;
        var frameCount = (int)Math.Round(seconds * sampleRate);
        var dataLength = frameCount * blockAlign;

        var buffer = new byte[HeaderLength + dataLength];
        using var stream = new MemoryStream(buffer, writable: true);
        using var writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true);

        writer.Write("RIFF"u8);
        writer.Write(36 + dataLength);
        writer.Write("WAVE"u8);
        writer.Write("fmt "u8);
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)channels);
        writer.Write(sampleRate);
        writer.Write(sampleRate * blockAlign);
        writer.Write((short)blockAlign);
        writer.Write((short)BitsPerSample);
        writer.Write("data"u8);
        writer.Write(dataLength);

        // Half amplitude keeps the tone clear of the clipping ceiling.
        var amplitude = short.MaxValue / 2.0;
        var step = 2.0 * Math.PI * frequency / sampleRate;

        foreach (var sample in Enumerable.Range(0, frameCount).Select(i => (short)(amplitude * Math.Sin(step * i))))
            for (var channel = 0; channel < channels; channel++)
                writer.Write(sample);

        writer.Flush();

        return new(buffer, sampleRate, channels, BitsPerSample, dataLength);
    }
}
