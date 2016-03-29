using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Winamp
{
    /// <summary>
    /// BassWinamp: Wraps bass_winamp.dll.
    /// </summary>
    /// <remarks>
    /// Adds Winamp Input plugins support.
    /// BassWinamp doesn't support Bass plugin system.
    /// </remarks>
    public static class BassWinamp
    {
        const string DllName = "bass_winamp";
        static IntPtr hLib;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        /// <summary>
        /// Winamp input timeout (in milliseconds) to wait until timing out, because the plugin is not using the output system.
        /// </summary>
        public static int InputTimeout
        {
            get { return Bass.GetConfig(Configuration.WinampInputTimeout); }
            set { Bass.Configure(Configuration.WinampInputTimeout, value); }
        }

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_AboutPlugin")]
        public static extern void AboutPlugin(int handle, IntPtr win);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_ConfigPlugin")]
        public static extern void ConfigPlugin(int handle, IntPtr win);

        [DllImport(DllName)]
        static extern IntPtr BASS_WINAMP_FindPlugins(string directory, WinampFindPluginFlags flags);

        public static string[] FindPlugins(string plugindirectory, WinampFindPluginFlags flags)
        {
            return Extensions.ExtractMultiStringAnsi(BASS_WINAMP_FindPlugins(plugindirectory,
                                                                             flags & (WinampFindPluginFlags.Input | WinampFindPluginFlags.Recursive)));
        }

        [DllImport(DllName)]
        static extern IntPtr BASS_WINAMP_GetExtentions(int handle);

        public static string[] GetExtentions(int handle) => Extensions.ExtractMultiStringAnsi(BASS_WINAMP_GetExtentions(handle));

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_GetIsSeekable")]
        public static extern bool IsSeekable(int handle);

        [DllImport(DllName)]
        static extern bool BASS_WINAMP_GetFileInfoInt(string file, IntPtr title, out int lenms);

        public static bool GetFileInfo(string file, out string title, out int lenms)
        {
            var titleptr = Marshal.AllocHGlobal(255);

            var Result = BASS_WINAMP_GetFileInfoInt(file, titleptr, out lenms);

            title = Marshal.PtrToStringAnsi(titleptr);

            Marshal.FreeHGlobal(titleptr);

            return Result;
        }

        [DllImport(DllName)]
        static extern IntPtr BASS_WINAMP_GetName(int handle);

        public static string GetName(int handle)
        {
            return Marshal.PtrToStringAnsi(BASS_WINAMP_GetName(handle));
        }

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_GetUsesOutput")]
        public static extern bool GetUsesOutput(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_GetVersion")]
        public static extern int GetVersion(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_InfoDlg")]
        public static extern bool InfoDialog(string file, IntPtr win);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_LoadPlugin")]
        public static extern int LoadPlugin(string file);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_UnloadPlugin")]
        public static extern void UnloadPlugin(int handle);

        /// <remarks>BassWinamp supports only ASCII filenames</remarks>
        [DllImport(DllName, EntryPoint = "BASS_WINAMP_StreamCreate")]
        public static extern int CreateStream(string filename, BassFlags flags);
    }
}
