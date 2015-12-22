using ManagedBass.Dynamics;

namespace ManagedBass
{
    public abstract class Playable : Channel
    {
        static Playable() { PlaybackDevice.DefaultDevice.Initialize(); }

        bool RestartOnNextPlayback = false;

        public bool Start()
        {
            bool Result = Bass.PlayChannel(Handle, RestartOnNextPlayback);
            if (Result) RestartOnNextPlayback = false;
            return Result;
        }

        protected Playable(Resolution BufferKind = Resolution.Short) : base(BufferKind) { Bass.CurrentDevice = 1; }

        public PlaybackDevice Device
        {
            get { return PlaybackDevice.Get(Bass.ChannelGetDevice(Handle)); }
            set
            {
                value.Initialize();
                Bass.ChannelSetDevice(Handle, value.DeviceIndex);
            }
        }

        public bool IsPlaying { get { return Bass.IsChannelActive(Handle) == PlaybackState.Playing; } }

        public bool Pause() { return Bass.PauseChannel(Handle); }

        public bool Stop()
        {
            RestartOnNextPlayback = true;
            return Bass.StopChannel(Handle);
        }
    }
}
