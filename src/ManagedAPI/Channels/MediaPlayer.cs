using ManagedBass.Dynamics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManagedBass
{
    /// <summary>
    /// A Reusable Channel which can Load files like a Player.
    /// </summary>
    public class MediaPlayer : Channel, INotifyPropertyChanged
    {
        #region Frequency
        double? freq;

        public override double Frequency
        {
            get { return freq.HasValue ? freq.Value : base.Frequency; }
            set
            {
                if (Bass.ChannelSetAttribute(Handle, ChannelAttribute.Frequency, value))
                {
                    freq = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Balance
        double? pan;

        public override double Balance
        {
            get { return pan.HasValue ? pan.Value : base.Balance; }
            set
            {
                if (Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pan, value))
                {
                    pan = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Device
        protected PlaybackDevice dev;

        public override PlaybackDevice Device
        {
            get { return dev ?? base.Device; }
            set
            {
                if (!value.DeviceInfo.IsInitialized)
                    if (!value.Init())
                        return;

                if (Bass.ChannelSetDevice(Handle, value.DeviceIndex))
                {
                    dev = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Volume
        double? vol;

        public override double Volume
        {
            get { return vol.HasValue ? vol.Value : base.Volume; }
            set
            {
                if (Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, value))
                {
                    vol = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Loop
        bool? loop;

        public override bool Loop
        {
            get { return loop.HasValue ? loop.Value : base.Loop; }
            set
            {
                if (value ? AddFlag(BassFlags.Loop) : RemoveFlag(BassFlags.Loop))
                {
                    loop = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// Loads a file into the player.
        /// </summary>
        /// <param name="FileName">Path to the file to Load.</param>
        /// <returns><see langword="true"/> on succes, <see langword="false"/> on failure.</returns>
        public virtual bool Load(string FileName)
        {
            try
            {
                if (Handle != 0)
                    Bass.StreamFree(Handle);
            }
            catch { }

            if (dev != null)
                PlaybackDevice.CurrentDevice = dev;

            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);

            int h = Bass.CreateStream(FileName);

            if (h == 0)
                return false;

            Handle = h;

            InitProperties();

            return true;
        }

        protected void InitProperties()
        {
            if (freq.HasValue)
                Frequency = freq.Value;
            if (pan.HasValue)
                Balance = pan.Value;
            if (vol.HasValue)
                Volume = vol.Value;
            if (loop.HasValue)
                Loop = loop.Value;
        }

        protected void OnPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// A Reusable Channel which can Load files like a Player including Tempo, Pitch and Reverse options.
    /// </summary>
    public class MediaPlayerFX : MediaPlayer
    {
        int TempoHandle;

        #region Reverse
        bool rev = false;

        /// <summary>
        /// Gets or Sets the Media playback direction.
        /// </summary>
        public bool Reverse
        {
            get { return rev; }
            set
            {
                if (Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1))
                {
                    rev = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Pitch
        double? pitch;

        /// <summary>
        /// Gets or Sets the Pitch in Semitones (-60 ... 0 ... 60).
        /// </summary>
        public double Pitch
        {
            get { return pitch.HasValue ? pitch.Value : Bass.ChannelGetAttribute(TempoHandle, ChannelAttribute.Pitch); }
            set
            {
                if (Bass.ChannelSetAttribute(TempoHandle, ChannelAttribute.Pitch, value))
                {
                    pitch = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Tempo
        double? tempo;

        /// <summary>
        /// Gets or Sets the Tempo in Percentage (-95% ... 0 ... 5000%)
        /// </summary>
        public double Tempo
        {
            get { return tempo.HasValue ? tempo.Value : Bass.ChannelGetAttribute(TempoHandle, ChannelAttribute.Tempo); }
            set
            {
                if (Bass.ChannelSetAttribute(TempoHandle, ChannelAttribute.Tempo, value))
                {
                    tempo = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// Loads a file into the player.
        /// </summary>
        /// <param name="FileName">Path to the file to Load.</param>
        /// <returns><see langword="true"/> on succes, <see langword="false"/> on failure.</returns>
        public override bool Load(string FileName)
        {
            try
            {
                if (Handle != 0)
                    Bass.StreamFree(Handle);
            }
            catch { }

            if (dev != null)
                PlaybackDevice.CurrentDevice = dev;

            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);

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

            Reverse = rev;

            InitProperties();

            if (tempo.HasValue)
                Tempo = tempo.Value;

            if (pitch.HasValue)
                Pitch = pitch.Value;

            return true;
        }
    }
}