using System;

namespace ManagedBass
{
    /// <summary>
    /// Plays Silence on the specified Device. Useful for WasapiLoopbackCapture.
    /// </summary>
    public sealed class Silence : IDisposable
    {
        readonly int _handle;

        /// <summary>
        /// Creates a new instance of <see cref="Silence"/>.
        /// </summary>
        /// <param name="Device">The <see cref="PlaybackDevice"/> to use.</param>
        public Silence(PlaybackDevice Device)
        {
            _handle = Bass.CreateStream(44100, 1, BassFlags.Byte, (h, b, l, u) => l, IntPtr.Zero);
            Bass.ChannelSetDevice(_handle, Device.Index);
            Bass.ChannelSetAttribute(_handle, ChannelAttribute.Volume, 0);
        }

        /// <summary>
        /// Play Silence.
        /// </summary>
        /// <returns>true on success, else false.</returns>
        public bool Play() => Bass.ChannelPlay(_handle);

        /// <summary>
        /// Stop playing Silence.
        /// </summary>
        /// <returns>true on success, else false.</returns>
        public bool Stop() => Bass.ChannelStop(_handle);

        /// <summary>
        /// Dispose all resources used by this instance.
        /// </summary>
        public void Dispose() => Bass.StreamFree(_handle);
    }
}