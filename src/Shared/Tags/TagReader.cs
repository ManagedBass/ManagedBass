using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.Marshal;
using static ManagedBass.Bass;
using static ManagedBass.Extensions;

namespace ManagedBass.Tags
{
    /// <summary>
    /// Reads tags from a File or a Channel depending on the <see cref="ChannelType"/>.
    /// </summary>
    public sealed class TagReader
    {
        #region Properties
        /// <summary>
        /// Provides tags that didn't fit into any Properties.
        /// </summary>
        public Dictionary<string, string> Other { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Provides the Pictures read from File/Channel.
        /// </summary>
        public List<PictureTag> Pictures { get; } = new List<PictureTag>();

        /// <summary>
        /// Gets the Title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the Artist.
        /// </summary>
        public string Artist { get; private set; }

        /// <summary>
        /// Gets the Album.
        /// </summary>
        public string Album { get; private set; }

        /// <summary>
        /// Gets the Album Artist.
        /// </summary>
        public string AlbumArtist { get; private set; }

        /// <summary>
        /// Gets the Subtitle.
        /// </summary>
        public string Subtitle { get; private set; }

        /// <summary>
        /// Gets the Beats per Minute (BPM).
        /// </summary>
        public string BPM { get; private set; }
        
        /// <summary>
        /// Gets the Composer.
        /// </summary>
        public string Composer { get; private set; }

        /// <summary>
        /// Gets the Copyright.
        /// </summary>
        public string Copyright { get; private set; }
        
        /// <summary>
        /// Gets the Genre.
        /// </summary>
        public string Genre { get; private set; }

        /// <summary>
        /// Gets the Grouping.
        /// </summary>
        public string Grouping { get; private set; }

        /// <summary>
        /// Gets the Publisher.
        /// </summary>
        public string Publisher { get; private set; }
        
        /// <summary>
        /// Gets the Encoder.
        /// </summary>
        public string Encoder { get; private set; }

        /// <summary>
        /// Gets the Lyricist.
        /// </summary>
        public string Lyricist { get; private set; }

        /// <summary>
        /// Gets the Year.
        /// </summary>
        public string Year { get; private set; }

        /// <summary>
        /// Gets the Conductor.
        /// </summary>
        public string Conductor { get; private set; }
        
        /// <summary>
        /// Gets the Track Number.
        /// </summary>
        public string Track { get; private set; }

        /// <summary>
        /// Gets the Producer.
        /// </summary>
        public string Producer { get; private set; }

        /// <summary>
        /// Gets the Comment.
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// Gets the Mood.
        /// </summary>
        public string Mood { get; private set; }
        
        /// <summary>
        /// Gets the Rating.
        /// </summary>
        public string Rating { get; private set; }

        /// <summary>
        /// Gets the ISRC.
        /// </summary>
        public string ISRC { get; private set; }

        /// <summary>
        /// Gets the Remixer.
        /// </summary>
        public string Remixer { get; private set; }

        /// <summary>
        /// Gets the Lyrics.
        /// </summary>
        public string Lyrics { get; private set; }
        #endregion

        public static TagReader Read(string FileName)
        {
            Init();

            var h = CreateStream(FileName, Flags: BassFlags.Prescan);

            TagReader result = null;

            if (h != 0)
            {
                result = Read(h);

                StreamFree(h);
            }
            else
            {
                h = MusicLoad(FileName, Flags: BassFlags.Prescan);

                if (h != 0)
                {
                    result = Read(h);

                    MusicFree(h);
                }
            }

            if (!string.IsNullOrWhiteSpace(result?.Title))
                result.Title = System.IO.Path.GetFileNameWithoutExtension(FileName);

            return result;
        }

        public static TagReader Read(int Channel)
        {
            var result = new TagReader();
            var info = ChannelGetInfo(Channel);
            
            switch (info.ChannelType)
            {
                case ChannelType.MP1:
                case ChannelType.MP2:
                case ChannelType.MP3:
                    if (result.ReadID3v2(Channel)) { }

                    else if (result.ReadID3v1(Channel)) { }

                    else if (result.ReadApe(Channel)) { }

                    else if (result.ReadBWF(Channel)) { }
                    break;

                case ChannelType.OGG:
                    if (result.ReadOgg(Channel)) { }

                    else if (result.ReadApe(Channel)) { }
                    break;

                case ChannelType.MP4:
                case ChannelType.AAC:
                    if (result.ReadMp4(Channel)) { }

                    else if (result.ReadID3v2(Channel)) { }

                    else if (result.ReadApe(Channel)) { }

                    else if (result.ReadOgg(Channel)) { }
                    break;

                case ChannelType.Wave:
                case ChannelType.WavePCM:
                case ChannelType.WaveFloat:
                    if (result.ReadRiffInfo(Channel)) { }

                    else if (result.ReadBWF(Channel)) { }

                    else if (result.ReadID3v2(Channel)) { }
                    break;

                case ChannelType.DSD:
                    result.Title = PtrToStringAnsi(ChannelGetTags(Channel, TagType.DSDTitle));
                    result.Artist = PtrToStringAnsi(ChannelGetTags(Channel, TagType.DSDArtist));
                    break;

                case ChannelType.MOD:
                    result.Title = PtrToStringAnsi(ChannelGetTags(Channel, TagType.MusicName));
                    result.Artist = PtrToStringAnsi(ChannelGetTags(Channel, TagType.MusicAuth));
                    result.Comment = PtrToStringAnsi(ChannelGetTags(Channel, TagType.MusicMessage));
                    break;

                default:
                    if (result.ReadApe(Channel)) { }

                    else if (result.ReadOgg(Channel)) { }

                    else if (result.ReadID3v2(Channel)) { }

                    else if (result.ReadID3v1(Channel)) { }
                    break;
            }

            if (string.IsNullOrWhiteSpace(result.Lyrics))
            {
                var ptr = ChannelGetTags(Channel, TagType.Lyrics3v2);

                if (ptr != IntPtr.Zero)
                    result.Lyrics = PtrToStringAnsi(ptr);
            }

            return result;
        }

        public bool ReadApe(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.APE);

            if (ptr == IntPtr.Zero)
                return false;

            var arr = ExtractMultiStringUtf8(ptr);

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

            return true;
        }

