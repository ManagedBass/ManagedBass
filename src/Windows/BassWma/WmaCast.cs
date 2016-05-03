using ManagedBass.Enc;

namespace ManagedBass.Wma
{
    public class WmaCast : ICast
    {
        readonly PCMFormat _format;
        readonly int _bitrate;

        public WmaCast(PCMFormat Format, int BitRate = 128000)
        {
            _format = Format;
            _bitrate = BitRate;
        }
        
        int _handle;

        public bool IsConnected => _handle != 0;

        public void Connect()
        {
            if (!IsConnected)
            {
                var flags = WMAEncodeFlags.Default;
                
                if (_format.Resolution == Resolution.Byte)
                    flags |= WMAEncodeFlags.Byte;
                else if (_format.Resolution == Resolution.Float)
                    flags |= WMAEncodeFlags.Float;

                _handle = BassWma.EncodeOpenPublish(_format.Frequency, _format.Channels, flags, _bitrate, Url, UserName, PassWord);
            }
        }

        public void Disconnect()
        {
            if (IsConnected && BassWma.EncodeClose(_handle))
                _handle = 0;
        }

        public long DataSent => -1;
        
        public string Url { get; set; }
        
        public string UserName { get; set; }
        
        public string PassWord { get; set; }
    }
}