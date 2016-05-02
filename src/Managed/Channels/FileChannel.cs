using System;
using System.IO;

namespace ManagedBass
{
    /// <summary>
    /// Stream an audio file using Bass' file handling or from Memory.
    /// </summary>
    public sealed class FileChannel : Channel
    {
        /// <summary>
        /// Path of the loaded file or null if loaded from Memory.
        /// </summary>
        public string FilePath { get; }
        
        public FileChannel(string FilePath, bool IsDecoder = false, Resolution Resolution = Resolution.Short, long Offset = 0, long Length = 0)
        {
            if (!File.Exists(FilePath)) 
                throw new FileNotFoundException();

            this.FilePath = FilePath;
            
            Handle = Bass.CreateStream(FilePath, Offset, Length, FlagGen(IsDecoder, Resolution, BassFlags.Prescan));
        }

        public FileChannel(IntPtr Memory, int Offset, long Length, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = Bass.CreateStream(Memory, Offset, Length, FlagGen(IsDecoder, Resolution));
        }

        public FileChannel(byte[] Memory, int Offset, long Length, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = Bass.CreateStream(Memory, Offset, Length, FlagGen(IsDecoder, Resolution));
        }
    }
}