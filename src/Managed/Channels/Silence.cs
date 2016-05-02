using System;

namespace ManagedBass
{
    /// <summary>
    /// Plays Silence on the specified Device. Useful for WasapiLoopbackCapture.
    /// </summary>
    public sealed class Silence : Channel
    {
        public Silence(PlaybackDevice Device)
        {
            Handle = Bass.CreateStream(44100, 1, BassFlags.Byte, (h, b, l, u) => l, IntPtr.Zero);
            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);
            Bass.ChannelSetAttribute(Handle, ChannelAttribute.Volume, 0);
        }

        ~Silence() { Dispose(); }
    }
}