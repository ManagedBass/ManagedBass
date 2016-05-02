using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Tags
{
    [StructLayout(LayoutKind.Sequential)]
    public class CACodecTag
    {
        public int ftype, atype;

        IntPtr name;

        public string Name => Marshal.PtrToStringAnsi(name);

        public static CACodecTag Read(int Handle)
        {
            return (CACodecTag)Marshal.PtrToStructure(Bass.ChannelGetTags(Handle, TagType.CoreAudioCodec), typeof(CACodecTag));
        }
    }
}