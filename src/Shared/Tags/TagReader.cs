using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.Marshal;
using static ManagedBass.Bass;
using static ManagedBass.Extensions;

namespace ManagedBass.Tags
{
    /// <summary>
    /// Reads tags from a File or a Channel depending on the <see cref="ChannelType"/>.
    /// </summary>
    public sealed class TagReader : TagProperties<string>
    {
        /// <summary>
        /// Provides tags that didn't fit into any Properties.
        /// </summary>
        public Dictionary<string, string> Other { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Provides the Pictures read from File/Channel.
        /// </summary>
        public List<PictureTag> Pictures { get; } = new List<PictureTag>();

        #region Lookup Tables
        static readonly TagProperties<IEnumerable<string>> ApeLookupTable = new TagProperties<IEnumerable<string>>
        {
            Title = new[] {"title"},
            Artist = new[] {"artist"},
            Album = new[] {"album"},
            AlbumArtist = new [] {"album artist"},
            Track = new[] {"track"},
            Year = new[] {"year"},
            Genre = new[] {"genre"},
            Copyright = new[] {"copyright"},
            Encoder = new[] {"encodedby"},
            Publisher = new[] {"label"},
            Composer = new[] {"composer"},
            Conductor = new[] {"conductor"},
            Lyricist = new[] {"lyricist"},
            Remixer = new[] {"remixer"},
            Producer = new[] {"producer"},
            Comment = new[] {"comment"},
            Grouping = new[] {"grouping"},
            Mood = new[] {"mood"},
            Rating = new[] {"rating"},
            ISRC = new[] {"isrc"},
            BPM = new[] {"bpm"}
        };

        static readonly TagProperties<IEnumerable<string>> OggLookupTable = new TagProperties<IEnumerable<string>>
        {
            Title = new[] {"title"},
            Artist = new[] {"artist"},
            Album = new[] {"album"},
            AlbumArtist = new[] {"albumartist"},
            Track = new[] {"tracknumber"},
            Year = new[] {"date"},
            Genre = new[] {"genre"},
            Copyright = new[] {"copyright"},
            Encoder = new[] {"encodedby"},
            Publisher = new[] {"label"},
            Composer = new[] {"composer"},
            Conductor = new[] {"conductor"},
            Lyricist = new[] {"lyricist"},
            Remixer = new[] {"remixer"},
            Producer = new[] {"producer"},
            Comment = new[] {"comment"},
            Grouping = new[] {"grouping"},
            Mood = new[] {"mood"},
            Rating = new[] {"rating"},
            ISRC = new[] {"isrc"},
            BPM = new[] {"bpm"}
        };

        static readonly TagProperties<IEnumerable<string>> RiffInfoLookupTable = new TagProperties<IEnumerable<string>>
        {
            Title = new[] {"inam"},
            Artist = new[] {"iart"},
            Album = new[] {"iprd"},
            AlbumArtist = new[] {"isbj"},
            Track = new[] {"itrk", "iprt"},
            Year = new[] {"icrd"},
            Genre = new[] {"ignr"},
            Copyright = new[] {"icop"},
            Encoder = new[] {"isft"},
            Publisher = new[] {"icms"},
            Composer = new[] {"ieng"},
            Conductor = new[] {"itch"},
            Lyricist = new[] {"iwri"},
            Remixer = new[] {"iedt"},
            Producer = new[] {"ipro"},
            Comment = new[] {"icmt"},
            Grouping = new[] {"isrf"},
            Mood = new[] {"ikey"},
            Rating = new[] {"ishp"},
            ISRC = new[] {"isrc"},
            BPM = new[] {"ibpm"}
        };

        static readonly TagProperties<IEnumerable<string>> Mp4LookupTable = new TagProperties<IEnumerable<string>>
        {
            Title = new[] { "©nam" },
            Artist = new[] { "©art" },
            Album = new[] { "©alb" },
            AlbumArtist = new[] { "aart" },
            Track = new[] { "trkn" },
            Year = new[] { "©day" },
            Genre = new[] { "©gen" },
            Copyright = new[] { "cprt" },
            Encoder = new[] { "©too" },
            Composer = new[] { "©wrt" },
            Comment = new[] { "©cmt" },
            Grouping = new[] { "©grp" },
            Rating = new[] { "rtng" },
        };

        static readonly TagProperties<IEnumerable<string>> Id3v2LookupTable = new TagProperties<IEnumerable<string>>
        {
            Title = new[] { "TIT2", "TT2" },
            Artist = new[] { "TPE1", "TP1" },
            Album = new[] { "TALB", "TAL" },
            AlbumArtist = new[] { "TPE2", "TP2" },
            Subtitle = new[] { "TIT3", "TT3" },
            Track = new[] { "TRK", "TRCK" },
            Year = new[] { "TYER", "TYE" },
            Genre = new[] { "TCON", "TCO" },
            Copyright = new[] { "TCOP", "TCR" },
            Encoder = new[] { "TENC", "TEN" },
            Publisher = new[] { "TPUB", "TPB" },
            Composer = new[] { "TCOM", "TCM" },
            Conductor = new[] { "TPE3", "TP3" },
            Lyricist = new[] { "TEXT", "TXT" },
            Remixer = new[] { "TPE4", "TP4" },
            Producer = new[] { "TIPL" },
            Comment = new[] { "COMM", "COM" },
            Grouping = new[] { "TIT1", "TT1" },
            Mood = new[] { "TMOO" },
            Rating = new[] { "POPM" },
            ISRC = new[] { "TSCR" },
            BPM = new[] { "TBPM", "TBP" }
        };
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

        IEnumerable<KeyValuePair<string, string>> ReadUsingLookupTable(IEnumerable<string> Tags, TagProperties<IEnumerable<string>> LookupTable, char Separator)
        {
            foreach (var tag in Tags)
            {
                var arrx = tag.Split(Separator);
                var key = arrx[0].ToLower();
                var value = arrx[1];

                if (!SetTagUsingLookupTable(key, value, LookupTable))
                    yield return new KeyValuePair<string, string>(arrx[0], value);
            }
        }

        bool SetTagUsingLookupTable(string Key, string Value, TagProperties<IEnumerable<string>> LookupTable)
        {
            if (LookupTable.Title.Contains(Key))
                Title = Value;

            else if (LookupTable.Artist.Contains(Key))
                Artist = Value;

            else if (LookupTable.Album.Contains(Key))
                Album = Value;

            else if (LookupTable.AlbumArtist.Contains(Key))
                AlbumArtist = Value;

            else if (LookupTable.Subtitle.Contains(Key))
                Subtitle = Value;

            else if (LookupTable.BPM.Contains(Key))
                BPM = Value;

            else if (LookupTable.Composer.Contains(Key))
                Composer = Value;

            else if (LookupTable.Copyright.Contains(Key))
                Copyright = Value;

            else if (LookupTable.Genre.Contains(Key))
                Genre = Value;

            else if (LookupTable.Grouping.Contains(Key))
                Grouping = Value;

            else if (LookupTable.Publisher.Contains(Key))
                Publisher = Value;

            else if (LookupTable.Encoder.Contains(Key))
                Encoder = Value;

            else if (LookupTable.Lyricist.Contains(Key))
                Lyricist = Value;

            else if (LookupTable.Year.Contains(Key))
                Year = Value;

            else if (LookupTable.Conductor.Contains(Key))
                Conductor = Value;

            else if (LookupTable.Track.Contains(Key))
                Track = Value;

            else if (LookupTable.Producer.Contains(Key))
                Producer = Value;

            else if (LookupTable.Comment.Contains(Key))
                Comment = Value;

            else if (LookupTable.Mood.Contains(Key))
                Mood = Value;

            else if (LookupTable.Rating.Contains(Key))
                Rating = Value;

            else if (LookupTable.ISRC.Contains(Key))
                ISRC = Value;

            else if (LookupTable.Remixer.Contains(Key))
                Remixer = Value;

            else if (LookupTable.Lyrics.Contains(Key))
                Lyrics = Value;

            else return false;

            return true;
        }

        #region Specific Tag Types
        public bool ReadApe(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.APE);

            if (ptr == IntPtr.Zero)
                return false;

            foreach (var otherTag in ReadUsingLookupTable(ExtractMultiStringUtf8(ptr), ApeLookupTable, '='))
                Other.Add(otherTag.Key, otherTag.Value);

            return true;
        }

