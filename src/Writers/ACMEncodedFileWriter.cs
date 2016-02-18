using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Writes audio to a file, encoding it using ACM
    /// </summary>
    public class ACMEncodedFileWriter : ACMFileEncoder, IAudioFileWriter
    {
        public ACMEncodedFileWriter(string FileName, WaveFormatTag encoding,
            int Channels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short, EncodeFlags flags = EncodeFlags.Default)
            : base(FileName, Bass.CreateStream(SampleRate, Channels, Resolution.ToBassFlag() | BassFlags.Decode, StreamProcedureType.Push),
                   EncodeFlags.Default, encoding)
        {
            this.Resolution = Resolution;
        }

        public void Write(IntPtr Buffer, int Length)
        {
            // Push Data to the encoder
            Bass.StreamPutData(Channel, Buffer, Length);

            // Retrieve data from encoder to encode the data
            Bass.ChannelGetData(Channel, Buffer, Length);
        }

        void Write(object buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            Write(gch.AddrOfPinnedObject(), Length);

            gch.Free();
        }

        public void Write(byte[] buffer, int Length) => Write(buffer as object, Length);

        public void Write(short[] buffer, int Length) => Write(buffer as object, Length);

        public void Write(float[] buffer, int Length) => Write(buffer as object, Length);

        public Resolution Resolution { get; }
    }
}