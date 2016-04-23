using System;
using ManagedBass.Wma;

namespace ManagedBass.Enc
{
    public class WMAEncoder : IDisposable
    {
        public int Handle { get; }

        public WMAEncoder(string FileName, PCMFormat Format, int BitRate = 128000)
        {
            var flags = WMAEncodeFlags.Default;

            switch (Format.Resolution)
            {
                case Resolution.Byte:
                    flags |= WMAEncodeFlags.Byte;
                    break;

                case Resolution.Float:
                    flags |= WMAEncodeFlags.Float;
                    break;
            }

            Handle = BassWma.EncodeOpenFile(Format.Frequency, Format.Channels, flags, BitRate, FileName);
        }
        
        public WMAEncoder(int Port, int Clients, PCMFormat Format, int BitRate = 128000)
        {
            var flags = WMAEncodeFlags.Default;

            switch (Format.Resolution)
            {
                case Resolution.Byte:
                    flags |= WMAEncodeFlags.Byte;
                    break;

                case Resolution.Float:
                    flags |= WMAEncodeFlags.Float;
                    break;
            }

            Handle = BassWma.EncodeOpenNetwork(Format.Frequency, Format.Channels, flags, BitRate, Port, Clients);
        }

        public WMAEncoder(string Url, string UserName, string Password, PCMFormat Format, int BitRate = 128000)
        {
            var flags = WMAEncodeFlags.Default;

            switch (Format.Resolution)
            {
                case Resolution.Byte:
                    flags |= WMAEncodeFlags.Byte;
                    break;

                case Resolution.Float:
                    flags |= WMAEncodeFlags.Float;
                    break;
            }

            Handle = BassWma.EncodeOpenPublish(Format.Frequency, Format.Channels, flags, BitRate, Url, UserName, Password);
        }

        public bool Write(IntPtr Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);
        public bool Write(byte[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);
        public bool Write(short[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);
        public bool Write(int[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);
        public bool Write(float[] Buffer, int Length) => BassWma.EncodeWrite(Handle, Buffer, Length);

        public void Dispose() => BassWma.EncodeClose(Handle);
    }
}
