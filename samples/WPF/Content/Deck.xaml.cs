using ManagedBass;
using ManagedBass.Fx;
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
        public ReverbEffect Reverb { get; private set; }
        public DistortionEffect Distortion { get; private set; }
        public EchoEffect Echo { get; private set; }
        public AutoWahEffect AutoWah { get; private set; }
        public RotateEffect Rotate { get; private set; }

        public PeakEQ EQ { get; private set; }

        DispatcherTimer ProgressBarTimer;
        public PanDSP Pan { get; private set; }

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

            Pan = new PanDSP(Player);

            Reverb = new ReverbEffect(Player);
            Distortion = new DistortionEffect(Player);
            Echo = new EchoEffect(Player);
            AutoWah = new AutoWahEffect(Player);
            Rotate = new RotateEffect(Player);

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

            EQ = new PeakEQ(Player.Handle);
            EQ.AddBand(400);
            EQ.AddBand(620);
            EQ.AddBand(960);
            EQ.AddBand(1500);
            EQ.AddBand(2400);
            EQ.AddBand(3700);
            EQ.AddBand(6200);

            Ready = true;

            BPlay.Content = "/Resources/Play.png";
            Status.Content = "Ready";

            if (MusicLoaded != null) 
                MusicLoaded();
        }

        public event Action MusicLoaded;
        
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

        void ResetBalance(object sender, MouseButtonEventArgs e) { Pan.Pan = 0; }

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

        #region Effect Presets
        void SoftDistortion(object sender, RoutedEventArgs e) { Distortion.Soft(); }

        void MediumDistortion(object sender, RoutedEventArgs e) { Distortion.Medium(); }

        void HardDistortion(object sender, RoutedEventArgs e) { Distortion.Hard(); }

        void VeryHardDistortion(object sender, RoutedEventArgs e) { Distortion.VeryHard(); }

        void ManyEchoes(object sender, RoutedEventArgs e) { Echo.ManyEchoes(); }

        void ReverseEchoes(object sender, RoutedEventArgs e) { Echo.ReverseEchoes(); }

        void RoboticEchoes(object sender, RoutedEventArgs e) { Echo.RoboticVoice(); }

        void SmallEchoes(object sender, RoutedEventArgs e) { Echo.Small(); }

        void SlowAutoWah(object sender, RoutedEventArgs e) { AutoWah.Slow(); }

        void FastAutoWah(object sender, RoutedEventArgs e) { AutoWah.Fast(); }

        void HiFastAutoWah(object sender, RoutedEventArgs e) { AutoWah.HiFast(); }
        #endregion

        void EQChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;

            EQ.UpdateBand(int.Parse(slider.Tag.ToString()), slider.Value);
        }
    }
}