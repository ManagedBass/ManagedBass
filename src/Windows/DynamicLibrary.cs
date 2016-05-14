using System;
using System.Runtime.InteropServices;
using System.IO;

namespace ManagedBass
{
    static class DynamicLibrary
    {
        const string DllName = "kernel32.dll";

        [DllImport(DllName)]
        static extern IntPtr LoadLibrary(string DllToLoad);
        
        [DllImport(DllName, EntryPoint = "FreeLibrary")]
        public static extern bool Unload(IntPtr hLib);

        public static IntPtr Load(string DllName, string Folder)
        {
            var extName = $"{DllName}.dll";

            return LoadLibrary(!string.IsNullOrWhiteSpace(Folder) ? Path.Combine(Folder, extName) : extName);
        }
    }
}