using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// A Reusable Channel which can Load files like a Player.
    /// </summary>
    public class MediaPlayer : Channel
    {
        /// <summary>
        /// Loads a file into the player.
        /// </summary>
        /// <param name="FileName">Path to the file to Load.</param>
        /// <returns><see langword="true"/> on succes, <see langword="false"/> on failure.</returns>
        public bool Load(string FileName)
        {
            try
            {
                if (Handle != 0)
                    Bass.StreamFree(Handle);
            }
            catch { }

            int h = Bass.CreateStream(FileName);

            if (h == 0)
                return false;

            Handle = h;

            return true;
        }
    }

    /// <summary>
    /// A Reusable Channel which can Load files like a Player including Tempo, Pitch and Reverse options.
    /// </summary>
    public class MediaPlayerFX : Channel
    {
        int TempoHandle;

        /// <summary>
        /// Loads a file into the player.
        /// </summary>
        /// <param name="FileName">Path to the file to Load.</param>
        /// <returns><see langword="true"/> on succes, <see langword="false"/> on failure.</returns>
        public bool Load(string FileName)
        {
            try
            {
                if (Handle != 0)
                    Bass.StreamFree(Handle);
            }
            catch { }

            int h = Bass.CreateStream(FileName, Flags: BassFlags.Decode);

            if (h == 0)
                return false;

            h = BassFx.TempoCreate(h, BassFlags.Decode | BassFlags.FxFreeSource);
            
            if (h == 0)
                return false;

            TempoHandle = h;

            h = BassFx.ReverseCreate(h, 2, BassFlags.FxFreeSource);

            if (h == 0)
                return false;

            Handle = h;

            Reverse = false;

            return true;
        }

        /// <summary>
        /// Gets or Sets the Media playback direction.
        /// </summary>
        public bool Reverse
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        /// <summary>
        /// Gets or Sets the Pitch in Semitones (-60 ... 0 ... 60).
        /// </summary>
        public double Pitch
        {
            get { return Bass.ChannelGetAttribute(TempoHandle, ChannelAttribute.Pitch); }
            set { Bass.ChannelSetAttribute(TempoHandle, ChannelAttribute.Pitch, value); }
        }

        /// <summary>
        /// Gets or Sets the Tempo in Percentage (-95% ... 0 ... 5000%)
        /// </summary>
        public double Tempo
        {
            get { return Bass.ChannelGetAttribute(TempoHandle, ChannelAttribute.Tempo); }
            set { Bass.ChannelSetAttribute(TempoHandle, ChannelAttribute.Tempo, value); }
        }
    }
}