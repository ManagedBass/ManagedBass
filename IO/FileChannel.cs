using System.IO;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class FileChannel : Channel
    {
        public string FilePath { get; set; }
        
        public FileChannel(string FilePath, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            this.FilePath = FilePath;

            var flags = BufferKind.ToBassFlag() | BassFlags.Unicode | BassFlags.Prescan;
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = Bass.CreateStream(FilePath, 0, 0, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }
    }
}