        public bool ReadOgg(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.OGG);

            if (ptr == IntPtr.Zero)
                return false;

            var arr = ExtractMultiStringUtf8(ptr);

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

            var encoder = PtrToStringUtf8(ChannelGetTags(Channel, TagType.OggEncoder));
            if (!string.IsNullOrWhiteSpace(encoder))
                Encoder = encoder;

            return true;
        }

        public bool ReadRiffInfo(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.RiffInfo);

            if (ptr == IntPtr.Zero)
                return false;

            var arr = ExtractMultiStringAnsi(ptr);

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

            return true;
        }

        public bool ReadMp4(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.MP4);

            if (ptr == IntPtr.Zero)
                return false;

            var arr = ExtractMultiStringUtf8(ptr);

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

            return true;
        }

        public bool ReadID3v1(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.ID3);

            if (ptr == IntPtr.Zero)
                return false;
            
            var id3v1 = (ID3v1Tag)PtrToStructure(ptr, typeof(ID3v1Tag));

            Title = id3v1.Title;
            Artist = id3v1.Artist;
            Album = id3v1.Album;
            Year = id3v1.Year;
            Genre = id3v1.Genre;
            Track = id3v1.TrackNo.ToString();
            Comment = id3v1.Comment;

            return true;
        }

        public bool ReadID3v2(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.ID3v2);

            if (ptr == IntPtr.Zero)
                return false;

            var id3V2 = new ID3v2Tag(ptr);

            foreach (var frame in id3V2.TextFrames)
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

                    case "POPM":
                        Rating = frame.Value;
                        break;

                    default:
                        Other.Add(frame.Key, frame.Value);
                        break;
                }
            }

            Pictures.AddRange(id3V2.PictureFrames);

            return true;
        }

        public bool ReadBWF(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.RiffBext);

            if (ptr == IntPtr.Zero)
                return false;

            var tag = (BextTag)PtrToStructure(ptr, typeof(BextTag));

            Title = tag.Description;
            Artist = tag.Originator;
            Encoder = tag.OriginatorReference;
            Year = tag.OriginationDate.Split('-')[0];

            return true;
        }
    }
}