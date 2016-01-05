using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public delegate int UserStreamCallback(BufferProvider buffer);

    public class UserStream : Channel
    {
        StreamProcedure Procedure;
        UserStreamCallback call;

        public UserStream(UserStreamCallback callback, PlaybackDevice Device, bool IsDecoder=false, Resolution BufferKind = Resolution.Short, bool IsMono = false)
            : base(IsDecoder,BufferKind)
        {
            call = callback;
            Procedure = new StreamProcedure(Callback);
            
            // Stream Flags
            BassFlags Flags = BufferKind.ToBassFlag();
            if (IsDecoder) Flags |= BassFlags.Decode;

            // Set Mono            
            if (IsMono) Flags |= BassFlags.Mono;

            Handle = Bass.CreateStream(44100, 2, Flags, Procedure, IntPtr.Zero);

            Bass.ChannelSetDevice(Handle, Device.DeviceIndex);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        int Callback(int handle, IntPtr Buffer, int Length, IntPtr User) { return call.Invoke(new BufferProvider(Buffer, Length, BufferKind)); }
    }
}
