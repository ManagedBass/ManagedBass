using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public abstract class Channel : IDisposable
    {
        #region Fields
        public bool IsDisposed { get; protected set; }

        int HCHANNEL;
        public virtual int Handle
        {
            get
            {
                if (!IsDisposed) return HCHANNEL;
                else throw new ObjectDisposedException(this.ToString());
            }
            protected set
            {
                if (value == 0) throw new Exception("Stream Creation Failed: " + Bass.LastError);
                HCHANNEL = value;

                Free_Delegate = new SyncProcedure(OnFree);
                Bass.ChannelSetSync(Handle, SyncFlags.Freed, 0, Free_Delegate, IntPtr.Zero);
            }
        }
        #endregion

        #region Events
        SyncProcedure Free_Delegate;

        void OnFree(int handle, int channel, int data, IntPtr User)
        {
            IsDisposed = true;
            if (Disposed != null) Disposed.Invoke(this, new EventArgs());
        }

        public event EventHandler Disposed;
        #endregion

        public double CPUUsage { get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.CPUUsage); } }

        protected Resolution BufferKind { get; private set; }

        protected Channel(bool IsDecoder, Resolution BufferKind = Resolution.Short)
        {
            this.IsDecoder = IsDecoder;
            this.BufferKind = BufferKind;

            IsDisposed = false;
        }

        public ChannelInfo Info { get { return Bass.ChannelInfo(Handle); } }

        #region Read
        int Read(object Buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            int Return = Bass.ChannelGetData(Handle, gch.AddrOfPinnedObject(), Length);

            gch.Free();

            return Return;
        }

        public virtual int Read(byte[] Buffer, int Length) { return Read(Buffer as object, Length); }

        public virtual byte[] ReadByte(int Length)
        {
            var Buffer = new byte[Length];

            Read(Buffer, Length);

            return Buffer;
        }

        public virtual int Read(float[] Buffer, int Length) { return Read(Buffer as object, Length); }

        public virtual float[] ReadFloat(int Length)
        {
            var Buffer = new float[Length / 4];

            Read(Buffer, Length);

            return Buffer;
        }
        #endregion

        public long Seconds2Bytes(double Seconds) { return Bass.ChannelSeconds2Bytes(Handle, Seconds); }

        public double Bytes2Seconds(long Bytes) { return Bass.ChannelBytes2Seconds(Handle, Bytes); }

        public bool Lock() { return Bass.LockChannel(Handle, true); }

        public bool Unlock() { return Bass.LockChannel(Handle, false); }

        public virtual void Dispose() { if (!IsDisposed) IsDisposed = Bass.StreamFree(Handle); }

        public override int GetHashCode() { return Handle; }

        public bool IsDecoder { get; private set; }

        public IDecoder Decoder { get; protected set; }

        public IPlayable Player { get; protected set; }
    }
}
