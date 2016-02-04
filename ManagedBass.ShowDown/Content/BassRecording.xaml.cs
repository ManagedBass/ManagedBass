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
    /// Demonstrates using <see cref="Recording"/> to Capture audio from a <see cref="RecordingDevice"/>.
    /// </summary>
    public partial class BassRecording : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<RecordingDevice> AvailableAudioSources { get; private set; }
        
        IAudioCaptureClient R;
        IAudioFileWriter Writer;

        RecordingDevice dev;
        public RecordingDevice SelectedAudioDevice
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

        public BassRecording()
        {
            DataContext = this;
            
            AvailableAudioSources = new ObservableCollection<RecordingDevice>();

            Refresh();

            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (s, e) => Refresh()));
        }

        void Refresh()
        {
            AvailableAudioSources.Clear();
            foreach (var dev in RecordingDevice.Devices)
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
                R.Stop();

                Status.Content = "Paused";
                BPlay.Content = "/Resources/Record.png";
            }
        }

        public void Stop(object sender = null, RoutedEventArgs e = null)
        {
            if (R != null && R.Stop())
            {
                Status.Content = "Stopped";
                BPlay.Content = "/Resources/Record.png";

                Writer.Dispose();

                R = null;
                Writer = null;
            }
        }

        void Start()
        {
            var writerKind = MainWindow.SelectedWriterKind;

            string filePath = Path.Combine(MainWindow.OutFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + writerKind.ToString().ToLower());

            switch (writerKind)
            {
                // Loopback provides 32-bit floating point audio
                case WriterKind.Mp3:
                    Writer = new ACMEncodedFileWriter(filePath, WaveFormatTag.Mp3);
                    break;
                case WriterKind.Wav:
                    Writer = new WaveFileWriter(filePath);
                    break;
                case WriterKind.Wma:
                    Writer = new WmaFileWriter(filePath);
                    break;
            }

            R = new Recording(dev);
            R.DataAvailable += L_DataAvailable;
            R.Start();
        }

        float[] Buffer = null;

        void L_DataAvailable(BufferProvider obj)
        {
            if (Buffer == null || Buffer.Length < obj.FloatLength)
                Buffer = new float[obj.FloatLength];

            obj.Read(Buffer);

            Writer.Write(Buffer, obj.ByteLength);
        }

        void Pause() { R.Stop(); }

        void OnPropertyChanged(string e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
