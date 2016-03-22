using System;

namespace ManagedBass
{
    /// <summary>
    /// Plays Silence on the specified Device. Useful for WasapiLoopbackCapture.
    /// </summary>
    public class Silence : Channel
    {
        StreamProcedure Procedure = new StreamProcedure((h, b, l, u) => l);

        public Silence(PlaybackDevice Device)
        {
            Handle = Bass.CreateStream(44100, 1, BassFlags.Byte, Procedure, IntPtr.Zero);
            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);
            Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, 0);
        }

        ~Silence() { Dispose(); }
    }
}