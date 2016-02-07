using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Used with <see cref="BassAsio.ChannelGetInfo(bool,int,out AsioChannelInfo)" /> to retrieve information on the current device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsioChannelInfo
    {
        int group;
        AsioSampleFormat format;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        char[] name;

        /// <summary>
        /// The channel's group.
        /// </summary>
        public int Group { get { return group; } }

        /// <summary>
        /// The channel's sample format
        /// </summary>
        public AsioSampleFormat Format { get { return format; } }

        /// <summary>
        /// The name of the channel.
        /// </summary>
        public string Name { get { return new string(name).Replace("\0", "").Trim(); } }
    }
}