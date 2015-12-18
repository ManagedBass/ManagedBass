using System;
using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Writes a WMA File. Requires BassWma.dll
    /// </summary>
    public class WmaFileWriter : AudioFileWriter
    {        
        int EncoderHandle;
        
        public WmaFileWriter(string FilePath, int NoOfChannels = 2, int SampleRate = 44100, int BitRate = 128000)
        {
            EncoderHandle = BassWma.EncodeOpenFile(SampleRate, NoOfChannels, WMAEncodeFlags.Float, BitRate, FilePath);
        }

        public void Write(IntPtr Buffer, int Length) { BassWma.EncodeWrite(EncoderHandle, Buffer, Length); }

        public void Write(BufferProvider buffer) 
        {
            int Length = buffer.BufferKind == Resolution.Float ? buffer.FloatLength
                : buffer.ByteLength;

            Write(buffer.Pointer, Length);
        }

        public override void Write(float[] buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            Write(gch.AddrOfPinnedObject(), Length);

            gch.Free();
        }

        public override void Close() { BassWma.EncodeClose(EncoderHandle); }
    }
}