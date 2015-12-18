using ManagedBass.Dynamics;

namespace ManagedBass
{
    class Midi : Playable
    {
        public Midi(string FilePath, Resolution BufferKind = Resolution.Short)
            : base(BufferKind)
        {
            Handle = BassMidi.CreateStream(false, FilePath, 0, 0, BufferKind.ToBassFlag(), 0);
        }
    }
}
