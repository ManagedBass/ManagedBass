using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Wma
{
    /// <summary>
    /// Writes a WMA File. Requires BassWma.dll
    /// </summary>
    public class WmaEncoder : IAudioWriter
    {
        readonly Stream _outStream;
        
        public WmaEncoder(string FileName, int Frequency, int Channels, WMAEncodeFlags Flags, int BitRate = 128000)
        {
            Handle = BassWma.EncodeOpenFile(Frequency, Channels, Flags, BitRate, FileName);
        }
        
        public WmaEncoder(int Port, int Clients, int Frequency, int Channels, WMAEncodeFlags Flags, int BitRate = 128000)
        {
            Handle = BassWma.EncodeOpenNetwork(Frequency, Channels, Flags, BitRate, Port, Clients);
        }

        public WmaEncoder(Stream OutStream, int Frequency, int Channels, WMAEncodeFlags Flags, int BitRate = 128000)
        {
            _outStream = OutStream;

            if (!OutStream.CanWrite || !OutStream.CanSeek)
                throw new ArgumentException("Expected and Writable and Seekable Stream", nameof(OutStream));

            Handle = BassWma.EncodeOpen(Frequency, Channels, Flags, BitRate, WMStreamProc);
        }

        public WmaEncoder(string Url, string UserName, string Password, int Frequency, int Channels, WMAEncodeFlags Flags, int BitRate = 128000)
        {
            Handle = BassWma.EncodeOpenPublish(Frequency, Channels, Flags, BitRate, Url, UserName, Password);
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
        /// Write data from an IntPtr.
        /// </summary>
        /// <param name="Buffer">IntPtr to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public bool Write(IntPtr Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);

        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Buffer">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public bool Write(byte[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);

        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Buffer">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        public bool Write(short[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);

        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Buffer">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        public bool Write(float[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);
        #endregion

        /// <summary>
        /// Frees all resources used by the writer.
        /// </summary>
        public void Dispose() => Stop();
        
        public bool Stop()
        {
            if (!BassWma.EncodeClose(Handle))
                return false;

            Handle = 0;
            return true;
        }

        public int Handle { get; private set; }
    }
}
