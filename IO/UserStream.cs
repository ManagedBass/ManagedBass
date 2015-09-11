using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public delegate int UserStreamCallback(BufferProvider buffer);

    public class UserStream : Playable
    {
        StreamProcedure Procedure;
        UserStreamCallback call;

        public UserStream(UserStreamCallback callback, PlaybackDevice Device, BufferKind BufferKind = BufferKind.Short, bool IsMono = false)
            : base(BufferKind)
        {
            call = callback;
            Procedure = new StreamProcedure(Callback);
            
            // Stream Flags
            BassFlags Flags = BufferKind.ToBassFlag();

            // Set Mono            
            if (IsMono) Flags |= BassFlags.Mono;

            Handle = Bass.CreateStream(44100, 2, Flags, Procedure, IntPtr.Zero);

            Bass.ChannelSetDevice(Handle, Device.DeviceId);
        }

        int Callback(int handle, IntPtr Buffer, int Length, IntPtr User) { return call.Invoke(new BufferProvider(Buffer, Length, BufferKind)); }
    }
}
