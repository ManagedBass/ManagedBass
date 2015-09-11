using ManagedBass.Effects;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ManagedBass.ShowDown
{
    public partial class Deck : UserControl, INotifyPropertyChanged
    {
        #region Fields
        ReverseChannel ReverseDecoder;
        public TempoChannel TempoChannel;
        DispatcherTimer ProgressBarTimer;
        string FilePath;

        public static readonly DependencyProperty ReadyProperty = DependencyProperty.Register("Ready", typeof(bool), typeof(Deck), new UIPropertyMetadata(false));
        #endregion

        #region Properties
        public bool Ready
        {
            get { return (bool)GetValue(ReadyProperty); }
            set { SetValue(ReadyProperty, value); }
        }

        public double Position
        {
            get { return TempoChannel != null ? TempoChannel.Position : 0; }
            set
            {
                if (TempoChannel != null) TempoChannel.Position = value;

                OnPropertyChanged("Position");
            }
        }

        public double Duration
        {
            get { return TempoChannel != null ? TempoChannel.Duration : 0; }
            set { OnPropertyChanged("Duration"); }
        }

        public double Volume { set { TempoChannel.Volume = value; } }

        public bool Reverse
        {
            get { return ReverseDecoder != null ? ReverseDecoder.Reverse : false; }
            set
            {
                if (ReverseDecoder != null) ReverseDecoder.Reverse = value;

                OnPropertyChanged("Reverse");
            }
        }

        public bool Loop
        {
            get { return TempoChannel != null ? TempoChannel.Loop : false; }
            set
            {
                if (TempoChannel != null) TempoChannel.Loop = value;

                OnPropertyChanged("Loop");
            }
        }

        public double Tempo
        {
            get { return TempoChannel != null ? TempoChannel.Tempo : 0; }
            set
            {
                if (TempoChannel != null) TempoChannel.Tempo = value;

                OnPropertyChanged("Tempo");
            }
        }

        public double Frequency
        {
            get { return TempoChannel != null ? TempoChannel.Frequency : 44100; }
            set
            {
                if (TempoChannel != null) TempoChannel.Frequency = value;

                OnPropertyChanged("Frequency");
            }
        }

        public double Pitch
        {
            get { return TempoChannel != null ? TempoChannel.Pitch : 0; }
            set
            {
                if (TempoChannel != null) TempoChannel.Pitch = value;

                OnPropertyChanged("Pitch");
            }
        }

        public double Balance
        {
            get { return TempoChannel != null ? TempoChannel.Balance : 0; }
            set
            {
                if (TempoChannel != null) TempoChannel.Balance = value;

                OnPropertyChanged("Balance");
            }
        }
        #endregion

        #region Reverb
        ReverbEffect ReverbEffect;

        public bool Reverb
        {
            get { return ReverbEffect != null ? ReverbEffect.IsActive : false; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.IsActive = value;

                OnPropertyChanged("Reverb");
            }
        }

        public double Reverb_Damp
        {
            get { return ReverbEffect != null ? ReverbEffect.Damp : 0.5; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.Damp = value;

                OnPropertyChanged("Reverb_Damp");
            }
        }

        public double Reverb_DryMix
        {
            get { return ReverbEffect != null ? ReverbEffect.DryMix : 0; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.DryMix = value;

                OnPropertyChanged("Reverb_DryMix");
            }
        }

        public double Reverb_WetMix
        {
            get { return ReverbEffect != null ? ReverbEffect.WetMix : 1; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.WetMix = value;

                OnPropertyChanged("Reverb_WetMix");
            }
        }

        public double Reverb_RoomSize
        {
            get { return ReverbEffect != null ? ReverbEffect.RoomSize : 0.5; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.RoomSize = value;

                OnPropertyChanged("Reverb_RoomSize");
            }
        }

        public double Reverb_Width
        {
            get { return ReverbEffect != null ? ReverbEffect.Width : 1; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.Width = value;

                OnPropertyChanged("Reverb_Width");
            }
        }
        #endregion

        #region Distortion
        DistortionEffect DistortionEffect;

        public bool Distortion
        {
            get { return DistortionEffect != null ? DistortionEffect.IsActive : false; }
            set
            {
                if (DistortionEffect != null) DistortionEffect.IsActive = value;

                OnPropertyChanged("Distortion");
            }
        }
        #endregion

        public Deck()
        {
            InitializeComponent();

            DataContext = this;

            ProgressBarTimer = new DispatcherTimer(DispatcherPriority.Send) { Interval = TimeSpan.FromMilliseconds(100) };
            ProgressBarTimer.Tick += (s, e) =>
            {
                if (TempoChannel.IsPlaying && !IsDragging)
                    OnPropertyChanged("Position");
            };
        }

        void Load(string FilePath)
        {
            this.FilePath = FilePath;

            Stop();

            ReverseDecoder = new ReverseChannel(new ManagedFileChannel(FilePath, true), true) { Reverse = false };

            TempoChannel = new TempoChannel(ReverseDecoder);

            ReverbEffect = new ReverbEffect(TempoChannel.Handle);
            DistortionEffect = new DistortionEffect(TempoChannel.Handle);

            Ready = true;

            Reverse = false;

            string artist = "Unknown Artist";
            try { artist = ID3v1Tag.Read(TempoChannel.Handle).Artist; }
            catch { }

            Title.Content = Path.GetFileNameWithoutExtension(FilePath) + " - " + artist;

            OnPropertyChanged("Duration");
            OnPropertyChanged("Position");

            OnPropertyChanged("Reverb");
            OnPropertyChanged("Reverb_Damp");
            OnPropertyChanged("Reverb_DryMix");
            OnPropertyChanged("Reverb_WetMix");
            OnPropertyChanged("Reverb_RoomSize");
            OnPropertyChanged("Reverb_Width");

            OnPropertyChanged("Distorion");

            BPlay.Content = "/Resources/Play.png";
            Status.Content = "Ready";

            Frequency = 44100;
            Balance = Pitch = Tempo = 0;

            TempoChannel.MediaEnded += (s, e) =>
                {
                    Status.Content = "Stopped";
                    TempoChannel.Stop();
                    Position = Reverse ? Duration : 0;
                    ProgressBarTimer.Stop();
                    BPlay.Content = "/Resources/Play.png";
                };

            if (MusicLoaded != null) MusicLoaded();
            //}
            //catch { Error(); }
        }

        public event Action MusicLoaded;

        void Error()
        {
            Title.Content = "Error";
            Status.Content = "Error";
            Ready = false;
        }

        public void Play(object sender = null, RoutedEventArgs e = null)
        {
            if (TempoChannel != null)
            {
                if (BPlay.Content.ToString().Contains("Play"))
                {
                    if (Reverse && Position == 0)
                        Position = Duration;

                    TempoChannel.Start();

                    Status.Content = "Playing";
                    BPlay.Content = "/Resources/Pause.png";
                    ProgressBarTimer.Start();
                }

                else if (BPlay.Content.ToString().Contains("Pause"))
                {
                    TempoChannel.Pause();

                    Status.Content = "Paused";
                    BPlay.Content = "/Resources/Play.png";
                    ProgressBarTimer.Stop();
                }
            }
        }

        public void Stop(object sender = null, RoutedEventArgs e = null)
        {
            if (TempoChannel != null && TempoChannel.Stop())
            {
                Status.Content = "Stopped";
                BPlay.Content = "/Resources/Play.png";
                Position = Reverse ? Duration : 0;
                ProgressBarTimer.Stop();
            }
        }

        #region Progress Slider
        bool IsDragging = false;

        void Slider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsDragging)
            {
                TempoChannel.Position = ((Slider)sender).Value;

                IsDragging = false;
            }
        }

        void Slider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) { IsDragging = true; }

        void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { TempoChannel.Position = ((Slider)sender).Value; }
        #endregion

        void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                Load((string)e.Data.GetData(DataFormats.StringFormat));
        }

        #region Reset
        void ResetPitch(object sender, MouseButtonEventArgs e) { Pitch = 0; }

        void ResetFrequency(object sender, MouseButtonEventArgs e) { Frequency = 44100; }

        void ResetBalance(object sender, MouseButtonEventArgs e) { Balance = 0; }

        void ResetTempo(object sender, MouseButtonEventArgs e) { Tempo = 0; }
        #endregion

        #region INotifyPropertyChanged
        void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}