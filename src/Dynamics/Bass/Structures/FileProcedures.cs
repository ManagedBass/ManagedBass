using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Table of callback functions used with <see cref="Bass.CreateStream(StreamSystem,BassFlags,FileProcedures,IntPtr)" />.
    /// </summary>
    /// <remarks>
    /// A copy is made of the procs callback function table, so it does not have to persist beyond this function call. 
    /// This means it is not required to pin the 'procs' instance, but it is still required to keep a reference as long as BASS uses the callback delegates in order to prevent the callbacks from being garbage collected.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FileProcedures
    {
        /// <summary>
        /// Callback function to close the file.
        /// </summary>
        public FileCloseProcedure Close;

        /// <summary>
        /// Callback function to get the file Length.
        /// </summary>
        public FileLengthProcedure Length;

        /// <summary>
        /// Callback function to read from the file.
        /// </summary>
        public FileReadProcedure Read;

        /// <summary>
        /// Callback function to seek in the file. Not used by buffered file streams.
        /// </summary>
        public FileSeekProcedure Seek;
    }
}
