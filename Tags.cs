using System;
using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public interface ITagsProvider { int Handle { get; } }

    public sealed class Tags
    {
        [StructLayout(LayoutKind.Sequential)]
        class ID3v1Tag
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] id;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public char[] title;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public char[] artist;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public char[] album;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] year;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
            public char[] comment;

            public byte genre;
        }

        #region Genres
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
        #endregion

        public Tags()
        {
            Title = "Untitled";
            Artist = "Unknown Artist";
            Album = "Unknown Album";
            Year = string.Empty;
            Comment = string.Empty;
            Genre = Genres[147];
            TrackNo = 0;
        }

        #region Properties
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }
        public int TrackNo { get; set; }
        #endregion

        public static Tags GetID3v1Tags(ITagsProvider ITA)
        {
            var tmp = (ID3v1Tag)Marshal.PtrToStructure(Bass.GetChannelTags(ITA.Handle, TagType.ID3), typeof(ID3v1Tag));

            Tags tags = new Tags();

            if (tmp.comment[28] == (char)0)
            {
                // if 29 byte of comment is null ('\0') then 30 byte of comment string is track nomber
                tags.TrackNo = tmp.comment[29];
                tmp.comment[29] = (char)0;
            }

            tags.Album = new string(tmp.album).Replace("\0", "").Trim();
            tags.Artist = new string(tmp.artist).Replace("\0", "").Trim();
            tags.Comment = new string(tmp.comment).Replace("\0", "").Trim();

            if (tmp.genre > 147) tags.Genre = Genres[147];
            else tags.Genre = Genres[tmp.genre];

            tags.Title = new string(tmp.title).Replace("\0", "").Trim();
            tags.Year = new string(tmp.year).Replace("\0", "").Trim();

            return tags;
        }

        static Tags GetID3v2Tags(ITagsProvider ITA)
        {
            IntPtr ptr = Bass.GetChannelTags(ITA.Handle, TagType.ID3v2);

            throw new NotImplementedException();
        }

        public static string GetLyrics3v2(ITagsProvider ITA) { return Marshal.PtrToStringAnsi(Bass.GetChannelTags(ITA.Handle, TagType.Lyrics3v2)); }

        public static string GetVendor(ITagsProvider ITA) { return Marshal.PtrToStringAnsi(Bass.GetChannelTags(ITA.Handle, TagType.Vendor)); }

        static Tags ChannelGetTags(int handle, TagType flags)
        {
            Tags tags = null;

            switch (flags)
            {
                case TagType.OGG:
                    break;
                case TagType.HTTP:
                    break;
                case TagType.ICY:
                    break;
                case TagType.META:
                    break;
                case TagType.APE:
                    break;
                case TagType.MP4:
                    break;
                case TagType.MF:
                    break;
                case TagType.WaveFormat:
                    break;
                case TagType.RIFFInfo:
                    break;
                case TagType.BASS_TAG_RIFF_BEXT:
                    break;
                case TagType.BASS_TAG_RIFF_CART:
                    break;
                case TagType.BASS_TAG_RIFF_DISP:
                    tags.Comment = Marshal.PtrToStringAnsi(Bass.GetChannelTags(handle, TagType.BASS_TAG_RIFF_DISP));
                    break;
                case TagType.BASS_TAG_APE_BINARY:
                    break;
                case TagType.MusicOrders:
                    break;
                case TagType.MusicInstrument:
                    break;
                case TagType.MusicSample:
                    break;
                default:
                    break;
            }

            return tags;
        }
    }
}