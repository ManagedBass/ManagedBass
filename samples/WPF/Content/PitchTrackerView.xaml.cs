using ManagedBass;
using ManagedBass.Effects;
using System.Windows;
using System.Windows.Controls;

namespace MBassWPF
{
    /// <summary>
    /// Interaction logic for PitchTracker.xaml
    /// </summary>
    public partial class PitchTrackerView : UserControl
    {
        PitchDSP pitchTracker;
        Recording R;

        public PitchTrackerView()
        {
            InitializeComponent();

            R = new Recording(Resolution: Resolution.Float);

            pitchTracker = new PitchDSP(R.Handle);
            
            pitchTracker.PitchDetected += Record =>
            {
                if (Record != null && Record.Pitch > 1)
                    Dispatcher.Invoke(() =>
                    {
                        Frequency.Content = Record.Pitch;
                        Note.Content = Record.NoteName;
                        Cents.Content = Record.MidiCents;
                    });
            };
        }

        void DetectClick(object sender, RoutedEventArgs e)
        {
            if (DetectButton.Content.ToString().Contains("Detect"))
            {
                DetectButton.Content = "Stop";
                R.Start();
            }
            else
            {
                DetectButton.Content = "Detect";
                R.Stop();

                Frequency.Content = Note.Content = Cents.Content = "--";
            }
        }
    }
}
