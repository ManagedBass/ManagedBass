using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.Marshal;

namespace ManagedBass
{
    public class MusicModule : Channel
    {
        public MusicModule(string FilePath, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = Bass.MusicLoad(FilePath, 0, 0, FlagGen(IsDecoder, Resolution), 0);
        }

        MusicModule(byte[] Memory, int Length, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            GCHandle GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);
            Handle = Bass.MusicLoad(GCPin.AddrOfPinnedObject(), 0, Length, FlagGen(IsDecoder, Resolution));
            GCPin.Free();
        }

        public string Title => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicName));

        public string Author => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicAuth));

        public string Message => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicMessage));

        public string Instrument(int Index) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicInstrument + Index));

        public string MusicSampleName(int index) => Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicSample + index));

        public byte[] MusicOrders
        {
            get
            {
                int n = (int)Bass.ChannelGetLength(Handle, PositionFlags.MusicOrders);

                byte[] b = new byte[n];

                IntPtr ptr = Bass.ChannelGetTags(Handle, TagType.MusicOrders);

                Marshal.Copy(ptr, b, 0, n);

                return b;
            }
        }

        public int MusicChannelCount
        {
            get
            {
                int channels = 0;
                float dummy;

                while (Bass.ChannelGetAttribute(Handle, ChannelAttribute.MusicVolumeChannel + channels, out dummy))
                    channels++;

                return channels;
            }
        }

        public int InstrumentCount
        {
            get
            {
                int instruments = 0;
                float dummy;

                while (Bass.ChannelGetAttribute(Handle, ChannelAttribute.MusicVolumeInstrument + instruments, out dummy))
                    instruments++;

                return instruments;
            }
        }

        public override void Dispose() => Bass.MusicFree(Handle);
    }
}
