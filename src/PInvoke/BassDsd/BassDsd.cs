using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dsd
{
    /// <summary>
    /// Wraps BassDsd: bassdsd.dll
    /// </summary> 
    /// <remarks>
    /// <para>Supports: *.dsf, *.dff, *.dsd</para>
    /// </remarks>
    public static class BassDsd
    {
#if __IOS__
        const string DllName = "__internal";
#else
        const string DllName = "bassdsd";
#endif

#if __ANDROID__ || WINDOWS || LINUX || __MAC__
        static IntPtr hLib;
        
        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = DynamicLibrary.Load(DllName, Folder);

        public static void Unload() => DynamicLibrary.Unload(hLib);
#endif

        public static readonly Plugin Plugin = new Plugin(DllName);

        /// <summary>
        /// The default sample rate when converting to PCM.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This setting determines what sample rate is used by default when converting to PCM.
        /// The rate actually used may be different if the specified rate is not valid for a particular DSD rate, in which case it will be rounded up (or down if there are none higher) to the nearest valid rate;
        /// the valid rates are 1/8, 1/16, 1/32, etc. of the DSD rate down to a minimum of 44100 Hz.
        /// </para>
        /// <para>
        /// The default setting is 88200 Hz.
        /// Changes only affect subsequently created streams, not any that already exist.
        /// </para>
        /// </remarks>
        public static int DefaultFrequency
        {
            get { return Bass.GetConfig(Configuration.DSDFrequency); }
            set { Bass.Configure(Configuration.DSDFrequency, value); }
        }

        /// <summary>
        /// The default gain applied when converting to PCM.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This setting determines what gain is applied by default when converting to PCM.
        /// Changes only affect subsequently created streams, not any that already exist.
        /// An existing stream's gain can be changed via the <see cref="ChannelAttribute.DSDGain" /> attribute.
        /// </para>
        /// <para>The default setting is 6dB.</para>
        /// </remarks>
        public static int DefaultGain
        {
            get { return Bass.GetConfig(Configuration.DSDGain); }
            set { Bass.Configure(Configuration.DSDGain, value); }
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_DSD_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags, int freq);

        [DllImport(DllName)]
        static extern int BASS_DSD_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags, int freq);

        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            return BASS_DSD_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode, Frequency);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            return BASS_DSD_StreamCreateFile(true, new IntPtr(Memory.ToInt64() + Offset), 0, Length, Flags, Frequency);
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 0)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            var Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags, Frequency);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }
        
        [DllImport(DllName)]
        static extern int BASS_DSD_StreamCreateFileUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user, int freq);

        public static int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default(IntPtr), int freq = 0)
        {
            var h = BASS_DSD_StreamCreateFileUser(system, flags, procs, user, freq);

            if (h != 0)
                Extensions.ChannelReferences.Add(h, 0, procs);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_DSD_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User, int Frequency);

        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 0)
        {
            var h = BASS_DSD_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User, Frequency);

            if (h != 0)
                Extensions.ChannelReferences.Add(h, 0, Procedure);

            return h;
        }
    }
}