using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PluginInfo
    {
        int version;
        int formatc;
        PluginFormat formats;
    }
}