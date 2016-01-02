using ManagedBass.Dynamics;
using ManagedBass.Effects;
using System;

namespace ManagedBass
{
    public class Recording : Channel, IAudioCaptureClient, IEffectAssignable
    {
        #region Fields
        RecordProcedure RecordProcedure;
        int DeviceIndex;
        #endregion

        public Recording(RecordingDevice Device = null, Resolution BufferKind = Resolution.Short)
            : base(BufferKind)
        {
            if (Device == null) Device = RecordingDevice.DefaultDevice;

            Device.Initialize();
            DeviceIndex = Device.DeviceIndex;

            Bass.CurrentRecordingDevice = DeviceIndex;

            RecordProcedure = new RecordProcedure(Processing);

            Handle = Bass.StartRecording(44100, 2, BassFlags.RecordPause | BassFlags.Float, RecordProcedure);
        }

        public double Level { get { return Bass.GetChannelLevel(Handle); } }

        public bool IsActive { get { return Bass.IsChannelActive(Handle) == PlaybackState.Playing; } }
        
        public bool Start() { return Bass.PlayChannel(Handle, false); }

        public bool Stop() { return Bass.StopChannel(Handle); }
        
        #region Callback
        public event Action<BufferProvider> DataAvailable;

        bool Processing(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            if (DataAvailable != null) 
                DataAvailable.Invoke(new BufferProvider(Buffer, Length, BufferKind));
            return true;
        }
        #endregion
    }
}