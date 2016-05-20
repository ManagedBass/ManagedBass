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

        public UserStream(int Frequency, int Channels, UserStreamCallback Callback, BassFlags Flags = BassFlags.Default)
        {
            _call = Callback;
            
            Handle = Bass.CreateStream(Frequency, Channels, Flags, OnCallback, IntPtr.Zero);
        }

        int OnCallback(int Handle, IntPtr Buffer, int Length, IntPtr User) => _call.Invoke(new BufferProvider(Buffer, Length));
    }
}
