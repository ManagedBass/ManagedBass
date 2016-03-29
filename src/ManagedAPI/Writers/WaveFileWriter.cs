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
        Stream ofstream;
        BinaryWriter Writer;
        readonly long dataSizePos, factSampleCountPos;
        readonly object locker = new object();

        public Resolution Resolution { get; }

        /// <summary>
        /// Number of bytes of audio
        /// </summary>
        public long Length { get; set; }

        WaveFormat WaveFormat;
        #endregion

        #region Factory
        WaveFileWriter(Stream outStream, WaveFormat format)
        {
            ofstream = outStream;
            Writer = new BinaryWriter(outStream, Encoding.ASCII);

            Writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            Writer.Write(0); // placeholder
            Writer.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            WaveFormat = format;

            Writer.Write(18 + format.ExtraSize); // wave format Length
            format.Serialize(Writer);

            // CreateFactChunk
            if (format.Encoding != WaveFormatTag.Pcm)
            {
                Writer.Write(Encoding.ASCII.GetBytes("fact"));
                Writer.Write(4);
                factSampleCountPos = outStream.Position;
                Writer.Write(0); // number of samples
            }

            // WriteDataChunkHeader
            Writer.Write(Encoding.ASCII.GetBytes("data"));
            dataSizePos = outStream.Position;
            Writer.Write(0); // placeholder

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
            try
            {
                lock (locker)
                    Writer.Write(data, 0, count);

                Length += count;
            }
            catch { }
        }

        /// <summary>
        /// Writes 16 bit samples to the Wave file
        /// </summary>
        /// <param name="data">The Buffer containing the wave data</param>
        /// <param name="count">The number of bytes to write</param>
        public void Write(short[] data, int count)
        {
            try
            {
                var n = count / 2;

                lock (locker)
                    for (var i = 0; i < n; i++)
                        Writer.Write(data[i]);

                Length += count;
            }
            catch { }
        }

        /// <summary>
        /// Writes 32 bit float samples to the Wave file
        /// </summary>
        /// <param name="data">The Buffer containing the wave data</param>
        /// <param name="count">The number of bytes to write</param>
        public void Write(float[] data, int count)
        {
            try
            {
                var n = count / 4;

                lock (locker)
                    for (var i = 0; i < n; i++)
                        Writer.Write(data[i]);

                Length += count;
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// Ensures data is written to disk
        /// </summary>
        public void Flush()
        {
            lock (locker)
                Writer.Flush();
        }

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
            if (!disposing || Writer == null)
                return;
            try
            {
                lock (locker)
                {
                    Writer.Flush();

                    Writer.Seek(4, SeekOrigin.Begin);
                    Writer.Write((int) (ofstream.Length - 8));

                    if (WaveFormat.Encoding != WaveFormatTag.Pcm)
                    {
                        Writer.Seek((int) factSampleCountPos, SeekOrigin.Begin);
                        Writer.Write((int) (Length*8/WaveFormat.BitsPerSample));
                    }

                    Writer.Seek((int) dataSizePos, SeekOrigin.Begin);
                    Writer.Write((int) Length);
                }
            }
            finally
            {
                lock (locker)
                {
                    Writer.Close();
                    Writer = null;
                }

                ofstream.Close(); // will close the underlying base stream
                ofstream = null;
            }
        }

        /// <summary>
        /// Finaliser - should only be called if the User forgot to close this WaveFileWriter
        /// </summary>
        ~WaveFileWriter() { Dispose(false); }
        #endregion
    }
}