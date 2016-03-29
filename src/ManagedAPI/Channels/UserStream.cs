using System;

namespace ManagedBass
{
    public delegate int UserStreamCallback(BufferProvider buffer);

    /// <summary>
    /// Streams audio through a UserStreamCallback.
    /// </summary>
    public sealed class UserStream : Channel
    {
        readonly StreamProcedure _procedure;
        readonly UserStreamCallback _call;

        public UserStream(UserStreamCallback callback,
                          PlaybackDevice Device,
                          bool IsDecoder = false,
                          Resolution Resolution = Resolution.Short,
                          bool IsMono = false)
        {
            _call = callback;
            _procedure = Callback;

            // Stream Flags
            var flags = FlagGen(IsDecoder, Resolution);

            // Set Mono            
            if (IsMono)
                flags |= BassFlags.Mono;

            Handle = Bass.CreateStream(44100, 2, flags, _procedure, IntPtr.Zero);

            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);
        }

        int Callback(int Handle, IntPtr Buffer, int Length, IntPtr User) => _call.Invoke(new BufferProvider(Buffer, Length));
    }
}
