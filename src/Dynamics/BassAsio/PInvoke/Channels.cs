using System;
using System.Runtime.InteropServices;
using static ManagedBass.Extensions;

namespace ManagedBass.Dynamics
{
    public static partial class BassAsio
    {
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnable")]
        public static extern bool ChannelEnable(bool input, int channel, AsioProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnableMirror")]
        public static extern bool ChannelEnableMirror(int channel, bool input2, int channel2);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetFormat")]
        public static extern AsioSampleFormat ChannelGetFormat(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetInfo")]
        public static extern bool ChannelGetInfo(bool input, int channel, out AsioChannelInfo info);

        public static AsioChannelInfo ChannelGetInfo(bool input, int channel)
        {
            AsioChannelInfo info;
            ChannelGetInfo(input, channel, out info);
            return info;
        }

        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetLevel(bool input, int channel);

        public static double ChannelGetLevel(bool input, int channel) => BASS_ASIO_ChannelGetLevel(input, channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetRate")]
        public static extern double ChannelGetRate(bool input, int channel);

        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetVolume(bool input, int channel);

        public static double ChannelGetVolume(bool input, int channel) => BASS_ASIO_ChannelGetVolume(input, channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelIsActive")]
        public static extern AsioChannelActive ChannelIsActive(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelJoin")]
        public static extern bool ChannelJoin(bool input, int channel, int channel2);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelPause")]
        public static extern bool ChannelPause(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelReset")]
        public static extern bool ChannelReset(bool input, int channel, AsioChannelResetFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetFormat")]
        public static extern bool ChannelSetFormat(bool input, int channel, AsioSampleFormat format);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetRate")]
        public static extern bool ChannelSetRate(bool input, int channel, double rate);

        [DllImport(DllName)]
        static extern bool BASS_ASIO_ChannelSetVolume(bool input, int channel, float volume);

        public static bool ChannelSetVolume(bool input, int channel, double volume)
        {
            return BASS_ASIO_ChannelSetVolume(input, channel, (float)volume);
        }
    }
}
