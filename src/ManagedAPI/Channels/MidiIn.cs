using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    public class MidiIn : IDisposable
    {
        public int DeviceId { get; }

        public string DeviceName { get; }

        public MidiIn(int DeviceId)
        {
            this.DeviceId = DeviceId;

            DeviceName = BassMidi.InGetDeviceInfo(DeviceId).Name;

            BassMidi.InInit(DeviceId, Callback);
        }

        public bool Start() => BassMidi.InStart(DeviceId);

        public bool Stop() => BassMidi.InStop(DeviceId);

        byte[] _data;

        void Callback(int device, double time, IntPtr buffer, int length, IntPtr user)
        {
            if (_data == null || _data.Length < length)
                _data = new byte[length];

            Marshal.Copy(buffer, _data, 0, length);
         
            MessageReceived?.Invoke(_data, length);
        }

        public event Action<byte[], int> MessageReceived;

        public void Dispose() => BassMidi.InFree(DeviceId);
    }
}
