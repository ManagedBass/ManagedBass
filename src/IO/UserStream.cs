using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public delegate int UserStreamCallback(BufferProvider buffer);

    /// <summary>
    /// Streams audio through a UserStreamCallback.
    /// </summary>
    public class UserStream : Channel
    {
        StreamProcedure Procedure;
        UserStreamCallback call;

        public UserStream(UserStreamCallback callback,
                          PlaybackDevice Device,
                          bool IsDecoder = false,
                          Resolution Resolution = Resolution.Short,
                          bool IsMono = false)
        {
            call = callback;
            Procedure = new StreamProcedure(Callback);

            // Stream Flags
            BassFlags Flags = FlagGen(IsDecoder, Resolution);

            // Set Mono            
            if (IsMono) Flags |= BassFlags.Mono;

            Handle = Bass.CreateStream(44100, 2, Flags, Procedure, IntPtr.Zero);

            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);
        }

        int Callback(int handle, IntPtr Buffer, int Length, IntPtr User) { return call.Invoke(new BufferProvider(Buffer, Length)); }
    }
}
