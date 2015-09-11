using System.IO;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class FilePlayer : AdvancedPlayable
    {
        public string FilePath { get; set; }

        ~FilePlayer() { Dispose(); }

        public FilePlayer(string FilePath, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            this.FilePath = FilePath;

            Handle = Bass.CreateStream(FilePath, 0, 0, BassFlags.Unicode | BassFlags.Prescan | BufferKind.ToBassFlag());
        }
    }
}