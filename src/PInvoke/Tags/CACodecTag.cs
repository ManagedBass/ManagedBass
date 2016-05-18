#if __IOS__ || __MAC__
using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Tags
{
    [StructLayout(LayoutKind.Sequential)]
    public class CACodecTag
    {
		/// <summary>
		/// The file format identifier.
		/// </summary>
        public int ftype;

		/// <summary>
		/// The audio format identifier.
		/// </summary>
        public int atype;

        IntPtr name;

		/// <summary>
		/// The description of the audio file format.
		/// </summary>
        public string Name => Marshal.PtrToStringAnsi(name);

        public static CACodecTag Read(int Handle)
        {
            return (CACodecTag)Marshal.PtrToStructure(Bass.ChannelGetTags(Handle, TagType.CoreAudioCodec), typeof(CACodecTag));
        }
    }
}
#endif