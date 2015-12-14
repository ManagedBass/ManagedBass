using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static class BassWasapi
    {
        // TODO: BASS_WASAPI_GetLevelEx

        const string DllName = "basswasapi.dll";

        static BassWasapi() { BassManager.Load(DllName); }

        #region CPU
        [DllImport(DllName)]
        extern static float BASS_WASAPI_GetCPU();

        public static double CPUUsage { get { return BASS_WASAPI_GetCPU(); } }
        #endregion

        #region Current Device
        [DllImport(DllName)]
        extern static int BASS_WASAPI_GetDevice();

        [DllImport(DllName)]
        extern static bool BASS_WASAPI_SetDevice(int device);

        public static int CurrentDevice
        {
            get { return BASS_WASAPI_GetDevice(); }
            set { BASS_WASAPI_SetDevice(value); }
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetDeviceInfo")]
        public extern static bool GetDeviceInfo(int device, out WasapiDeviceInfo info);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_SetNotify")]
        public extern static bool SetNotify(WasapiNotifyProcedure proc, IntPtr User = default(IntPtr));

        public static WasapiDeviceInfo GetDeviceInfo(int Device)
        {
            WasapiDeviceInfo Temp;
            GetDeviceInfo(Device, out Temp);
            return Temp;
        }

        public static int DeviceCount
        {
            get
            {
                int Count = 0;
                WasapiDeviceInfo info = new WasapiDeviceInfo();

                for (int i = 0; GetDeviceInfo(i, out info); i++)
                    if (info.IsEnabled) Count++;

                return Count;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_CheckFormat")]
        public extern static WasapiFormat CheckFormat(int device, int freq, int chans, WasapiInitFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetInfo")]
        public extern static bool GetInfo(out WasapiInfo info);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Free")]
        public extern static bool Free();

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetData")]
        public extern static int Read(IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_PutData")]
        public extern static int Write(IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Lock")]
        public extern static bool Lock(bool State);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetMute")]
        public extern static bool GetMute(WasapiVolumeTypes mode = WasapiVolumeTypes.Device);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_SetMute")]
        public extern static bool SetMute(WasapiVolumeTypes mode, bool mute);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetDeviceLevel")]
        public extern static float GetDeviceLevel(int device, int chan = -1);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetVolume")]
        public extern static float GetVolume(WasapiVolumeTypes curve = WasapiVolumeTypes.Device);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_SetVolume")]
        public extern static bool SetVolume(WasapiVolumeTypes curve, float volume);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Init")]
        public extern static bool Init(int device,
                                        int freq,
                                        int chans,
                                        WasapiInitFlags flags,
                                        float buffer = 0,
                                        float period = 0,
                                        WasapiProcedure proc = null,
                                        IntPtr User = default(IntPtr));

        #region IsStarted
        [DllImport(DllName)]
        extern static bool BASS_WASAPI_IsStarted();

        public static bool IsStarted { get { return BASS_WASAPI_IsStarted(); } }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Start")]
        public extern static bool Start();

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Stop")]
        public extern static bool Stop(bool Reset = true);
        
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetLevel")]
        public extern static int GetLevel();

        [DllImport(DllName)]
        extern static int BASS_WASAPI_GetVersion();

        public static int Version { get { return BASS_WASAPI_GetVersion(); } }
    }
}