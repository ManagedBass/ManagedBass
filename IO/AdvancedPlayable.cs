using System;
using ManagedBass.Dynamics;
using ManagedBass.Effects;

namespace ManagedBass
{
    public abstract class AdvancedPlayable : Playable, IEffectAssignable, ITagsProvider
    {
        public override int Handle
        {
            get { return base.Handle; }
            protected set
            {
                base.Handle = value;

                // Media Ended
                End_Delegate = new SyncProcedure(OnMediaEnded);
                Bass.ChannelSetSync(Handle, SyncFlags.End, 0, End_Delegate);

                // Media Failed
                Fail_Delegate = new SyncProcedure(OnMediaFailed);
                Bass.ChannelSetSync(Handle, SyncFlags.Stop, 0, Fail_Delegate);
            }
        }

        protected AdvancedPlayable(Resolution BufferKind = Resolution.Short)
            : base(BufferKind)
        {
            Bass.CurrentDevice = Bass.DefaultDevice; 
        }

        public double Volume
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.Volume); }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.Volume, value); }
        }

        public double Position
        {
            get { return Bytes2Seconds(Bass.GetChannelPosition(Handle)); }
            set { Bass.SetChannelPosition(Handle, Seconds2Bytes(value)); }
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

        public bool IsSliding(ChannelAttribute attrib) { return Bass.IsChannelSliding(Handle, attrib); }

        public bool Slide(ChannelAttribute attrib, double Value, int Time) { return Bass.SlideChannelAttribute(Handle, attrib, (float)Value, Time); }

        public void Link(AdvancedPlayable target) { Bass.LinkChannels(Handle, target.Handle); }

        public double Duration { get { return Bytes2Seconds(Bass.ChannelGetLength(Handle)); } }

        public virtual long Length { get { return Bass.StreamGetFilePosition(Handle, FileStreamPosition.End); } }

        public int Bitrate { get { return (int)(Length / (125 * Duration) + 0.5d); } }

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
    }
}