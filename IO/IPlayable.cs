using System;

namespace ManagedBass
{
    public interface IPlayable : IDisposable
    {
        int Handle { get; }

        bool IsPlaying { get; }

        bool Start();
        bool Pause();
        bool Stop();

        event EventHandler MediaEnded, MediaFailed;

        PlaybackDevice Device { get; set; }

        double Volume { get; set; }
        double Position { get; set; }
        double Level { get; }
        double Duration { get; }
        double Frequency { get; set; }
        double Balance { get; set; }
        bool Loop { get; set; }
        bool IsMono { get; }
    }
}