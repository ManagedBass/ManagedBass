using System;
using System.IO;
using System.Runtime.InteropServices;
using ManagedBass.Tags;

namespace ManagedBass.Enc
{
    public abstract class BassEncEncoder : IEncoder
    {
        protected static int GetDummyChannel(WaveFormat Format)
        {
            return Bass.CreateStream(Format.SampleRate, Format.Channels, BassFlags.Decode | ToBassFlags(Format.Encoding, Format.Channels), StreamProcedureType.Push);
        }

        static BassFlags ToBassFlags(WaveFormatTag WfTag, int Channels)
        {
            if (WfTag == WaveFormatTag.Pcm && Channels == 16)
                return BassFlags.Default;
            if (WfTag == WaveFormatTag.Pcm && Channels == 8)
                return BassFlags.Byte;
            if (WfTag == WaveFormatTag.IeeeFloat && Channels == 32)
                return BassFlags.Float;

            throw new ArgumentException(nameof(WfTag));
        }

        readonly EncodeNotifyProcedure _notifyProcedure;
        protected readonly EncodeProcedure _encodeProcedure;
        readonly Stream _stream;
        byte[] _buffer;
        
        internal BassEncEncoder()
        {
            _notifyProcedure = NotifyProcedure;
        }

        internal BassEncEncoder(Stream Stream) : this()
        {
            _encodeProcedure = EncodeProcedure;
            _stream = Stream;
        }

        void EncodeProcedure(int Handle, int Channel, IntPtr Ptr, int Length, IntPtr User)
        {
            if (_buffer == null || _buffer.Length < Length)
                _buffer = new byte[Length];

            Marshal.Copy(Ptr, _buffer, 0, Length);

            _stream.Write(_buffer, 0, Length);
        }

        public int Channel => BassEnc.EncodeGetChannel(Handle);

        int _handle;
        public int Handle
        {
            get { return _handle; }
            private set
            {
                BassEnc.EncodeSetNotify(_handle, null);

                _handle = value;

                if (value == 0)
                    return;

                BassEnc.EncodeSetNotify(_handle, _notifyProcedure);
            }
        }
        
        void NotifyProcedure(int handle, EncodeNotifyStatus status, IntPtr user) => Notification?.Invoke(status);

        public bool AddChunk(string ID, byte[] buffer, int length) => BassEnc.EncodeAddChunk(Handle, ID, buffer, length);

        public long GetCount(EncodeCount Type) => BassEnc.EncodeGetCount(Handle, Type);

        /// <summary>
        /// Encodes all the available data in a Decoding Channel.
        /// </summary>
        public void FlushDecoder()
        {
            if (!Bass.ChannelGetInfo(Channel).IsDecodingChannel)
                throw new InvalidOperationException("Not a Decoding Channel");
            
            var blockLength = (int)Bass.ChannelSeconds2Bytes(Channel, 2);
            
            // while Decoder has Data
            while (Bass.ChannelIsActive(Channel) == PlaybackState.Playing)
                Bass.ChannelGetData(Channel, IntPtr.Zero, blockLength);
            
            BassEnc.EncodeStop(Handle);
        }

        public event Action<EncodeNotifyStatus> Notification;
        
        #region IEncoder
        public bool CanPause => true;

        public bool IsActive => BassEnc.EncodeIsActive(Handle) == PlaybackState.Playing;

        public bool IsPaused
        {
            get { return BassEnc.EncodeIsActive(Handle) == PlaybackState.Paused; }
            set { BassEnc.EncodeSetPaused(Handle, value); }
        }

        public abstract string OutputFileExtension { get; }

        public abstract ChannelType OutputType { get; }

        public TagReader Tags { get; set; }

        public void Dispose()
        {
            if (BassEnc.EncodeStop(Handle, true))
                Handle = 0;
        }
        
        public abstract int OnStart();

        public bool Start()
        {
            if (Handle == 0)
                return (Handle = OnStart()) != 0;

            return true;
        }

        public bool Stop()
        {
            var result = BassEnc.EncodeStop(Handle);

            if (result)
                Handle = 0;

            return result;
        }

        public bool Write(byte[] Buffer, int Length)
        {
            Bass.StreamPutData(Channel, Buffer, Length);

            Bass.ChannelGetData(Channel, Buffer, Length);

            return true;
        }

        public bool Write(short[] Buffer, int Length)
        {
            Bass.StreamPutData(Channel, Buffer, Length);

            Bass.ChannelGetData(Channel, Buffer, Length);

            return true;
        }

        public bool Write(float[] Buffer, int Length)
        {
            Bass.StreamPutData(Channel, Buffer, Length);

            Bass.ChannelGetData(Channel, Buffer, Length);

            return true;
        }
        
        public bool Write(IntPtr Buffer, int Length)
        {
            Bass.StreamPutData(Channel, Buffer, Length);

            Bass.ChannelGetData(Channel, Buffer, Length);

            return true;
        }

        public virtual int OutputBitRate => -1;
        #endregion
    }
}
