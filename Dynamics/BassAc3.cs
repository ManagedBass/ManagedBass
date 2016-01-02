using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static class BassAc3
    {
        const string DllName = "bassac3.dll";

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

        [DllImport(DllName)]
        static extern int BASS_AC3_StreamCreateFile(bool mem, IntPtr memory, long offset, long length, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_AC3_StreamCreateFile(bool mem, [MarshalAs(UnmanagedType.BStr)] string file, long offset, long length, BassFlags flags);

        public static int CreateStream(IntPtr memory, long offset, long length, BassFlags flags)
        {
            return BASS_AC3_StreamCreateFile(true, memory, offset, length, flags);
        }

        public static int CreateStream(string file, long offset, long length, BassFlags flags)
        {
            return BASS_AC3_StreamCreateFile(false, file, offset, length, flags | BassFlags.Unicode);
        }

        [DllImport(DllName, EntryPoint = "BASS_AC3_StreamCreateUser")]
        public static extern int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user);

        // TODO: Unicode
        [DllImport(DllName, EntryPoint = "BASS_AC3_StreamCreateURL")]
        public static extern int CreateStream([MarshalAs(UnmanagedType.LPWStr)]string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr));
        
        /// <summary>
        /// Dynamic range compression option
        /// dynrng (bool): If true dynamic range compression is enbaled (default is false).
        /// </summary>
        public static bool DRC
        {
            get { return Bass.GetConfigBool(Configuration.AC3DynamicRangeCompression); }
            set { Bass.Configure(Configuration.AC3DynamicRangeCompression, value); }
        }
    }
}