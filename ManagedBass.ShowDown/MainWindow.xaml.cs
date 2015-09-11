using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ManagedBass.ShowDown
{
    public partial class MainWindow : Window
    {
        static MainWindow Instance;

        public MainWindow()
        {
            if (!Directory.Exists(OutFolder)) Directory.CreateDirectory(OutFolder);

            Instance = this;
            InitializeComponent();
        }

        public static string OutFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ManagedBassShowDown\\");

        public static WriterKind SelectedWriterKind { get { return (WriterKind)Instance.WriterKindBox.SelectedItem; } }

        void OpenOutFolder(object sender, RoutedEventArgs e) { Process.Start("explorer.exe", OutFolder); }
    }

    public enum WriterKind { Wav, Wma, Mp3 }
}
