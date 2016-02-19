using ManagedBass;
using ManagedBass.Dynamics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBassWPF
{
    public partial class CD : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<CDInfo> AvailableDrives { get; private set; }

        public ObservableCollection<string> CDAFiles { get; private set; }

        CDInfo dev;
        public CDInfo SelectedDrive
        {
            get { return dev; }
            set
            {
                dev = value;
                OnPropertyChanged();

                CDAFiles.Clear();

                CDInfo DevInfo;

                for (CurrentDriveIndex = 0; BassCd.GetDriveInfo(CurrentDriveIndex, out DevInfo); ++CurrentDriveIndex)
                    if (DevInfo.DriveLetter == SelectedDrive.DriveLetter)
                        break;

                CDAFiles.Clear();

                if (BassCd.IsReady(CurrentDriveIndex))
                    foreach (var file in Directory.EnumerateFiles(SelectedDrive.DriveLetter + ":\\", "*.cda"))
                        CDAFiles.Add(file);

            }
        }

        int CurrentDriveIndex = 0;

        string cda;
        public string SelectedCDA
        {
            get { return cda; }
            set
            {
                cda = value;
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

            CDInfo DevInfo;

            for (int i = 0; BassCd.GetDriveInfo(i, out DevInfo); ++i)
                AvailableDrives.Add(DevInfo);
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

            try
            {
                if (b.SelectedIndex == -1)
                    b.SelectedIndex = 0;
            }
            catch { }
        }

        CDChannel CDC;

        public void Play(object sender = null, RoutedEventArgs e = null)
        {
            if (BPlay.Content.ToString().Contains("Play"))
            {
                if (BassCd.IsReady(CurrentDriveIndex))
                {
                    CDC = new CDChannel(SelectedCDA);
                    CDC.Start();

                    BPlay.Content = "/Resources/Stop.png";
                }
            }
            else
            {
                CDC.Dispose();

                BPlay.Content = "/Resources/Play.png";
            }
        }
    }
}
