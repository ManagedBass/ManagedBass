using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedBass
{
    [StructLayout(LayoutKind.Sequential)]
    public class ApeBinaryTag
    {
        IntPtr key, data;

        int length;

        /// <summary>
        /// The binary tag data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                if (data == IntPtr.Zero || length == 0)
                    return null;

                byte[] arr = new byte[length];
                Marshal.Copy(data, arr, 0, length);
                return arr;
            }
        }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Key
        {
            get
            {
                if (key == IntPtr.Zero)
                    return null;

                return Marshal.PtrToStringAnsi(key);
            }
        }

        /// <summary>
        /// The size of data in bytes.
        /// </summary>
        public int Length => length;

        public static ApeBinaryTag Read(int handle, int index)
        {
            IntPtr intPtr = Bass.ChannelGetTags(handle, TagType.ApeBinary + index);

            if (intPtr == IntPtr.Zero)
                return null;

            return (ApeBinaryTag)Marshal.PtrToStructure(intPtr, typeof(ApeBinaryTag));
        }

        /// <summary>
        /// Read all APE binary tags
        /// </summary>
        public static IEnumerable<ApeBinaryTag> ReadAll(int Handle)
        {
            ApeBinaryTag tag;

            for (int i = 0; (tag = Read(Handle, i)) != null; ++i)
                yield return tag;
        }

        /// <summary>
        /// Returns the Key of the binary tag.
        /// </summary>
        public override string ToString() => Key;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TAG_BEXT
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Description;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Originator;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string OriginatorReference;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string OriginationDate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string OriginationTime;

        public long TimeReference;

        public short Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] umid;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 190)]
        byte[] reserved;

        // TODO: Check what's size of codinghistory
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string codinghistory;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TAG_CART_TIMER
    {
        int Usage, Value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TAG_CART
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
        public TAG_CART_TIMER[] PostTimer;	// 8 time markers after head

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 276)]
        char[] Reserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string URL;					// uniform resource locator

        // TODO: What's size of this??
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        char[] TagText;				// free form text for scripts or tags

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
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TAG_CA_CODEC
    {
        public int ftype, atype;

        IntPtr name;

        public string Name => Marshal.PtrToStringAnsi(name);
    }

    /// <summary>
    /// Wraps an ID3v1 tag
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class ID3v1Tag
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        char[] id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Title;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Artist;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Album;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Year;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        char[] comment;

        public byte genre = 147;

        static string[] Genres = new string[148]
            {
                "Blues", "Classic Rock", "Country", "Dance", "Disco", "Funk", "Grunge", "Hip-Hop", "Jazz", "Metal",
                "New Age", "Oldies", "Other", "Pop", "R&B", "Rap", "Reggae", "Rock", "Techno", "Industrial",
                "Alternative", "Ska", "Death Metal", "Pranks", "Soundtrack", "Euro-Techno", "Ambient", "Trip-Hop",
                "Vocal", "Jazz+Funk", "Fusion", "Trance", "Classical", "Instrumental", "Acid", "House", "Game",
                "Sound Clip", "Gospel", "Noise", "Alternative Rock", "Bass", "Soul", "Punk", "Space", "Meditative",
                "Instrumental Pop", "Instrumental Rock", "Ethnic", "Gothic", "Darkwave", "Techno-Industrial",
                "Electronic", "Pop-Folk", "Eurodance", "Dream", "Southern Rock", "Comedy", "Cult", "Gangsta",
                "Top Christian Rap", "Pop/Funk", "Jungle", "Native US", "Cabaret", "New Wave", "Psychadelic", "Rave",
                "Showtunes", "Trailer", "Lo-Fi", "Tribal", "Acid Punk", "Acid Jazz", "Polka", "Retro", "Musical",
                "Rock & Roll", "Hard Rock", "Folk", "Folk-Rock", "National Folk", "Swing", "Fast Fusion", "Bebob",
                "Latin", "Revival", "Celtic", "Bluegrass", "Avantgarde", "Gothic Rock", "Progressive Rock",
                "Psychedelic Rock", "Symphonic Rock", "Slow Rock", "Big Band", "Chorus", "Easy Listening", "Acoustic",
                "Humour", "Speech", "Chanson", "Opera", "Chamber Music", "Sonata", "Symphony", "Booty Bass", "Primus",
                "Porn Groove", "Satire", "Slow Jam", "Club", "Tango", "Samba", "Folklore", "Ballad", "Power Ballad",
                "Rhythmic Soul", "Freestyle", "Duet", "Punk Rock", "Drum Solo", "Acapella", "Euro-House", "Dance Hall",
                "Goa", "Drum & Bass", "Club - House", "Hardcore", "Terror", "Indie", "BritPop", "Negerpunk",
                "Polsk Punk", "Beat", "Christian Gangsta Rap", "Heavy Metal", "Black Metal", "Crossover",
                "Contemporary Christian", "Christian Rock", "Merengue", "Salsa", "Thrash Metal", "Anime", "JPop",
                "Synthpop", "Unknown"
            };

        public static ID3v1Tag Read(int Handle)
        {
            return (ID3v1Tag)Marshal.PtrToStructure(Bass.ChannelGetTags(Handle, TagType.ID3), typeof(ID3v1Tag));
        }

        public string Comment
        {
            get
            {
                return comment == null ? ""
                                       : new string(comment).Replace("\0", "").Trim();
            }
        }

        public string Genre
        {
            get
            {
                if (genre > 147) return Genres[147];
                else return Genres[genre];
            }
        }

        public int TrackNo
        {
            get
            {
                // If 29th byte of comment[] is null ('\0'), then 30th byte is track number
                return comment[28] == '\0' ? comment[29] : -1;
            }
        }
    }

    public static class Tags
    {
        public static string GetLyrics3v2(int Handle) { return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.Lyrics3v2)); }

        public static string GetVendor(int Handle) { return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.OggEncoder)); }

        public static string GetRiffDisp(int Handle) { return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffDISP)); }

        public static string GetIcyShoutcastMeta(int Handle) { return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.META)); }

        public static string GetOggEncoder(int Handle) { return PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.OggEncoder)); }

        public static string GetWMAMidStreamTag(int Handle) { return PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.WmaMeta)); }

        public static IEnumerable<string> GetOggComments(int Handle)
        {
            return ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.OGG));
        }

        public static IEnumerable<string> GetHTTPHeaders(int Handle)
        {
            return ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.HTTP));
        }

        public static IEnumerable<string> GetICYHeaders(int Handle)
        {
            return ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.ICY));
        }

        public static IEnumerable<string> GetAPETags(int Handle)
        {
            return ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.APE));
        }

        public static IEnumerable<string> GetMP4Metadata(int Handle)
        {
            return ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MP4));
        }

        public static IEnumerable<string> GetWMATags(int Handle)
        {
            return ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.WMA));
        }

        public static IEnumerable<string> GetMFTags(int Handle)
        {
            return ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MF));
        }

        public static IEnumerable<string> GetRiffTags(int Handle)
        {
            return ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffInfo));
        }

        #region Multi Strings
        internal static IEnumerable<string> ExtractMultiStringAnsi(IntPtr ptr)
        {
            while (true)
            {
                string str = Marshal.PtrToStringAnsi(ptr);
                if (str.Length == 0)
                    break;
                yield return str;
                ptr += str.Length + 1 /* char \0 */;
            }
        }

        internal static IEnumerable<string> ExtractMultiStringUtf8(IntPtr ptr)
        {
            while (true)
            {
                string str = PtrToStringUtf8(ptr);
                if (str == null)
                    break;
                yield return str;
            }
        }

        static unsafe string PtrToStringUtf8(IntPtr ptr)
        {
            byte* bytes = (byte*)ptr.ToPointer();
            int size = 0;
            while (bytes[size] != 0) ++size;

            if (size == 0) return null;

            byte[] buffer = new byte[size];
            Marshal.Copy((IntPtr)ptr, buffer, 0, size);

            ptr += size;

            return Encoding.UTF8.GetString(buffer);
        }
        #endregion
    }
}
