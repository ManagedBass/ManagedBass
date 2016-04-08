using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture audio from Microphone. This class inherits from <see cref="Channel"/> and can have Effects/DSP applied on it.
    /// </summary>
    public sealed class Recording : Channel, IAudioCaptureClient
    {
        /// <summary>
        /// Creates a new instance of <see cref="Recording"/>.
        /// </summary>
        /// <param name="Device">The <see cref="RecordingDevice"/> to use.</param>
        /// <param name="Channels">No of Channels.</param>
        /// <param name="SampleRate">Sample Rate.</param>
        /// <param name="Resolution">Bits per Sample.</param>
        public Recording(RecordingDevice Device = null, int Channels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short)
        {
            if (Device == null)
                Device = RecordingDevice.DefaultDevice;

            Device.Init();
            var deviceIndex = Device.DeviceIndex;

            Bass.CurrentRecordingDevice = deviceIndex;
            
            Handle = Bass.RecordStart(SampleRate, Channels, BassFlags.RecordPause | Resolution.ToBassFlag(), Processing);
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