using ManagedBass.Dynamics;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace ManagedBass
{
    /// <summary>
    /// A Reusable Channel which can Load files like a Player.
    /// <para><see cref="MediaPlayer"/> is perfect for UIs, as it implements <see cref="INotifyPropertyChanged"/>.</para>
    /// <para>Also, unlike normal, Properties/Effects/DSP set on a <see cref="MediaPlayer"/> persist through subsequent loads.</para>
    /// </summary>
    public class MediaPlayer : Channel, INotifyPropertyChanged
    {
        #region Frequency
        double? freq;
        
        /// <summary>
        /// Gets or Sets the Playback Frequency in Hertz.
        /// Default is 44100 Hz.
        /// </summary>
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
        
        /// <summary>
        /// Gets or Sets Balance (Panning) (-1 ... 0 ... 1).
        /// -1 Represents Completely Left.
        ///  1 Represents Completely Right.
        /// Default is 0.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets the Playback Device used.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets the Playback Volume.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets whether the Playback is looped.
        /// </summary>
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
        /// Override this method for custom loading procedure.
        /// </summary>
        /// <param name="FileName">Path to the File to Load.</param>
        /// <returns><see langword="true"/> on Success, <see langword="false"/> on failure</returns>
        protected virtual int OnLoad(string FileName) => Bass.CreateStream(FileName);

        string title = "", artist = "", album = "";

        /// <summary>
        /// Title of the Loaded Media.
        /// </summary>
        public string Title 
        {
            get { return title; }
            private set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Artist of the Loaded Media.
        /// </summary>
        public string Artist
        {
            get { return artist; }
            private set
            {
                artist = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Album of the Loaded Media.
        /// </summary>
        public string Album
        {
            get { return album; }
            private set
            {
                album = value;
                OnPropertyChanged();
            }
        }

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

            if (dev != null)
                PlaybackDevice.CurrentDevice = dev;

            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);

            int h = OnLoad(FileName);

            if (h == 0)
                return false;

            Handle = h;

            ID3v1Tag tags = null;

            try { tags = ID3v1Tag.Read(Handle); }
            catch { }
            
            if (tags != null)
            {
                Title = !string.IsNullOrWhiteSpace(tags.Title) ? tags.Title 
                                                               : Path.GetFileNameWithoutExtension(FileName);
                Artist = tags.Artist;
                Album = tags.Album;
            }
            else
            {
                Title = Path.GetFileNameWithoutExtension(FileName);
                Artist = Album = "";
            }

            InitProperties();

            MediaLoaded?.Invoke(h);

            OnPropertyChanged("");

            return true;
        }

        /// <summary>
        /// Fired when a Media is Loaded.
        /// </summary>
        public event Action<int> MediaLoaded;

        protected virtual void InitProperties()
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

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}