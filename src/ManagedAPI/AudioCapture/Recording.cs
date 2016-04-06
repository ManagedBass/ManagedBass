using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture audio from Microphone
    /// </summary>
    public sealed class Recording : Channel, IAudioCaptureClient
    {   
        public Recording(RecordingDevice Device = null, int Channels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short)
        {
            if (Device == null)
                Device = RecordingDevice.DefaultDevice;

            Device.Init();
            var deviceIndex = Device.DeviceIndex;

            Bass.CurrentRecordingDevice = deviceIndex;
            
            Handle = Bass.RecordStart(SampleRate, Channels, BassFlags.RecordPause | Resolution.ToBassFlag(), Processing);
        }

        public bool IsActive => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        public override bool Stop() => Bass.ChannelPause(Handle);

        #region Callback
        public event Action<BufferProvider> DataAvailable;

        bool Processing(int HRecord, IntPtr Buffer, int Length, IntPtr User)
        {
            DataAvailable?.Invoke(new BufferProvider(Buffer, Length));

            return true;
        }
        #endregion
    }
}