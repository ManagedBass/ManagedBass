using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ManagedBass.ShowDown
{
    public partial class DJ : UserControl
    {
        static DJ() { PlaybackDevice.DefaultDevice.Initialize(); }

        OpenFileDialog OFD;

        public DJ()
        {
            InitializeComponent();

            OFD = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.aac",
                Title = "Select Audio File",
                ValidateNames = true,
                Multiselect = true
            };

            DeckA.MusicLoaded += () =>
                {
                    try { DeckA.Volume = 1 - Crossfader.Value; }
                    catch { }
                };

            DeckB.MusicLoaded += () =>
                {
                    try { DeckB.Volume = Crossfader.Value; }
                    catch { }
                };
        }

        void Browse(object sender, RoutedEventArgs e)
        {
            if (OFD.ShowDialog().Value)
                foreach (string FileName in OFD.FileNames)
                    Playlist.Items.Add(new PlaylistLabel(FileName));
        }

        void Playlist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                foreach (var Item in Playlist.SelectedItems.OfType<PlaylistLabel>().ToArray())
                    Playlist.Items.Remove(Item);
        }

        void CrossfaderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try { DeckB.Volume = e.NewValue; }
            catch { }
            try { DeckA.Volume = 1 - e.NewValue; }
            catch { }
        }

        void SyncStart(object sender, RoutedEventArgs e)
        {
            try { DeckA.Play(); }
            catch { }

            try { DeckB.Play(); }
            catch { }
        }
    }

    class PlaylistLabel : ContentControl
    {
        static SaveFileDialog SFD;

        static PlaylistLabel()
        {
            SFD = new SaveFileDialog()
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "Windows Media Audio|*.wma|Wave Audio|*.wav",
                Title = "Save",
                ValidateNames = true
            };
        }

        public PlaylistLabel(string FileName)
        {
            ContextMenu = new ContextMenu();
            
            var SaveReverseMenuItem = new MenuItem() { Header = "Save Reverse" };

            SaveReverseMenuItem.Click += (s, e) => SaveReverse(FileName);

            ContextMenu.Items.Add(SaveReverseMenuItem);

            Content = FileName;

            PreviewMouseLeftButtonDown += (s, e) => DragDrop.DoDragDrop(this, FileName, DragDropEffects.All);
        }

        void SaveReverse(string FileName)
        {
            lock (SFD)
            {
                if (SFD.ShowDialog().Value)
                {
                    try
                    {
                        var writer = SFD.FilterIndex == 1 ? (IAudioFileWriter)new WmaFileWriter(SFD.FileName) : new WaveFileWriter(SFD.FileName);

                        new ReverseChannel(new FileChannel(FileName, true, Resolution.Float).Decoder, true, Resolution.Float, 0.1).Decoder.Write(writer);

                        MessageBox.Show("Saved");
                    }
                    catch { MessageBox.Show("Failed"); }
                }
            }
        }
    }
}