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

        protected virtual int OnProc(IntPtr Buffer, int Length, IntPtr User)
        {
            Callback?.Invoke(Buffer, Length);
            return Length;
        }

        public virtual event Action<IntPtr, int> Callback;
        #endregion

        protected void Ensure() => BassWasapi.CurrentDevice = DeviceIndex;

        public void Dispose()
        {
            Ensure();
            Callback = null;
            BassWasapi.Free();
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