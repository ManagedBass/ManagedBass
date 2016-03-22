using ManagedBass;
using ManagedBass.Enc;
using ManagedBass.Pitch;
using ManagedBass.Wasapi;
using ManagedBass.Wma;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBassWPF
{
    /// <summary>
    /// Demonstrates using <see cref="Recording"/> and <see cref="Loopback"/>.
    /// </summary>
    public partial class Recorder : UserControl, INotifyPropertyChanged
    {
        PitchDSP pitchTracker;
        public ObservableCollection<IDisposable> AvailableAudioSources { get; private set; }

        IAudioCaptureClient R;
        IAudioFileWriter Writer;

        IDisposable dev;
        public IDisposable SelectedAudioDevice
        {
            get { return dev; }
            set
            {
                dev = value;
                OnPropertyChanged();
            }
        }

        public Recorder()
        {
            DataContext = this;

            AvailableAudioSources = new ObservableCollection<IDisposable>();

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

            foreach (var dev in WasapiLoopbackDevice.Devices)
                if (dev.DeviceInfo.IsEnabled)
                    AvailableAudioSources.Add(dev);
        }

        public void Play(object sender = null, RoutedEventArgs e = null)
        {
            if (BPlay.Content.ToString().Contains("Record"))
            {
                if (R == null)
                    New();
                else R.Start();

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
            if (R != null)
            {
                Status.Content = "Stopped";
                BPlay.Content = "/Resources/Record.png";

                pitchTracker.Dispose();
                R.Dispose();

                R = null;

                Writer.Dispose();

                Writer = null;
            }
        }

        void New()
        {
            var writerKind = MainWindow.SelectedWriterKind;

            string filePath = Path.Combine(MainWindow.OutFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + writerKind.ToString().ToLower());

            bool isLoopback = dev is WasapiLoopbackDevice;

            int chan = 2, freq = 44100;

            if (isLoopback)
            {
                var info = (dev as WasapiLoopbackDevice).DeviceInfo;

                chan = info.MixChannels;
                freq = info.MixFrequency;
            }

            switch (writerKind)
            {
                case WriterKind.Mp3:
                    Writer = new ACMEncodedFileWriter(filePath, WaveFormatTag.Mp3, chan, freq, Resolution.Float);
                    break;
                case WriterKind.Wav:
                    Writer = new WaveFileWriter(filePath, chan, freq, Resolution.Float);
                    break;
                case WriterKind.Wma:
                    Writer = new WmaFileWriter(filePath, chan, freq, 128000, Resolution.Float);
                    break;
            }

            R = isLoopback
                ? (IAudioCaptureClient)new Loopback(dev as WasapiLoopbackDevice)
                : new Recording(dev as RecordingDevice, Resolution: Resolution.Float);

            if (!isLoopback)
            {
                pitchTracker = new PitchDSP((R as Recording).Handle);

                pitchTracker.PitchDetected += Record =>
                {
                    if (Record != null && Record.Pitch > 1)
                        Dispatcher.Invoke(() =>
                        {
                            Frequency.Content = Record.Pitch;
                            Note.Content = Record.NoteName;
                            Cents.Content = Record.MidiCents;
                        });
                };
            }

            R.DataAvailable += L_DataAvailable;
            R.Start();
        }

        float[] Buffer = null;

        void L_DataAvailable(BufferProvider obj)
        {
            if (Buffer == null || Buffer.Length < obj.FloatLength)
                Buffer = new float[obj.FloatLength];

            obj.Read(Buffer);

            if (Writer != null)
                Writer.Write(Buffer, obj.ByteLength);
        }

        void OnPropertyChanged([CallerMemberName] string e = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
