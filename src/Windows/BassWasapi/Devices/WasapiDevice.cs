using System;
using System.Collections.Generic;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Wraps a WASAPI Device
    /// </summary>
    public abstract class WasapiDevice : IDisposable
    {
        protected static readonly Dictionary<int, WasapiDevice> Singleton = new Dictionary<int, WasapiDevice>();

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
                    Ensure();
                    _notifyRegistered = BassWasapi.SetNotify(Notifyproc);
                }

                _StateChanged += value;
            }
            remove
            {
                _StateChanged -= value;

                if (_StateChanged != null || !_notifyRegistered)
                    return;

                Ensure();
                _notifyRegistered = !BassWasapi.SetNotify(null);
            }
        }
        #endregion

        internal WasapiDevice(int Index)
        {
            _proc = OnProc;
            DeviceIndex = Index;
        }
        
        #region Device Info
        public int DeviceIndex { get; }

        public WasapiDeviceInfo Info => BassWasapi.GetDeviceInfo(DeviceIndex);
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

        protected void Ensure() => BassWasapi.CurrentDevice = DeviceIndex;

        public void Dispose()
        {
            Ensure();
            BassWasapi.Free();
        }

        #region Read
        public int Read(IntPtr Buffer, int Length) => BassWasapi.GetData(Buffer, Length);

        public int Read(float[] Buffer, int Length) => BassWasapi.GetData(Buffer, Length);
        #endregion

        #region Write
        public void Write(IntPtr Buffer, int Length) => BassWasapi.PutData(Buffer, Length);

        public void Write(float[] Buffer, int Length) => BassWasapi.PutData(Buffer, Length);
        #endregion

        public bool Lock(bool Lock = true)
        {
            Ensure();
            return BassWasapi.Lock(Lock);
        }

        public bool Mute
        {
            get
            {
                Ensure();
                return BassWasapi.GetMute();
            }
            set
            {
                Ensure();
                BassWasapi.SetMute(WasapiVolumeTypes.Device, value);
            }
        }

        public double Level => BassWasapi.GetDeviceLevel(DeviceIndex);

        public double Volume
        {
            get
            {
                Ensure();
                return BassWasapi.GetVolume(WasapiVolumeTypes.Device | WasapiVolumeTypes.LinearCurve);
            }
            set
            {
                Ensure();
                BassWasapi.SetVolume(WasapiVolumeTypes.Device | WasapiVolumeTypes.LinearCurve, (float)value);
            }
        }

        protected bool _Init(int Frequency, int Channels, bool Shared, bool UseEventSync, int Buffer, int Period)
        {
            if (Info.IsInitialized) return true;

            var flags = Shared ? WasapiInitFlags.Shared : WasapiInitFlags.Exclusive;
            if (UseEventSync) flags |= WasapiInitFlags.EventDriven;

            var result = BassWasapi.Init(DeviceIndex,
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
                Ensure();
                return BassWasapi.IsStarted;
            }
        }

        public bool Start()
        {
            Ensure();
            return BassWasapi.Start();
        }

        public bool Stop(bool Reset = true)
        {
            Ensure();
            return BassWasapi.Stop(Reset);
        }

        #region Overrides
        public override bool Equals(object Obj) => Obj is WasapiDevice && DeviceIndex == ((WasapiDevice)Obj).DeviceIndex;

        public override string ToString()
        {
            return Info.Name
                + (Info.IsLoopback ? " (Loopback)" : string.Empty)
                + (Info.IsDefault ? " (Default)" : string.Empty);
        }

        public override int GetHashCode() => DeviceIndex;
        #endregion
    }
}