using ManagedBass.Tags;
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
        double _freq = 44100;
        
        /// <summary>
        /// Gets or Sets the Playback Frequency in Hertz.
        /// Default is 44100 Hz.
        /// </summary>
        public override double Frequency
        {
            get { return _freq; }
            set
            {
                if (!Bass.ChannelSetAttribute(Handle, ChannelAttribute.Frequency, value))
                    return;

                _freq = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Balance
        double _pan;
        
        /// <summary>
        /// Gets or Sets Balance (Panning) (-1 ... 0 ... 1).
        /// -1 Represents Completely Left.
        ///  1 Represents Completely Right.
        /// Default is 0.
        /// </summary>
        public override double Balance
        {
            get { return _pan; }
            set
            {
                if (!Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pan, value))
                    return;

                _pan = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Device
        PlaybackDevice _dev;

        /// <summary>
        /// Gets or Sets the Playback Device used.
        /// </summary>
        public override PlaybackDevice Device
        {
            get { return _dev ?? base.Device; }
            set
            {
                if (!value.DeviceInfo.IsInitialized)
                    if (!value.Init())
                        return;

                if (!Bass.ChannelSetDevice(Handle, value.DeviceIndex))
                    return;

                _dev = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Volume
        double _vol = 0.5;

        /// <summary>
        /// Gets or Sets the Playback Volume.
        /// </summary>
        public override double Volume
        {
            get { return _vol; }
            set
            {
                if (!Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, value))
                    return;

                _vol = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Loop
        bool _loop;

        /// <summary>
        /// Gets or Sets whether the Playback is looped.
        /// </summary>
        public override bool Loop
        {
            get { return _loop; }
            set
            {
                if (value ? !AddFlag(BassFlags.Loop) : !RemoveFlag(BassFlags.Loop))
                    return;

                _loop = value;
                OnPropertyChanged();
            }
        }
        #endregion
        
        /// <summary>
        /// Override this method for custom loading procedure.
        /// </summary>
        /// <param name="FileName">Path to the File to Load.</param>
        /// <returns><see langword="true"/> on Success, <see langword="false"/> on failure</returns>
        protected virtual int OnLoad(string FileName) => Bass.CreateStream(FileName);

        string _title = "", _artist = "", _album = "";

        /// <summary>
        /// Title of the Loaded Media.
        /// </summary>
        public string Title 
        {
            get { return _title; }
            private set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Artist of the Loaded Media.
        /// </summary>
        public string Artist
        {
            get { return _artist; }
            private set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Album of the Loaded Media.
        /// </summary>
        public string Album
        {
            get { return _album; }
            private set
            {
                _album = value;
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

            if (_dev != null)
                PlaybackDevice.CurrentDevice = _dev;

            var currentDev = Bass.CurrentDevice;

            if (currentDev == -1 || !Bass.GetDeviceInfo(Bass.CurrentDevice).IsInitialized)
                Bass.Init(currentDev);

            var h = OnLoad(FileName);

            if (h == 0)
                return false;

            Handle = h;

            var tags = TagReader.Read(Handle);

            Title = !string.IsNullOrWhiteSpace(tags.Title) ? tags.Title 
                                                           : Path.GetFileNameWithoutExtension(FileName);
            Artist = tags.Artist;
            Album = tags.Album;
            
            InitProperties();

            MediaLoaded?.Invoke(h);

            OnPropertyChanged("");

            return true;
        }

        /// <summary>
        /// Fired when a Media is Loaded.
        /// </summary>
        public event Action<int> MediaLoaded;

        /// <summary>
        /// Initializes Properties on every call to <see cref="Load"/>.
        /// </summary>
        protected virtual void InitProperties()
        {
            Frequency = _freq;
            Balance = _pan;
            Volume = _vol;
            Loop = _loop;
        }

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Fired when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}