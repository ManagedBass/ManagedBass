using ManagedBass.Dynamics;
using ManagedBass.Effects;
using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture audio from Microphone
    /// </summary>
    public class Recording : Channel, IAudioCaptureClient
    {
        #region Fields
        RecordProcedure RecordProcedure;
        int DeviceIndex;
        #endregion

        public Recording(RecordingDevice Device = null, int Channels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short)
        {
            if (Device == null) Device = RecordingDevice.DefaultDevice;

            Device.Init();
            DeviceIndex = Device.DeviceIndex;

            Bass.CurrentRecordingDevice = DeviceIndex;

            RecordProcedure = new RecordProcedure(Processing);

            Handle = Bass.RecordStart(SampleRate, Channels, BassFlags.RecordPause | Resolution.ToBassFlag(), RecordProcedure);
        }

        public bool IsActive { get { return Bass.ChannelIsActive(Handle) == PlaybackState.Playing; } }

        #region Callback
        public event Action<BufferProvider> DataAvailable;

        bool Processing(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            if (DataAvailable != null)
                DataAvailable.Invoke(new BufferProvider(Buffer, Length));
            return true;
        }
        #endregion
    }
}