using System;

namespace ManagedBass
{
    /// <summary>
    /// Implemented by an Audio Capturer.
    /// </summary>
    public interface IAudioCaptureClient : IDisposable
    {
        /// <summary>
        /// Start Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        bool Start();

        /// <summary>
        /// Stop Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        bool Stop();

        /// <summary>
        /// Returns the Audio Peak Level.
        /// </summary>
        double Level { get; }

        /// <summary>
        /// Gets if Capturing is in Progress.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Provides the captured data.
        /// </summary>
        event Action<BufferProvider> DataAvailable;
    }
}