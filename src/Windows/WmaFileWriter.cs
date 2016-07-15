namespace ManagedBass.Wma
{
    /// <summary>
    /// Writes a WMA File. Requires BassWma.dll
    /// </summary>
    public class WmaFileWriter : IAudioWriter
    {
        static WMAEncodeFlags ToWmaEncodeFlags(WaveFormatTag WfTag, int BitsPerSample)
        {
            if (WfTag == WaveFormatTag.Pcm && BitsPerSample == 8)
                return WMAEncodeFlags.Byte;
            if (WfTag == WaveFormatTag.IeeeFloat && BitsPerSample == 32)
                return WMAEncodeFlags.Float;

            return 0;
        }

        readonly object _syncLock = new object();
        readonly int _handle;

        public WmaFileWriter(string FileName, WaveFormat WaveFormat, int BitRate = 128000)
        {
            _handle = BassWma.EncodeOpenFile(WaveFormat.SampleRate, WaveFormat.Channels, ToWmaEncodeFlags(WaveFormat.Encoding, WaveFormat.BitsPerSample), BitRate, FileName);
        }
        
        #region Write
        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Buffer">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public bool Write(byte[] Buffer, int Length)
        {
            lock (_syncLock)
                return BassWma.EncodeWrite(_handle, Buffer, Length);
        }

        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Buffer">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        public bool Write(short[] Buffer, int Length)
        {
            lock (_syncLock)
                return BassWma.EncodeWrite(_handle, Buffer, Length);
        }

        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Buffer">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        public bool Write(float[] Buffer, int Length)
        {
            lock (_syncLock)
                return BassWma.EncodeWrite(_handle, Buffer, Length);
        }
        #endregion

        /// <summary>
        /// Frees all resources used by the writer.
        /// </summary>
        public void Dispose() => BassWma.EncodeClose(_handle);
    }
}
