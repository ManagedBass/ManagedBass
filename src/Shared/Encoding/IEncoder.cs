using ManagedBass.Tags;

namespace ManagedBass.Enc
{
    public interface IEncoder : IAudioWriter
    {
        ChannelType OutputType { get; }
        string OutputFileExtension { get; }

        int OutputBitRate { get; }
        
        bool IsActive { get; }
        bool IsPaused { get; set; }
        
        bool CanPause { get; }
        
        TagReader Tags { get; set; }
        
        bool Start();
        bool Stop();
    }
}