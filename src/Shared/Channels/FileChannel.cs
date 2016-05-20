using System.IO;

namespace ManagedBass
{
    public class FileChannel : Channel
    {
        /// <summary>
        /// Gets the FileName... null if steaming from Memory.
        /// </summary>
        public string FileName { get; }

        public FileChannel(string FileName, int Offset = 0, int Length = 0, BassFlags Flags = BassFlags.Default)
        {
#if !__HYBRID__
            if (!File.Exists(FileName))
                throw new FileNotFoundException();
#endif

            this.FileName = FileName;

            Handle = Bass.CreateStream(FileName, Offset, Length, Flags);
        }

        public FileChannel(byte[] Memory, int Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            Handle = Bass.CreateStream(Memory, Offset, Length, Flags);
        }

        public long GetFilePosition(FileStreamPosition Mode = FileStreamPosition.Current)
        {
            return Bass.StreamGetFilePosition(Handle, Mode);
        }
    }
}