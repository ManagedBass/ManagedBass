using System;
using System.Linq;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Capture SoundCard output using Wasapi Loopback. Requires basswasapi.dll.
    /// </summary>
    public class Loopback : IAudioRecorder
    {
        readonly Silence _silencePlayer;
        readonly WasapiLoopbackDevice _device;

        /// <summary>
        /// Creates a new instance of <see cref="Loopback"/> capture.
        /// </summary>
        /// <param name="Device">The <see cref="WasapiLoopbackDevice"/> to use.</param>
        /// <param name="IncludeSilence">Whether to include Silence in the Capture.</param>
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

            var info = Device.DeviceInfo;

            Format = new PCMFormat(info.MixFrequency, info.MixChannels, Resolution.Float);
        }

        public PCMFormat Format { get; }

        /// <summary>
        /// Returns the soundcard output level.
        /// </summary>
        public double Level => _device.Level;

        /// <summary>
        /// Gets if Capturing is in progress.
        /// </summary>
        public bool IsActive => _device.IsStarted;

        /// <summary>
        /// Start Loopback Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public bool Start()
        {
            _silencePlayer?.Start();

            var result = _device.Start();

            if (_silencePlayer != null && !result)
                _silencePlayer.Stop();

            return result;
        }

        /// <summary>
        /// Stop Loopback Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public bool Stop()
        {
            _silencePlayer?.Stop();

            return _device.Stop();
        }

        /// <summary>
        /// Frees all Resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            _device.Dispose();
            _silencePlayer?.Dispose();
        }

        /// <summary>
        /// Provides the captured data.
        /// </summary>
        public event Action<BufferProvider> DataAvailable;
    }
}