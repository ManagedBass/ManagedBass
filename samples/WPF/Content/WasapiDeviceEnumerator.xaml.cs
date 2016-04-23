using ManagedBass.Wasapi;
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
    public partial class WasapiDeviceEnumerator : INotifyPropertyChanged
    {
        public ObservableCollection<WasapiDeviceInfo> AvailableAudioSources { get; }

        WasapiDeviceInfo _dev;
        public WasapiDeviceInfo SelectedAudioDevice
        {
            get { return _dev; }
            set
            {
                _dev = value;
                OnPropertyChanged();
            }
        }

        public WasapiDeviceEnumerator()
        {
            DataContext = this;

            AvailableAudioSources = new ObservableCollection<WasapiDeviceInfo>();

            Refresh();

            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (s, e) => Refresh()));
        }

        void Refresh()
        {
            AvailableAudioSources.Clear();

            WasapiDeviceInfo devInfo;

            for (var i = 0; BassWasapi.GetDeviceInfo(i, out devInfo); ++i)
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

            if (b == null)
                return;

            if (b.SelectedItem == null)
                b.SelectedIndex = 0;
        }
    }
}
