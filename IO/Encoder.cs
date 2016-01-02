using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    abstract class Encoder
    {
        int Handle = 0;

        public bool AddChunk(string ID, IntPtr buffer, int length)
        {
            return BassEnc.EncodeAddChunk(Handle, ID, buffer, length);
        }

        public bool IsActive { get { return BassEnc.EncodeIsActive(Handle) == PlaybackState.Playing; } }

        public int Channel
        {
            get { return BassEnc.EncodeGetChannel(Handle); }
            set { BassEnc.EncodeSetChannel(Handle, value); }
        }

        public long QueueCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.Queue); } }
        public long InCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.In); } }
        public long OutCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.Out); } }
        public long QueueLimit { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.QueueLimit); } }
        public long CastCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.Cast); } }
        public long QueueFailCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.QueueFail); } }
    }
}
