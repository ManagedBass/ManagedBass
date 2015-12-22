using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiFontInfo
    {
        IntPtr name;
        IntPtr copyright;
        IntPtr comment;
        public int presents;
        public int samsize;
        public int samload;
        public int samtype;

        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        public string Copyright { get { return Marshal.PtrToStringAnsi(copyright); } }

        public string Comment { get { return Marshal.PtrToStringAnsi(comment); } }
    }
}