using ManagedBass;
using ManagedBass.Enc;
using ManagedBass.Pitch;
using ManagedBass.Wasapi;
using ManagedBass.Wma;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MBassWPF
{
    /// <summary>
    /// Demonstrates using <see cref="Recording"/> and <see cref="Loopback"/>.
    /// </summary>
    public partial class Recorder : INotifyPropertyChanged
    {
        PitchDSP _pitchTracker;
        public ObservableCollection<IDisposable> AvailableAudioSources { get; }

        IAudioCaptureClient _r;
        IAudioFileWriter _writer;

        IDisposable _dev;
        public IDisposable SelectedAudioDevice
        {
            get { return _dev; }
            set
            {
                _dev = value;
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

            foreach (var dev in RecordingDevice.Devices.Where(dev => dev.DeviceInfo.IsEnabled))
                AvailableAudioSources.Add(dev);

            foreach (var dev in WasapiLoopbackDevice.Devices.Where(dev => dev.DeviceInfo.IsEnabled))
                AvailableAudioSources.Add(dev);
        }

        public void Play(object sender = null, RoutedEventArgs e = null)
        {
            if (BPlay.Content.ToString().Contains("Record"))
            {
                if (_r == null)
                    New();
                else _r.Start();

                Status.Content = "Recording";
                BPlay.Content = "/Resources/Pause.png";
            }
            else if (BPlay.Content.ToString().Contains("Pause"))
            {
                _r.Stop();

                Status.Content = "Paused";
                BPlay.Content = "/Resources/Record.png";
            }
        }

        public void Stop(object sender = null, RoutedEventArgs e = null)
        {
            if (_r == null)
                return;

            Status.Content = "Stopped";
            BPlay.Content = "/Resources/Record.png";

            _pitchTracker.Dispose();
            _r.Dispose();

            _r = null;

            _writer.Dispose();

            _writer = null;
        }

        void New()
        {
            var writerKind = MainWindow.SelectedWriterKind;

            var filePath = Path.Combine(MainWindow.OutFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + writerKind.ToString().ToLower());

            var isLoopback = _dev is WasapiLoopbackDevice;

            var pcmFormat = new PCMFormat(Resolution: Resolution.Float);
            
            if (isLoopback)
            {
                var info = (_dev as WasapiLoopbackDevice).DeviceInfo;

                pcmFormat.Channels = info.MixChannels;
                pcmFormat.Frequency = info.MixFrequency;
            }
            
            switch (writerKind)
            {
                case WriterKind.Mp3:
                    _writer = new ACMEncodedFileWriter(filePath, WaveFormatTag.Mp3, pcmFormat);
                    break;

                case WriterKind.Wav:
                    _writer = new WaveFileWriter(filePath, pcmFormat);
                    break;

                case WriterKind.Wma:
                    _writer = new WmaFileWriter(filePath, pcmFormat);
                    break;
            }

            _r = isLoopback
                ? (IAudioCaptureClient)new Loopback(_dev as WasapiLoopbackDevice)
                : new Recording(_dev as RecordingDevice, new PCMFormat(Resolution: Resolution.Float));

            if (!isLoopback)
            {
                _pitchTracker = new PitchDSP((_r as Recording).Handle);

                _pitchTracker.PitchDetected += Record =>
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

            _r.DataAvailable += L_DataAvailable;
            _r.Start();
        }

        float[] _buffer;

        void L_DataAvailable(BufferProvider obj)
        {
            if (_buffer == null || _buffer.Length < obj.FloatLength)
                _buffer = new float[obj.FloatLength];

            obj.Read(_buffer);

            _writer?.Write(_buffer, obj.ByteLength);
        }

        void OnPropertyChanged([CallerMemberName] string e = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
