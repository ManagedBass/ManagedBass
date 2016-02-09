using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps basswasapi.dll: Windows Audio Session API driver library
    /// </summary>
    /// <remarks>
    /// <para>BASSWASAPI is basically a wrapper for Windows Audio Session API drivers, with the addition of channel joining, format conversion and resampling.</para>
    /// <para>
    /// BASSWASAPI requires a soundcard with a Windows Session API drivers installed (Vista or above).
    /// It also makes use of SSE2 and 3DNow optimizations, but is fully functional without them.
    /// BASS is not required by BASSWASAPI, but BASS can of course be used to decode, apply DSP/FX, etc.
    /// </para>
    /// </remarks>
    public static class BassWasapi
    {
        public const int DefaultDevice = -1,
                         DefaultInputDevice = -2,
                         DefaultLoopbackDevice = -3;

        const string DllName = "basswasapi";

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        #region CPU
        [DllImport(DllName)]
        extern static float BASS_WASAPI_GetCPU();

        /// <summary>
        /// Retrieves the current CPU usage of BASSWASAPI.
        /// </summary>
        /// <returns>The BASSWASAPI CPU usage as a percentage of total CPU time.</returns>
        /// <remarks>This function includes the time taken by the <see cref="WasapiProcedure" /> callback functions.</remarks>
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
                WasapiDeviceInfo info = new WasapiDeviceInfo();

                int i;

                for (i = 0; GetDeviceInfo(i, out info); i++) ;

                return i;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_CheckFormat")]
        public extern static WasapiFormat CheckFormat(int device, int freq, int chans, WasapiInitFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetInfo")]
        public extern static bool GetInfo(out WasapiInfo info);

        public static WasapiInfo Info
        {
            get
            {
                WasapiInfo info;
                GetInfo(out info);
                return info;
            }
        }

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
        public extern static bool Init(int Device,
                                        int Frequency = 0,
                                        int Channels = 0,
                                        WasapiInitFlags Flags = WasapiInitFlags.Shared,
                                        float Buffer = 0,
                                        float Period = 0,
                                        WasapiProcedure Procedure = null,
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

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetLevelEx")]
        public static extern int GetLevel(float[] Levels, float Length, LevelRetrievalFlags Flags);

        [DllImport(DllName)]
        extern static int BASS_WASAPI_GetVersion();

        public static Version Version { get { return Extensions.GetVersion(BASS_WASAPI_GetVersion()); } }
    }
}
