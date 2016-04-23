using ManagedBass;
using ManagedBass.Enc;
using ManagedBass.Fx;
using ManagedBass.Wma;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MBassWPF
{
    public partial class DJ
    {
        readonly OpenFileDialog _ofd;

        public DJ()
        {
            InitializeComponent();

            _ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.aac;*.m4a",
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
            if (!_ofd.ShowDialog().Value)
                return;

            foreach (var fileName in _ofd.FileNames)
                Playlist.Items.Add(new PlaylistLabel(fileName));
        }

        void Playlist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            foreach (var item in Playlist.SelectedItems.OfType<PlaylistLabel>().ToArray())
                Playlist.Items.Remove(item);
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

            var saveReverseMenuItem = new MenuItem { Header = "Save Reverse" };

            saveReverseMenuItem.Click += (s, e) => SaveReverse(FileName);

            ContextMenu.Items.Add(saveReverseMenuItem);

            Content = FileName;

            PreviewMouseLeftButtonDown += (s, e) => DragDrop.DoDragDrop(this, FileName, DragDropEffects.Copy);
        }

        static void SaveReverse(string FilePath)
        {
            try
            {
                var kind = MainWindow.SelectedWriterKind;
                var saveFilePath = Path.Combine(MainWindow.OutFolder, Path.GetFileNameWithoutExtension(FilePath) + ".Reverse." + kind.ToString().ToLower());

                // Using default Resolution.Short
                IAudioFileWriter writer;

                switch (kind)
                {
                    case WriterKind.Mp3:
                        writer = new ACMEncodedFileWriter(saveFilePath, WaveFormatTag.Mp3, new PCMFormat());
                        break;

                    case WriterKind.Wma:
                        writer = new WmaFileWriter(saveFilePath, new PCMFormat());
                        break;

                    default:
                        writer = new WaveFileWriter(saveFilePath, new PCMFormat());
                        break;
                }

                using (var fc = new FileChannel(FilePath, true))
                using (var rc = new ReverseChannel(fc, true))
                    rc.DecodeToFile(writer);

                MessageBox.Show("Saved");
            }
            catch (Exception e) { MessageBox.Show("Failed\n\n" + e); }
        }
    }
}
