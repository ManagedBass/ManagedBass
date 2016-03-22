using System;
using System.Runtime.InteropServices;
using System.Threading;
using static ManagedBass.Bass;

namespace ManagedBass
{
    /// <summary>
    /// Represents a Bass Channel. Base class for all Streams, Music and Samples.
    /// </summary>
    public class Channel : IDisposable
    {
        #region Fields
        SynchronizationContext syncContext;

        bool RestartOnNextPlayback = false;

        int HCHANNEL;
        public virtual int Handle
        {
            get { return HCHANNEL; }
            protected set
            {
                ChannelInfo info;

                if (!ChannelGetInfo(value, out info))
                    throw new ArgumentException("Invalid Channel Handle: " + value);

                // Populate info
                DefaultFrequency = info.Frequency;
                ChannelCount = info.Channels;
                Type = info.ChannelType;
                Plugin = info.Plugin;
                Resolution = info.Resolution;
                OriginalResolution = info.OriginalResolution;
                IsDecodingChannel = info.IsDecodingChannel;
                
                HCHANNEL = value;

                // Init Events
                ChannelSetSync(Handle, SyncFlags.Free, 0, Free_Delegate);
                ChannelSetSync(Handle, SyncFlags.End, 0, End_Delegate);
                ChannelSetSync(Handle, SyncFlags.Stop, 0, Fail_Delegate);
            }
        }
        #endregion

        #region Events
        SyncProcedure Free_Delegate, End_Delegate, Fail_Delegate;

        void OnFree(int handle, int channel, int data, IntPtr User)
        {
            try
            {
                HCHANNEL = 0;

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

        public double CPUUsage => ChannelGetAttribute(Handle, ChannelAttribute.CPUUsage);

        static Channel()
        {
            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);
        }

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

        public bool HasFlag(BassFlags Flag) => ChannelHasFlag(Handle, Flag);

        public bool AddFlag(BassFlags Flag) => ChannelAddFlag(Handle, Flag);

        public bool RemoveFlag(BassFlags Flag) => ChannelRemoveFlag(Handle, Flag);

        public Resolution Resolution { get; private set; }

        public bool IsMono => HasFlag(BassFlags.Mono);
        #endregion

        #region Read
        public virtual int Read(IntPtr Buffer, int Length) => ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(byte[] Buffer, int Length) => ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(float[] Buffer, int Length) => ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(short[] Buffer, int Length) => ChannelGetData(Handle, Buffer, Length);

        public virtual int Read(int[] Buffer, int Length) => ChannelGetData(Handle, Buffer, Length);
        #endregion

        public long Seconds2Bytes(double Seconds) => ChannelSeconds2Bytes(Handle, Seconds);

        public double Bytes2Seconds(long Bytes) => ChannelBytes2Seconds(Handle, Bytes);

        public bool Lock() => ChannelLock(Handle, true);

        public bool Unlock() => ChannelLock(Handle, false);

        public virtual void Dispose() 
        { 
            if (HCHANNEL != 0 && StreamFree(HCHANNEL))
                HCHANNEL = 0;
        }

        public override int GetHashCode() => Handle;

        #region Decoding
        public bool DecoderHasData => ChannelIsActive(Handle) == PlaybackState.Playing;

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

            int BlockLength = (int)ChannelSeconds2Bytes(Handle, 2);

            byte[] Buffer = new byte[BlockLength];

            var gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            while (DecoderHasData)
            {
                int BytesReceived = ChannelGetData(Handle, gch.AddrOfPinnedObject(), BlockLength);
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
            bool Result = ChannelPlay(Handle, RestartOnNextPlayback);
            if (Result) RestartOnNextPlayback = false;
            return Result;
        }

        public bool IsPlaying => ChannelIsActive(Handle) == PlaybackState.Playing;

        public virtual bool Pause() => ChannelPause(Handle);

        public virtual bool Stop()
        {
            RestartOnNextPlayback = true;
            return ChannelStop(Handle);
        }

        /// <summary>
        /// Gets or Sets the Playback Frequency in Hertz.
        /// Default is 44100 Hz.
        /// </summary>
        public virtual double Frequency
        {
            get { return ChannelGetAttribute(Handle, ChannelAttribute.Frequency); }
            set { ChannelSetAttribute(Handle, ChannelAttribute.Frequency, value); }
        }

        /// <summary>
        /// Gets or Sets Balance (Panning) (-1 ... 0 ... 1).
        /// -1 Represents Completely Left.
        ///  1 Represents Completely Right.
        /// Default is 0.
        /// </summary>
        public virtual double Balance
        {
            get { return ChannelGetAttribute(Handle, ChannelAttribute.Pan); }
            set { ChannelSetAttribute(Handle, ChannelAttribute.Pan, value); }
        }
        #endregion

        public virtual PlaybackDevice Device
        {
            get { return PlaybackDevice.Get(ChannelGetDevice(Handle)); }
            set
            {
                if (!value.DeviceInfo.IsInitialized)
                    value.Init();
                ChannelSetDevice(Handle, value.DeviceIndex);
            }
        }
        
        public virtual double Volume
        {
            get { return ChannelGetAttribute(Handle, ChannelAttribute.Volume); }
            set { ChannelSetAttribute(Handle, ChannelAttribute.Volume, value); }
        }

        public virtual double Position
        {
            get { return ChannelBytes2Seconds(Handle, ChannelGetPosition(Handle)); }
            set { ChannelSetPosition(Handle, ChannelSeconds2Bytes(Handle, value)); }
        }

        public double Level => ChannelGetLevel(Handle);

        public virtual double Duration => ChannelBytes2Seconds(Handle, ChannelGetLength(Handle));

        public virtual bool Loop
        {
            get { return HasFlag(BassFlags.Loop); }
            set
            {
                if (value && !Loop) AddFlag(BassFlags.Loop);
                else if (!value && Loop) RemoveFlag(BassFlags.Loop);
            }
        }
        
        public void Link(int target) => ChannelSetLink(Handle, target);

        public bool IsSliding(ChannelAttribute attrib) => ChannelIsSliding(Handle, attrib);

        public bool Slide(ChannelAttribute attrib, double Value, int Time) => ChannelSlideAttribute(Handle, attrib, (float)Value, Time);
    }
}
