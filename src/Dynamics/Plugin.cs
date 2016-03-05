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
    /// Currently Wraps: BassAAC, BassAC3, BassADX, BassAIX, BassALAC, BassAPE, BassFLAC, BassHLS, BassMPC, BassOFR, BassOPUS, BassSPX, BassTTA, BassWV
    /// </remarks>
    public class Plugin
    {
        #region Fields
        public string DllName { get; }
        readonly string ID;
        readonly bool SupportsURL;
        IntPtr HLib = IntPtr.Zero;

        DCreateStreamFile MStreamCreateFile;
        DCreateStreamFileMemory MStreamCreateFileMemory;
        DCreateStreamURL MStreamCreateURL;
        DCreateStreamUser MStreamCreateUser;
        #endregion

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        delegate int DCreateStreamFile(bool mem, string file, long offset, long length, BassFlags flags);

        delegate int DCreateStreamFileMemory(bool mem, IntPtr file, long offset, long length, BassFlags flags);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        delegate int DCreateStreamUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        delegate int DCreateStreamURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User);
        #endregion

        /// <summary>
        /// BassAc3 AddOn: Dynamic Range Compression (default is false).
        /// </summary>
        public static bool AC3_DRC
        {
            get { return Bass.GetConfigBool(Configuration.AC3DynamicRangeCompression); }
            set { Bass.Configure(Configuration.AC3DynamicRangeCompression, value); }
        }

        Plugin(string DllName, string ID, bool SupportsURL = true)
        {
            this.DllName = DllName;
            this.ID = ID;
            this.SupportsURL = SupportsURL;
        }

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public void Load(string Folder = null)
        {
            if (Extensions.IsWindows)
                HLib = Extensions.Load(DllName, Folder);
            else LoadAsPlugin(Folder);
        }

        /// <summary>
        /// Load the Library into Bass Plugin System.
        /// </summary>
        /// <param name="Folder">The Folder to load from. null (default) = Load from Current folder.</param>
        public void LoadAsPlugin(string Folder = null)
        {
            // Try for Windows, Linux/Android and OSX Libraries respectively.
            foreach (var lib in new string[] { DllName + ".dll", "lib" + DllName + ".so", "lib" + DllName + ".dylib" })
            {
                string path = Folder != null ? Path.Combine(Folder, lib) : lib;

                if (Bass.PluginLoad(path) != 0)
                {
                    if (HLib != IntPtr.Zero)
                        HLib = (IntPtr)(-1);

                    break;
                }
            }
        }

        #region Private Methods
        void EnsureLoaded()
        {
            if (HLib == IntPtr.Zero)
            {
                Load();
                if (HLib == IntPtr.Zero) throw new DllNotFoundException(DllName);
            }

            // Always Support MP4 files in BassAAC.CreateStream()
            if (ID == "AAC") Bass.Configure(Configuration.AacSupportMp4, true);
        }

        void EnsureFunction<T>(string MethodName, ref T Method)
        {
            if (Method == null)
            {
                EnsureLoaded();

                if (Extensions.IsWindows)
                {
                    IntPtr pAddress = WindowsNative.GetProcAddress(HLib, MethodName);

                    if (pAddress == IntPtr.Zero) throw new EntryPointNotFoundException(MethodName + " was not found in " + DllName);

                    Method = (T)(object)Marshal.GetDelegateForFunctionPointer(pAddress, typeof(T));
                }
            }
        }
        #endregion

        #region Create Stream
        public int CreateStream(string FileName, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            EnsureFunction("BASS_" + ID + "_StreamCreateFile", ref MStreamCreateFile);

            if (Extensions.IsWindows)
                return MStreamCreateFile(false, FileName, Offset, Length, Flags | BassFlags.Unicode);
            else return Bass.CreateStream(FileName, Offset, Length, Flags);
        }

        public int CreateStream(IntPtr Memory, int Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            EnsureFunction("BASS_" + ID + "_StreamCreateFile", ref MStreamCreateFileMemory);

            if (Extensions.IsWindows)
                return MStreamCreateFileMemory(true, Memory + Offset, 0, Length, Flags);
            else return Bass.CreateStream(Memory, Offset, Length, Flags);
        }

        public int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default(IntPtr))
        {
            EnsureFunction("BASS_" + ID + "_StreamCreateFileUser", ref MStreamCreateUser);

            if (Extensions.IsWindows)
                return MStreamCreateUser(system, flags, procs, user);
            else return Bass.CreateStream(system, flags, procs, user);
        }

        /// <exception cref="System.InvalidOperationException">
        /// The AddOn does not support streaming over the internet.
        /// </exception>
        public int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            if (!SupportsURL) throw new InvalidOperationException(DllName + " does not support streaming over internet");

            EnsureFunction("BASS_" + ID + "_StreamCreateURL", ref MStreamCreateURL);

            if (Extensions.IsWindows)
                return MStreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User);
            else return Bass.CreateStream(Url, Offset, Flags, Procedure, User);
        }

        public int CreateStream(byte[] Memory, int Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }
        #endregion

        #region Instances
        /// <summary>
        /// Wraps BassAAC: bass_aac.dll.
        /// </summary>
        /// <remarks>
        /// MP4 and AAC both are always supported.
        /// <para>Supports .aac, .adts, .mp4, .m4a, .m4b</para>
        /// </remarks>
        public static readonly Plugin BassAAC = new Plugin("bass_aac", "AAC");

        /// <summary>
        /// Wraps BassAC3: bassac3.dll
        /// </summary>
        /// <remarks>
        /// Supports: .ac3
        /// </remarks>
        public static readonly Plugin BassAC3 = new Plugin("bass_ac3", "AC3");

        /// <summary>
        /// Wraps BassADX: bassadx.dll
        /// </summary>
        /// <remarks>
        /// Supports: .adx
        /// </remarks>
        public static readonly Plugin BassADX = new Plugin("bass_adx", "ADX");

        /// <summary>
        /// Wraps BassAIX: bass_aix.dll
        /// </summary>
        public static readonly Plugin BassAIX = new Plugin("bass_aix", "AIX", false);

        /// <summary>
        /// Wraps BassALAC: bassalac.dll
        /// </summary>
        /// <remarks>
        /// Supports: .m4a, .aac, .mp4, .mov
        /// </remarks>
        public static readonly Plugin BassALAC = new Plugin("bassalac", "ALAC");

        /// <summary>
        /// Wraps BassAPE: bass_ape.dll
        /// </summary>
        /// <remarks>
        /// Supports: .ape, .ap1
        /// </remarks>
        public static readonly Plugin BassAPE = new Plugin("bass_ape", "APE", false);

        /// <summary>
        /// Wraps BassFLAC: bassflac.dll
        /// </summary>
        /// <remarks>
        /// Supports: .flac
        /// </remarks>
        public static readonly Plugin BassFLAC = new Plugin("bassflac", "FLAC");

        /// <summary>
        /// Wraps BassHLS: basshls.dll
        /// </summary>
        public static readonly Plugin BassHLS = new Plugin("basshls", "HLS");

        /// <summary>
        /// Wraps BassMPC: bass_mpc.dll
        /// </summary>
        /// <remarks>
        /// Supports: .mpc, .mpp, .mp+
        /// </remarks>
        public static readonly Plugin BassMPC = new Plugin("bass_mpc", "MPC");

        /// <summary>
        /// Wraps BassOFR: bass_ofr.dll
        /// </summary>
        /// <remarks>
        /// Supports: .ofr, .ofs
        /// </remarks>
        public static readonly Plugin BassOFR = new Plugin("bass_ofr", "OFR", false);

        /// <summary>
        /// Wraps BassOPUS: bassopus.dll
        /// </summary>
        /// <remarks>
        /// Supports: .opus
        /// </remarks>
        public static readonly Plugin BassOPUS = new Plugin("bassopus", "OPUS");

        /// <summary>
        /// Wraps BassSPX: bass_spx.dll
        /// </summary>
        /// <remarks>
        /// Supports: .spx
        /// </remarks>
        public static readonly Plugin BassSPX = new Plugin("bass_spx", "SPX");

        /// <summary>
        /// Wraps BassTTA: bass_tta.dll
        /// </summary>
        /// <remarks>
        /// Supports: .tta
        /// </remarks>
        public static readonly Plugin BassTTA = new Plugin("bass_tta", "TTA", false);

        /// <summary>
        /// Wraps BassWV: basswv.dll
        /// </summary>
        /// <remarks>
        /// Supports: .wv
        /// </remarks>
        public static readonly Plugin BassWV = new Plugin("basswv", "WV");
        #endregion
    }
}
