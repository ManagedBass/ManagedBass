using System;

namespace ManagedBass.Dynamics
{
    public abstract class BassDevice : IDisposable
    {
        public abstract void Dispose();

        protected abstract DeviceInfo DeviceInfo { get; }

        public int DeviceIndex { get; protected set; }

        public string Name { get { return DeviceInfo.Name; } }

        public bool IsEnabled { get { return DeviceInfo.IsEnabled; } }

        public bool IsDefault { get { return DeviceInfo.IsDefault; } }

        public bool IsInitialized { get { return DeviceInfo.IsInitialized; } }
    }
}