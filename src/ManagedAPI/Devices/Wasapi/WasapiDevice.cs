using System;
using System.Collections.Generic;
using static ManagedBass.Wasapi.BassWasapi;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Wraps a WASAPI Device
    /// </summary>
    public abstract class WasapiDevice : IDisposable
    {
        protected static Dictionary<int, WasapiDevice> Singleton = new Dictionary<int, WasapiDevice>();

        #region Notify
        static readonly WasapiNotifyProcedure Notifyproc = OnNotify;

        static void OnNotify(WasapiNotificationType Notify, int Device, IntPtr User)
        {
            Singleton[Device]._StateChanged?.Invoke(Notify);
        }

        bool _notifyRegistered;
        event Action<WasapiNotificationType> _StateChanged;

        public event Action<WasapiNotificationType> StateChanged
        {
            add
            {
                if (!_notifyRegistered)
                {
                    CurrentDevice = DeviceIndex;
                    _notifyRegistered = SetNotify(Notifyproc);
                }

                _StateChanged += value;
            }
            remove
            {
                _StateChanged -= value;

                if (_StateChanged != null || !_notifyRegistered)
                    return;

                CurrentDevice = DeviceIndex;
                _notifyRegistered = !SetNotify(null);
            }
        }
        #endregion

        protected WasapiDevice(int Index)
        {
            _proc = OnProc;
            DeviceIndex = Index;
        }

        #region Device Info
        public int DeviceIndex { get; }

        public WasapiDeviceInfo DeviceInfo => GetDeviceInfo(DeviceIndex);
        #endregion

        #region Callback
        readonly WasapiProcedure _proc;

        public virtual int OnProc(IntPtr Buffer, int Length, IntPtr User)
        {
            Callback?.Invoke(new BufferProvider(Buffer, Length));
            return Length;
        }

        public virtual event Action<BufferProvider> Callback;
        #endregion

        public void Dispose()
        {
            CurrentDevice = DeviceIndex;
            Free();
        }

        #region Read
        public int Read(IntPtr Buffer, int Length) => GetData(Buffer, Length);

        public int Read(float[] Buffer, int Length) => GetData(Buffer, Length);
        #endregion

        #region Write
        public void Write(IntPtr Buffer, int Length) => PutData(Buffer, Length);

        public void Write(float[] Buffer, int Length) => PutData(Buffer, Length);
        #endregion

        public bool Lock(bool Lock = true)
        {
            CurrentDevice = DeviceIndex;
            return BassWasapi.Lock(Lock);
        }

        public bool Mute
        {
            get
            {
                CurrentDevice = DeviceIndex;
                return GetMute();
            }
            set
            {
                CurrentDevice = DeviceIndex;
                SetMute(WasapiVolumeTypes.Device, value);
            }
        }

        public double Level => GetDeviceLevel(DeviceIndex);

        public double Volume
        {
            get
            {
                CurrentDevice = DeviceIndex;
                return GetVolume(WasapiVolumeTypes.Device | WasapiVolumeTypes.LinearCurve);
            }
            set
            {
                CurrentDevice = DeviceIndex;
                SetVolume(WasapiVolumeTypes.Device | WasapiVolumeTypes.LinearCurve, (float)value);
            }
        }

        protected bool _Init(int Frequency, int Channels, bool Shared, bool UseEventSync, int Buffer, int Period)
        {
            if (DeviceInfo.IsInitialized) return true;

            var flags = Shared ? WasapiInitFlags.Shared : WasapiInitFlags.Exclusive;
            if (UseEventSync) flags |= WasapiInitFlags.EventDriven;

            var result = Init(DeviceIndex,
                               Frequency,
                               Channels,
                               flags,
                               Buffer,
                               Period,
                               _proc);

            return result;
        }

        public bool IsStarted
        {
            get
            {
                CurrentDevice = DeviceIndex;
                return BassWasapi.IsStarted;
            }
        }

        public bool Start()
        {
            CurrentDevice = DeviceIndex;
            return BassWasapi.Start();
        }

        public bool Stop(bool Reset = true)
        {
            CurrentDevice = DeviceIndex;
            return BassWasapi.Stop(Reset);
        }

        #region Overrides
        public override bool Equals(object Obj) => Obj is WasapiDevice && DeviceIndex == ((WasapiDevice)Obj).DeviceIndex;

        public override string ToString()
        {
            return DeviceInfo.Name
                + (DeviceInfo.IsLoopback ? " (Loopback)" : string.Empty)
                + (DeviceInfo.IsDefault ? " (Default)" : string.Empty);
        }

        public override int GetHashCode() => DeviceIndex;
        #endregion
    }
}
