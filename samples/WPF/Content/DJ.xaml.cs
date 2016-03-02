using ManagedBass;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBassWPF
{
    public partial class DJ : UserControl
    {
        static DJ() { PlaybackDevice.DefaultDevice.Init(); }

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
        public PlaylistLabel(string FileName)
        {
            ContextMenu = new ContextMenu();

            var SaveReverseMenuItem = new MenuItem() { Header = "Save Reverse" };

            SaveReverseMenuItem.Click += (s, e) => SaveReverse(FileName);

            ContextMenu.Items.Add(SaveReverseMenuItem);

            Content = FileName;

            PreviewMouseLeftButtonDown += (s, e) => DragDrop.DoDragDrop(this, FileName, DragDropEffects.Copy);
        }

        void SaveReverse(string FilePath)
        {
            try
            {
                var kind = MainWindow.SelectedWriterKind;
                string SaveFilePath = Path.Combine(MainWindow.OutFolder, Path.GetFileNameWithoutExtension(FilePath) + ".Reverse." + kind.ToString().ToLower());

                // Using default Resolution.Short
                IAudioFileWriter writer;

                switch (kind)
                {
                    case WriterKind.Mp3:
                        writer = new ACMEncodedFileWriter(SaveFilePath, WaveFormatTag.Mp3);
                        break;
                    case WriterKind.Wma:
                        writer = new WmaFileWriter(SaveFilePath);
                        break;
                    default:
                    case WriterKind.Wav:
                        writer = new WaveFileWriter(SaveFilePath);
                        break;
                }

                using (var fc = new FileChannel(FilePath, true))
                using (var rc = new ReverseChannel(fc, true))
                    rc.DecodeToFile(writer);

                MessageBox.Show("Saved");
            }
            catch (Exception e) { MessageBox.Show("Failed\n\n" + e.ToString()); }
        }
    }
}
