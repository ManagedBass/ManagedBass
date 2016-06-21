using System;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Sample.
    /// </summary>
    public class Sample
    {
        int _sample;

        public Sample(int Length, int Frequency, int Channels, BassFlags Flags, int MaxPlaybacks = 1)
        {
            _sample = Bass.CreateSample(Length, Frequency, Channels, MaxPlaybacks, Flags);
        }

        public Sample(string FilePath, long Offset = 0, int Length = 0, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            _sample = Bass.SampleLoad(FilePath, Offset, Length, MaxPlaybacks, Resolution.ToBassFlag());
        }

        public Sample(byte[] Memory, long Offset, int Length, int MaxPlaybacks = 1, Resolution Resolution = Resolution.Short)
        {
            _sample = Bass.SampleLoad(Memory, Offset, Length, MaxPlaybacks, Resolution.ToBassFlag());
        }
        
        /// <summary>
        /// Gets the Length of the Sample Data.
        /// </summary>
        public long Length => Bass.SampleGetInfo(_sample).Length;

        /// <summary>
        /// Gets a Channel for this Sample (to use for Playing/DSP etc.).
        /// </summary>
        public Channel CreateChannel(bool OnlyNew = false) => new Channel(Bass.SampleGetChannel(_sample, OnlyNew));

        #region Read Sample Data
        public bool ReadSampleData(IntPtr Buffer) => Bass.SampleGetData(_sample, Buffer);

        public bool ReadSampleData(byte[] Buffer) => Bass.SampleGetData(_sample, Buffer);

        public bool ReadSampleData(float[] Buffer) => Bass.SampleGetData(_sample, Buffer);

        public bool ReadSampleData(short[] Buffer) => Bass.SampleGetData(_sample, Buffer);

        public bool ReadSampleData(int[] Buffer) => Bass.SampleGetData(_sample, Buffer);
        #endregion

        #region Write Sample Data
        public bool WriteSampleData(IntPtr Buffer) => Bass.SampleSetData(_sample, Buffer);

        public bool WriteSampleData(byte[] Buffer) => Bass.SampleSetData(_sample, Buffer);

        public bool WriteSampleData(float[] Buffer) => Bass.SampleSetData(_sample, Buffer);

        public bool WriteSampleData(short[] Buffer) => Bass.SampleSetData(_sample, Buffer);

        public bool WriteSampleData(int[] Buffer) => Bass.SampleSetData(_sample, Buffer);
        #endregion
        
        /// <summary>
        /// Frees all Resources used by this instance.
        /// </summary>
        public void Dispose() 
        {
            try 
            {
                if (Bass.SampleFree(_sample))
                    _sample = 0;
            }
            catch { }
        }
    }
}
