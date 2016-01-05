using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    class BassPlayer : IPlayable
    {
        IDisposable Owner;
        bool RestartOnNextPlayback = false;

        ~BassPlayer() { Dispose(); }

        public int Handle { get; private set; }

        public bool Start()
        {
            bool Result = Bass.PlayChannel(Handle, RestartOnNextPlayback);
            if (Result) RestartOnNextPlayback = false;
            return Result;
        }

        public BassPlayer(int Handle, IDisposable Owner)
        {
            this.Handle = Handle;
            this.Owner = Owner;

            // Media Ended
            End_Delegate = new SyncProcedure(OnMediaEnded);
            Bass.ChannelSetSync(Handle, SyncFlags.End, 0, End_Delegate);

            // Media Failed
            Fail_Delegate = new SyncProcedure(OnMediaFailed);
            Bass.ChannelSetSync(Handle, SyncFlags.Stop, 0, Fail_Delegate);
        }

        public PlaybackDevice Device
        {
            get { return PlaybackDevice.Get(Bass.ChannelGetDevice(Handle)); }
            set
            {
                value.Initialize();
                Bass.ChannelSetDevice(Handle, value.DeviceIndex);
            }
        }

        public bool IsPlaying { get { return Bass.IsChannelActive(Handle) == PlaybackState.Playing; } }

        public bool Pause() { return Bass.PauseChannel(Handle); }

        public bool Stop()
        {
            RestartOnNextPlayback = true;
            return Bass.StopChannel(Handle);
        }

        #region Events
        SyncProcedure End_Delegate, Fail_Delegate;

        void OnMediaEnded(int handle, int channel, int data, IntPtr User) { if (MediaEnded != null && !Loop) MediaEnded.Invoke(this, new EventArgs()); }

        void OnMediaFailed(int handle, int channel, int data, IntPtr User) { if (MediaFailed != null) MediaFailed.Invoke(this, new EventArgs()); }

        /// <summary>
        /// Fired when the Media Playback Ends
        /// </summary>
        public event EventHandler MediaEnded;

        /// <summary>
        /// Fired when the Playback fails
        /// </summary>
        public event EventHandler MediaFailed;
        #endregion

        public double Volume
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.Volume); }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.Volume, value); }
        }

        public double Position
        {
            get { return Bass.ChannelBytes2Seconds(Handle, Bass.GetChannelPosition(Handle)); }
            set { Bass.SetChannelPosition(Handle, Bass.ChannelSeconds2Bytes(Handle, value)); }
        }

        public double Level { get { return Bass.GetChannelLevel(Handle); } }

        /// <summary>
        /// Gets or Sets the Playback Frequency in Hertz.
        /// Default is 44100 Hz.
        /// </summary>
        public double Frequency
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.Frequency); }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.Frequency, value); }
        }

        public double Duration { get { return Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetLength(Handle)); } }

        public bool Loop
        {
            get { return Bass.ChannelHasFlag(Handle, BassFlags.Loop); }
            set
            {
                if (value && !Loop) Bass.ChannelAddFlag(Handle, BassFlags.Loop);
                else if (!value && Loop) Bass.ChannelRemoveFlag(Handle, BassFlags.Loop);
            }
        }

        public bool IsMono { get { return Bass.ChannelHasFlag(Handle, BassFlags.Mono); } }

        /// <summary>
        /// Gets or Sets Balance (Panning) (-1 ... 0 ... 1).
        /// -1 Represents Completely Left.
        ///  1 Represents Completely Right.
        /// Default is 0.
        /// </summary>
        public double Balance
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.Pan); }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.Pan, value); }
        }

        public void Link(int target) { Bass.LinkChannels(Handle, target); }

        public bool IsSliding(ChannelAttribute attrib) { return Bass.IsChannelSliding(Handle, attrib); }

        public bool Slide(ChannelAttribute attrib, double Value, int Time) { return Bass.SlideChannelAttribute(Handle, attrib, (float)Value, Time); }

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
