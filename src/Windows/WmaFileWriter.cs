using System;
using System.IO;
using System.Runtime.InteropServices;

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
                return WMAEncodeFlags.Byte;;
            if (WfTag == WaveFormatTag.IeeeFloat && BitsPerSample == 32)
                return WMAEncodeFlags.Float;

            return 0;
        }

        readonly Stream _outStream;
        readonly int _handle;

        public WmaFileWriter(string FileName, WaveFormat WaveFormat, int BitRate = 128000)
        {
            _handle = BassWma.EncodeOpenFile(WaveFormat.SampleRate, WaveFormat.Channels, ToWmaEncodeFlags(WaveFormat.Encoding, WaveFormat.BitsPerSample), BitRate, FileName);
        }
        
        public WmaFileWriter(Stream OutStream, WaveFormat WaveFormat, int BitRate = 128000)
        {
            _outStream = OutStream;

            if (!OutStream.CanWrite || !OutStream.CanSeek)
                throw new ArgumentException("Expected and Writable and Seekable Stream", nameof(OutStream));

            _handle = BassWma.EncodeOpen(WaveFormat.SampleRate, WaveFormat.Channels, ToWmaEncodeFlags(WaveFormat.Encoding, WaveFormat.BitsPerSample), BitRate, WMStreamProc);
        }
        
        byte[] _buffer;
        
        void WMStreamProc(int Handle, WMEncodeType WmEncodeType, IntPtr Ptr, int Length, IntPtr User)
        {
            if (Length > 0)
            {
                if (_buffer == null || _buffer.Length < Length)
                    _buffer = new byte[Length];

                Marshal.Copy(Ptr, _buffer, 0, Length);
            }

            switch (WmEncodeType)
            {
                case WMEncodeType.Header:
                    _outStream.Seek(0, SeekOrigin.Begin);
                    _outStream.Write(_buffer, 0, Length);
                    break;

                case WMEncodeType.Data:
                    _outStream.Write(_buffer, 0, Length);
                    break;
            }
        }

        #region Write
        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Buffer">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public bool Write(byte[] Buffer, int Length) => BassWma.EncodeWrite(_handle, Buffer, Length);

        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Buffer">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        public bool Write(short[] Buffer, int Length) => BassWma.EncodeWrite(_handle, Buffer, Length);

        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Buffer">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        public bool Write(float[] Buffer, int Length) => BassWma.EncodeWrite(_handle, Buffer, Length);
        #endregion

        /// <summary>
        /// Frees all resources used by the writer.
        /// </summary>
        public void Dispose() => Stop();
        
        public bool Stop() => BassWma.EncodeClose(_handle);
    }
}
