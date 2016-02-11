using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    // TODO: Won't work most probably
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

        MidiEvent[] data;
        static readonly int sMidiEvent = Marshal.SizeOf(typeof(MidiEvent));

        unsafe void Callback(int device, double time, IntPtr buffer, int length, IntPtr user)
        {
            int count = length / sMidiEvent;

            if (data == null || data.Length < count)
                data = new MidiEvent[count];
            
            var ptr = (MidiEvent*)buffer;

            for (int i = 0; i < count; ++i)
                data[i] = *(ptr + i);
         
            if (MessageReceived != null)
                MessageReceived(data, count);
        }

        public event Action<MidiEvent[], int> MessageReceived;

        public void Dispose() { BassMidi.InFree(DeviceId); }
    }
}
