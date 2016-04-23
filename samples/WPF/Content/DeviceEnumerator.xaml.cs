using ManagedBass;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBassWPF
{
    /// <summary>
    /// Interaction logic for WasapiDeviceEnumerator.xaml
    /// </summary>
    public partial class DeviceEnumerator : INotifyPropertyChanged
    {
        public ObservableCollection<DeviceInfo> AvailableAudioSources { get; }

        DeviceInfo _dev;
        public DeviceInfo SelectedAudioDevice
        {
            get { return _dev; }
            set
            {
                _dev = value;
                OnPropertyChanged();
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

            DeviceInfo devInfo;

            for (var i = 0; Bass.GetDeviceInfo(i, out devInfo); ++i)
                AvailableAudioSources.Add(devInfo);

            for (var i = 0; Bass.RecordGetDeviceInfo(i, out devInfo); ++i)
                AvailableAudioSources.Add(devInfo);
        }

        void OnPropertyChanged([CallerMemberName] string e = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var b = sender as ListBox;

            if (b?.SelectedIndex == -1)
                b.SelectedIndex = 0;
        }
    }
}
