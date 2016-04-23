using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    public class ACMEncoder : BassEncEncoder
    {
        readonly EncodeFlags _flags;
        readonly string _fileName;
        readonly WaveFormatTag _encoding;
        readonly int _channel;

        public ACMEncoder(EncodeFlags Flags, WaveFormatTag Encoding, int Channel)
        {
            _flags = Flags;
            _encoding = Encoding;
            _channel = Channel;
        }

        public ACMEncoder(EncodeFlags Flags, WaveFormatTag Encoding, PCMFormat Format)
            : this(Flags, Encoding, GetDummyChannel(Format))
        {
        }

        public ACMEncoder(string FileName, EncodeFlags Flags, WaveFormatTag Encoding, int Channel)
        {
            if (FileName == null)
                throw new ArgumentNullException(nameof(FileName));

            _fileName = FileName;
            _flags = Flags;
            _encoding = Encoding;
            _channel = Channel;
        }

        public ACMEncoder(string FileName, EncodeFlags Flags, WaveFormatTag Encoding, PCMFormat Format)
            : this(FileName, Flags, Encoding, GetDummyChannel(Format))
        {
        }

        public ACMEncoder(Stream OutStream, EncodeFlags Flags, WaveFormatTag Encoding, int Channel) : base(OutStream)
        {
            _flags = Flags;
            _encoding = Encoding;
            _channel = Channel;
        }

        public ACMEncoder(Stream OutStream, EncodeFlags Flags, WaveFormatTag Encoding, PCMFormat Format)
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
                    return _fileName == null ? BassEnc.EncodeStartACM(_channel, acmFormat, _flags, _encodeProcedure)
                                             : BassEnc.EncodeStartACM(_channel, acmFormat, _flags | EncodeFlags.AutoFree, _fileName);

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