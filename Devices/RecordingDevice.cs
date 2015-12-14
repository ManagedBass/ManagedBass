using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    public class RecordingDevice : BassDevice
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

        protected override DeviceInfo DeviceInfo { get { return Bass.GetRecordingDeviceInfo(DeviceId); } }

        RecordingDevice(int DeviceId) : base(DeviceId) { }

        public Return<bool> Initialize() { return Bass.RecordInit(DeviceId); }

        public override void Dispose()
        {
            Bass.CurrentRecordingDevice = DeviceId;
            Bass.RecordFree();
        }

        public static RecordingDevice DefaultDevice { get { return new RecordingDevice(0); } }
    }
}