using System;

namespace ManagedBass
{
    /// <summary>
    /// Plays Silence on the specified Device. Useful for WasapiLoopbackCapture.
    /// </summary>
    public sealed class Silence : IDisposable
    {
        readonly int _handle;

        public Silence(PlaybackDevice Device)
        {
            _handle = Bass.CreateStream(44100, 1, BassFlags.Byte, (h, b, l, u) => l, IntPtr.Zero);
            Bass.ChannelSetDevice(_handle, Device.Index);
            Bass.ChannelSetAttribute(_handle, ChannelAttribute.Volume, 0);
        }

        public bool Play() => Bass.ChannelPlay(_handle);

        public bool Stop() => Bass.ChannelStop(_handle);

        public void Dispose() => Bass.StreamFree(_handle);
    }
}