using System;
using System.Threading;

namespace ManagedBass
{
    /// <summary>
    /// Represents a Bass Channel.
    /// </summary>
    public class Channel : IDisposable
    {
        #region Fields
        readonly SynchronizationContext _syncContext;

        int _handle;

        /// <summary>
        /// Gets the Channel Handle.
        /// </summary>
        public int Handle
        {
            get { return _handle; }
            protected set
            {
                ChannelInfo info;

                if (!Bass.ChannelGetInfo(value, out info))
                    throw new ArgumentException("Invalid Channel Handle: " + value);

                _handle = value;

                // Init Events
                Syncs = new SyncCollection(this)
                {
                    new SyncParameters
                    {
                        Type = SyncFlags.Free,
                        Callback = (Sync, Data) => Disposed?.Invoke(this, EventArgs.Empty)
                    },
                    new SyncParameters
                    {
                        Type = SyncFlags.End,
                        Callback = (Sync, Data) =>
                        {
                            if (!Flags.Has(BassFlags.Loop))
                                MediaEnded?.Invoke(this, EventArgs.Empty);
                        }
                    },
                    new SyncParameters
                    {
                        Type = SyncFlags.Stop,
                        Callback = (Sync, Data) => MediaFailed?.Invoke(this, EventArgs.Empty)
                    }
                };
            }
        }

        bool _restartOnNextPlayback;
        #endregion

        public SyncCollection Syncs { get; private set; }
        
        #region Events
        void OnFree(int handle, int channel, int data, IntPtr User)
        {
            try
            {
                _handle = 0;

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
        
        /// <summary>
        /// Fired when the Media Playback Ends
        /// </summary>
        public event EventHandler MediaEnded;
        
        /// <summary>
        /// Fired when the Playback fails
        /// </summary>
        public event EventHandler MediaFailed;
        #endregion

        #region Construction
        static Channel()
        {
            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);
        }

        /// <summary>
        /// Parameterless Protected Constructor.
        /// </summary>
        protected Channel()
        {
            _syncContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Channel"/> from a Handle.
        /// </summary>
        public Channel(int Handle)
        {
            this.Handle = Handle;
        }
        #endregion

        /// <summary>
        /// Gets or Sets the Channel's Attributes.
        /// </summary>
        public double this[ChannelAttribute Attribute]
        {
            get { return Bass.ChannelGetAttribute(Handle, Attribute); }
            set { Bass.ChannelSetAttribute(Handle, Attribute, value); }
        }

        #region Properties
        /// <summary>
        /// Gets the Channels CPU Usage percentage.
        /// </summary>
        public double CPUUsage => Bass.ChannelGetAttribute(Handle, ChannelAttribute.CPUUsage);

        /// <summary>
        /// Gets or Sets the device used to decode/play the Channel.
        /// </summary>
        public virtual PlaybackDevice Device
        {
            get { return PlaybackDevice.GetByIndex(Bass.ChannelGetDevice(Handle)); }
            set
            {
                if (!value.Info.IsInitialized)
                    value.Init();

                Bass.ChannelSetDevice(Handle, value.Index);
            }
        }

        /// <summary>
        /// Gets more information about the Channel.
        /// </summary>
        public ChannelInfo Info => Bass.ChannelGetInfo(Handle);

        /// <summary>
        /// Gets the Playback State of the Channel.
        /// </summary>
        public PlaybackState IsActive => Bass.ChannelIsActive(Handle);

        FlagCollection _flags;

        /// <summary>
        /// Gets the <see cref="FlagCollection"/> for the Channel which can be used to get/add/remove flags.
        /// </summary>
        public FlagCollection Flags => _flags ?? (_flags = new FlagCollection(this));

        /// <summary>
        /// Gets the Channel's level as an average of left and right levels.
        /// </summary>
        public double Level
        {
            get
            {
                var l = Bass.ChannelGetLevel(Handle);
                return (l.LoWord() + l.HiWord()) / (2.0 * 32768);
            }
        }
        #endregion

        #region Playback
        /// <summary>
        /// Starts the Channel Playback.
        /// </summary>
        public virtual bool Play(bool Restart = false)
        {
            var result = Bass.ChannelPlay(Handle, _restartOnNextPlayback || Restart);

            if (result)
                _restartOnNextPlayback = false;

            return result;
        }

        /// <summary>
        /// Pauses the Channel Playback.
        /// </summary>
        public virtual bool Pause() => Bass.ChannelPause(Handle);

        /// <summary>
        /// Stops the Channel Playback.
        /// </summary>
        /// <remarks>Difference from <see cref="Bass.ChannelStop"/>: Playback is restarted when <see cref="Play"/> is called.</remarks>
        public virtual bool Stop()
        {
            _restartOnNextPlayback = true;
            return Bass.ChannelStop(Handle);
        }
        #endregion

        /// <summary>
        /// Locks/Unlocks the Channel to the Current thread.
        /// </summary>
        public bool Lock(bool Lock = true) => Bass.ChannelLock(Handle, Lock);

        #region Link
        /// <summary>
        /// Links to another channel (one-directional) for simultaneous playback starting.
        /// </summary>
        public bool Link(Channel Channel) => Bass.ChannelSetLink(Handle, Channel.Handle);

        /// <summary>
        /// Removes the link previously set using <see cref="Link"/>.
        /// </summary>
        /// <param name="Channel"></param>
        /// <returns></returns>
        public bool Unlink(Channel Channel) => Bass.ChannelRemoveLink(Handle, Channel.Handle);
        #endregion

        #region Length
        public long GetLength(PositionFlags Mode = PositionFlags.Bytes) => Bass.ChannelGetLength(Handle, Mode);

        public TimeSpan Duration => TimeSpan.FromSeconds(Bytes2Seconds(GetLength()));
        #endregion

        #region Conversion
        public double Bytes2Seconds(long Bytes) => Bass.ChannelBytes2Seconds(Handle, Bytes);

        public long Seconds2Bytes(double Seconds) => Bass.ChannelSeconds2Bytes(Handle, Seconds);
        #endregion

        #region Position
        public long GetPosition(PositionFlags Mode = PositionFlags.Bytes) => Bass.ChannelGetPosition(Handle, Mode);

        public bool Seek(long Position, PositionFlags Mode = PositionFlags.Bytes) => Bass.ChannelSetPosition(Handle, Position, Mode);

        public TimeSpan Position
        {
            get { return TimeSpan.FromSeconds(Bytes2Seconds(GetPosition())); }
            set { Seek(Seconds2Bytes(value.TotalSeconds)); }
        }
        #endregion

        #region Slide
        public bool IsSliding(ChannelAttribute Attribute) => Bass.ChannelIsSliding(Handle, Attribute);

        public bool SlideAttribute(ChannelAttribute Attribute, double Value, int Time)
        {
            return Bass.ChannelSlideAttribute(Handle, Attribute, (float)Value, Time);
        }
        #endregion

        public bool Update(int Length) => Bass.ChannelUpdate(Handle, Length);

        public virtual void Dispose()
        {
            if (Bass.StreamFree(Handle))
                _handle = 0;
        }

        /// <summary>
        /// Gets the Hash Code for the Current instance.
        /// </summary>
        public override int GetHashCode() => Handle;
        
        #region GetData
        public virtual int GetData(IntPtr Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int GetData(byte[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int GetData(float[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);

        public virtual int GetData(short[] Buffer, int Length) => Bass.ChannelGetData(Handle, Buffer, Length);
        #endregion
    }
}