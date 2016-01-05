using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName)]
        static extern bool BASS_Init(int Device, int Frequency, DeviceInitFlags Flags, IntPtr hParent = default(IntPtr), IntPtr ClsID = default(IntPtr));

        public static Return<bool> Initialize(int Device = DefaultDevice, int Frequency = 44100, DeviceInitFlags Flags = DeviceInitFlags.Default)
        {
            return BASS_Init(Device, Frequency, Flags);
        }

        #region Free
        [DllImport(DllName)]
        static extern bool BASS_Free();

        public static Return<bool> Free(int Device) { return BASS_SetDevice(Device) && BASS_Free(); }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetDevice")]
        public static extern int ChannelGetDevice(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetDevice")]
        public static extern bool ChannelSetDevice(int Handle, int Device);

        public static int DeviceCount
        {
            get
            {
                int i;
                DeviceInfo info;

                for (i = 0; GetDeviceInfo(i, out info); i++) ;

                return i;
            }
        }

        #region Current Device Volume
        [DllImport(DllName)]
        extern static float BASS_GetVolume();

        [DllImport(DllName)]
        extern static bool BASS_SetVolume(float volume);

        public static double Volume
        {
            get { return BASS_GetVolume(); }
            set { BASS_SetVolume((float)value); }
        }
        #endregion

        #region Current Device
        [DllImport(DllName)]
        extern static int BASS_GetDevice();

        [DllImport(DllName)]
        extern static bool BASS_SetDevice(int Device);

        public static int CurrentDevice
        {
            get { return BASS_GetDevice(); }
            set { BASS_SetDevice(value); }
        }
        #endregion

        #region Get Device Info
        [DllImport(DllName, EntryPoint = "BASS_GetDeviceInfo")]
        public static extern bool GetDeviceInfo(int Device, out DeviceInfo Info);

        public static DeviceInfo GetDeviceInfo(int Device)
        {
            DeviceInfo temp;
            GetDeviceInfo(Device, out temp);
            return temp;
        }
        #endregion
    }
}