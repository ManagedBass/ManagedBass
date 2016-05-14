using System;
using System.Runtime.InteropServices;
using System.IO;

namespace ManagedBass
{
    static class DynamicLibrary
    {
        const string DllName = "/usr/lib/libSystem.dylib";
        
        const int RtldNow = 2;

        [DllImport(DllName)]
        static extern IntPtr dlopen(string FileName, int Flags = RtldNow);

        [DllImport(DllName, EntryPoint = "dlclose")]
        public static extern bool Unload(IntPtr hLib);

        public static IntPtr Load(string DllName, string Folder)
        {
            var extName = $"lib{DllName}.dylib";

            return dlopen(!string.IsNullOrWhiteSpace(Folder) ? Path.Combine(Folder, extName) : extName);
        }
    }
}