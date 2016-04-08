#if WINDOWS
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Windows specific code.
    /// </summary>
    static class WindowsNative
    {
        const string DllName = "kernel32.dll";

        [DllImport(DllName)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(DllName)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport(DllName)]
        public static extern bool FreeLibrary(IntPtr hLib);
    }
}
#endif