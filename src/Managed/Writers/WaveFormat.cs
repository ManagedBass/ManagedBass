using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Represents a Wave file format (WAVEFORMATEX)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class WaveFormat
    {
        protected WaveFormatTag waveFormatTag;
        protected short channels;
        protected int sampleRate;
        protected int averageBytesPerSecond;
        protected short blockAlign;
        protected short bitsPerSample;
        protected short extraSize;

        /// <summary>
        /// Creates a new PCM 44.1Khz stereo 16 bit format
        /// </summary>
        public WaveFormat() : this(44100, 16, 2) { }

        /// <summary>
        /// Creates a new 16 bit wave format with the specified sample rate and channel count
        /// </summary>
        /// <param name="SampleRate">Sample Rate</param>
        /// <param name="Channels">Number of channels</param>
        public WaveFormat(int SampleRate, int Channels) : this(SampleRate, 16, Channels) { }

        /// <summary>
        /// Creates a new PCM format with the specified sample rate, bit depth and channels
        /// </summary>
        public WaveFormat(int SampleRate, int BitsPerSample, int Channels)
        {
            if (Channels < 1)
                throw new ArgumentOutOfRangeException(nameof(Channels), $"{nameof(Channels)} must be 1 or greater");

            // minimum 16 bytes, sometimes 18 for PCM
            waveFormatTag = WaveFormatTag.Pcm;
            channels = (short)Channels;
            sampleRate = SampleRate;
            bitsPerSample = (short)BitsPerSample;
            extraSize = 0;

            blockAlign = (short)(Channels * (BitsPerSample / 8));
            averageBytesPerSecond = sampleRate * blockAlign;
        }

        /// <summary>
        /// Creates a new 32 bit IEEE floating point wave format
        /// </summary>
        /// <param name="SampleRate">sample rate</param>
        /// <param name="Channels">number of channels</param>
        public static WaveFormat CreateIeeeFloat(int SampleRate, int Channels)
        {
            return new WaveFormat
            {
                waveFormatTag = WaveFormatTag.IeeeFloat,
                channels = (short)Channels,
                bitsPerSample = 32,
                sampleRate = SampleRate,
                blockAlign = (short)(4 * Channels),
                averageBytesPerSecond = SampleRate * 4 * Channels,
                extraSize = 0
            };
        }
        
        /// <summary>
        /// Gets or Sets the encoding Type used
        /// </summary>
        public WaveFormatTag Encoding
        {
            get { return waveFormatTag; }
            set { waveFormatTag = value; }
        }

        /// <summary>
        /// Writes this WaveFormat object to a stream
        /// </summary>
        /// <param name="Writer">the output stream</param>
        public virtual void Serialize(BinaryWriter Writer)
        {
            Writer.Write((short)Encoding);
            Writer.Write((short)Channels);
            Writer.Write(SampleRate);
            Writer.Write(AverageBytesPerSecond);
            Writer.Write((short)BlockAlign);
            Writer.Write((short)BitsPerSample);
            Writer.Write((short)ExtraSize);
        }

        /// <summary>
        /// Gets the number of channels (1=mono,2=stereo etc)
        /// </summary>
        public int Channels => channels;

        /// <summary>
        /// Gets the sample rate (samples per second)
        /// </summary>
        public int SampleRate => sampleRate;

        /// <summary>
        /// Gets the average number of bytes used per second
        /// </summary>
        public int AverageBytesPerSecond => averageBytesPerSecond;

        /// <summary>
        /// Gets the block alignment
        /// </summary>
        public virtual int BlockAlign => blockAlign;

        /// <summary>
        /// Gets the number of bits per sample (usually 16 or 32, sometimes 24 or 8)
        /// Can be 0 for some codecs
        /// </summary>
        public int BitsPerSample => bitsPerSample;

        /// <summary>
        /// Gets the number of extra bytes used by this waveformat.
        /// Often 0, except for compressed formats which store extra data after the WAVEFORMATEX header
        /// </summary>
        public int ExtraSize => extraSize;
    }
}