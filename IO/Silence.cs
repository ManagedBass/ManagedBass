using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class Silence : Playable
    {
        StreamProcedure Procedure = new StreamProcedure((h, b, l, u) => l);

        public Silence(PlaybackDevice Device)
            : base(Resolution.Byte)
        {
            Handle = Bass.CreateStream(44100, 1, BassFlags.Byte, Procedure, IntPtr.Zero);
            Bass.ChannelSetDevice(Handle, Device.DeviceId);
            Bass.SetChannelAttribute(Handle, ChannelAttribute.Volume, 0);
        }

        ~Silence() { Dispose(); }
    }
}