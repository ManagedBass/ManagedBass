using System;
using System.Collections.Generic;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Represents a WASAPI Device
    /// </summary>
    public abstract class WasapiDevice : IDisposable
    {
        internal static readonly Dictionary<int, WasapiDevice> Singleton = new Dictionary<int, WasapiDevice>();
        
        internal WasapiDevice(int Index)
        {
            _proc = OnProc;
            DeviceIndex = Index;
        }
        
        #region Device Info
        /// <summary>
        /// Gets the Index of the Wasapi Device.
        /// </summary>
        public int DeviceIndex { get; }

        /// <summary>
        /// Gets the Device Info.
        /// </summary>
        public WasapiDeviceInfo Info => BassWasapi.GetDeviceInfo(DeviceIndex);
        #endregion

        #region Callback
        readonly WasapiProcedure _proc;

        internal virtual int OnProc(IntPtr Buffer, int Length, IntPtr User)
        {
            Callback?.Invoke(Buffer, Length);
            return Length;
        }

        /// <summary>
        /// Wasapi Callback.
        /// </summary>
        public virtual event Action<IntPtr, int> Callback;
        #endregion

        internal void Ensure() => BassWasapi.CurrentDevice = DeviceIndex;

        /// <summary>
        /// Frees the Device.
        /// </summary>
        public void Dispose()
        {
            Ensure();
            Callback = null;
            BassWasapi.Free();
        }
        
        internal bool _Init(int Frequency, int Channels, bool Shared, bool UseEventSync, int Buffer, int Period)
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

        /// <summary>
        /// Gets whether the Device is started.
        /// </summary>
        public bool IsStarted
        {
            get
            {
                Ensure();
                return BassWasapi.IsStarted;
            }
        }

        /// <summary>
        /// Starts the device.
        /// </summary>
        public bool Start()
        {
            Ensure();
            return BassWasapi.Start();
        }

        /// <summary>
        /// Stops the device.
        /// </summary>
        public bool Stop(bool Reset = true)
        {
            Ensure();
            return BassWasapi.Stop(Reset);
        }

        #region Overrides
        /// <summary>
        /// Compares objects for Equality.
        /// </summary>
        public override bool Equals(object Obj) => Obj is WasapiDevice && DeviceIndex == ((WasapiDevice)Obj).DeviceIndex;

        /// <summary>
        /// Returns a string representation of the Wasapi Device.
        /// </summary>
        public override string ToString()
        {
            return Info.Name
                + (Info.IsLoopback ? " (Loopback)" : string.Empty)
                + (Info.IsDefault ? " (Default)" : string.Empty);
        }

        /// <summary>
        /// Returns the Device Index.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => DeviceIndex;
        #endregion
    }
}