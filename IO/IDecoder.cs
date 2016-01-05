using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public interface IDecoder : IDisposable
    {
        int Handle { get; }

        bool HasData { get; }
        long Position { get; set; }

        void Reset();
        void Write(IAudioFileWriter Writer, int Offset = 0);
    }

    class BassDecoder : IDecoder
    {
        IDisposable Owner;

        public BassDecoder(int Handle, IDisposable Owner) 
        {
            this.Handle = Handle;
            this.Owner = Owner;
        }

        public bool HasData { get { return Bass.IsChannelActive(Handle) == PlaybackState.Playing; } }

        public int Handle { get; private set; }

        ~BassDecoder() { Dispose(); }

        /// <summary>
        /// Position in Bytes
        /// </summary>
        public long Position
        {
            get { return Bass.GetChannelPosition(Handle); }
            set { Bass.SetChannelPosition(Handle, value); }
        }

        public void Reset() { Position = 0; }

        /// <summary>
        /// Writes all the Data in the decoder to a file
        /// </summary>
        /// <param name="Writer">Audio File Writer to write to</param>
        /// <param name="Offset">+ve for forward, -ve for backward</param>
        public void Write(IAudioFileWriter Writer, int Offset = 0)
        {
            long InitialPosition = Position;

            Position += Offset;

            int BlockLength = (int)Bass.ChannelSeconds2Bytes(Handle, 2);

            byte[] Buffer = new byte[BlockLength];

            var gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            while (HasData)
            {
                Bass.ChannelGetData(Handle, gch.AddrOfPinnedObject(), BlockLength);
                Writer.Write(Buffer, BlockLength);
            }

            gch.Free();

            Writer.Dispose();

            Position = InitialPosition;
        }

        public void Dispose()
        {
            if (Owner == null) return;

            try
            {
                Owner.Dispose();
                Owner = null;
            }
            catch { }
        }
    }
}
