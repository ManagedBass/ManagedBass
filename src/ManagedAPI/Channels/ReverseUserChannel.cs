using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Playback Direction without BassFx.
    /// </summary>
    public class ReverseUserChannel : Channel
    {
        Channel decoder;
        int dHandle, qLength, BytesRead;
        StreamProcedure proc;
        float[] buffer;

        public ReverseUserChannel(Channel DecodingSource)
        {
			this.decoder = DecodingSource;
            this.dHandle = DecodingSource.Handle;
            proc = new StreamProcedure(Callback);

			if (!decoder.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");
			
			Handle = Bass.CreateStream((int)decoder.Frequency, decoder.ChannelCount, BassFlags.Float, proc);
                        
            Bass.ChannelSetPosition(dHandle, Bass.ChannelGetLength(dHandle) - 1);
        }

        public bool Reverse { get; set; } = true;

        unsafe int Callback(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            qLength = Length / 4;

            if (buffer == null || buffer.Length < qLength)
                buffer = new float[qLength];

            if (Reverse)
                Bass.ChannelSetPosition(dHandle, Bass.ChannelGetPosition(dHandle) - Length);

            BytesRead = Bass.ChannelGetData(dHandle, buffer, Length | (int)DataFlags.Float);
            
            qLength  = BytesRead / 4;

            if (Reverse)
            {
                var b = (float*)Buffer;

                for (int i = 0; i < qLength; ++i)
                    b[i] = buffer[qLength - i - 1];
            }
            else Marshal.Copy(buffer, 0, Buffer, qLength);

            return BytesRead;
        }

        public override void Dispose()
        {
            base.Dispose();
            decoder = null;
        }
    }
}