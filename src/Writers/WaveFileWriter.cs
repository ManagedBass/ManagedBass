using System;
using System.IO;
using System.Text;

namespace ManagedBass
{
    /// <summary>
    /// Writes Wave data to a .wav file
    /// </summary>
    public class WaveFileWriter : IAudioFileWriter
    {
        #region Fields
        Stream outStream;
        long dataSizePos, factSampleCountPos;

        public Resolution Resolution { get; private set; }

        /// <summary>
        /// Number of bytes of audio
        /// </summary>
        public long Length { get; set; }

        WaveFormat WaveFormat;
        #endregion

        #region Factory
        WaveFileWriter(Stream outStream, WaveFormat format)
        {
            this.outStream = outStream;
            BinaryWriter w = new BinaryWriter(outStream, Encoding.ASCII);
            w.Write(Encoding.ASCII.GetBytes("RIFF"));
            w.Write((int)0); // placeholder
            w.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            WaveFormat = format;

            w.Write((int)(18 + format.ExtraSize)); // wave format Length
            format.Serialize(w);

            // CreateFactChunk
            if (format.Encoding != WaveFormatTag.Pcm)
            {
                w.Write(Encoding.ASCII.GetBytes("fact"));
                w.Write((int)4);
                factSampleCountPos = outStream.Position;
                w.Write((int)0); // number of samples
            }

            // WriteDataChunkHeader
            w.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            dataSizePos = outStream.Position;
            w.Write((int)0); // placeholder

            Length = 0;
        }

        static WaveFormat MakeWF(int NoOfChannels, int SampleRate, Resolution Resolution)
        {
            switch (Resolution)
            {
                case Resolution.Float:
                    return WaveFormat.CreateIeeeFloat(SampleRate, NoOfChannels);
                case Resolution.Byte:
                    return new WaveFormat(SampleRate, 8, NoOfChannels);
                default:
                    return new WaveFormat(SampleRate, 16, NoOfChannels);
            }
        }

        public WaveFileWriter(Stream outStream, int NoOfChannels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short)
            : this(outStream, MakeWF(NoOfChannels, SampleRate, Resolution))
        {
            this.Resolution = Resolution;
        }

        public WaveFileWriter(string FilePath, int NoOfChannels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short)
            : this(new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read), NoOfChannels, SampleRate, Resolution) { }
        #endregion

        #region Write
        /// <summary>
        /// Writes bytes to the WaveFile
        /// </summary>
        /// <param name="data">the Buffer containing the wave data</param>
        /// <param name="count">the number of bytes to write</param>
        public void Write(byte[] data, int count)
        {
            outStream.Write(data, 0, count);
            Length += count;
        }

        /// <summary>
        /// Writes 16 bit samples to the Wave file
        /// </summary>
        /// <param name="data">The Buffer containing the wave data</param>
        /// <param name="count">The number of bytes to write</param>
        public void Write(short[] data, int count)
        {
            BinaryWriter w = new BinaryWriter(outStream);
            for (int n = 0; n < count / 2; n++) w.Write(data[n]);
            Length += count;
        }

        /// <summary>
        /// Writes 32 bit float samples to the Wave file
        /// </summary>
        /// <param name="data">The Buffer containing the wave data</param>
        /// <param name="count">The number of bytes to write</param>
        public void Write(float[] data, int count)
        {
            BinaryWriter w = new BinaryWriter(outStream);
            for (int n = 0; n < count / 4; n++) w.Write(data[n]);
            Length += count;
        }
        #endregion

        /// <summary>
        /// Ensures data is written to disk
        /// </summary>
        public void Flush() { outStream.Flush(); }

        #region IDisposable Members
        /// <summary>
        /// Closes this WaveFile
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Actually performs the close,making sure the header contains the correct data
        /// </summary>
        /// <param name="disposing">True if called from <see>Dispose</see></param>
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (outStream != null)
                {
                    try
                    {
                        outStream.Flush();
                        BinaryWriter w = new BinaryWriter(outStream, Encoding.ASCII);
                        w.Seek(4, SeekOrigin.Begin);
                        w.Write((int)(outStream.Length - 8));

                        if (WaveFormat.Encoding != WaveFormatTag.Pcm)
                        {
                            w.Seek((int)factSampleCountPos, SeekOrigin.Begin);
                            w.Write((int)((Length * 8) / WaveFormat.BitsPerSample));
                        }

                        w.Seek((int)dataSizePos, SeekOrigin.Begin);
                        w.Write((int)(Length));
                    }
                    finally
                    {
                        // in a finally block as we don't want the FileStream to run its disposer in
                        // the GC thread if the code above caused an IOException (e.g. due to disk full)
                        outStream.Close(); // will close the underlying base stream
                        outStream = null;
                    }
                }
            }
        }

        /// <summary>
        /// Finaliser - should only be called if the User forgot to close this WaveFileWriter
        /// </summary>
        ~WaveFileWriter() { Dispose(false); }
        #endregion
    }
}