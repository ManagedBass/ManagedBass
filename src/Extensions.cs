using ManagedBass.Dynamics;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Contains Helper and Extension methods.
    /// </summary>
    public static class Extensions
    {
        internal static readonly bool IsWindows = false;

        static Extensions()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32Windows:
                    IsWindows = true;
                    break;
            }
        }

        /// <summary>
        /// Converts <see cref="Resolution"/> to <see cref="BassFlags"/>
        /// </summary>
        public static BassFlags ToBassFlag(this Resolution Resolution)
        {
            switch (Resolution)
            {
                case Resolution.Byte:
                    return BassFlags.Byte;
                case Resolution.Float:
                    return BassFlags.Float;
                default:
                    return BassFlags.Default;
            }
        }

        internal static IntPtr Load(string DllName, string Folder)
        {
            if (IsWindows) return WindowsNative.LoadLibrary(!string.IsNullOrWhiteSpace(Folder) ? Path.Combine(Folder, DllName + ".dll") : DllName);
            else throw new PlatformNotSupportedException("Only available on Windows");            
        }

        static bool? floatable = null;

        /// <summary>
        /// Check whether Floating point streams are supported in the Current Environment.
        /// </summary>
        public static bool SupportsFloatingPoint
        {
            get
            {
                if (floatable.HasValue) return floatable.Value;

                // try creating a floating-point stream
                int hStream = Bass.CreateStream(44100, 1, BassFlags.Float, StreamProcedureType.Dummy);

                floatable = hStream != 0;

                // floating-point channels are supported! (free the test stream)
                if (floatable.Value) Bass.StreamFree(hStream);

                return floatable.Value;
            }
        }

        internal static Version GetVersion(int num)
        {
            return new Version(num >> 24 & 0xff,
                               num >> 16 & 0xff, 
                               num >> 8 & 0xff,
                               num & 0xff);
        }

        #region Converters
        // Converters from https://github.com/aybe/audioObjects/
        // ms = samples * 1000 / samplerate. 
        // bytes = samples * bits * channels / 8. 

        public static long BytesToSamples(long bytes, int bits, int channels) { return ((bytes * 8) / bits) / channels; }

        public static long MilisecondsToSamples(int samplerate, long ms) { return (ms * samplerate) / 1000; }

        public static long SamplesToBytes(long samples, int bits, int channels) { return (samples * bits * channels) / 8; }

        public static long SamplesToMiliseconds(long samples, int samplerate) { return (samples * 1000) / samplerate; }

        public static long SamplesToSamplerate(long samples, long ms) { return (samples * 1000) / ms; }

        public static double SamplesToSeconds(long samples, int samplerate) { return samples / (double)samplerate; }

        public static long SecondsToSamples(double seconds, int samplerate) { return (long)(seconds * samplerate); }

        public static double BytesToSeconds(long bytes, int bits, int channels, int samplerate)
        {
            return SamplesToSeconds(BytesToSamples(bytes, bits, channels), samplerate);
        }

        public static long SecondsToBytes(double seconds, int samplerate, int bits, int channels)
        {
            return SamplesToBytes(SecondsToSamples(seconds, samplerate), bits, channels);
        }
        #endregion

        public static double BpmToSeconds(double bpm) { return 60 / bpm; }

        public static int DbToLevel(double dB, int maxLevel)
        {
            return (int)Math.Round((double)maxLevel * Math.Pow(10, dB / 20));
        }

        public static double DbToLevel(double dB, double maxLevel)
        {
            return maxLevel * Math.Pow(10, dB / 20);
        }

        public static string ChannelCountToString(int Channels)
        {
            switch (Channels)
            {
                case 1:
                    return "Mono";
                case 2:
                    return "Stereo";
                case 3:
                    return "2.1";
                case 4:
                    return "Quad";
                case 5:
                    return "4.1";
                case 6:
                    return "5.1";
                case 7:
                    return "6.1";
                default:
                    if (Channels < 1) throw new ArgumentException("Channels must be greater than Zero.");
                    else return Channels + " Channels";
            }
        }
    }
}
