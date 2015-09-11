using System;
using System.IO;

namespace ManagedBass
{
    enum WaveFormatEncoding : ushort
    {
        Pcm = 0x0001,
        Adpcm = 0x0002,
        IeeeFloat = 0x0003,
        Extensible = 0xFFFE
    }

    /// <summary>
    /// Represents a Wave file format
    /// </summary>
    class WaveFormat
    {
        /// <summary>
        /// Creates a new PCM format with the specified sample rate, bit depth and channels
        /// </summary>
        public WaveFormat(int rate, int bits, int channels)
        {
            if (channels < 1) throw new ArgumentOutOfRangeException("Channels must be 1 or greater", "channels");
            // minimum 16 bytes, sometimes 18 for PCM
            Encoding = WaveFormatEncoding.Pcm;
            Channels = (short)channels;
            SampleRate = rate;
            BitsPerSample = (short)bits;
            ExtraSize = 0;

            BlockAlign = (short)(channels * (bits / 8));
            AverageBytesPerSecond = SampleRate * BlockAlign;
        }

        /// <summary>
        /// Creates a new 32 bit IEEE floating point wave format
        /// </summary>
        /// <param name="SampleRate">sample rate</param>
        /// <param name="Channels">number of channels</param>
        public static WaveFormat CreateIeeeFloat(int SampleRate, int Channels)
        {
            WaveFormat wf = new WaveFormat(SampleRate, 32, (short)Channels);
            wf.Encoding = WaveFormatEncoding.IeeeFloat;
            return wf;
        }

        /// <summary>
        /// Writes this WaveFormat object to a stream
        /// </summary>
        /// <param name="writer">the output stream</param>
        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write((int)(18 + ExtraSize)); // wave format length
            writer.Write((short)Encoding);
            writer.Write((short)Channels);
            writer.Write((int)SampleRate);
            writer.Write((int)AverageBytesPerSecond);
            writer.Write((short)BlockAlign);
            writer.Write((short)BitsPerSample);
            writer.Write((short)ExtraSize);
        }

        #region Auto Implemented Properties
        /// <summary>
        /// Returns the number of channels (1=mono,2=stereo etc)
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        /// Returns the encoding type used
        /// </summary>
        public WaveFormatEncoding Encoding { get; set; }

        /// <summary>
        /// Returns the sample rate (samples per second)
        /// </summary>
        public int SampleRate { get; set; }

        /// <summary>
        /// Returns the average number of bytes used per second
        /// </summary>
        public int AverageBytesPerSecond { get; set; }

        /// <summary>
        /// Returns the block alignment
        /// </summary>
        public virtual int BlockAlign { get; set; }

        /// <summary>
        /// Returns the number of bits per sample (usually 16 or 32, sometimes 24 or 8)
        /// Can be 0 for some codecs
        /// </summary>
        public int BitsPerSample { get; set; }

        /// <summary>
        /// Returns the number of extra bytes used by this waveformat. Often 0,
        /// except for compressed formats which store extra data after the WAVEFORMATEX header
        /// </summary>
        public int ExtraSize { get; set; }
        #endregion
    }
}