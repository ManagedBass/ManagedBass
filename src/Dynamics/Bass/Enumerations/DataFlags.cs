using System;

namespace ManagedBass
{
    /// <summary>
    /// Flags to be used with <see cref="Bass.ChannelGetData(int,IntPtr,int)" />.
    /// </summary>
    [Flags]
    public enum DataFlags : uint
    {
        /// <summary>
        /// 256 sample FFT (returns 128 floating-point values)
        /// </summary>
        FFT256 = 0x80000000,

        /// <summary>
        /// 512 sample FFT (returns 256 floating-point values)
        /// </summary>
        FFT512 = 0x80000001,

        /// <summary>
        /// 1024 sample FFT (returns 512 floating-point values)
        /// </summary>
        FFT1024 = 0x80000002,

        /// <summary>
        /// 2048 sample FFT (returns 1024 floating-point values)
        /// </summary>
        FFT2048 = 0x80000003,

        /// <summary>
        /// 4096 sample FFT (returns 2048 floating-point values)
        /// </summary>
        FFT4096 = 0x80000004,

        /// <summary>
        /// 8192 sample FFT (returns 4096 floating-point values)
        /// </summary>
        FFT8192 = 0x80000005,

        /// <summary>
        /// 16384 sample FFT (returns 8192 floating-point values)
        /// </summary>
        FFT16384 = 0x80000006,

        /// <summary>
        /// 32768 FFT
        /// </summary>
        FFT32768 = 0x80000007,

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
        Fixed = 0x20000000,

        /// <summary>
        /// flag: return floating-point sample data
        /// </summary>
        Float = 0x40000000
    }
}
