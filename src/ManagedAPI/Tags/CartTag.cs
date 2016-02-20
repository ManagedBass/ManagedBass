using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    [StructLayout(LayoutKind.Sequential)]
    public class CartTimer { int Usage, Value; }

    [StructLayout(LayoutKind.Sequential)]
    public class CartTag
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Version;				// version of the data structure

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Title;					// title of cart audio sequence

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Artist;				// artist or creator name

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string CutID;					// cut number identification

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ClientID;				// client identification

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Category;				// category ID, PSA, NEWS, etc

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Classification;		// classification or auxiliary key

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string OutCue;				// out cue text

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        string startDate;				// yyyy-mm-dd

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        string startTime;				// hh:mm:ss

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        string endDate;				// yyyy-mm-dd

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        string endTime;				// hh:mm:ss

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ProducerAppID;			// name of vendor or application

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ProducerAppVersion;	// version of producer application

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string UserDef;				// user defined text

        public int dwLevelReference;			// sample value for 0 dB reference

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public CartTimer[] PostTimer;	// 8 time markers after head

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 276)]
        char[] Reserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string URL;					// uniform resource locator

        IntPtr tagText;				// free form text for scripts or tags

        public string TagText => Marshal.PtrToStringAnsi(tagText);

        public DateTime StartTime
        {
            get
            {
                string[] date = startDate.Split('-'),
                         time = startTime.Split(':');

                return new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]),
                                    int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
            }
        }

        public DateTime EndTime
        {
            get
            {
                string[] date = endDate.Split('-'),
                         time = endTime.Split(':');

                return new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]),
                                    int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
            }
        }

        public static CartTag Read(int handle)
        {
            return (CartTag)Marshal.PtrToStructure(Bass.ChannelGetTags(handle, TagType.RiffCart), typeof(CartTag));
        }
    }
}