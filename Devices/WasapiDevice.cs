using System;
using System.Collections.Generic;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public partial class WasapiDevice : IDisposable
    {
        internal static Dictionary<int, WasapiDevice> Singleton = new Dictionary<int, WasapiDevice>();

        static WasapiDevice Create(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                WasapiDevice Dev = new WasapiDevice() { DeviceIndex = Device, Mute = false };
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        #region Devices
        public static WasapiDevice[] Devices
        {
            get
            {
                List<WasapiDevice> Result = new List<WasapiDevice>();

                for (int i = 0; i < DeviceCount; ++i) Result.Add(Create(i));

                return Result.ToArray();
            }
        }

        public static int DeviceCount { get { return BassWasapi.DeviceCount; } }

        public static WasapiDevice DefaultDevice { get { return Devices[0]; } }
        #endregion

        #region Output Devices
        public static WasapiDevice[] OutputDevices
        {
            get
            {
                List<WasapiDevice> Result = new List<WasapiDevice>();

                foreach (WasapiDevice dev in Devices) if (!dev.IsInput) Result.Add(dev);

                return Result.ToArray();
            }
        }

        public static WasapiDevice DefaultOutputDevice { get { return OutputDevices[0]; } }

        public static int OutputDeviceCount { get { return OutputDevices.Length; } }
        #endregion

        #region Loopback Devices
        public static WasapiDevice[] LoopbackDevices
        {
            get
            {
                List<WasapiDevice> Result = new List<WasapiDevice>();

                foreach (WasapiDevice dev in Devices) if (dev.IsLoopback) Result.Add(dev);

                return Result.ToArray();
            }
        }

        public static WasapiDevice DefaultLoopbackDevice { get { return LoopbackDevices[0]; } }

        public static int LoopbackDeviceCount { get { return LoopbackDevices.Length; } }
        #endregion

        #region Recording Devices
        public static WasapiDevice[] RecordingDevices
        {
            get
            {
                List<WasapiDevice> Result = new List<WasapiDevice>();

                foreach (WasapiDevice dev in Devices) if (dev.IsInput && !dev.IsLoopback) Result.Add(dev);

                return Result.ToArray();
            }
        }

        public static WasapiDevice DefaultRecordingDevice { get { return RecordingDevices[0]; } }

        public static int RecordingDeviceCount { get { return RecordingDevices.Length; } }
        #endregion

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
        public int DeviceIndex { get; private set; }

        WasapiDeviceInfo DeviceInfo { get { return BassWasapi.DeviceInfo(DeviceIndex); } }

        public int Frequency { get { return DeviceInfo.mixfreq; } }

        public int NoOfChannels { get { return DeviceInfo.mixfreq; } }

        public bool IsEnabled { get { return DeviceInfo.IsEnabled; } }

        public string Name { get { return DeviceInfo.Name; } }

        public bool IsDefault { get { return DeviceInfo.IsDefault; } }

        public bool IsInitialized { get { return DeviceInfo.IsInitialized; } }

        public bool IsLoopback { get { return DeviceInfo.IsLoopback; } }

        public bool IsInput { get { return DeviceInfo.IsInput; } }
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
            if (Callback != null) Callback.Invoke(new BufferProvider(Buffer, Length, BufferKind.Float));
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
        void Read(object Buffer, int Length)
        {
            BassWasapi.CurrentDevice = DeviceIndex;

            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            BassWasapi.Read(gch.AddrOfPinnedObject(), Length);

            gch.Free();
        }

        public virtual void Read(byte[] Buffer, int Length) { Read(Buffer as object, Length); }

        public virtual byte[] ReadByte(int Length)
        {
            var Buffer = new byte[Length];

            Read(Buffer, Length);

            return Buffer;
        }

        public virtual void Read(float[] Buffer, int Length) { Read(Buffer as object, Length); }

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

        public bool Lock(bool State)
        {
            BassWasapi.CurrentDevice = DeviceIndex;
            return BassWasapi.Lock(State);
        }

        public bool Mute
        {
            get { return BassWasapi.GetMute(); }
            set { BassWasapi.SetMute(WasapiVolumeTypes.Device, value); }
        }

        public double Level { get { return BassWasapi.DeviceLevel(DeviceIndex); } }

        public double Volume
        {
            get { return BassWasapi.GetVolume(); }
            set { BassWasapi.SetVolume(WasapiVolumeTypes.Device, (float)value); }
        }

        public bool Init(int Frequency = 44100, int Channels = 2, bool Shared = true)
        {
            if (IsInitialized) return true;
            return BassWasapi.Initialize(DeviceIndex, Frequency, Channels, Shared ? WasapiInitFlags.Shared : WasapiInitFlags.Exclusive, proc: proc);
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

        public override string ToString() { return Name + (IsDefault ? " (Default)" : string.Empty); }

        public override int GetHashCode() { return DeviceIndex; }
        #endregion
    }
}