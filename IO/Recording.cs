using System;
using ManagedBass.Dynamics;
using ManagedBass.Effects;

namespace ManagedBass
{
    public class Recording : Channel, IEffectAssignable
    {
        #region Fields
        RecordProcedure RecordProcedure;
        int DeviceIndex;
        #endregion

        #region Constructors
        public Recording(RecordingDevice Device = null, Resolution BufferKind = Resolution.Short)
            : base(BufferKind)
        {
            if (Device == null) Device = RecordingDevice.DefaultDevice;

            Device.Initialize();
            DeviceIndex = Device.DeviceId;

            Bass.CurrentRecordingDevice = DeviceIndex;

            RecordProcedure = new RecordProcedure(Processing);

            Handle = Bass.StartRecording(44100, 2, BassFlags.RecordPause | BassFlags.Float, RecordProcedure);
        }
        #endregion

        #region Properties
        public double Level { get { return Bass.GetChannelLevel(Handle); } }

        public bool IsActive { get { return Bass.IsChannelActive(Handle) == PlaybackState.Playing; } }
        #endregion

        #region Control
        public bool Start() { return Bass.PlayChannel(Handle, false); }

        public bool Pause() { return Bass.PauseChannel(Handle); }

        public bool Stop()
        {
            bool Result = Bass.StopChannel(Handle);

            if (Result)
            {
                if (Stopped != null) Stopped.Invoke();

                Bass.StreamFree(Handle);
            }

            return Result;
        }
        #endregion

        #region Callback
        public event Action<BufferProvider> Callback;

        bool Processing(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            if (Callback != null) Callback.Invoke(new BufferProvider(Buffer, Length, BufferKind));
            return true;
        }
        #endregion

        public event Action Stopped;
    }
}