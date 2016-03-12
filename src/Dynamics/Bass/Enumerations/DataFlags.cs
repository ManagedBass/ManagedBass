using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Flags to be used with <see cref="Bass.ChannelGetData(int,IntPtr,int)" />.
    /// </summary>
    [Flags]
    public enum DataFlags
    {
        /// <summary>
        /// 256 sample FFT (returns 128 floating-point values)
        /// </summary>
        FFT256 = -2147483648,

        /// <summary>
        /// 512 sample FFT (returns 256 floating-point values)
        /// </summary>
        FFT512 = -2147483647,

        /// <summary>
        /// 1024 sample FFT (returns 512 floating-point values)
        /// </summary>
        FFT1024 = -2147483646,

        /// <summary>
        /// 2048 sample FFT (returns 1024 floating-point values)
        /// </summary>
        FFT2048 = -2147483645,

        /// <summary>
        /// 4096 sample FFT (returns 2048 floating-point values)
        /// </summary>
        FFT4096 = -2147483644,

        /// <summary>
        /// 8192 sample FFT (returns 4096 floating-point values)
        /// </summary>
        FFT8192 = -2147483643,

        /// <summary>
        /// 16384 sample FFT (returns 8192 floating-point values)
        /// </summary>
        FFT16384 = -2147483642,

        /// <summary>
        /// Query how much data is buffered
        /// </summary>
        Available = 0,

        /// <summary>
        /// FFT flag: FFT for each channel, else all combined
        /// </summary>
        FFTIndividual = 16,

        /// <summary>
        /// FFT flag: no Hanning window
        /// </summary>
        FFTNoWindow = 32,

        /// <summary>
        /// FFT flag: pre-remove DC bias
        /// </summary>
        FFTRemoveDC = 64,

        /// <summary>
        /// FFT flag: return complex data
        /// </summary>
        FFTComplex = 128,

        /// <summary>
        /// flag: return 8.24 fixed-point data
        /// </summary>
        Fixed = 536870912,

        /// <summary>
        /// flag: return floating-point sample data
        /// </summary>
        Float = 1073741824

        // TODO: Bass 2.4.12: BASS_DATA_FFT32768
    }
}
