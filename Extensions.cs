using ManagedBass.Dynamics;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public static class Extensions
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string dllToLoad);

        public static BassFlags ToBassFlag(this Resolution BufferKind)
        {
            switch (BufferKind)
            {
                case Resolution.Byte:
                    return BassFlags.Byte;
                case Resolution.Float:
                    return BassFlags.Float;
                default:
                    return BassFlags.Default;
            }
        }

        internal static void Load(string DllName, string Folder)
        {
            if (!string.IsNullOrWhiteSpace(Folder))
                LoadLibrary(Path.Combine(Folder, DllName));
            else LoadLibrary(DllName);
        }

        public static short HiWord(this int pDWord) { return ((short)(((pDWord) >> 16) & 0xFFFF)); }

        public static short LoWord(this int pDWord) { return ((short)pDWord); }

        // TODO: MakeWord();

        static bool? floatable = null;

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
    }
}
