﻿using System;
using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Writes a WMA File. Requires BassWma.dll
    /// </summary>
    public class WmaFileWriter : IAudioFileWriter
    {
        int EncoderHandle;

        public WmaFileWriter(string FilePath, int NoOfChannels = 2, int SampleRate = 44100, int BitRate = 128000)
        {
            EncoderHandle = BassWma.EncodeOpenFile(SampleRate, NoOfChannels, WMAEncodeFlags.Float, BitRate, FilePath);
        }

        public void Write(IntPtr Buffer, int Length) { BassWma.EncodeWrite(EncoderHandle, Buffer, Length); }

        public void Write(BufferProvider buffer) { Write(buffer.Pointer, buffer.ByteLength); }

        void Write(object buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            Write(gch.AddrOfPinnedObject(), Length);

            gch.Free();
        }

        public void Write(byte[] buffer, int Length) { Write(buffer as object, Length); }

        public void Write(short[] buffer, int Length) { Write(buffer as object, Length); }

        public void Write(float[] buffer, int Length) { Write(buffer as object, Length); }

        public void Dispose() { BassWma.EncodeClose(EncoderHandle); }
    }
}