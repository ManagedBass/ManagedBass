using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    public class AcmFileWriter : IAudioWriter
    {
        static BassFlags ToBassFlags(WaveFormatTag WfTag, int BitsPerSample)
        {
            if (WfTag == WaveFormatTag.Pcm && BitsPerSample == 8)
                return BassFlags.Byte;
            if (WfTag == WaveFormatTag.IeeeFloat && BitsPerSample == 32)
                return BassFlags.Float;

            return 0;
        }

        static int GetDummyChannel(WaveFormat Format)
        {
            return Bass.CreateStream(Format.SampleRate, Format.Channels, BassFlags.Decode | ToBassFlags(Format.Encoding, Format.BitsPerSample), StreamProcedureType.Push);
        }
        
        readonly int _channel, _handle;
        readonly WaveFormatTag _encoding;

        readonly EncodeProcedure _encodeProcedure;
        readonly Stream _stream;
        byte[] _buffer;
        
        public AcmFileWriter(string FileName, WaveFormatTag Encoding, WaveFormat Format)
        {
            if (FileName == null)
                throw new ArgumentNullException(nameof(FileName));

            _channel = GetDummyChannel(Format);
            _encoding = Encoding;
            _handle = OnStart(AcmFormat => BassEnc.EncodeStartACM(_channel, AcmFormat, 0, FileName));
        }

        public AcmFileWriter(Stream OutStream, WaveFormatTag Encoding, WaveFormat Format)
        {
            _channel = GetDummyChannel(Format);
            _encoding = Encoding;
            _stream = OutStream;
            _encodeProcedure = EncodeProcedure;
            _handle = OnStart(AcmFormat => BassEnc.EncodeStartACM(_channel, AcmFormat, 0, _encodeProcedure));
        }

        void EncodeProcedure(int Handle, int Channel, IntPtr Ptr, int Length, IntPtr User)
        {
            if (_buffer == null || _buffer.Length < Length)
                _buffer = new byte[Length];

            Marshal.Copy(Ptr, _buffer, 0, Length);

            _stream.Write(_buffer, 0, Length);
        }

        int OnStart(Func<IntPtr, int> Starter)
        {
            // Get the Length of the ACMFormat structure
            var suggestedFormatLength = BassEnc.GetACMFormat(0);
            var acmFormat = Marshal.AllocHGlobal(suggestedFormatLength);

            try
            {
                // Retrieve ACMFormat and Init Encoding
                if (BassEnc.GetACMFormat(_channel,
                     acmFormat,
                     suggestedFormatLength,
                     null,
                     // If encoding is Unknown, then let the User choose encoding.
                     _encoding == WaveFormatTag.Unknown ? 0 : ACMFormatFlags.Suggest,
                     _encoding) != 0)
                    return Starter(acmFormat);

            }
            finally
            {
                // Free the ACMFormat structure
                Marshal.FreeHGlobal(acmFormat);
            }

            throw new Exception();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => BassEnc.EncodeStop(_handle, true);

        #region Write
        public bool Write(byte[] Buffer, int Length)
        {
            Bass.StreamPutData(_channel, Buffer, Length);

            Bass.ChannelGetData(_channel, Buffer, Length);

            return true;
        }

        public bool Write(short[] Buffer, int Length)
        {
            Bass.StreamPutData(_channel, Buffer, Length);

            Bass.ChannelGetData(_channel, Buffer, Length);

            return true;
        }

        public bool Write(float[] Buffer, int Length)
        {
            Bass.StreamPutData(_channel, Buffer, Length);

            Bass.ChannelGetData(_channel, Buffer, Length);

            return true;
        }
        #endregion
    }
}