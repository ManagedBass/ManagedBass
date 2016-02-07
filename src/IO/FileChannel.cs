using System.IO;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Stream an audio file using Bass' file handling
    /// </summary>
    public class FileChannel : Channel
    {
        public string FilePath { get; set; }
        
        public FileChannel(string FilePath, bool IsDecoder = false, Resolution Resolution = Resolution.Short, long Offset = 0, long Length = 0)
        {
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            this.FilePath = FilePath;
            
            Handle = Bass.CreateStream(FilePath, Offset, Length, FlagGen(IsDecoder, Resolution, BassFlags.Prescan));
        }
    }
}