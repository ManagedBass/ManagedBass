using System.Collections.Generic;

namespace ManagedBass
{
    public class ID3v2Tag
    {
        Dictionary<string, string> Other = new Dictionary<string, string>();

        ID3v2Tag(Dictionary<string, string> Frames)
        {
            foreach (var frame in Frames)
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

                    default:
                        Other.Add(frame.Key, frame.Value);
                        break;
                }
            }
        }

        public string Artist { get; }
        public string Album { get; }
        public string AlbumArtist { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public string BPM { get; }
        public string Composer { get; }
        public string Copyright { get; }
        public string Genre { get; }
        public string Grouping { get; }
        public string Publisher { get; }
        public string Encoder { get; }
        public string Lyricist { get; }
        public string Year { get; }
        public string Conductor { get; }

        public static ID3v2Tag Read(int Channel)
        {
            var ptr = Bass.ChannelGetTags(Channel, TagType.ID3v2);

            return new ID3v2Tag(new ID3v2Reader(ptr).TextFrames);
        }
    }
}