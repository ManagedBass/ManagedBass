using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.Marshal;

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
            IsWindows = Environment.OSVersion.Platform.Is(PlatformID.Win32NT, PlatformID.Win32Windows);
        }

        public static T Clip<T>(this T Item, T MinValue, T MaxValue)
            where T : IComparable<T>
        {
            if (Item.CompareTo(MaxValue) > 0)
                return MaxValue;

            if (Item.CompareTo(MinValue) < 0)
                return MinValue;
            
            return Item;
        }

        public static bool Is<T>(this T Item, params T[] Args)
        {
            foreach (var arg in Args)
                if (Item.Equals(arg))
                    return true;

            return false;
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

        internal static bool Unload(IntPtr hLib)
        {
            if (IsWindows) return WindowsNative.FreeLibrary(hLib);
            else throw new PlatformNotSupportedException("Only available on Windows");
        }

        /// <summary>
        /// Returns the <param name="n">n'th (max 15)</param> pair of Speaker Assignment Flags
        /// </summary>
        public static BassFlags SpeakerN(int n) => (BassFlags)(n << 24);

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

        public static long BytesToSamples(long bytes, int bits, int channels) => ((bytes * 8) / bits) / channels;

        public static long MilisecondsToSamples(int samplerate, long ms) => (ms * samplerate) / 1000;

        public static long SamplesToBytes(long samples, int bits, int channels) => (samples * bits * channels) / 8;

        public static long SamplesToMiliseconds(long samples, int samplerate) => (samples * 1000) / samplerate;

        public static long SamplesToSamplerate(long samples, long ms) => (samples * 1000) / ms;

        public static double SamplesToSeconds(long samples, int samplerate) => samples / (double)samplerate;

        public static long SecondsToSamples(double seconds, int samplerate) => (long)(seconds * samplerate);

        public static double BytesToSeconds(long bytes, int bits, int channels, int samplerate)
        {
            return SamplesToSeconds(BytesToSamples(bytes, bits, channels), samplerate);
        }

        public static long SecondsToBytes(double seconds, int samplerate, int bits, int channels)
        {
            return SamplesToBytes(SecondsToSamples(seconds, samplerate), bits, channels);
        }
        #endregion

        public static double BpmToSeconds(double bpm) => 60 / bpm;

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

        public static string[] ExtractMultiStringAnsi(IntPtr ptr)
        {
            var l = new List<string>();

            while (true)
            {
                string str = Marshal.PtrToStringAnsi(ptr);

                if (str.Length == 0)
                    break;
                
                l.Add(str);

                // char '\0'
                ptr += str.Length + 1;
            }

            return l.ToArray();
        }

        public static string[] ExtractMultiStringUtf8(IntPtr ptr)
        {
            var l = new List<string>();

            while (true)
            {
                string str = PtrToStringUtf8(ptr);

                if (str == null)
                    break;
 
                l.Add(str);
            }

            return l.ToArray();
        }

        public static unsafe string PtrToStringUtf8(IntPtr ptr)
        {
            byte* bytes = (byte*)ptr.ToPointer();
            int size = 0;
            while (bytes[size] != 0) ++size;

            if (size == 0) return null;

            byte[] buffer = new byte[size];
            Marshal.Copy((IntPtr)ptr, buffer, 0, size);

            ptr += size;

            return Encoding.UTF8.GetString(buffer);
        }

        #region Tags
        public static string TagGetLyrics3v2(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.Lyrics3v2));

        public static string TagGetVendor(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.OggEncoder));

        public static string TagGetRiffDisp(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffDISP));

        public static string TagGetIcyShoutcastMeta(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.META));

        public static string TagGetOggEncoder(int Handle) => PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.OggEncoder));

        public static string TagGetWMAMidStreamTag(int Handle) => PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.WmaMeta));

        public static string[] TagGetOggComments(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.OGG));

        public static string[] TagGetHTTPHeaders(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.HTTP));

        public static string[] TagGetICYHeaders(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.ICY));

        public static string[] TagGetAPE(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.APE));

        public static string[] TagGetMP4Metadata(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MP4));

        public static string[] TagGetWMA(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.WMA));

        public static string[] TagGetMF(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MF));

        public static string[] TagGetRiff(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffInfo));
        #endregion
    }
}
