using System;
using System.Collections.Generic;
using System.Linq;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public abstract class WasapiDevice : IDisposable
    {
        #region Notifier Statics
        static Dictionary<int, WasapiNotifier> notifyprocs = new Dictionary<int, WasapiNotifier>();

        class WasapiNotifier
        {
            WasapiNotifyProcedure proc;

            public WasapiNotifier(int Device)
            {
                proc = new WasapiNotifyProcedure(OnNotify);

                WasapiDevice.SetNotify(Device, proc);
            }

            public event Action Receivers;

            public void OnNotify(WasapiNotificationType notify, int device, IntPtr User) { if (Receivers != null) Receivers.Invoke(); }
        }

        static bool SetNotify(int Device, WasapiNotifyProcedure proc)
        {
            BassWasapi.CurrentDevice = Device;
            return BassWasapi.SetNotify(proc);
        }

        static void RegisterNotifier(int Device, Action receiver)
        {
            if (notifyprocs.ContainsKey(Device)) notifyprocs[Device].Receivers += receiver;
            else
            {
                WasapiNotifier notifier = new WasapiNotifier(Device);
                notifier.Receivers += receiver;

                notifyprocs.Add(Device, notifier);
            }
        }

        static void UnregisterNotifier(int Device, Action receiver)
        {
            if (notifyprocs.ContainsKey(Device)) notifyprocs[Device].Receivers -= receiver;
        }
        #endregion

        protected WasapiDevice() { proc = new WasapiProcedure(OnProc); }

        #region Device Info
        public int DeviceIndex { get; protected set; }

        public WasapiDeviceInfo DeviceInfo { get { return BassWasapi.GetDeviceInfo(DeviceIndex); } }
        #endregion

        #region Notify
        List<Action> Notifiers = new List<Action>();

        public event Action StateChanged
        {
            add
            {
                RegisterNotifier(DeviceIndex, value);
                Notifiers.Add(value);
            }
            remove
            {
                UnregisterNotifier(DeviceIndex, value);
                Notifiers.Remove(value);
            }
        }
        #endregion

        #region Callback
        WasapiProcedure proc;

        public int OnProc(IntPtr Buffer, int Length, IntPtr User)
        {
            if (Callback != null) Callback.Invoke(new BufferProvider(Buffer, Length, Resolution.Float));
            return Length;
        }

        public event Action<BufferProvider> Callback;
        #endregion

        public void Dispose()
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            BassWasapi.Free();
        }

        #region Read
        int Read(object Buffer, int Length)
        {
            BassWasapi.CurrentDevice = DeviceIndex;

            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            int Return = BassWasapi.Read(gch.AddrOfPinnedObject(), Length);

            gch.Free();

            return Return;
        }

        public virtual int Read(byte[] Buffer, int Length) { return Read(Buffer as object, Length); }

        public virtual byte[] ReadByte(int Length)
        {
            var Buffer = new byte[Length];

            Read(Buffer, Length);

            return Buffer;
        }

        public virtual int Read(float[] Buffer, int Length) { return Read(Buffer as object, Length); }

        public virtual float[] ReadFloat(int Length)
        {
            var Buffer = new float[Length / 4];

            Read(Buffer, Length);

            return Buffer;
        }
        #endregion

        #region Write
        public void Write(byte[] Buffer, int Length) { Write(Buffer as object, Length); }

        public void Write(float[] Buffer, int Length) { Write(Buffer as object, Length); }

        void Write(object Buffer, int Length)
        {
            BassWasapi.CurrentDevice = DeviceIndex;

            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            BassWasapi.Write(gch.AddrOfPinnedObject(), Length);
            gch.Free();
        }
        #endregion

        #region Lock
        public bool Lock()
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Lock(true);
        }

        public bool Unlock()
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Lock(false);
        }
        #endregion

        public bool Mute
        {
            get { return BassWasapi.GetMute(); }
            set { BassWasapi.SetMute(WasapiVolumeTypes.Device, value); }
        }

        public double Level { get { return BassWasapi.GetDeviceLevel(DeviceIndex); } }

        public double Volume
        {
            get { return BassWasapi.GetVolume(); }
            set { BassWasapi.SetVolume(WasapiVolumeTypes.Device, (float)value); }
        }

        public bool Init(int Frequency = 44100, int Channels = 2, bool Shared = true)
        {
            if (DeviceInfo.IsInitialized) return true;
            return BassWasapi.Init(DeviceIndex, Frequency, Channels, Shared ? WasapiInitFlags.Shared : WasapiInitFlags.Exclusive, proc: proc);
        }

        public bool IsStarted
        {
            get
            {
                BassWasapi.CurrentDevice = DeviceIndex;
                return BassWasapi.IsStarted;
            }
        }

        public bool Start()
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Start();
        }

        public bool Stop(bool Reset = true)
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Stop(Reset);
        }

        #region Overrides
        public override bool Equals(object obj) { return (obj is WasapiDevice) && (DeviceIndex == ((WasapiDevice)obj).DeviceIndex); }

        public override string ToString()
        {
            return DeviceInfo.Name
                + (DeviceInfo.IsLoopback ? " (Loopback)" : string.Empty)
                + (DeviceInfo.IsDefault ? " (Default)" : string.Empty);
        }

        public override int GetHashCode() { return DeviceIndex; }
        #endregion
    }
}