        public bool ReadOgg(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.OGG);

            if (ptr == IntPtr.Zero)
                return false;
            
            foreach (var otherTag in ReadUsingLookupTable(ExtractMultiStringUtf8(ptr), OggLookupTable, '='))
                Other.Add(otherTag.Key, otherTag.Value);

            if (string.IsNullOrWhiteSpace(Encoder))
            {
                var encoderPtr = ChannelGetTags(Channel, TagType.OggEncoder);
                if (encoderPtr != IntPtr.Zero)
                    Encoder = PtrToStringUtf8(encoderPtr);
            }

            return true;
        }

        public bool ReadRiffInfo(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.RiffInfo);

            if (ptr == IntPtr.Zero)
                return false;

            foreach (var otherTag in ReadUsingLookupTable(ExtractMultiStringAnsi(ptr), RiffInfoLookupTable, '='))
                Other.Add(otherTag.Key, otherTag.Value);
            
            return true;
        }

        public bool ReadMp4(int Channel)
        {
            var ptr = ChannelGetTags(Channel, TagType.MP4);

            if (ptr == IntPtr.Zero)
                return false;

            foreach (var otherTag in ReadUsingLookupTable(ExtractMultiStringUtf8(ptr), Mp4LookupTable, '='))
                Other.Add(otherTag.Key, otherTag.Value);
            
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
                if (!SetTagUsingLookupTable(frame.Key, frame.Value, Id3v2LookupTable))
                    Other.Add(frame.Key, frame.Value);

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
        #endregion
    }
}