using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    public class RecordingDevice : IDisposable
    {
        public static RecordingDevice[] Devices
        {
            get
            {
                int NoOfDevices = Count;

                RecordingDevice[] Devices = new RecordingDevice[NoOfDevices];

                for (int i = 0; i < NoOfDevices; ++i) Devices[i] = new RecordingDevice(i);

                return Devices;
            }
        }

        public static int Count { get { return Bass.RecordingDeviceCount; } }

        DeviceInfo DeviceInfo { get { return Bass.RecordingDeviceInfo(DeviceId); } }

        public int DeviceId { get; protected set; }

        public string Name { get { return DeviceInfo.Name; } }

        public bool IsEnabled { get { return DeviceInfo.IsEnabled; } }

        public bool IsDefault { get { return DeviceInfo.IsDefault; } }

        public bool IsInitialized { get { return DeviceInfo.IsInitialized; } }

        RecordingDevice(int DeviceId) { this.DeviceId = DeviceId; }

        public Return<bool> Initialize() { return Bass.RecordInit(DeviceId); }

        public void Dispose()
        {
            Bass.CurrentRecordingDevice = DeviceId;
            Bass.RecordFree();
        }

        public static RecordingDevice DefaultDevice { get { return new RecordingDevice(0); } }
    }
}