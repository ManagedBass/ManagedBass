using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ManagedBass
{
    /// <summary>
    /// Represents a Bass Channel. Base class for all Streams, Music and Samples.
    /// </summary>
    public class Channel : IDisposable
    {
        #region Fields
        readonly SynchronizationContext _syncContext;

        bool _restartOnNextPlayback;

        int _hchannel;
        
        /// <summary>
        /// Gets the Channel Handle.
        /// </summary>
        public virtual int Handle
        {
            get { return _hchannel; }
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
                
                _hchannel = value;

                // Init Events
                Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, OnFree);
                Bass.ChannelSetSync(Handle, SyncFlags.End, 0, OnMediaEnded);
                Bass.ChannelSetSync(Handle, SyncFlags.Stop, 0, OnMediaFailed);
            }
        }
        #endregion

        #region Events
        void OnFree(int handle, int channel, int data, IntPtr User)
        {
            try
            {
                _hchannel = 0;

                var handler = Disposed;

                if (handler == null)
                    return;
                if (_syncContext == null)
                    handler(this, new EventArgs());
                else _syncContext.Post(state => handler(this, new EventArgs()), null);
            }
            catch { }
        }

        /// <summary>
        /// Fired when this Channel is Disposed.
        /// </summary>
        public event EventHandler Disposed;

        void OnMediaEnded(int handle, int channel, int data, IntPtr User)
        {
            var handler = MediaEnded;

            if (handler == null || Loop)
                return;
            if (_syncContext == null)
                handler(this, new EventArgs());
            else _syncContext.Post(state => handler(this, new EventArgs()), null);
        }

        /// <summary>
        /// Fired when the Media Playback Ends
        /// </summary>
        public event EventHandler MediaEnded;

        void OnMediaFailed(int handle, int channel, int data, IntPtr User)
        {
            var handler = MediaFailed;

            if (handler == null)
                return;
            if (_syncContext == null)
                handler(this, new EventArgs());
            else _syncContext.Post(state => handler(this, new EventArgs()), null);
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

        static Channel()
        {
            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);
        }

        protected Channel()
        {
            _syncContext = SynchronizationContext.Current;
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

        public bool Lock() => Bass.ChannelLock(Handle);

        public bool Unlock() => Bass.ChannelLock(Handle, false);

        public virtual void Dispose() 
        { 
            if (_hchannel != 0 && Bass.StreamFree(_hchannel))
                _hchannel = 0;
        }

        public override int GetHashCode() => Handle;

        #region Decoding
        public bool DecoderHasData => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        /// <summary>
        /// Writes all the Data in the decoder to a file
        /// </summary>
        /// <param name="Writer">Audio File Writer to write to</param>
        /// <param name="Offset">+ve for forward, -ve for backward</param>
        public void DecodeToFile(IAudioWriter Writer, int Offset = 0)
        {
            if (!IsDecodingChannel)
                throw new InvalidOperationException("Not a Decoding Channel!");

            var InitialPosition = Position;

            Position += Offset;

            var BlockLength = (int) Bass.ChannelSeconds2Bytes(Handle, 2);

            var Buffer = new byte[BlockLength];

            var gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            while (DecoderHasData)
            {
                var BytesReceived = Bass.ChannelGetData(Handle, gch.AddrOfPinnedObject(), BlockLength);
                Writer.Write(Buffer, BytesReceived);
            }

            gch.Free();

            Writer.Dispose();

            Position = InitialPosition;
        }
        #endregion

        #region Playable
        /// <summary>
        /// Starts the Channel Playback.
        /// </summary>
        public virtual bool Start()
        {
            var Result = Bass.ChannelPlay(Handle, _restartOnNextPlayback);
            if (Result) _restartOnNextPlayback = false;
            return Result;
        }

        /// <summary>
        /// Gets if the Channels is Playing.
        /// </summary>
        public bool IsPlaying => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        /// <summary>
        /// Pauses the Channel Playback.
        /// </summary>
        public virtual bool Pause() => Bass.ChannelPause(Handle);

        /// <summary>
        /// Stops the Channel Playback.
        /// </summary>
        public virtual bool Stop()
        {
            _restartOnNextPlayback = true;
            return Bass.ChannelStop(Handle);
        }

        /// <summary>
        /// Gets or Sets the Playback Frequency in Hertz.
        /// Default is 44100 Hz.
        /// </summary>
        public virtual double Frequency
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
        public virtual double Balance
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Pan); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pan, value); }
        }
        #endregion

        public virtual PlaybackDevice Device
        {
            get { return PlaybackDevice.Get(Bass.ChannelGetDevice(Handle)); }
            set
            {
                if (!value.DeviceInfo.IsInitialized)
                    value.Init();
                Bass.ChannelSetDevice(Handle, value.DeviceIndex);
            }
        }
        
        public virtual double Volume
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Volume); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, value); }
        }

        public virtual double Position
        {
            get { return Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetPosition(Handle)); }
            set { Bass.ChannelSetPosition(Handle, Bass.ChannelSeconds2Bytes(Handle, value)); }
        }

        public double Level => Bass.ChannelGetLevel(Handle);

        public virtual double Duration => Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetLength(Handle));

        public virtual bool Loop
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
