using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Tags
{
    [StructLayout(LayoutKind.Sequential)]
    public class FlacCueTrack
    {
        public long Offset;

        public int Number;

        IntPtr isrc;

        public string ISRC => Marshal.PtrToStringAnsi(isrc);

        public CueSheetTrackType TrackFlags;

        public int IndexesCount;

        IntPtr indexes;

        public FlacCueTrackIndex[] Indexes
		{
			get
			{
				if (IndexesCount == 0)
                    return null;
				
				var arr = new FlacCueTrackIndex[IndexesCount];
				var ptr = indexes;

				for (int i = 0; i < IndexesCount; i++)
				{
					arr[i] = (FlacCueTrackIndex)Marshal.PtrToStructure(ptr, typeof(FlacCueTrackIndex));
					ptr += Marshal.SizeOf(arr[i]);
				}

				return arr;
			}
		}
    }
}