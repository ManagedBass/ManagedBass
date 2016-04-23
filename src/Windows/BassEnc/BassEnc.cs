using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    public static partial class BassEnc
    {
        /// <summary>
        /// ACM codec name to give priority for the formats it supports (e.g. 'l3codecp.acm').
        /// </summary>
        public static string ACMLoad
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.EncodeACMLoad)); }
            set
            {
                var ptr = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.EncodeACMLoad, ptr);

                Marshal.FreeHGlobal(ptr);
            }
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_GetACMFormat(int handle, IntPtr form, int formlen, string title, int flags);

        public static int GetACMFormat(int handle,
                                       IntPtr form = default(IntPtr),
                                       int formlen = 0,
                                       string title = null,
                                       ACMFormatFlags flags = ACMFormatFlags.Default,
                                       WaveFormatTag encoding = WaveFormatTag.Unknown)
        {
            var acMflags = BitHelper.MakeLong((short)(flags | ACMFormatFlags.Unicode), (short)encoding);

            return BASS_Encode_GetACMFormat(handle, form, formlen, title, acMflags);
        }

        [DllImport(DllName, EntryPoint = "BASS_Encode_StartACM")]
        public static extern int EncodeStartACM(int handle, IntPtr form, EncodeFlags flags, EncodeProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartACMFile(int handle, IntPtr form, EncodeFlags flags, string filename);

        public static int EncodeStartACM(int handle, IntPtr form, EncodeFlags flags, string filename)
        {
            return BASS_Encode_StartACMFile(handle, form, flags | EncodeFlags.Unicode, filename);
        }
    }
}