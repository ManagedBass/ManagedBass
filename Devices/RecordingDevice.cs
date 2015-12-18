using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;

namespace ManagedBass
{
    public class RecordingDevice : BassDevice
    {
        public static IEnumerable<RecordingDevice> Devices
        {
            get
            {
                DeviceInfo info;

                for (int i = 0; Bass.GetRecordingDeviceInfo(i, out info); ++i)
                    if (info.IsEnabled)
                        yield return new RecordingDevice(i);
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