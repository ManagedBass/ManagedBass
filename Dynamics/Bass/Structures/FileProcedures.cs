using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FileProcedures
    {
        FileCloseProcedure Close;
        FileLengthProcedure Length;
        FileReadProcedure Read;
        FileSeekProcedure Seek;
    }
}
