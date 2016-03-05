using System;
using System.Runtime.InteropServices;
using ManagedBass.Dynamics;
using static ManagedBass.Dynamics.Bass;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Sample.
    /// </summary>
    public class AudioSample
    {
        int Sample;

        public AudioSample(int Length, int Frequency = 44100, int Channels = 2, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            Sample = CreateSample(Length, Frequency, Channels, MaxPlaybacks, Resolution.ToBassFlag());
        }

        public AudioSample(string FilePath, long Offset = 0, int Length = 0, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            Sample = SampleLoad(FilePath, Offset, Length, MaxPlaybacks, Resolution.ToBassFlag());
        }

        public AudioSample(byte[] Memory, long Offset, int Length, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            Sample = SampleLoad(Memory, Offset, Length, MaxPlaybacks, Resolution.ToBassFlag());
        }
        
        public long Length => SampleGetInfo(Sample).Length;

        public Channel CreateChannel(bool OnlyNew = false) => new Channel(SampleGetChannel(Sample, OnlyNew));

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

        public void Dispose() 
        {
            try 
            {
                if (SampleFree(Sample))
                    Sample = 0;
            }
            catch { }
        }
    }
}
