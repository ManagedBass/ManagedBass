using System.IO;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class FileDecoder : Decoder
    {
        public string FilePath { get; set; }

        ~FileDecoder() { Dispose(); }

        public FileDecoder(string FilePath, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            this.FilePath = FilePath;

            Handle = Bass.CreateStream(FilePath, 0, 0, BassFlags.Decode | BassFlags.Unicode | BassFlags.Prescan | BufferKind.ToBassFlag());
        }
    }
}