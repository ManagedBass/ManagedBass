using System;
using System.Runtime.InteropServices;
using System.IO;

namespace ManagedBass
{
    static class DynamicLibrary
    {
#if WINDOWS
        const string DllName = "kernel32.dll";

        [DllImport(DllName)]
        static extern IntPtr LoadLibrary(string dllToLoad);
        
        [DllImport(DllName, EntryPoint = "FreeLibrary")]
        public static extern bool Unload(IntPtr hLib);

        public static IntPtr Load(string DllName, string Folder)
        {
            return LoadLibrary(!string.IsNullOrWhiteSpace(Folder) ? Path.Combine(Folder, DllName + ".dll") : DllName);
        }
        
#elif LINUX || __ANDROID__
        const string DllName = "libdl.so";
#elif __MAC__
        const string DllName = "/usr/lib/libSystem.dylib";
#endif

#if LINUX || __ANDROID__ || __MAC__
        const int RTLD_NOW = 2;

        [DllImport(DllName)]
        static extern IntPtr dlopen(string fileName, int flags);

        [DllImport(DllName, EntryPoint="dlclose")]
        public static extern bool Unload(IntPtr hLib);

        public static IntPtr Load(string DllName, string Folder)
        {
            return dlopen(!string.IsNullOrWhiteSpace(Folder) ? Path.Combine(Folder, DllName + ".dll") : DllName, RTLD_NOW);
        }
#endif
    }
}