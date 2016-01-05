using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class Silence : Channel
    {
        StreamProcedure Procedure = new StreamProcedure((h, b, l, u) => l);

        public Silence(PlaybackDevice Device)
            : base(false, Resolution.Byte)
        {
            Handle = Bass.CreateStream(44100, 1, BassFlags.Byte, Procedure, IntPtr.Zero);
            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);
            Bass.SetChannelAttribute(Handle, ChannelAttribute.Volume, 0);

            Player = new BassPlayer(Handle, this);
        }

        ~Silence() { Dispose(); }
    }
}