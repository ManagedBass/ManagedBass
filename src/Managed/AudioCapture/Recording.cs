using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture audio from Microphone. This class inherits from <see cref="Channel"/> and can have Effects/DSP applied on it.
    /// </summary>
    public sealed class Recording : Channel, IAudioCaptureClient
    {
        public Recording() : this(RecordingDevice.DefaultDevice, new PCMFormat()) { }

        public Recording(RecordingDevice Device) : this(Device, new PCMFormat()) { }
        
        /// <summary>
        /// Creates a new instance of <see cref="Recording"/>.
        /// </summary>
        /// <param name="Device">The <see cref="RecordingDevice"/> to use.</param>
        /// <param name="Format">Channels, SampleRate, Resolution.</param>
        public Recording(RecordingDevice Device, PCMFormat Format)
        {
            Device.Init();
            var deviceIndex = Device.DeviceIndex;

            Bass.CurrentRecordingDevice = deviceIndex;
            
            Handle = Bass.RecordStart(Format.Frequency, Format.Channels, BassFlags.RecordPause | Format.Resolution.ToBassFlag(), Processing);
        }

        /// <summary>
        /// Gets if Capturing is in progress.
        /// </summary>
        public bool IsActive => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        /// <summary>
        /// Stop Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public override bool Stop() => Bass.ChannelPause(Handle);

        #region Callback
        /// <summary>
        /// Provides the captured data.
        /// </summary>
        public event Action<BufferProvider> DataAvailable;

        bool Processing(int HRecord, IntPtr Buffer, int Length, IntPtr User)
        {
            DataAvailable?.Invoke(new BufferProvider(Buffer, Length));

            return true;
        }
        #endregion
    }
}