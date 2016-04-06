using System;
using System.Linq;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Capture SoundCard output using Wasapi Loopback
    /// </summary>
    public class Loopback : IAudioCaptureClient
    {
        readonly Silence _silencePlayer;
        readonly WasapiLoopbackDevice _device;

        public Loopback(WasapiLoopbackDevice Device, bool IncludeSilence = true)
        {
            _device = Device;

            if (IncludeSilence)
            {
                var playbackDevice = PlaybackDevice.Devices.First(Dev => Dev.DeviceInfo.Driver == Device.DeviceInfo.ID);
                
                _silencePlayer = new Silence(playbackDevice);
            }

            Device.Init();
            Device.Callback += B => DataAvailable?.Invoke(B);
        }

        public double Level => _device.Level;

        public bool IsActive => _device.IsStarted;

        public bool Start()
        {
            _silencePlayer?.Start();

            var result = _device.Start();

            if (_silencePlayer != null && !result)
                _silencePlayer.Stop();

            return result;
        }

        public bool Stop()
        {
            _silencePlayer?.Stop();

            return _device.Stop();
        }

        public void Dispose()
        {
            _device.Dispose();
            _silencePlayer?.Dispose();
        }

        public event Action<BufferProvider> DataAvailable;
    }
}
