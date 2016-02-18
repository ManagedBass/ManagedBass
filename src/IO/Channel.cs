using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;
using System.Threading;

namespace ManagedBass
{
    public class Channel : IDisposable
    {
        #region Fields
        public bool IsDisposed { get; protected set; }

        SynchronizationContext syncContext;

        bool RestartOnNextPlayback = false;

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
                ChannelInfo info;

                if (!Bass.ChannelGetInfo(value, out info))
                    throw new ArgumentException("Invalid Channel Handle: " + value);

                // Populate info
                DefaultFrequency = info.Frequency;
                ChannelCount = info.Channels;
                Type = info.ChannelType;
                Plugin = info.Plugin;
                Resolution = info.Resolution;
                OriginalResolution = info.OriginalResolution;
                IsDecodingChannel = info.IsDecodingChannel;

                IsDisposed = false;

                HCHANNEL = value;

                Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, Free_Delegate);
                Bass.ChannelSetSync(Handle, SyncFlags.End, 0, End_Delegate);
                Bass.ChannelSetSync(Handle, SyncFlags.Stop, 0, Fail_Delegate);
            }
        }
        #endregion

        #region Events
        SyncProcedure Free_Delegate, End_Delegate, Fail_Delegate;

        void OnFree(int handle, int channel, int data, IntPtr User)
        {
            try
            {
                Dispose();
                IsDisposed = true;

                var handler = Disposed;

                if (handler != null)
                {
                    if (syncContext == null)
                        handler(this, new EventArgs());
                    else syncContext.Post(state => handler(this, new EventArgs()), null);
                }
            }
            catch { }
        }

        public event EventHandler Disposed;

        void OnMediaEnded(int handle, int channel, int data, IntPtr User)
        {
            var handler = MediaEnded;

            if (handler != null && !Loop)
            {
                if (syncContext == null)
                    handler(this, new EventArgs());
                else syncContext.Post(state => handler(this, new EventArgs()), null);
            }
        }

        /// <summary>
        /// Fired when the Media Playback Ends
        /// </summary>
        public event EventHandler MediaEnded;

        void OnMediaFailed(int handle, int channel, int data, IntPtr User)
        {
            var handler = MediaFailed;

            if (handler != null)
            {
                if (syncContext == null)
                    handler(this, new EventArgs());
                else syncContext.Post(state => handler(this, new EventArgs()), null);
            }
        }

        /// <summary>
        /// Fired when the Playback fails
        /// </summary>
        public event EventHandler MediaFailed;
        #endregion

        protected static BassFlags FlagGen(bool Decode, Resolution Resolution, BassFlags Default = BassFlags.Default)
        {
            var flags = Default | Resolution.ToBassFlag();
            if (Decode) flags |= BassFlags.Decode;

            return flags;
        }

        public double CPUUsage => Bass.ChannelGetAttribute(Handle, ChannelAttribute.CPUUsage);

        protected Channel()
        {
            syncContext = SynchronizationContext.Current;

            Free_Delegate = new SyncProcedure(OnFree);
            End_Delegate = new SyncProcedure(OnMediaEnded);
            Fail_Delegate = new SyncProcedure(OnMediaFailed);
        }

        public Channel(int Handle) : this() { this.Handle = Handle; }

        #region Info
        public int DefaultFrequency { get; private set; }
        public int ChannelCount { get; private set; }
        public ChannelType Type { get; private set; }
        public int Plugin { get; private set; }
        public int OriginalResolution { get; private set; }
        public bool IsDecodingChannel { get; private set; }

        public bool HasFlag(BassFlags Flag) => Bass.ChannelHasFlag(Handle, Flag);

        public bool AddFlag(BassFlags Flag) => Bass.ChannelAddFlag(Handle, Flag);

        public bool RemoveFlag(BassFlags Flag) => Bass.ChannelRemoveFlag(Handle, Flag);

        public Resolution Resolution { get; private set; }

        public bool IsMono => HasFlag(BassFlags.Mono);
        #endregion

        #region Read
        public virtual int Read(IntPtr Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(byte[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(float[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(short[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(int[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);
        #endregion

        public long Seconds2Bytes(double Seconds) => Bass.ChannelSeconds2Bytes(Handle, Seconds);

        public double Bytes2Seconds(long Bytes) => Bass.ChannelBytes2Seconds(Handle, Bytes);

        public bool Lock() => Bass.ChannelLock(Handle, true);

        public bool Unlock() => Bass.ChannelLock(Handle, false);

        public virtual void Dispose() { if (!IsDisposed) IsDisposed = Bass.StreamFree(Handle); }

        public override int GetHashCode() => Handle;

        #region Decoding
        public bool DecoderHasData => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        /// <summary>
        /// Writes all the Data in the decoder to a file
        /// </summary>
        /// <param name="Writer">Audio File Writer to write to</param>
        /// <param name="Offset">+ve for forward, -ve for backward</param>
        public void DecodeToFile(IAudioFileWriter Writer, int Offset = 0)
        {
            if (!IsDecodingChannel)
                throw new InvalidOperationException("Not a Decoding Channel!");

            double InitialPosition = Position;

            Position += Offset;

            int BlockLength = (int)Bass.ChannelSeconds2Bytes(Handle, 2);

            byte[] Buffer = new byte[BlockLength];

            var gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            while (DecoderHasData)
            {
                int BytesReceived = Bass.ChannelGetData(Handle, gch.AddrOfPinnedObject(), BlockLength);
                Writer.Write(Buffer, BytesReceived);
            }

            gch.Free();

            Writer.Dispose();

            Position = InitialPosition;
        }
        #endregion

        #region Playable
        public virtual bool Start()
        {
            bool Result = Bass.ChannelPlay(Handle, RestartOnNextPlayback);
            if (Result) RestartOnNextPlayback = false;
            return Result;
        }

        public bool IsPlaying => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        public virtual bool Pause() => Bass.ChannelPause(Handle);

        public virtual bool Stop()
        {
            RestartOnNextPlayback = true;
            return Bass.ChannelStop(Handle);
        }

        /// <summary>
        /// Gets or Sets the Playback Frequency in Hertz.
        /// Default is 44100 Hz.
        /// </summary>
        public double Frequency
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Frequency); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Frequency, value); }
        }

        /// <summary>
        /// Gets or Sets Balance (Panning) (-1 ... 0 ... 1).
        /// -1 Represents Completely Left.
        ///  1 Represents Completely Right.
        /// Default is 0.
        /// </summary>
        public double Balance
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Pan); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pan, value); }
        }
        #endregion

        public PlaybackDevice Device
        {
            get { return PlaybackDevice.Get(Bass.ChannelGetDevice(Handle)); }
            set
            {
                value.Init();
                Bass.ChannelSetDevice(Handle, value.DeviceIndex);
            }
        }
        
        public double Volume
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Volume); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, value); }
        }

        public double Position
        {
            get { return Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetPosition(Handle)); }
            set { Bass.ChannelSetPosition(Handle, Bass.ChannelSeconds2Bytes(Handle, value)); }
        }

        public double Level => Bass.ChannelGetLevel(Handle);

        public double Duration => Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetLength(Handle));

        public bool Loop
        {
            get { return HasFlag(BassFlags.Loop); }
            set
            {
                if (value && !Loop) AddFlag(BassFlags.Loop);
                else if (!value && Loop) RemoveFlag(BassFlags.Loop);
            }
        }
        
        public void Link(int target) => Bass.ChannelSetLink(Handle, target);

        public bool IsSliding(ChannelAttribute attrib) => Bass.ChannelIsSliding(Handle, attrib);

        public bool Slide(ChannelAttribute attrib, double Value, int Time) => Bass.ChannelSlideAttribute(Handle, attrib, (float)Value, Time);
    }
}
