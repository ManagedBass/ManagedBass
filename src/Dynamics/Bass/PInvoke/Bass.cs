using ManagedBass.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps bass.dll.
    /// 
    /// <para>
    /// Supports: .mp3, .ogg, .wav, .mp2, .mp1, .aiff, .m2a, .mpa, .m1a, .mpg, .mpeg, .aif, .mp3pro, .bwf, .mus,
    /// .mod, .mo3, .s3m, .xm, .it, .mtm, .umx, .mdz, .s3z, .itz, .xmz
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// BASS is a multiplatform audio library. 
    /// It's purpose is to provide the most powerful and efficient (yet easy to use),
    /// sample, stream, MOD music, and recording functions.
    /// All in a tiny DLL, under 100KB in size.
    /// </para>
    /// </remarks>
    public static partial class Bass
    {
        /// <summary>
        /// The Bass way to say <see langword="false" /> = 0.
        /// </summary>
        public const int FALSE = 0;

        /// <summary>
        /// The Bass way to say <see langword="true" /> = 1.
        /// </summary>
        public const int TRUE = 1;

        /// <summary>
        /// The Bass way to say Error = -1.
        /// </summary>
        public const int ERROR = -1;

        const string DllName = "bass";

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        [DllImport(DllName, EntryPoint = "BASS_Start")]
        public static extern bool Start();

        [DllImport(DllName, EntryPoint = "BASS_Pause")]
        public static extern bool Pause();

        [DllImport(DllName, EntryPoint = "BASS_Stop")]
        public static extern bool Stop();

        [DllImport(DllName, EntryPoint = "BASS_Update")]
        public static extern bool Update(int Length);

        [DllImport(DllName)]
        static extern float BASS_GetCPU();

        /// <summary>
        /// Retrieves the current CPU usage of BASS as a percentage of total CPU time.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This function includes the time taken to render stream (HSTREAM) and MOD music (HMUSIC) channels during playback, and any DSP functions set on those channels.
        /// Also, any FX that are not using the "with FX flag" DX8 effect implementation.
        /// </para>
        /// <para>
        /// The rendering of some add-on stream formats may not be entirely included, if they use additional decoding threads.
        /// See the add-on documentation for details.
        /// </para>
        /// <para>
        /// This function does not strictly tell the CPU usage, but rather how timely the Buffer updates are.
        /// For example, if it takes 10ms to render 100ms of data, that would be 10%.
        /// If the reported usage gets to 100%, that means the channel data is being played faster than it can be rendered, and Buffer underruns are likely to occur.
        /// </para>
        /// <para>
        /// If automatic updating is disabled, then the value returned by this function is only updated after each call to <see cref="Update" />.
        /// <see cref="ChannelUpdate" /> usage is not included.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// On Windows, the CPU usage does not include sample channels (HCHANNEL), which are mixed by the output device/drivers (hardware mixing) or Windows (software mixing). 
        /// On other platforms, the CPU usage does include sample playback as well as the generation of the final output mix.
        /// </para>
        /// </remarks>
        public static double CPUUsage { get { return BASS_GetCPU(); } }

        [DllImport(DllName)]
        static extern int BASS_GetVersion();

        public static Version Version { get { return Extensions.GetVersion(BASS_GetVersion()); } }

        [DllImport(DllName, EntryPoint = "BASS_GetInfo")]
        public static extern bool GetInfo(out BassInfo Info);

        public static BassInfo Info
        {
            get
            {
                BassInfo temp;
                GetInfo(out temp);
                return temp;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_GetDSoundObject")]
        public static extern IntPtr GetDSoundObject(int obj);

        #region Plugin
        [DllImport(DllName, EntryPoint = "BASS_PluginGetInfo")]
        public static extern PluginInfo GetPluginInfo(int Handle);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_PluginLoad(string FileName, BassFlags Flags = BassFlags.Unicode);

        public static int PluginLoad(string FileName) { return BASS_PluginLoad(FileName); }

        [DllImport(DllName, EntryPoint = "BASS_PluginFree")]
        public static extern bool PluginFree(int Handle);

        public static IEnumerable<int> PluginLoadDirectory(string directory)
        {
            if (Directory.Exists(directory))
                foreach (var lib in Directory.EnumerateFiles(directory, "bass*.dll"))
                {
                    int h = BASS_PluginLoad(lib);
                    if (h != 0) yield return h;
                }
        }
        #endregion

        #region FX Parameters
        [DllImport(DllName, EntryPoint = "BASS_FXSetParameters")]
        public static extern bool FXSetParameters(int Handle, IntPtr param);

        [DllImport(DllName, EntryPoint = "BASS_FXGetParameters")]
        public static extern bool FXGetParameters(int Handle, IntPtr param);

        [DllImport(DllName, EntryPoint = "BASS_FXReset")]
        public static extern bool FXReset(int handle);
        #endregion

        #region Error Code
        [DllImport(DllName)]
        extern static Errors BASS_ErrorGetCode();

        public static Errors LastError { get { return BASS_ErrorGetCode(); } }
        #endregion

        #region Music
        [DllImport(DllName, EntryPoint = "BASS_MusicFree")]
        public extern static bool MusicFree(int Handle);

        #region Music Load
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        extern static int BASS_MusicLoad(bool mem, string file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, IntPtr file, long offset, int Length, BassFlags flags, int freq);

        public static int MusicLoad(string File, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return BASS_MusicLoad(false, File, Offset, Length, Flags | BassFlags.Unicode, Frequency);
        }

        public static int MusicLoad(IntPtr Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency);
        }
        #endregion
        #endregion
    }
}