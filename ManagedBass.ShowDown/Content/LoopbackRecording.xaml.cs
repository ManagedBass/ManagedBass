using System.Collections.ObjectModel;
using System.ComponentModel;
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
        Loopback L;
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
        
        void OnPropertyChanged(string e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
