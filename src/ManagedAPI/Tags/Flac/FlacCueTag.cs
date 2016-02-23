using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    [StructLayout(LayoutKind.Sequential)]
    public class FlacCueTag
	{
		IntPtr catalog;

        public string Catalog => Marshal.PtrToStringAnsi(catalog);

		public int LeadInSampleCount;

		public bool IsCD;

		public int TrackCount;

		IntPtr tracks;

        public FlacCueTrack[] Tracks
		{
			get
			{
				if (TrackCount == 0)
                    return null;

				var arr = new FlacCueTrack[TrackCount];
				var ptr = tracks;

				for (int i = 0; i < TrackCount; i++)
				{
					arr[i] = (FlacCueTrack)Marshal.PtrToStructure(ptr, typeof(FlacCueTrack));
					ptr += Marshal.SizeOf(arr[i]);
				}

				return arr;
			}
		}

        public static FlacCueTag Read(int Channel)
        {
            return (FlacCueTag)Marshal.PtrToStructure(Bass.ChannelGetTags(Channel, TagType.FlacCue), typeof(FlacCueTag));
        }
    }
}