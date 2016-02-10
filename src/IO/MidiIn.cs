using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    class MidiIn : IDisposable
    {
        public int DeviceId { get; private set; }

        public MidiIn(int DeviceId)
        {
            this.DeviceId = DeviceId;

            DeviceName = BassMidi.InGetDeviceInfo(DeviceId).Name;

            BassMidi.InInit(DeviceId, Callback);
        }

        public bool Start() { return BassMidi.InStart(DeviceId); }

        public bool Stop() { return BassMidi.InStop(DeviceId); }

        public string DeviceName { get; private set; }

        void Callback(int device, double time, IntPtr buffer, int length, IntPtr user)
        {
            var b = new byte[length];

            Marshal.Copy(buffer, b, 0, length);

            if (MessageReceived != null)
                MessageReceived(b);
        }

        // TODO: Convert Raw Midi data to MidiEvent
        public event Action<byte[]> MessageReceived;

        public void Dispose() { BassMidi.InFree(DeviceId); }
    }
}
