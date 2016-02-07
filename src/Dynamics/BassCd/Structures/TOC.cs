using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TOC
    {
        short size;
        byte first;
        byte last;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        TOCTrack[] tracks;

        /// <summary>
        /// The first track number (or index number if <see cref="TOCMode.Index"/> is used).
        /// </summary>
        public int First { get { return first; } }

        /// <summary>
        /// The last track number (or index number if <see cref="TOCMode.Index"/> is used).
        /// </summary>
        public int Last { get { return last; } }

        /// <summary>
        /// The list of tracks retrieved (see <see cref="TOCTrack" />, up to 100 tracks).
        /// </summary>
        public IList<TOCTrack> Tracks
        {
            get
            {
                int n = size / Marshal.SizeOf(typeof(TOC));

                var list = new List<TOCTrack>(n);
                
                for (int i = 0; i < n; ++i)
                    list.Add(tracks[i]);

                return list;
            }
        }
    }
}