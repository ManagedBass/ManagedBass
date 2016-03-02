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

        public Loopback(WasapiLoopbackDevice Device, bool IncludeSilence = true)
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
            Device.Callback += (b) => DataAvailable?.Invoke(b);
        }

        public double Level => Device.Level;

        public bool IsActive => Device.IsStarted;

        public bool Start()
        {
            SilencePlayer?.Start();

            bool Result = Device.Start();

            if (SilencePlayer != null && !Result) SilencePlayer.Stop();

            return Result;
        }

        public bool Stop()
        {
            SilencePlayer?.Stop();

            return Device.Stop();
        }

        public void Dispose()
        {
            Device.Dispose();
            SilencePlayer?.Dispose();
        }

        public event Action<BufferProvider> DataAvailable;
    }
}
