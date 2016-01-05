using System.Collections.Generic;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class MixerStream : Channel
    {
        public MixerStream(int Frequency = 44100, int NoOfChannels = 2, bool IsDecoder = true, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassMix.CreateMixerStream(Frequency, NoOfChannels, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        int Read(object Buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            int Return = BassMix.MixerChannelGetData(Handle, gch.AddrOfPinnedObject(), Length);

            gch.Free();

            return Return;
        }

        public override int Read(byte[] Buffer, int Length) { return Read(Buffer as object, Length); }

        public override byte[] ReadByte(int Length)
        {
            var Buffer = new byte[Length];

            Read(Buffer, Length);

            return Buffer;
        }

        public override int Read(float[] Buffer, int Length) { return Read(Buffer as object, Length); }

        public override float[] ReadFloat(int Length)
        {
            var Buffer = new float[Length / 4];

            Read(Buffer, Length);

            return Buffer;
        }

        public bool AddChannel(Channel channel) { return BassMix.MixerAddChannel(Handle, channel.Handle, BassFlags.Default); }

        public bool RemoveChannel(Channel channel) { return BassMix.MixerRemoveChannel(channel.Handle); }
    }
}