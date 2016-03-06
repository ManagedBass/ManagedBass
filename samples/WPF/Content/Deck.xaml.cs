using ManagedBass;
using ManagedBass.Effects;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MBassWPF
{
    public partial class Deck : UserControl, INotifyPropertyChanged
    {
        #region Fields
        public MediaPlayerFX Player { get; private set; }
        DispatcherTimer ProgressBarTimer;
        PanDSP pan;

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
            get { return Player.Position; }
            set
            {
                Player.Position = value;

                OnPropertyChanged();
            }
        }
        
        public double Volume { set { Player.Volume = value; } }
        
        public double Balance
        {
            get { return pan != null ? pan.Pan : 0; }
            set
            {
                if (pan != null) pan.Pan = value;

                OnPropertyChanged();
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

                OnPropertyChanged();
            }
        }

        public double Reverb_Damp
        {
            get { return ReverbEffect != null ? ReverbEffect.Damp : 0.5; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.Damp = value;

                OnPropertyChanged();
            }
        }

        public double Reverb_DryMix
        {
            get { return ReverbEffect != null ? ReverbEffect.DryMix : 0; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.DryMix = value;

                OnPropertyChanged();
            }
        }

        public double Reverb_WetMix
        {
            get { return ReverbEffect != null ? ReverbEffect.WetMix : 1; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.WetMix = value;

                OnPropertyChanged();
            }
        }

        public double Reverb_RoomSize
        {
            get { return ReverbEffect != null ? ReverbEffect.RoomSize : 0.5; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.RoomSize = value;

                OnPropertyChanged();
            }
        }

        public double Reverb_Width
        {
            get { return ReverbEffect != null ? ReverbEffect.Width : 1; }
            set
            {
                if (ReverbEffect != null) ReverbEffect.Width = value;

                OnPropertyChanged();
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

                OnPropertyChanged();
            }
        }
        #endregion

        public Deck()
        {
            Player = new MediaPlayerFX();

            Player.MediaEnded += (s, e) =>
            {
                Status.Content = "Stopped";
                Player.Stop();
                Position = Player.Reverse ? Player.Duration : 0;
                ProgressBarTimer.Stop();
                BPlay.Content = "/Resources/Play.png";
            };

            InitializeComponent();

            DataContext = this;

            ProgressBarTimer = new DispatcherTimer(DispatcherPriority.Send) { Interval = TimeSpan.FromMilliseconds(100) };
            ProgressBarTimer.Tick += (s, e) =>
            {
                if (Player.IsPlaying && !IsDragging)
                    OnPropertyChanged("Position");
            };
        }

        void Load(string FilePath)
        {
            Stop();

            if (!Player.Load(FilePath))
                return;

            pan = new PanDSP(Player.Handle);

            ReverbEffect = new ReverbEffect(Player.Handle);
            DistortionEffect = new DistortionEffect(Player.Handle);

            Ready = true;

            ID3v2Tag t = null;
            try { t = ID3v2Tag.Read(Player.Handle); }
            catch { }

            Title.Content = (t != null ? t.Title : Path.GetFileNameWithoutExtension(FilePath)) + " - "
                             + (t != null ? t.Artist : "Unknown Artist");

            // Update all bindings
            DataContext = null;
            DataContext = this;

            BPlay.Content = "/Resources/Play.png";
            Status.Content = "Ready";
            
            if (MusicLoaded != null) MusicLoaded();
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
            if (BPlay.Content.ToString().Contains("Play"))
            {
                if (Player.Reverse && Position == 0)
                    Position = Player.Duration;

                Player.Start();

                Status.Content = "Playing";
                BPlay.Content = "/Resources/Pause.png";
                ProgressBarTimer.Start();
            }

            else if (BPlay.Content.ToString().Contains("Pause"))
            {
                Player.Pause();

                Status.Content = "Paused";
                BPlay.Content = "/Resources/Play.png";
                ProgressBarTimer.Stop();
            }
        }

        public void Stop(object sender = null, RoutedEventArgs e = null)
        {
            if (Player.Stop())
            {
                Status.Content = "Stopped";
                BPlay.Content = "/Resources/Play.png";
                Position = Player.Reverse ? Player.Duration : 0;
                ProgressBarTimer.Stop();
            }
        }

        #region Progress Slider
        bool IsDragging = false;

        void Slider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsDragging)
            {
                Player.Position = ((Slider)sender).Value;

                IsDragging = false;
            }
        }

        void Slider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) { IsDragging = true; }

        void Slider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { Player.Position = ((Slider)sender).Value; }
        #endregion

        void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                Load((string)e.Data.GetData(DataFormats.StringFormat));
        }

        #region Reset
        void ResetPitch(object sender, MouseButtonEventArgs e) { Player.Pitch = 0; }

        void ResetFrequency(object sender, MouseButtonEventArgs e) { Player.Frequency = 44100; }

        void ResetBalance(object sender, MouseButtonEventArgs e) { pan.Pan = 0; }

        void ResetTempo(object sender, MouseButtonEventArgs e) { Player.Tempo = 0; }
        #endregion

        #region INotifyPropertyChanged
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}