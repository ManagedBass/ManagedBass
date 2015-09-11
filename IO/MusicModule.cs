using System.Runtime.InteropServices;
using ManagedBass.Dynamics;
using ManagedBass.Effects;

namespace ManagedBass
{
    public class MusicModule : AdvancedPlayable, IEffectAssignable
    {
        public MusicModule(string FilePath, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            Handle = Bass.LoadMusic(FilePath, 0, 0, BufferKind.ToBassFlag(), 0);
        }

        MusicModule(byte[] Memory, int Length, BufferKind BufferKind = BufferKind.Short)
           : base(BufferKind)
        {
            GCHandle GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);
            Handle = Bass.LoadMusic(GCPin.AddrOfPinnedObject(), 0, Length, BassFlags.Byte);
            GCPin.Free();
        }

        public MusicModule(float[] Memory, int Length, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            GCHandle GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);
            Handle = Bass.LoadMusic(GCPin.AddrOfPinnedObject(), 0, Length, BassFlags.Byte);
            GCPin.Free();
        }

        public string Title { get { return Marshal.PtrToStringAnsi(Bass.GetChannelTags(Handle, TagType.MusicName)); } }

        public string Message { get { return Marshal.PtrToStringAnsi(Bass.GetChannelTags(Handle, TagType.MusicMessage)); } }

        public string Instrument(int Index)
        {
            return Marshal.PtrToStringAnsi(Bass.GetChannelTags(Handle, TagType.MusicInstrument + Index)); 
        }

        public override void Dispose() { Bass.MusicFree(Handle); }
    }
}
