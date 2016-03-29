using System;
using static ManagedBass.Bass;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Sample.
    /// </summary>
    public class AudioSample
    {
        int _sample;

        public AudioSample(int Length, int Frequency = 44100, int Channels = 2, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            _sample = CreateSample(Length, Frequency, Channels, MaxPlaybacks, Resolution.ToBassFlag());
        }

        public AudioSample(string FilePath, long Offset = 0, int Length = 0, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            _sample = SampleLoad(FilePath, Offset, Length, MaxPlaybacks, Resolution.ToBassFlag());
        }

        public AudioSample(byte[] Memory, long Offset, int Length, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            _sample = SampleLoad(Memory, Offset, Length, MaxPlaybacks, Resolution.ToBassFlag());
        }
        
        public long Length => SampleGetInfo(_sample).Length;

        public Channel CreateChannel(bool OnlyNew = false) => new Channel(SampleGetChannel(_sample, OnlyNew));

        #region Read Sample Data
        public bool ReadSampleData(IntPtr Buffer) => SampleGetData(_sample, Buffer);

        public bool ReadSampleData(byte[] Buffer) => SampleGetData(_sample, Buffer);

        public bool ReadSampleData(float[] Buffer) => SampleGetData(_sample, Buffer);

        public bool ReadSampleData(short[] Buffer) => SampleGetData(_sample, Buffer);

        public bool ReadSampleData(int[] Buffer) => SampleGetData(_sample, Buffer);
        #endregion

        #region Write Sample Data
        public bool WriteSampleData(IntPtr Buffer) => SampleSetData(_sample, Buffer);

        public bool WriteSampleData(byte[] Buffer) => SampleSetData(_sample, Buffer);

        public bool WriteSampleData(float[] Buffer) => SampleSetData(_sample, Buffer);

        public bool WriteSampleData(short[] Buffer) => SampleSetData(_sample, Buffer);

        public bool WriteSampleData(int[] Buffer) => SampleSetData(_sample, Buffer);
        #endregion

        public void Dispose() 
        {
            try 
            {
                if (SampleFree(_sample))
                    _sample = 0;
            }
            catch { }
        }
    }
}
