using System.Collections.Generic;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class MixerStream : Playable
    {
        public MixerStream(int Frequency = 44100, int NoOfChannels = 2, bool Buffer = true, Resolution BufferKind = Resolution.Short)
            : base(BufferKind)
        {
            Handle = BassMix.CreateMixerStream(Frequency, NoOfChannels, BufferKind.ToBassFlag());
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