using System;

namespace ManagedBass.Enc
{
    public sealed class IceCast : BassEncCast
    {
        public IceCast(BassEncEncoder Encoder) : base(Encoder)
        {
            switch (Encoder.OutputType)
            {
                case ChannelType.OGG:
                case ChannelType.MP3:
                case ChannelType.AAC:
                case ChannelType.FLAC_OGG:
                case ChannelType.OPUS:
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
                    case ChannelType.OGG:
                    case ChannelType.FLAC_OGG:
                    case ChannelType.OPUS:
                        return MimeOgg;
                    
                    case ChannelType.MP3:
                        return MimeMp3;
                        
                    case ChannelType.AAC:
                        return MimeAac;
                    
                    default:
                        return null;
                }                
            }
        }
        
        #region Override
        protected override string _server => $"{ServerAddress}:{ServerPort}{MountPoint}";
        protected override string _headers => Quality == null ? null : $"ice-bitrate: {Quality}\r\n";
        protected override int _bitrate => Quality == null ? Encoder.OutputBitRate : 0;
        #endregion
        
        public string MountPoint { get; set; } = "/stream.ogg";
        
        public string Quality { get; set; }
    }
}