using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Windows specific code.
    /// </summary>
    static class WindowsNative
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
    }
}

