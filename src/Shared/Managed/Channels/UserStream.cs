using System;

namespace ManagedBass
{
    public delegate int UserStreamCallback(BufferProvider Buffer);

    /// <summary>
    /// Streams audio through a <see cref="UserStreamCallback"/>.
    /// </summary>
    public sealed class UserStream : Channel
    {
        readonly UserStreamCallback _call;

        public UserStream(UserStreamCallback Callback,
                          PlaybackDevice Device,
                          bool IsDecoder = false,
                          Resolution Resolution = Resolution.Short,
                          bool IsMono = false)
        {
            _call = Callback;
     
            // Stream Flags
            var flags = FlagGen(IsDecoder, Resolution);

            // Set Mono            
            if (IsMono)
                flags |= BassFlags.Mono;

            Handle = Bass.CreateStream(44100, 2, flags, OnCallback, IntPtr.Zero);

            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);
        }

        int OnCallback(int Handle, IntPtr Buffer, int Length, IntPtr User) => _call.Invoke(new BufferProvider(Buffer, Length));
    }
}
