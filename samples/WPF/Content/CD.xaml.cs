using ManagedBass.Cd;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBassWPF
{
    public partial class CD : INotifyPropertyChanged
    {
        public ObservableCollection<CDInfo> AvailableDrives { get; }

        public ObservableCollection<string> CDAFiles { get; }

        CDInfo _dev;
        public CDInfo SelectedDrive
        {
            get { return _dev; }
            set
            {
                _dev = value;
                OnPropertyChanged();

                CDAFiles.Clear();

                CDInfo devInfo;

                for (_currentDriveIndex = 0; BassCd.GetInfo(_currentDriveIndex, out devInfo); ++_currentDriveIndex)
                    if (devInfo.DriveLetter == SelectedDrive.DriveLetter)
                        break;

                CDAFiles.Clear();

                if (!BassCd.IsReady(_currentDriveIndex))
                    return;

                foreach (var file in Directory.EnumerateFiles(SelectedDrive.DriveLetter + ":\\", "*.cda"))
                    CDAFiles.Add(file);
            }
        }

        int _currentDriveIndex;

        string _cda;
        public string SelectedCDA
        {
            get { return _cda; }
            set
            {
                _cda = value;
                OnPropertyChanged();
            }
        }

        public CD()
        {
            DataContext = this;

            AvailableDrives = new ObservableCollection<CDInfo>();

            CDAFiles = new ObservableCollection<string>();

            Refresh();

            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (s, e) => Refresh()));
        }

        void Refresh()
        {
            AvailableDrives.Clear();

            CDInfo devInfo;

            for (var i = 0; BassCd.GetInfo(i, out devInfo); ++i)
                AvailableDrives.Add(devInfo);
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

            try
            {
                if (b.SelectedIndex == -1)
                    b.SelectedIndex = 0;
            }
            catch { }
        }

        CDChannel _cdc;

        public void Play(object sender = null, RoutedEventArgs e = null)
        {
            if (BPlay.Content.ToString().Contains("Play"))
            {
                if (!BassCd.IsReady(_currentDriveIndex))
                    return;

                _cdc = new CDChannel(SelectedCDA);
                _cdc.Start();

                BPlay.Content = "/Resources/Stop.png";
            }
            else
            {
                _cdc.Dispose();

                BPlay.Content = "/Resources/Play.png";
            }
        }
    }
}
