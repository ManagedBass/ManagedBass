using System;
using ManagedBass.Tags;

namespace ManagedBass.Enc
{
    public interface IEncoder : IDisposable
    {
        ChannelType OutputType { get; }
        string OutputFileExtension { get; }

        int OutputBitRate { get; }
        
        bool IsActive { get; }
        bool IsPaused { get; set; }
        
        bool CanPause { get; }
        
        bool Write(IntPtr Buffer, int Length);
        bool Write(byte[] Buffer, int Length);
        bool Write(short[] Buffer, int Length);
        bool Write(int[] Buffer, int Length);
        bool Write(float[] Buffer, int Length);
        
        TagReader Tags { get; set; }
        
        bool Start();
        bool Stop();
    }
}