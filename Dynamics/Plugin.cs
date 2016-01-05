using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps AddOns that are no more than Plugins.
    /// </summary>
    /// 
    /// <remarks>
    /// Currently Wraps: BassAAC, BassAC3, BassADX, BassAIX, BassALAC, BassAPE, BassFLAC, BassMPC, BassOFR, BassOPUS, BassSPX, BassTTA, BassWV
    /// </remarks>
    public class Plugin
    {
        #region Fields
        readonly string DllName, ID;
        readonly bool SupportsURL;
        IntPtr HLib = IntPtr.Zero;

        DCreateStreamFile MStreamCreateFile;
        DCreateStreamURL MStreamCreateURL;
        DCreateStreamUser MStreamCreateUser;
        #endregion

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        #region Delegates
        delegate int DCreateStreamFile(bool mem, IntPtr file, long offset, long length, BassFlags flags);
        delegate int DCreateStreamUser(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user);
        delegate int DCreateStreamURL(IntPtr Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User);
        #endregion

        /// <summary>
        /// BassAc3 AddOn:
        /// Dynamic range compression option
        /// dynrng (bool): If true dynamic range compression is enbaled (default is false).
        /// </summary>
        public static bool AC3_DRC
        {
            get { return Bass.GetConfigBool(Configuration.AC3DynamicRangeCompression); }
            set { Bass.Configure(Configuration.AC3DynamicRangeCompression, value); }
        }

        #region Construction
        static Plugin()
        {
            // Always play audio from Mp4 using BassAAC
            Bass.Configure(Configuration.PlayAudioFromMp4, true);

            // Always Support MP4 files in BassAAC.CreateStream()
            Bass.Configure(Configuration.AacSupportMp4, true);
        }

        Plugin(string DllName, string ID, bool SupportsURL = true)
        {
            this.DllName = DllName;
            this.ID = ID;
            this.SupportsURL = SupportsURL;
        }
        #endregion

        public void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        public void LoadAsPlugin(string Folder = null)
        {
            Bass.LoadPlugin(Folder != null ? Path.Combine(Folder, DllName) : DllName);
        }

        #region Private Methods
        void EnsureLoaded()
        {
            if (HLib == IntPtr.Zero)
            {
                HLib = Extensions.LoadLibrary(DllName);
                if (HLib == IntPtr.Zero) throw new DllNotFoundException(DllName);
            }
        }

        void EnsureFunction<T>(string MethodName, ref T Method)
        {
            if (Method == null)
            {
                EnsureLoaded();

                IntPtr pAddress = GetProcAddress(HLib, MethodName);

                if (pAddress == IntPtr.Zero) throw new EntryPointNotFoundException(MethodName + " was not found in " + DllName);

                Method = (T)(object)Marshal.GetDelegateForFunctionPointer(pAddress, typeof(T));
            }
        }

        void LoadStreamCreateFile()
        {
            EnsureFunction("BASS_" + ID + "_StreamCreateFile",
                           ref MStreamCreateFile);
        }
        #endregion

        #region Create Stream
        public int CreateStream(string FileName, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            LoadStreamCreateFile();

            IntPtr ptr = Marshal.StringToHGlobalUni(FileName);

            int Result = MStreamCreateFile(false, ptr, Offset, Length, Flags | BassFlags.Unicode);

            Marshal.FreeHGlobal(ptr);

            return Result;
        }

        public int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            LoadStreamCreateFile();
            return MStreamCreateFile(true, Memory, Offset, Length, Flags);
        }

        public int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default(IntPtr))
        {
            EnsureFunction("BASS_" + ID + "_StreamCreateFileUser",
                           ref MStreamCreateUser);

            return MStreamCreateUser(system, flags, procs, user);
        }

        /// <exception cref="System.InvalidOperationException">
        /// The AddOn does not support streaming over the internet.
        /// </exception>
        public int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            if (!SupportsURL) throw new InvalidOperationException(DllName + " does not support streaming over internet");

            EnsureFunction("BASS_" + ID + "_StreamCreateURL",
                           ref MStreamCreateURL);

            IntPtr ptr = Marshal.StringToHGlobalUni(Url);

            int Result = MStreamCreateURL(ptr, Offset, Flags | BassFlags.Unicode, Procedure, User);

            Marshal.FreeHGlobal(ptr);

            return Result;
        }

        #region From Array
        int CreateStream(object Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Freed, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }

        public int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStream(Memory as object, Offset, Length, Flags);
        }

        public int CreateStream(short[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStream(Memory as object, Offset, Length, Flags);
        }

        public int CreateStream(int[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStream(Memory as object, Offset, Length, Flags);
        }

        public int CreateStream(float[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStream(Memory as object, Offset, Length, Flags);
        }
        #endregion

        public int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags);
        }
        #endregion

        #region Instances
        /// <summary>
        /// Wraps BassAAC: bass_aac.dll.
        /// MP4 and AAC both are always supported. 
        /// </summary>
        public static readonly Plugin BassAAC = new Plugin("bass_aac.dll", "AAC");

        /// <summary>
        /// Wraps BassAC3: bassac3.dll
        /// </summary>
        public static readonly Plugin BassAC3 = new Plugin("bass_ac3.dll", "AC3");

        /// <summary>
        /// Wraps BassADX: bassadx.dll
        /// </summary>
        public static readonly Plugin BassADX = new Plugin("bass_adx.dll", "ADX");

        /// <summary>
        /// Wraps BassAIX: bass_aix.dll
        /// </summary>
        public static readonly Plugin BassAIX = new Plugin("bass_aix.dll", "AIX", false);

        /// <summary>
        /// Wraps BassALAC: bass_alac.dll
        /// </summary>
        public static readonly Plugin BassALAC = new Plugin("bass_alac.dll", "ALAC");

        /// <summary>
        /// Wraps BassAPE: bass_ape.dll
        /// </summary>
        public static readonly Plugin BassAPE = new Plugin("bass_ape.dll", "APE", false);

        /// <summary>
        /// Wraps BassFLAC: bassflac.dll
        /// </summary>
        public static readonly Plugin BassFLAC = new Plugin("bassflac.dll", "FLAC");

        /// <summary>
        /// Wraps BassMPC: bass_mpc.dll
        /// </summary>
        public static readonly Plugin BassMPC = new Plugin("bass_mpc.dll", "MPC");

        /// <summary>
        /// Wraps BassOFR: bass_ofr.dll
        /// </summary>
        public static readonly Plugin BassOFR = new Plugin("bass_ofr.dll", "OFR", false);

        /// <summary>
        /// Wraps BassOPUS: bassopus.dll
        /// </summary>
        public static readonly Plugin BassOPUS = new Plugin("bassopus.dll", "OPUS");

        /// <summary>
        /// Wraps BassSPX: bass_spx.dll
        /// </summary>
        public static readonly Plugin BassSPX = new Plugin("bass_spx.dll", "SPX");

        /// <summary>
        /// Wraps BassTTA: bass_tta.dll
        /// </summary>
        public static readonly Plugin BassTTA = new Plugin("bass_tta.dll", "TTA", false);

        /// <summary>
        /// Wraps BassWV: basswv.dll
        /// </summary>
        public static readonly Plugin BassWV = new Plugin("basswv.dll", "WV");
        #endregion
    }
}
