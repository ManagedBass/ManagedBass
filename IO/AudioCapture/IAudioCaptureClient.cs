using System;

namespace ManagedBass
{
    public interface IAudioCaptureClient : IDisposable
    {
        bool Start();
        bool Stop();

        double Level { get; }
        bool IsActive { get; }

        event Action<BufferProvider> DataAvailable;
    }
}