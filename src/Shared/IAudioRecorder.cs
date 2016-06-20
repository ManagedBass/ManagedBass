using System;

namespace ManagedBass
{
    /// <summary>
    /// Implemented by an Audio Recorder.
    /// </summary>
    public interface IAudioRecorder : IDisposable
    {
        /// <summary>
        /// Start Recording.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        bool Start();

        /// <summary>
        /// Stop Recording.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        bool Stop();

        /// <summary>
        /// Returns the Audio Peak Level.
        /// </summary>
        double Level { get; }

        /// <summary>
        /// Gets if Recording is in Progress.
        /// </summary>
        bool IsRecording { get; }

        /// <summary>
        /// Format of the recorded data.
        /// </summary>
        PCMFormat Format { get; }

        /// <summary>
        /// Provides the recorded data.
        /// </summary>
        event Action<BufferProvider> DataAvailable;
    }
}