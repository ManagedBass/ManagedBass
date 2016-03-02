using System;
using System.Runtime.InteropServices;
using static ManagedBass.Dynamics.Bass;

namespace ManagedBass
{
    public class AudioSample : Channel
    {
        int Sample;

        public AudioSample(int Length, int Frequency = 44100, int Channels = 2, Resolution Resolution = Resolution.Short)
        {
            Sample = CreateSample(Length, Frequency, Channels, 1, Resolution.ToBassFlag());
            Handle = SampleGetChannel(Sample, true);
        }

        public AudioSample(string FilePath, Resolution Resolution = Resolution.Short)
        {
            Sample = SampleLoad(FilePath, 0, 0, 1, Resolution.ToBassFlag());
            Handle = SampleGetChannel(Sample, true);
        }

        public AudioSample(byte[] Memory, int Length, Resolution Resolution = Resolution.Short)
        {
            // Pin
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Sample = SampleLoad(GCPin.AddrOfPinnedObject(), 0, Length, 1, Resolution.ToBassFlag());
            Handle = SampleGetChannel(Sample, true);

            GCPin.Free();
        }

        public long Length => SampleGetInfo(Sample).Length;

        #region Read Sample Data
        public bool ReadSampleData(IntPtr Buffer) => SampleGetData(Sample, Buffer);

        public bool ReadSampleData(byte[] Buffer) => SampleGetData(Sample, Buffer);

        public bool ReadSampleData(float[] Buffer) => SampleGetData(Sample, Buffer);

        public bool ReadSampleData(short[] Buffer) => SampleGetData(Sample, Buffer);

        public bool ReadSampleData(int[] Buffer) => SampleGetData(Sample, Buffer);
        #endregion

        #region Write Sample Data
        public bool WriteSampleData(IntPtr Buffer) => SampleSetData(Sample, Buffer);

        public bool WriteSampleData(byte[] Buffer) => SampleSetData(Sample, Buffer);

        public bool WriteSampleData(float[] Buffer) => SampleSetData(Sample, Buffer);

        public bool WriteSampleData(short[] Buffer) => SampleSetData(Sample, Buffer);

        public bool WriteSampleData(int[] Buffer) => SampleSetData(Sample, Buffer);
        #endregion

        public override void Dispose() 
        {
            try 
            {
                if (Handle != 0 && SampleFree(Sample))
                    Sample = 0;
            }
            catch { }
        }
    }
}
