using Pitch;
using System.Windows;
using System.Windows.Controls;

namespace ManagedBass.ShowDown
{
    /// <summary>
    /// Interaction logic for PitchTracker.xaml
    /// </summary>
    public partial class PitchTrackerView : UserControl
    {
        PitchTracker pitchTracker;
        Recording R;

        public PitchTrackerView()
        {
            InitializeComponent();

            pitchTracker = new PitchTracker() { SampleRate = 44100 };
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

            R = new Recording(Resolution: Resolution.Float);
            R.DataAvailable += R_DataAvailable;
        }

        float[] Buffer = null;

        void R_DataAvailable(BufferProvider obj)
        {
            if (Buffer == null || Buffer.Length < obj.FloatLength)
                Buffer = new float[obj.FloatLength];

            obj.Read(Buffer);

            pitchTracker.ProcessBuffer(Buffer, obj.FloatLength);
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
