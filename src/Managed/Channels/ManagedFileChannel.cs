#if WINDOWS
using System.IO;

namespace ManagedBass
{
    /// <summary>
    /// Stream an audio file using .Net file handling using <see cref="FileProcedures"/>.
    /// </summary>
    public sealed class ManagedFileChannel : StreamChannel
    {
        /// <summary>
        /// Gets the path of the Loaded file
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Creates a new Instance of <see cref="ManagedFileChannel"/>.
        /// </summary>
        /// <param name="FileName">Path to the file to load.</param>
        /// <param name="IsDecoder">Whether to create a Decoding Channel.</param>
        /// <param name="Resolution">Channel Resolution to use.</param>
        public ManagedFileChannel(string FileName, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
            : base(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read), IsDecoder, Resolution)
        {
            this.FileName = FileName;
        }
    }
}
#endif