using System;

namespace ManagedBass.Enc
{
    public class ShoutCast : BassEncCast
    {
        public ShoutCast(BassEncEncoder Encoder) : base(Encoder)
        {
            switch (Encoder.OutputType)
            {
                case ChannelType.MP3:
                case ChannelType.AAC:
                    break;

                default:
                    throw new ArgumentException("Only Mp3, Aac, Flac_Ogg, Ogg and Opus Encoder are supported.");
            }
        }

        protected override string Mime
        {
            get
            {
                switch (Encoder.OutputType)
                {
                    case ChannelType.MP3:
                        return MimeTypes.Mp3;

                    case ChannelType.AAC:
                        return MimeTypes.Aac;

                    default:
                        return null;
                }
            }
        }

        protected override string _server => SID == null ? $"{ServerAddress}:{ServerPort}" : $"{ServerAddress}:{ServerPort},{SID}";
        
        public string SID { get; set; }
    }
}