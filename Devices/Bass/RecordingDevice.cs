using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;

namespace ManagedBass
{
    public class RecordingDevice : BassDevice
    {
        static Dictionary<int, RecordingDevice> Singleton = new Dictionary<int, RecordingDevice>();

        RecordingDevice() { }

        static RecordingDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                var Dev = new RecordingDevice() { DeviceIndex = Device };
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        public static IEnumerable<RecordingDevice> Devices
        {
            get
            {
                DeviceInfo info;

                for (int i = 0; Bass.GetRecordingDeviceInfo(i, out info); ++i)
                    if (info.IsEnabled)
                        yield return Get(i);
            }
        }

        public static int Count { get { return Bass.RecordingDeviceCount; } }

        protected override DeviceInfo DeviceInfo { get { return Bass.GetRecordingDeviceInfo(DeviceIndex); } }
        
        public Return<bool> Initialize() { return Bass.RecordInit(DeviceIndex); }

        public override void Dispose()
        {
            Bass.CurrentRecordingDevice = DeviceIndex;
            Bass.RecordFree();
        }

        public static RecordingDevice DefaultDevice { get { return Get(0); } }
    }
}