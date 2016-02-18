using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Used with <see cref="Bass.GetPluginInfo" /> to retrieve information on a plugin.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PluginInfo
    {
        int version;
        int formatc;
        IntPtr formats;

        /// <summary>
        /// Plugin version.
        /// </summary>
        public Version Version => Extensions.GetVersion(version);

        /// <summary>
        /// The collection of supported formats.
        /// </summary>
        /// <remarks>
        /// Note: There is no guarantee that the list of supported formats is complete or might contain formats not being supported on your particular OS/machine (due to additional or missing audio codecs).
        /// </remarks>
        public PluginFormat[] Formats
        {
            get
            {
                var arr = new PluginFormat[formatc];

                for (int i = 0; i < formatc; ++i)
                {
                    PluginFormat format;
                    format = (PluginFormat)Marshal.PtrToStructure(formats, typeof(PluginFormat));
                    formats += Marshal.SizeOf(format);
                    arr[i] = format;
                }

                return arr;
            }
        }
    }
}