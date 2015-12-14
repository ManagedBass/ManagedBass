using System.Collections.Generic;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class MixerStream : Playable
    {
        List<Channel> Sources = new List<Channel>();

        public MixerStream(int Frequency = 44100, int NoOfChannels = 2, bool Buffer = true, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            Handle = BassMix.CreateMixerStream(Frequency, NoOfChannels, BufferKind.ToBassFlag());
        }

        void Read(object Buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            BassMix.MixerChannelGetData(Handle, gch.AddrOfPinnedObject(), Length);

            gch.Free();
        }

        public override void Read(byte[] Buffer, int Length) { Read(Buffer as object, Length); }

        public override byte[] ReadByte(int Length)
        {
            var Buffer = new byte[Length];

            Read(Buffer, Length);

            return Buffer;
        }

        public override void Read(float[] Buffer, int Length) { Read(Buffer as object, Length); }

        public override float[] ReadFloat(int Length)
        {
            var Buffer = new float[Length / 4];

            Read(Buffer, Length);

            return Buffer;
        }

        public bool AddChannel(Channel channel)
        {
            bool Result = BassMix.MixerAddChannel(Handle, channel.Handle, BassFlags.Default);

            if (Result) Sources.Add(channel);

            return Result;
        }

        public bool RemoveChannel(Channel channel)
        {
            for (int i = 0; i < Sources.Count; ++i)
            {
                if (Sources[i].Handle == channel.Handle)
                {
                    bool Result = BassMix.MixerRemoveChannel(channel.Handle);

                    if (Result) Sources.RemoveAt(i);

                    return Result;
                }
            }

            return false;
        }
    }
}