using System.Runtime.InteropServices;
using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    public class AudioSample : Channel
    {
        int Sample;

        public AudioSample(int Length, int Frequency = 44100, int Channels = 2, Resolution Resolution = Resolution.Short)
        {
            Sample = Bass.CreateSample(Length, Frequency, Channels, 1, Resolution.ToBassFlag());
            Handle = Bass.SampleGetChannel(Sample, true);
        }

        public AudioSample(string FilePath, Resolution Resolution = Resolution.Short)
        {
            Sample = Bass.SampleLoad(FilePath, 0, 0, 1, Resolution.ToBassFlag());
            Handle = Bass.SampleGetChannel(Sample, true);
        }

        public AudioSample(byte[] Memory, int Length, Resolution Resolution = Resolution.Short)
        {
            // Pin
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Sample = Bass.SampleLoad(GCPin.AddrOfPinnedObject(), 0, Length, 1, Resolution.ToBassFlag());
            Handle = Bass.SampleGetChannel(Sample, true);

            GCPin.Free();
        }

        public long Length { get { return Bass.SampleGetInfo(Sample).Length; } }

        #region Read Sample Data
        public bool ReadSampleData(IntPtr Buffer) { return Bass.SampleGetData(Sample, Buffer); }

        bool ReadObj(object Buffer)
        {
            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            bool Result = Bass.SampleGetData(Sample, gch.AddrOfPinnedObject());

            gch.Free();

            return Result;
        }

        public bool ReadSampleData(byte[] Buffer) { return ReadObj(Buffer); }

        public bool ReadSampleData(float[] Buffer) { return ReadObj(Buffer); }

        public bool ReadSampleData(short[] Buffer) { return ReadObj(Buffer); }

        public bool ReadSampleData(int[] Buffer) { return ReadObj(Buffer); }
        #endregion

        #region Write Sample Data
        public bool WriteSampleData(IntPtr Buffer) { return Bass.SampleSetData(Sample, Buffer); }

        bool WriteObj(object buffer)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            bool Result = Bass.SampleSetData(Sample, gch.AddrOfPinnedObject());

            gch.Free();

            return Result;
        }

        public bool WriteSampleData(byte[] buffer) { return WriteObj(buffer); }

        public bool WriteSampleData(float[] buffer) { return WriteObj(buffer); }

        public bool WriteSampleData(short[] buffer) { return WriteObj(buffer); }

        public bool WriteSampleData(int[] buffer) { return WriteObj(buffer); }
        #endregion

        public override void Dispose() { try { Bass.SampleFree(Sample); } catch { } }
    }
}
