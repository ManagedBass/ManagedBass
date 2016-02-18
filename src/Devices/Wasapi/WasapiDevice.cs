using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a WASAPI Device
    /// </summary>
    public abstract class WasapiDevice : IDisposable
    {
        protected static Dictionary<int, WasapiDevice> Singleton = new Dictionary<int, WasapiDevice>();

        #region Notify
        static WasapiNotifyProcedure notifyproc = new WasapiNotifyProcedure(OnNotify);

        static void OnNotify(WasapiNotificationType notify, int device, IntPtr User)
        {
            Singleton[device]._StateChanged?.Invoke(notify);
        }

        bool notifyRegistered = false;
        event Action<WasapiNotificationType> _StateChanged;

        public event Action<WasapiNotificationType> StateChanged
        {
            add
            {
                if (!notifyRegistered)
                {
                    BassWasapi.CurrentDevice = DeviceIndex;
                    notifyRegistered = BassWasapi.SetNotify(notifyproc);
                }

                _StateChanged += value;
            }
            remove
            {
                _StateChanged -= value;

                if (_StateChanged == null
                    && notifyRegistered)
                {
                    BassWasapi.CurrentDevice = DeviceIndex;
                    notifyRegistered = !BassWasapi.SetNotify(null);
                }
            }
        }
        #endregion

        protected WasapiDevice(int Index)
        {
            proc = new WasapiProcedure(OnProc);
            DeviceIndex = Index;
        }

        #region Device Info
        public int DeviceIndex { get; }

        public WasapiDeviceInfo DeviceInfo => BassWasapi.GetDeviceInfo(DeviceIndex);
        #endregion

        #region Callback
        WasapiProcedure proc;

        public virtual int OnProc(IntPtr Buffer, int Length, IntPtr User)
        {
            Callback?.Invoke(new BufferProvider(Buffer, Length));
            return Length;
        }

        public virtual event Action<BufferProvider> Callback;
        #endregion

        public void Dispose()
        {
            BassWasapi.CurrentDevice = DeviceIndex;
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
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Lock(Lock);
        }

        public bool Mute
        {
            get
            {
                BassWasapi.CurrentDevice = DeviceIndex;
                return BassWasapi.GetMute();
            }
            set
            {
                BassWasapi.CurrentDevice = DeviceIndex;
                BassWasapi.SetMute(WasapiVolumeTypes.Device, value);
            }
        }

        public double Level => BassWasapi.GetDeviceLevel(DeviceIndex);

        public double Volume
        {
            get
            {
                BassWasapi.CurrentDevice = DeviceIndex;
                return BassWasapi.GetVolume(WasapiVolumeTypes.Device | WasapiVolumeTypes.LinearCurve);
            }
            set
            {
                BassWasapi.CurrentDevice = DeviceIndex;
                BassWasapi.SetVolume(WasapiVolumeTypes.Device | WasapiVolumeTypes.LinearCurve, (float)value);
            }
        }

        protected bool _Init(int Frequency, int Channels, bool Shared, bool UseEventSync, int Buffer, int Period)
        {
            if (DeviceInfo.IsInitialized) return true;

            var flags = Shared ? WasapiInitFlags.Shared : WasapiInitFlags.Exclusive;
            if (UseEventSync) flags |= WasapiInitFlags.EventDriven;

            bool Result = BassWasapi.Init(DeviceIndex,
                                          Frequency,
                                          Channels,
                                          flags,
                                          Buffer,
                                          Period,
                                          proc);

            return Result;
        }

        public bool IsStarted
        {
            get
            {
                BassWasapi.CurrentDevice = DeviceIndex;
                return BassWasapi.IsStarted;
            }
        }

        public bool Start()
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Start();
        }

        public bool Stop(bool Reset = true)
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Stop(Reset);
        }

        #region Overrides
        public override bool Equals(object obj) => (obj is WasapiDevice) && (DeviceIndex == ((WasapiDevice)obj).DeviceIndex);

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
