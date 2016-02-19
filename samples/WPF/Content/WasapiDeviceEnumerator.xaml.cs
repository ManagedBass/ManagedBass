using ManagedBass.Dynamics;
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
    public partial class WasapiDeviceEnumerator : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<WasapiDeviceInfo> AvailableAudioSources { get; private set; }

        WasapiDeviceInfo dev;
        public WasapiDeviceInfo SelectedAudioDevice
        {
            get { return dev; }
            set
            {
                dev = value;
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

            WasapiDeviceInfo DevInfo;

            for (int i = 0; BassWasapi.GetDeviceInfo(i, out DevInfo); ++i)
                AvailableAudioSources.Add(DevInfo);
        }

        void OnPropertyChanged([CallerMemberName] string e = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox b = sender as ListBox;

            if (b == null) return;

            if (b.SelectedItem == null) b.SelectedIndex = 0;
        }
    }
}
