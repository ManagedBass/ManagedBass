using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PluginFormat
    {
        int ctype;
        IntPtr name;
        IntPtr exts;
    }
}