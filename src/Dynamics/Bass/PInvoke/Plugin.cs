using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static ManagedBass.Extensions;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_PluginGetInfo")]
        public static extern PluginInfo GetPluginInfo(int Handle);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_PluginLoad(string FileName, BassFlags Flags = BassFlags.Unicode);

        public static int PluginLoad(string FileName) => BASS_PluginLoad(FileName);

        [DllImport(DllName, EntryPoint = "BASS_PluginFree")]
        public static extern bool PluginFree(int Handle);

        public static IEnumerable<int> PluginLoadDirectory(string directory)
        {
            List<int> list = new List<int>();

            if (Directory.Exists(directory))
            {
                foreach (var wildcard in new[] { "bass*.dll", "libbass*.so", "libbass*.dylib" })
                {
                    var libs = Directory.GetFiles(directory, wildcard);

                    if (libs == null || libs.Length == 0)
                        continue;

                    foreach (var lib in libs)
                    {
                        int h = BASS_PluginLoad(lib);
                        if (h != 0) list.Add(h);
                    }

                    if (list.Count > 0)
                        break;
                }
            }

            return list;
        }
    }
}
