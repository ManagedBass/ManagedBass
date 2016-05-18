using System.Runtime.InteropServices;

namespace ManagedBass.Flac
{
    [StructLayout(LayoutKind.Sequential)]
    public class FlacCueTrackIndex
    {        
        /// <summary>
        /// Index offset in samples relative to the track offset.
        /// </summary>
        public long Offser;

        /// <summary>
        /// The index number.
        /// </summary>
        public int Number;
    }
}