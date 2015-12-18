using System;
using System.IO;
using System.Text;

namespace ManagedBass
{
    /// <summary>
    /// This class writes WAV data to a .wav file on disk
    /// </summary>
    public class WaveFileWriter : IAudioFileWriter
    {
        Stream outStream;
        long dataSizePos, factSampleCountPos;

        #region Factory
        /// <summary>
        /// WaveFileWriter that actually writes to a stream
        /// </summary>
        /// <param name="outStream">Stream to be written to</param>
        /// <param name="format">Wave format to use</param>
        WaveFileWriter(Stream outStream, WaveFormat format)
        {
            this.outStream = outStream;
            BinaryWriter w = new BinaryWriter(outStream, Encoding.ASCII);
            w.Write(Encoding.ASCII.GetBytes("RIFF"));
            w.Write((int)0); // placeholder
            w.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            WaveFormat = format;

            format.Serialize(w);

            CreateFactChunk(outStream, format, w);

            WriteDataChunkHeader(outStream, w);
        }

        /// <summary>
        /// Creates a new WaveFileWriter
        /// </summary>
        /// <param name="filename">The filename to write to</param>
        /// <param name="format">The Wave Format of the output data</param>
        WaveFileWriter(string filename, WaveFormat format)
            : this(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), format) { Length = 0; }

        public WaveFileWriter(string FilePath, int NoOfChannels = 2, int SampleRate = 44100)
            : this(FilePath, WaveFormat.CreateIeeeFloat(SampleRate, NoOfChannels)) { }
        #endregion

        void WriteDataChunkHeader(Stream outStream, BinaryWriter w)
        {
            w.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            dataSizePos = outStream.Position;
            w.Write((int)0); // placeholder
        }

        void CreateFactChunk(Stream outStream, WaveFormat format, BinaryWriter w)
        {
            if (format.Encoding != WaveFormatEncoding.Pcm)
            {
                w.Write(Encoding.ASCII.GetBytes("fact"));
                w.Write((int)4);
                factSampleCountPos = outStream.Position;
                w.Write((int)0); // number of samples
            }
        }

        #region Auto Implemented Properties
        /// <summary>
        /// Number of bytes of audio
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// WaveFormat of this wave file
        /// </summary>
        WaveFormat WaveFormat;
        #endregion

        #region Write
        /// <summary>
        /// Writes bytes to the WaveFile
        /// </summary>
        /// <param name="data">the buffer containing the wave data</param>
        /// <param name="count">the number of bytes to write</param>
        public void Write(byte[] data, int count)
        {
            outStream.Write(data, 0, count);
            Length += count;
        }

        /// <summary>
        /// Writes 16 bit samples to the Wave file
        /// </summary>
        /// <param name="data">The buffer containing the wave data</param>
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
        /// <param name="data">The buffer containing the wave data</param>
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

                        if (WaveFormat.Encoding != WaveFormatEncoding.Pcm)
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
        /// Finaliser - should only be called if the user forgot to close this WaveFileWriter
        /// </summary>
        ~WaveFileWriter() { Dispose(false); }
        #endregion
    }
}