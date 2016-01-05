using ManagedBass.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        public const int NoSoundDevice = 0, DefaultDevice = -1;

        const string DllName = "bass.dll";

        public static void Load(string folder = null)
        {
            Extensions.Load(DllName, folder);
        }

        // TODO: BASS_ChannelGetAttributeEx
        // TODO: BASS_ChannelSetAttributeEx
        // TODO: BASS_ChannelGetLevelEx

        [DllImport(DllName, EntryPoint = "BASS_Start")]
        public static extern bool Start();

        [DllImport(DllName, EntryPoint = "BASS_Pause")]
        public static extern bool Pause();

        [DllImport(DllName, EntryPoint = "BASS_Stop")]
        public static extern bool Stop();

        [DllImport(DllName, EntryPoint = "BASS_Update")]
        public static extern bool Update(int Length);

        [DllImport(DllName)]
        static extern int BASS_GetVersion();

        public static int Version { get { return BASS_GetVersion(); } }

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

        [DllImport(DllName)]
        static extern int BASS_PluginLoad([MarshalAs(UnmanagedType.LPWStr)]string FileName, BassFlags Flags = BassFlags.Unicode);

        public static int LoadPlugin(string FileName) { return BASS_PluginLoad(FileName); }

        [DllImport(DllName, EntryPoint = "BASS_PluginFree")]
        public static extern bool FreePlugin(int Handle);

        public static IEnumerable<int> LoadPluginsFromDirectory(string directory)
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
        [DllImport(DllName)]
        static extern bool BASS_FXSetParameters(int Handle, [MarshalAs(UnmanagedType.Struct)] IEffectParameter param);

        public static bool SetFXParameters(int Handle, IEffectParameter Parameters)
        {
            try { return Bass.BASS_FXSetParameters(Handle, Parameters); }
            catch (MarshalDirectiveException) { return true; }
        }

        [DllImport(DllName)]
        static extern bool BASS_FXGetParameters(int Handle, [MarshalAs(UnmanagedType.Struct)] IEffectParameter param);

        public static bool GetFXParameters(int Handle, IEffectParameter Param)
        {
            try { return BASS_FXGetParameters(Handle, Param); }
            catch { return true; }
        }

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
        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, string file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, IntPtr file, long offset, int Length, BassFlags flags, int freq);

        public static int LoadMusic(string File, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return BASS_MusicLoad(false, File, Offset, Length, Flags, Frequency);
        }

        public static int LoadMusic(IntPtr Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency);
        }
        #endregion
        #endregion
    }
}