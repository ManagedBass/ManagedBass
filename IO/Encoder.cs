using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public abstract class Encoder : IDisposable
    {
        protected int Channel { get; private set; }

        public int Handle { get; protected set; }

        public Encoder(int Channel) { this.Channel = Channel; }

        public bool AddChunk(string ID, IntPtr buffer, int length)
        {
            return BassEnc.EncodeAddChunk(Handle, ID, buffer, length);
        }

        public void EncodeAvailableFromDecoder()
        {
            if (!Bass.ChannelGetInfo(Channel).IsDecodingChannel)
                throw new InvalidOperationException("Not a Decoding Channel");

            int BlockLength = (int)Bass.ChannelSeconds2Bytes(Channel, 2);

            byte[] Buffer = new byte[BlockLength];

            var gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            // while Decoder has Data
            while (Bass.ChannelIsActive(Channel) == PlaybackState.Playing)
                Bass.ChannelGetData(Channel, gch.AddrOfPinnedObject(), BlockLength);

            gch.Free();

            BassEnc.EncodeStop(Handle);
        }

        public bool IsActive { get { return BassEnc.EncodeIsActive(Handle) == PlaybackState.Playing; } }

        public long QueueCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.Queue); } }
        public long InCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.In); } }
        public long OutCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.Out); } }
        public long QueueLimit { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.QueueLimit); } }
        public long CastCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.Cast); } }
        public long QueueFailCount { get { return BassEnc.EncodeGetCount(Handle, EncodeCount.QueueFail); } }

        public void Dispose() { Bass.StreamFree(Channel); }
    }

    public class ACMFileEncoder : Encoder
    {
        public ACMFileEncoder(string FileName, int Channel, EncodeFlags flags, WaveFormatTag encoding)
            : base(Channel)
        {
            // Get the Length of the ACMFormat structure
            int SuggestedFormatLength = BassEnc.GetACMFormat(0);
            IntPtr ACMFormat = Marshal.AllocHGlobal(SuggestedFormatLength);

            // Retrieve ACMFormat and Init Encoding
            if (BassEnc.GetACMFormat(Channel, ACMFormat, SuggestedFormatLength, null,
                                     // If encoding is Unknown, then let the User choose encoding.
                                     encoding == WaveFormatTag.Unknown ? 0 : ACMFormatFlags.Suggest,
                                     encoding) != 0)
                Handle = BassEnc.EncodeStartACM(Channel, ACMFormat, flags | EncodeFlags.AutoFree, FileName);

            else throw new ManagedBassException("GetACMFormat Failed");

            // Free the ACMFormat structure
            Marshal.FreeHGlobal(ACMFormat);

            if (Handle == 0) throw new ManagedBassException("EncoderStartACM Failed");
        }
    }
}
