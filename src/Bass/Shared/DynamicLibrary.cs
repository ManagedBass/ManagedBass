using System;
using System.Runtime.InteropServices;
using System.IO;

namespace ManagedBass
{
    /// <summary>
    /// Supports loading libraries dynamically on Desktop.
    /// </summary>
    public static class DynamicLibrary
    {
#if __DESKTOP__
        const string LinuxDllName = "libdl.so",
            MacDllName = "/usr/lib/libSystem.dylib",
            WinDllName = "kernel32.dll";

        const int RtldNow = 2;

        [DllImport(LinuxDllName, EntryPoint = "dlopen")]
        static extern IntPtr LoadLinux(string FileName, int Flags = RtldNow);

        [DllImport(LinuxDllName, EntryPoint = "dlclose")]
        static extern bool UnloadLinux(IntPtr hLib);

        [DllImport(MacDllName, EntryPoint = "dlopen")]
        static extern IntPtr LoadMac(string FileName, int Flags = RtldNow);
        
        [DllImport(MacDllName, EntryPoint = "dlclose")]
        static extern bool UnloadMac(IntPtr hLib);
        
        [DllImport(WinDllName, EntryPoint = "LoadLibrary")]
        static extern IntPtr LoadWin(string DllToLoad);

        [DllImport(WinDllName, EntryPoint = "FreeLibrary")]
        static extern bool UnloadWin(IntPtr hLib);

        static string GetPath(string FileName, string Folder)
        {
            return !string.IsNullOrWhiteSpace(Folder) ? Path.Combine(Folder, FileName) : FileName;
        }
#endif

        /// <summary>
        /// Loads a Library.
        /// </summary>
        /// <returns>The Handle of the loaded library on success, else <see cref="IntPtr.Zero"/>.</returns>
        public static IntPtr Load(string DllName, string Folder)
        {
#if __DESKTOP__
            try { return LoadWin(GetPath($"{DllName}.dll", Folder)); }
            catch
            {
                try { return LoadLinux(GetPath($"lib{DllName}.so", Folder)); }
                catch
                {
                    try { return LoadMac(GetPath($"lib{DllName}.dylib", Folder)); }
                    catch { return IntPtr.Zero; }
                }
            }
#else
            return IntPtr.Zero;
#endif
        }

        /// <summary>
        /// Unloads a library loaded using <see cref="Load"/>.
        /// </summary>
        /// <param name="hLib">The Handle of a library loaded using <see cref="Load"/>.</param>
        /// <returns>true on success, else false.</returns>
        public static bool Unload(IntPtr hLib)
        {
#if __DESKTOP__
            try { return UnloadWin(hLib); }
            catch
            {
                try { return UnloadLinux(hLib); }
                catch
                {
                    try { return UnloadMac(hLib); }
                    catch { return false; }
                }
            }
#else
            return false;
#endif
        }
    }
}