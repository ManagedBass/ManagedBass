using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture SoundCard output using Wasapi Loopback
    /// </summary>
    public class Loopback : IAudioCaptureClient
    {
        Silence SilencePlayer;
        WasapiLoopbackDevice Device;

        public Loopback(WasapiLoopbackDevice Device, bool IncludeSilence)
        {
            this.Device = Device;

            if (IncludeSilence)
            {
                PlaybackDevice playbackDevice = PlaybackDevice.DefaultDevice;

                foreach (var dev in PlaybackDevice.Devices)
                    if (dev.DeviceInfo.Driver == Device.DeviceInfo.ID)
                        playbackDevice = dev;

                SilencePlayer = new Silence(playbackDevice);
            }

            Device.Init();
            Device.Callback += (b) =>
                {
                    if (DataAvailable != null)
                        DataAvailable(b);
                };
        }

        public double Level { get { return Device.Level; } }

        public bool IsActive { get { return Device.IsStarted; } }

        public bool Start()
        {
            if (SilencePlayer != null) SilencePlayer.Start();

            bool Result = Device.Start();

            if (SilencePlayer != null && !Result) SilencePlayer.Stop();

            return Result;
        }

        public bool Stop()
        {
            if (SilencePlayer != null) SilencePlayer.Stop();

            return Device.Stop();
        }

        public void Dispose()
        {
            Device.Dispose();
            if (SilencePlayer != null)
                SilencePlayer.Dispose();
        }

        public event Action<BufferProvider> DataAvailable;
    }
}
