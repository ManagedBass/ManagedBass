using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass.Tags
{
    public class TagReader
    {
        TagReader() { }

        public Dictionary<string, string> Other { get; } = new Dictionary<string, string>();

        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string AlbumArtist { get; private set; }
        public string Subtitle { get; private set; }
        public string BPM { get; private set; }
        public string Composer { get; private set; }
        public string Copyright { get; private set; }
        public string Genre { get; private set; }
        public string Grouping { get; private set; }
        public string Publisher { get; private set; }
        public string Encoder { get; private set; }
        public string Lyricist { get; private set; }
        public string Year { get; private set; }
        public string Conductor { get; private set; }
        public string Track { get; private set; }
        public string Producer { get; private set; }
        public string Comment { get; private set; }
        public string Mood { get; private set; }
        public string Rating { get; private set; }
        public string ISRC { get; private set; }
        public string Remixer { get; private set; }

        public static TagReader Read(string FileName)
        {
            int h = Bass.CreateStream(FileName, Flags: BassFlags.Prescan);

            TagReader Result = null;

            if (h != 0)
            {
                Result = Read(h);

                Bass.StreamFree(h);
            }
            else
            {
                h = Bass.MusicLoad(FileName, Flags: BassFlags.Prescan);

                if (h != 0)
                {
                    Result = Read(h);

                    Bass.MusicFree(h);
                }
            }

            return Result;
        }

        public static TagReader Read(int Channel)
        {
            TagReader TH = new TagReader();
            var info = Bass.ChannelGetInfo(Channel);
            var ctype = info.ChannelType;

            IntPtr ptr = IntPtr.Zero;

            // Mp3, Mp2, Mp1
            if (ctype.Is(ChannelType.MP3, ChannelType.MP2, ChannelType.MP1))
            {
                // ID3v2
                ptr = Bass.ChannelGetTags(Channel, TagType.ID3v2);
                
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadID3v2(ptr);
                    return TH;
                }

                // ID3v1
                ptr = Bass.ChannelGetTags(Channel, TagType.ID3);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadID3v1(ptr);
                    return TH;
                }

                // APE
                ptr = Bass.ChannelGetTags(Channel, TagType.APE);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadApe(ptr);
                    return TH;
                }

                // BWF
                ptr = Bass.ChannelGetTags(Channel, TagType.RiffBext);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadBWF(ptr);
                    return TH;
                }
            }

            else if (ctype == ChannelType.OGG)
            {
                // OGG
                ptr = Bass.ChannelGetTags(Channel, TagType.OGG);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadOgg(ptr);
                    
                    var encoder = Extensions.PtrToStringUtf8(Bass.ChannelGetTags(Channel, TagType.OggEncoder));
                    if (!string.IsNullOrWhiteSpace(encoder))
                        TH.Encoder = encoder;

                    return TH;
                }

                // APE
                ptr = Bass.ChannelGetTags(Channel, TagType.APE);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadApe(ptr);
                    return TH;
                }
            }

            // WMA
            else if (ctype == ChannelType.WMA)
            {
                ptr = Bass.ChannelGetTags(Channel, TagType.WMA);

                // TODO: Implement specific to WMA
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadApe(ptr);
                    return TH;
                }
            }

            // Mp4, Aac
            else if (ctype.Is(ChannelType.MP4, ChannelType.AAC))
            {
                // Mp4
                ptr = Bass.ChannelGetTags(Channel, TagType.MP4);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadMp4(ptr);
                    return TH;
                }

                // ID3v2
                ptr = Bass.ChannelGetTags(Channel, TagType.ID3v2);
                
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadID3v2(ptr);
                    return TH;
                }

                // APE
                ptr = Bass.ChannelGetTags(Channel, TagType.APE);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadApe(ptr);
                    return TH;
                }

                // OGG
                ptr = Bass.ChannelGetTags(Channel, TagType.OGG);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadOgg(ptr);
                    
                    var encoder = Extensions.PtrToStringUtf8(Bass.ChannelGetTags(Channel, TagType.OggEncoder));
                    if (!string.IsNullOrWhiteSpace(encoder))
                        TH.Encoder = encoder;

                    return TH;
                }
            }

            // Wave
            else if (ctype == ChannelType.Wave)
            {
                // RiffInfo
                ptr = Bass.ChannelGetTags(Channel, TagType.RiffInfo);
                
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadRiffInfo(ptr);
                    return TH;
                }

                // BWF
                ptr = Bass.ChannelGetTags(Channel, TagType.RiffBext);
                
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadBWF(ptr);
                    return TH;
                }

                // ID3v2
                ptr = Bass.ChannelGetTags(Channel, TagType.ID3v2);
                
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadID3v2(ptr);
                    return TH;
                }
            }

            else if (ctype == ChannelType.DSD)
            {
                TH.Title = Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Channel, TagType.DSDTitle));
                TH.Artist = Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Channel, TagType.DSDArtist));
            }

            else if (ctype.HasFlag(ChannelType.MOD))
            {
                TH.Title = Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Channel, TagType.MusicName));
                TH.Artist = Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Channel, TagType.MusicAuth));
                TH.Comment = Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Channel, TagType.MusicMessage));
            }

            else
            {
                // APE
                ptr = Bass.ChannelGetTags(Channel, TagType.APE);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadApe(ptr);
                    return TH;
                }

                // OGG
                ptr = Bass.ChannelGetTags(Channel, TagType.OGG);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadOgg(ptr);
                    
                    var encoder = Extensions.PtrToStringUtf8(Bass.ChannelGetTags(Channel, TagType.OggEncoder));
                    if (!string.IsNullOrWhiteSpace(encoder))
                        TH.Encoder = encoder;

                    return TH;
                }

                // ID3v2
                ptr = Bass.ChannelGetTags(Channel, TagType.ID3v2);
                
                if (ptr != IntPtr.Zero)
                {
                    TH.ReadID3v2(ptr);
                    return TH;
                }

                // ID3v1
                ptr = Bass.ChannelGetTags(Channel, TagType.ID3);

                if (ptr != IntPtr.Zero)
                {
                    TH.ReadID3v1(ptr);
                    return TH;
                }
            }

            return TH;
        }

        void ReadApe(IntPtr ptr)
        {
            var arr = Extensions.ExtractMultiStringUtf8(ptr);

            foreach (var item in arr)
            {
                var arrx = item.Split('=');
                var Key = arrx[0];
                var Value = arrx[1];

                switch (Key.ToLower())
                {
                    case "title":
                        Title = Value;
                        break;

                    case "artist":
                        Artist = Value;
                        break;

                    case "album":
                        Album = Value;
                        break;

                    case "album artist":
                        AlbumArtist = Value;
                        break;
                        
                    case "track":
                        Track = Value;
                        break;

                    case "year":
                        Year = Value;
                        break;

                    case "genre":
                        Genre = Value;
                        break;

                    case "copyright":
                        Copyright = Value;
                        break;

                    case "encodedby":
                        Encoder = Value;
                        break;

                    case "label":
                        Publisher = Value;
                        break;

                    case "composer":
                        Composer = Value;
                        break;

                    case "conductor":
                        Conductor = Value;
                        break;

                    case "lyricist":
                        Lyricist = Value;
                        break;

                    case "remixer":
                        Remixer = Value;
                        break;

                    case "producer":
                        Producer = Value;
                        break;

                    case "comment":
                        Comment = Value;
                        break;

                    case "grouping":
                        Grouping = Value;
                        break;

                    case "mood":
                        Mood = Value;
                        break;

                    case "rating":
                        Rating = Value;
                        break;

                    case "isrc":
                        ISRC = Value;
                        break;

                    case "bpm":
                        BPM = Value;
                        break;

                    default:
                        Other.Add(Key, Value);
                        break;
                }
            }
        }

        void ReadOgg(IntPtr ptr)
        {
            var arr = Extensions.ExtractMultiStringUtf8(ptr);

            foreach (var item in arr)
            {
                var arrx = item.Split('=');
                var Key = arrx[0];
                var Value = arrx[1];

                switch (Key.ToLower())
                {
                    case "title":
                        Title = Value;
                        break;

                    case "artist":
                        Artist = Value;
                        break;

                    case "album":
                        Album = Value;
                        break;

                    case "albumartist":
                        AlbumArtist = Value;
                        break;
                        
                    case "tracknumber":
                        Track = Value;
                        break;

                    case "date":
                        Year = Value;
                        break;

                    case "genre":
                        Genre = Value;
                        break;

                    case "copyright":
                        Copyright = Value;
                        break;

                    case "encodedby":
                        Encoder = Value;
                        break;

                    case "label":
                        Publisher = Value;
                        break;

                    case "composer":
                        Composer = Value;
                        break;

                    case "conductor":
                        Conductor = Value;
                        break;

                    case "lyricist":
                        Lyricist = Value;
                        break;

                    case "remixer":
                        Remixer = Value;
                        break;

                    case "producer":
                        Producer = Value;
                        break;

                    case "comment":
                        Comment = Value;
                        break;

                    case "grouping":
                        Grouping = Value;
                        break;

                    case "mood":
                        Mood = Value;
                        break;

                    case "rating":
                        Rating = Value;
                        break;

                    case "isrc":
                        ISRC = Value;
                        break;

                    case "bpm":
                        BPM = Value;
                        break;

                    default:
                        Other.Add(Key, Value);
                        break;
                }
            }
        }

        void ReadRiffInfo(IntPtr ptr)
        {
            var arr = Extensions.ExtractMultiStringAnsi(ptr);

            foreach (var item in arr)
            {
                var arrx = item.Split('=');
                var Key = arrx[0];
                var Value = arrx[1];

                switch (Key.ToLower())
                {
                    case "inam":
                        Title = Value;
                        break;

                    case "iart":
                        Artist = Value;
                        break;

                    case "iprd":
                        Album = Value;
                        break;

                    case "isbj":
                        AlbumArtist = Value;
                        break;
                        
                    case "iprt":
                    case "itrk":
                        Track = Value;
                        break;

                    case "icrd":
                        Year = Value;
                        break;

                    case "ignr":
                        Genre = Value;
                        break;

                    case "icop":
                        Copyright = Value;
                        break;

                    case "isft":
                        Encoder = Value;
                        break;

                    case "icms":
                        Publisher = Value;
                        break;

                    case "ieng":
                        Composer = Value;
                        break;

                    case "itch":
                        Conductor = Value;
                        break;

                    case "iwri":
                        Lyricist = Value;
                        break;

                    case "iedt":
                        Remixer = Value;
                        break;

                    case "ipro":
                        Producer = Value;
                        break;

                    case "icmt":
                        Comment = Value;
                        break;

                    case "isrf":
                        Grouping = Value;
                        break;

                    case "ikey":
                        Mood = Value;
                        break;

                    case "ishp":
                        Rating = Value;
                        break;

                    case "isrc":
                        ISRC = Value;
                        break;

                    case "ibpm":
                        BPM = Value;
                        break;

                    default:
                        Other.Add(Key, Value);
                        break;
                }
            }
        }

        void ReadMp4(IntPtr ptr)
        {
            var arr = Extensions.ExtractMultiStringUtf8(ptr);

            foreach (var item in arr)
            {
                var arrx = item.Split('=');
                var Key = arrx[0];
                var Value = arrx[1];

                switch (Key.ToLower())
                {
                    case "©nam":
                        Title = Value;
                        break;

                    case "©art":
                        Artist = Value;
                        break;

                    case "©alb":
                        Album = Value;
                        break;

                    case "aart":
                        AlbumArtist = Value;
                        break;
                        
                    case "trkn":
                        Track = Value;
                        break;

                    case "©day":
                        Year = Value;
                        break;

                    case "©gen":
                        Genre = Value;
                        break;

                    case "cprt":
                        Copyright = Value;
                        break;

                    case "©too":
                        Encoder = Value;
                        break;
                        
                    case "©wrt":
                        Composer = Value;
                        break;
                        
                    case "©cmt":
                        Comment = Value;
                        break;

                    case "©grp":
                        Grouping = Value;
                        break;
                        
                    case "rtng":
                        Rating = Value;
                        break;
                        
                    default:
                        Other.Add(Key, Value);
                        break;
                }
            }
        }

        void ReadID3v1(IntPtr ptr)
        {
            var id3v1 = (ID3v1Tag)Marshal.PtrToStructure(ptr, typeof(ID3v1Tag));

            Title = id3v1.Title;
            Artist = id3v1.Artist;
            Album = id3v1.Album;
            Year = id3v1.Year;
            Genre = id3v1.Genre;
            Track = id3v1.TrackNo.ToString();
            Comment = id3v1.Comment;
        }

        void ReadID3v2(IntPtr ptr)
        {
            foreach (var frame in new ID3v2Tag(ptr).TextFrames)
            {
                switch (frame.Key)
                {
                    case "TALB":
                    case "TAL":
                        Album = frame.Value;
                        break;

                    case "TBPM":
                    case "TBP":
                        BPM = frame.Value;
                        break;

                    case "TCOM":
                    case "TCM":
                        Composer = frame.Value;
                        break;

                    case "TCOP":
                    case "TCR":
                        Copyright = frame.Value;
                        break;

                    case "TIT1":
                    case "TT1":
                        Grouping = frame.Value;
                        break;

                    case "TIT2":
                    case "TT2":
                        Title = frame.Value;
                        break;

                    case "TIT3":
                    case "TT3":
                        Subtitle = frame.Value;
                        break;

                    case "TPE1":
                    case "TP1":
                        Artist = frame.Value;
                        break;

                    case "TPE2":
                    case "TP2":
                        AlbumArtist = frame.Value;
                        break;

                    case "TCON":
                    case "TCO":
                        Genre = frame.Value;
                        break;

                    case "TPUB":
                    case "TPB":
                        Publisher = frame.Value;
                        break;

                    case "TENC":
                    case "TEN":
                        Encoder = frame.Value;
                        break;

                    case "TEXT":
                    case "TXT":
                        Lyricist = frame.Value;
                        break;

                    case "TYER":
                    case "TYE":
                        Year = frame.Value;
                        break;

                    case "TPE3":
                    case "TP3":
                        Conductor = frame.Value;
                        break;

                    case "TRK":
                    case "TRCK":
                        Track = frame.Value;
                        break;

                    case "TPE4":
                    case "TP4":
                        Remixer = frame.Value;
                        break;

                    case "TIPL":
                        Producer = frame.Value;
                        break;

                    case "COMM":
                    case "COM":
                        Comment = frame.Value;
                        break;

                    case "TMOO":
                        Mood = frame.Value;
                        break;

                    case "TSCR":
                        ISRC = frame.Value;
                        break;

                    default:
                        Other.Add(frame.Key, frame.Value);
                        break;
                }
            }
        }

        void ReadBWF(IntPtr ptr)
        {
            var tag = (BextTag)Marshal.PtrToStructure(ptr, typeof(BextTag));

            Title = tag.Description;
            Artist = tag.Originator;
            Encoder = tag.OriginatorReference;
            Year = tag.OriginationDate.Split('-')[0];
        }
    }
}