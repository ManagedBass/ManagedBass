using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    public class ACMEncoder : BassEncEncoder
    {
        readonly Func<IntPtr, int> _starter;
        readonly int _channel;
        readonly WaveFormatTag _encoding;
        
        public ACMEncoder(string FileName, EncodeFlags Flags, WaveFormatTag Encoding, int Channel)
        {
            if (FileName == null)
                throw new ArgumentNullException(nameof(FileName));

            _channel = Channel;
            _encoding = Encoding;
            _starter = AcmFormat => BassEnc.EncodeStartACM(_channel, AcmFormat, Flags, FileName);
        }

        public ACMEncoder(string FileName, EncodeFlags Flags, WaveFormatTag Encoding, WaveFormat Format)
            : this(FileName, Flags, Encoding, GetDummyChannel(Format))
        {
        }

        public ACMEncoder(Stream OutStream, EncodeFlags Flags, WaveFormatTag Encoding, int Channel)
            : base(OutStream)
        {
            _channel = Channel;
            _encoding = Encoding;
            _starter = AcmFormat => BassEnc.EncodeStartACM(_channel, AcmFormat, Flags, _encodeProcedure);
        }

        public ACMEncoder(Stream OutStream, EncodeFlags Flags, WaveFormatTag Encoding, WaveFormat Format)
            : this(OutStream, Flags, Encoding, GetDummyChannel(Format))
        {
        }

        public override string OutputFileExtension => "wav";
        public override ChannelType OutputType => ChannelType.Wave;

        public override int OnStart()
        {
            // Get the Length of the ACMFormat structure
            var suggestedFormatLength = BassEnc.GetACMFormat(0);
            var acmFormat = Marshal.AllocHGlobal(suggestedFormatLength);

            try
            {
                // Retrieve ACMFormat and Init Encoding
                if (BassEnc.GetACMFormat(_channel,
                     acmFormat,
                     suggestedFormatLength,
                     null,
                     // If encoding is Unknown, then let the User choose encoding.
                     _encoding == WaveFormatTag.Unknown ? 0 : ACMFormatFlags.Suggest,
                     _encoding) != 0)
                    return _starter(acmFormat);

            }
            catch { }
            finally
            {
                // Free the ACMFormat structure
                Marshal.FreeHGlobal(acmFormat);
            }

            return 0;
        }
    }
}