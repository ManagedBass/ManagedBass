using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ManagedBass.ShowDown
{
    /// <summary>
    /// Interaction logic for LoopbackRecording.xaml
    /// </summary>
    public partial class LoopbackRecording : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<WasapiLoopbackDevice> AvailableAudioSources { get; private set; }
        
        IAudioCaptureClient L;
        IAudioFileWriter Writer;

        WasapiLoopbackDevice dev;
        public WasapiLoopbackDevice SelectedAudioDevice
        {
            get { return dev; }
            set
            {
                if (dev != value)
                {
                    dev = value;
                    OnPropertyChanged("SelectedAudioDevice");
                }
            }
        }

        public LoopbackRecording()
        {
            DataContext = this;
            
            AvailableAudioSources = new ObservableCollection<WasapiLoopbackDevice>();

            Refresh();

            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (s, e) => Refresh()));
        }

        void Refresh()
        {
            AvailableAudioSources.Clear();
            foreach (var dev in WasapiLoopbackDevice.Devices)
                if (dev.DeviceInfo.IsEnabled)
                    AvailableAudioSources.Add(dev);
        }

        public void Play(object sender = null, RoutedEventArgs e = null)
        {
            if (BPlay.Content.ToString().Contains("Record"))
            {
                Start();

                Status.Content = "Recording";
                BPlay.Content = "/Resources/Pause.png";
            }
            else if (BPlay.Content.ToString().Contains("Pause"))
            {
                L.Stop();

                Status.Content = "Paused";
                BPlay.Content = "/Resources/Record.png";
            }
        }

        public void Stop(object sender = null, RoutedEventArgs e = null)
        {
            if (L != null && L.Stop())
            {
                Status.Content = "Stopped";
                BPlay.Content = "/Resources/Record.png";

                Writer.Dispose();

                L = null;
                Writer = null;
            }
        }

        void Start()
        {
            var writerKind = MainWindow.SelectedWriterKind;

            string filePath = Path.Combine(MainWindow.OutFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + writerKind.ToString().ToLower());

            var info = dev.DeviceInfo;

            switch (writerKind)
            {
                // Loopback provides 32-bit floating point audio
                case WriterKind.Mp3:
                    Writer = new ACMEncodedFileWriter(filePath, WaveFormatTag.Mp3, info.MixChannels, info.MixFrequency, Resolution.Float);
                    break;
                case WriterKind.Wav:
                    Writer = new WaveFileWriter(filePath, info.MixChannels, info.MixFrequency, Resolution.Float);
                    break;
                case WriterKind.Wma:
                    Writer = new WmaFileWriter(filePath, info.MixChannels, info.MixFrequency, 128000, Resolution.Float);
                    break;
            }

            L = new Loopback(dev, true);
            L.DataAvailable += L_DataAvailable;
            L.Start();
        }

        float[] Buffer = null;

        void L_DataAvailable(BufferProvider obj)
        {
            if (Buffer == null || Buffer.Length < obj.FloatLength)
                Buffer = new float[obj.FloatLength];

            obj.Read(Buffer);

            Writer.Write(Buffer, obj.ByteLength);
        }

        void Pause() { L.Stop(); }

        void OnPropertyChanged(string e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
