using ManagedBass.Dynamics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ManagedBass.ShowDown
{
    /// <summary>
    /// Interaction logic for WasapiDeviceEnumerator.xaml
    /// </summary>
    public partial class DeviceEnumerator : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<DeviceInfo> AvailableAudioSources { get; private set; }

        DeviceInfo dev;
        public DeviceInfo SelectedAudioDevice
        {
            get { return dev; }
            set
            {
                dev = value;
                OnPropertyChanged("SelectedAudioDevice");
            }
        }

        public DeviceEnumerator()
        {
            DataContext = this;

            AvailableAudioSources = new ObservableCollection<DeviceInfo>();

            Refresh();

            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (s, e) => Refresh()));
        }

        void Refresh()
        {
            AvailableAudioSources.Clear();

            DeviceInfo DevInfo;

            for (int i = 0; Bass.GetDeviceInfo(i, out DevInfo); ++i)
                AvailableAudioSources.Add(DevInfo);

            for (int i = 0; Bass.RecordGetDeviceInfo(i, out DevInfo); ++i)
                AvailableAudioSources.Add(DevInfo);
        }

        void OnPropertyChanged(string e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox b = sender as ListBox;

            if (b == null) return;

            if (b.SelectedIndex == -1) b.SelectedIndex = 0;
        }
    }
}
