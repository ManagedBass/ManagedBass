using ManagedBass.Dynamics;

namespace ManagedBass
{
    class Midi : Playable
    {
        public Midi(string FilePath, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            Handle = BassMidi.CreateStream(false, FilePath, 0, 0, BufferKind.ToBassFlag(), 0);
        }
    }
}
