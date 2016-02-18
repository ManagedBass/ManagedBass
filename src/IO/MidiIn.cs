using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    // TODO: Won't work most probably
    class MidiIn : IDisposable
    {
        public int DeviceId { get; }

        public MidiIn(int DeviceId)
        {
            this.DeviceId = DeviceId;

            DeviceName = BassMidi.InGetDeviceInfo(DeviceId).Name;

            BassMidi.InInit(DeviceId, Callback);
        }

        public bool Start() => BassMidi.InStart(DeviceId);

        public bool Stop() => BassMidi.InStop(DeviceId);

        public string DeviceName { get; }

        byte[] data;

        unsafe void Callback(int device, double time, IntPtr buffer, int length, IntPtr user)
        {
            if (data == null || data.Length < length)
                data = new byte[length];

            Marshal.Copy(buffer, data, 0, length);
         
            MessageReceived?.Invoke(data, length);
        }

        public event Action<byte[], int> MessageReceived;

        public void Dispose() => BassMidi.InFree(DeviceId);
    }
}
