#if !__HYBRID__
using System;
using System.IO;
using System.Text;

namespace ManagedBass
{
    /// <summary>
    /// Writes Wave data to a .wav file
    /// </summary>
    public class WaveFileWriter : IAudioWriter
    {
        #region Fields
        Stream _ofstream;
        BinaryWriter _writer;
        readonly long _dataSizePos, _factSampleCountPos;
        readonly object _locker = new object();

        public PCMFormat InputFormat { get; }

        /// <summary>
        /// Number of bytes of audio
        /// </summary>
        public long Length { get; set; }

        readonly WaveFormat WaveFormat;
        #endregion

        #region Factory
        WaveFileWriter(Stream outStream, WaveFormat format)
        {
            _ofstream = outStream;
            _writer = new BinaryWriter(outStream, Encoding.ASCII);

            _writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            _writer.Write(0); // placeholder
            _writer.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            WaveFormat = format;

            _writer.Write(18 + format.ExtraSize); // wave format Length
            format.Serialize(_writer);

            // CreateFactChunk
            if (format.Encoding != WaveFormatTag.Pcm)
            {
                _writer.Write(Encoding.ASCII.GetBytes("fact"));
                _writer.Write(4);
                _factSampleCountPos = outStream.Position;
                _writer.Write(0); // number of samples
            }

            // WriteDataChunkHeader
            _writer.Write(Encoding.ASCII.GetBytes("data"));
            _dataSizePos = outStream.Position;
            _writer.Write(0); // placeholder

            Length = 0;
        }
        
        public WaveFileWriter(Stream outStream, PCMFormat InputFormat)
            : this(outStream, InputFormat.ToWaveFormat())
        {
            this.InputFormat = InputFormat;
        }

        public WaveFileWriter(string FilePath, PCMFormat InputFormat)
            : this(new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read), InputFormat) { }
        #endregion

        #region Write
        /// <summary>
        /// Writes bytes to the WaveFile
        /// </summary>
        /// <param name="data">the Buffer containing the wave data</param>
        /// <param name="count">the number of bytes to write</param>
        public bool Write(byte[] data, int count)
        {
            try
            {
                lock (_locker)
                    _writer.Write(data, 0, count);

                Length += count;

                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Writes 16 bit samples to the Wave file
        /// </summary>
        /// <param name="data">The Buffer containing the wave data</param>
        /// <param name="count">The number of bytes to write</param>
        public bool Write(short[] data, int count)
        {
            try
            {
                var n = count / 2;

                lock (_locker)
                    for (var i = 0; i < n; i++)
                        _writer.Write(data[i]);

                Length += count;

                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Writes 32 bit float samples to the Wave file
        /// </summary>
        /// <param name="data">The Buffer containing the wave data</param>
        /// <param name="count">The number of bytes to write</param>
        public bool Write(float[] data, int count)
        {
            try
            {
                var n = count / 4;

                lock (_locker)
                    for (var i = 0; i < n; i++)
                        _writer.Write(data[i]);

                Length += count;

                return true;
            }
            catch { return false; }
        }
        #endregion

        /// <summary>
        /// Ensures data is written to disk
        /// </summary>
        public void Flush()
        {
            lock (_locker)
                _writer.Flush();
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
            if (!disposing || _writer == null)
                return;
            try
            {
                lock (_locker)
                {
                    _writer.Flush();

                    _writer.Seek(4, SeekOrigin.Begin);
                    _writer.Write((int) (_ofstream.Length - 8));

                    if (WaveFormat.Encoding != WaveFormatTag.Pcm)
                    {
                        _writer.Seek((int) _factSampleCountPos, SeekOrigin.Begin);
                        _writer.Write((int) (Length*8/WaveFormat.BitsPerSample));
                    }

                    _writer.Seek((int) _dataSizePos, SeekOrigin.Begin);
                    _writer.Write((int) Length);
                }
            }
            finally
            {
                lock (_locker)
                {
                    _writer.Dispose();
                    _writer = null;
                }

                _ofstream.Dispose(); // will close the underlying base stream
                _ofstream = null;
            }
        }

        /// <summary>
        /// Finaliser - should only be called if the User forgot to close this WaveFileWriter
        /// </summary>
        ~WaveFileWriter() { Dispose(false); }
        #endregion
    }
}
#endif