using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Runtime.InteropServices.Marshal;

namespace ManagedBass
{
    /// <summary>
    /// Contains Helper and Extension methods.
    /// </summary>
    public static class Extensions
    {
        internal static readonly bool IsWindows;

        internal static ReferenceHolder ChannelReferences = new ReferenceHolder();

        static Extensions()
        {
            IsWindows = Environment.OSVersion.Platform.Is(PlatformID.Win32NT, PlatformID.Win32Windows);
        }

        /// <summary>
        /// Clips a value between a Minimum and a Maximum.
        /// </summary>
        public static T Clip<T>(this T Item, T MinValue, T MaxValue)
            where T : IComparable<T>
        {
            if (Item.CompareTo(MaxValue) > 0)
                return MaxValue;

            return Item.CompareTo(MinValue) < 0 ? MinValue : Item;
        }

        /// <summary>
        /// Checks for equality of an item with any element of an array of items.
        /// </summary>
        public static bool Is<T>(this T Item, params T[] Args) => Args.Contains(Item);

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
        
        /// <summary>
        /// Returns the <param name="N">n'th (max 15)</param> pair of Speaker Assignment Flags
        /// </summary>
        public static BassFlags SpeakerN(int N) => (BassFlags)(N << 24);

        static bool? _floatable;

        /// <summary>
        /// Check whether Floating point streams are supported in the Current Environment.
        /// </summary>
        public static bool SupportsFloatingPoint
        {
            get
            {
                if (_floatable.HasValue) 
                    return _floatable.Value;

                // try creating a floating-point stream
                var hStream = Bass.CreateStream(44100, 1, BassFlags.Float, StreamProcedureType.Dummy);

                _floatable = hStream != 0;

                // floating-point channels are supported! (free the test stream)
                if (_floatable.Value) 
                    Bass.StreamFree(hStream);

                return _floatable.Value;
            }
        }

        internal static Version GetVersion(int Num)
        {
            return new Version(Num >> 24 & 0xff,
                               Num >> 16 & 0xff,
                               Num >> 8 & 0xff,
                               Num & 0xff);
        }

        #region Converters
        // Converters from https://github.com/aybe/audioObjects/
        // ms = samples * 1000 / samplerate. 
        // bytes = samples * bits * channels / 8. 

        public static long BytesToSamples(long Bytes, int bits, int channels) => Bytes * 8 / bits / channels;

        public static long MilisecondsToSamples(int samplerate, long ms) => ms * samplerate / 1000;

        public static long SamplesToBytes(long samples, int bits, int channels) => samples * bits * channels / 8;

        public static long SamplesToMiliseconds(long samples, int samplerate) => samples * 1000 / samplerate;

        public static long SamplesToSamplerate(long samples, long ms) => samples * 1000 / ms;

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
            return (int)Math.Round(maxLevel * Math.Pow(10, dB / 20));
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
                    if (Channels < 1)
                        throw new ArgumentException("Channels must be greater than Zero.");
                    return Channels + " Channels";
            }
        }

        public static string[] ExtractMultiStringAnsi(IntPtr ptr)
        {
            var l = new List<string>();

            while (true)
            {
                var str = PtrToStringAnsi(ptr);

                if (string.IsNullOrEmpty(str))
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
                int size;
                var str = PtrToStringUtf8(ptr, out size);

                if (string.IsNullOrEmpty(str))
                    break;
 
                l.Add(str);

                ptr += size + 1;
            }

            return l.ToArray();
        }

        public static unsafe string PtrToStringUtf8(IntPtr ptr, out int size)
        {
            size = 0;

            if (ptr == IntPtr.Zero)
                return null;

            var bytes = (byte*)ptr.ToPointer();
            
            while (bytes[size] != 0)
                ++size;

            if (size == 0)
                return null;

            var buffer = new byte[size];
            Copy(ptr, buffer, 0, size);
            
            return Encoding.UTF8.GetString(buffer);
        }

        public static string PtrToStringUtf8(IntPtr ptr)
        {
            int size;
            return PtrToStringUtf8(ptr, out size);
        }

        #region Tags
        public static string TagGetLyrics3v2(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.Lyrics3v2));

        public static string TagGetRiffDisp(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.RiffDISP));

        public static string TagGetIcyShoutcastMeta(int Handle) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.META));

        public static string TagGetWMAMidStreamTag(int Handle) => PtrToStringUtf8(Bass.ChannelGetTags(Handle, TagType.WmaMeta));

        public static string[] TagGetHTTPHeaders(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.HTTP));

        public static string[] TagGetICYHeaders(int Handle) => ExtractMultiStringAnsi(Bass.ChannelGetTags(Handle, TagType.ICY));

        public static string[] TagGetMF(int Handle) => ExtractMultiStringUtf8(Bass.ChannelGetTags(Handle, TagType.MF));
        #endregion
    }
}
