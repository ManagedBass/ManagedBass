using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    // TODO: Test BassAsio

    /// <summary>
    /// Wraps BassAsio: bassasio.dll
    /// </summary>
    public static class BassAsio
    {
        const string DllName = "bassasio.dll";

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        [DllImport(DllName, CharSet = CharSet.Ansi, EntryPoint = "BASS_ASIO_AddDevice")]
        public static extern int AddDevice(Guid clsid, string driver, string name);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_CheckRate")]
        public static extern bool CheckRate(double rate);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ControlPanel")]
        public static extern bool ControlPanel();

        [DllImport(DllName)]
        static extern Errors BASS_ASIO_ErrorGetCode();

        public static Errors LastError { get { return BASS_ASIO_ErrorGetCode(); } }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Free")]
        public static extern bool Free();

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Future")]
        public static extern bool Future(int selector, IntPtr param);

        [DllImport(DllName)]
        static extern float BASS_ASIO_GetCPU();

        public static double CPUUsage { get { return BASS_ASIO_GetCPU(); } }

        #region Current Device
        [DllImport(DllName)]
        static extern int BASS_ASIO_GetDevice();

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetDevice(int device);

        public static int CurrentDevice
        {
            get { return BASS_ASIO_GetDevice(); }
            set { if (!BASS_ASIO_SetDevice(value)) throw new Exception("Could not set device"); }
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetDeviceInfo")]
        public static extern bool GetDeviceInfo(int device, out AsioDeviceInfo info);

        public static AsioDeviceInfo GetDeviceInfo(int device)
        {
            AsioDeviceInfo info;
            GetDeviceInfo(device, out info);
            return info;
        }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetInfo")]
        public static extern bool GetInfo(out AsioInfo info);

        public static int DeviceCount
        {
            get
            {
                int i;
                AsioDeviceInfo info;

                for (i = 0; GetDeviceInfo(i, out info); ++i) ;

                return i;
            }
        }

        public static AsioInfo Info
        {
            get
            {
                AsioInfo info;
                GetInfo(out info);
                return info;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetLatency")]
        public static extern int GetLatency(bool input);

        #region Rate
        [DllImport(DllName)]
        static extern double BASS_ASIO_GetRate();

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetRate(double rate);

        public static double Rate
        {
            get { return BASS_ASIO_GetRate(); }
            set { if (!BASS_ASIO_SetRate(value)) throw new Exception("Could not set rate");}
        }
        #endregion

        [DllImport(DllName)]
        static extern int BASS_ASIO_GetVersion();

        public static Version Version { get { return Extensions.GetVersion(BASS_ASIO_GetVersion()); } }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Init")]
        public static extern bool Init(int device, AsioInitFlags flags);

        [DllImport(DllName)]
        static extern bool BASS_ASIO_IsStarted();

        public static bool IsStarted { get { return BASS_ASIO_IsStarted(); } }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Monitor")]
        public static extern bool Monitor(int input, int output, int gain, int state, int pan);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_SetDSD")]
        public static extern bool SetDSD(bool dsd);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_SetNotify")]
        public static extern bool SetNotify(AsioNotifyProcedure proc, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ASIO_SetUnicode")]
        public static extern bool SetUnicode(bool Unicode = true);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Start")]
        public static extern bool Start(int buflen = 0, int threads = 0);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Stop")]
        public static extern bool Stop();

        #region Channels
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnable")]
        public static extern bool ChannelEnable(bool input, int channel, AsioProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnableMirror")]
        public static extern bool ChannelEnableMirror(int channel, bool input2, int channel2);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetFormat")]
        public static extern AsioSampleFormat ChannelGetFormat(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetInfo")]
        public static extern bool ChannelGetInfo(bool input, int channel, out AsioChannelInfo info);

        public static AsioChannelInfo ChannelGetInfo(bool input, int channel)
        {
            AsioChannelInfo info;
            ChannelGetInfo(input, channel, out info);
            return info;
        }

        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetLevel(bool input, int channel);

        public static double ChannelGetLevel(bool input, int channel) { return BASS_ASIO_ChannelGetLevel(input, channel); }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetRate")]
        public static extern double ChannelGetRate(bool input, int channel);

        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetVolume(bool input, int channel);

        public static double ChannelGetVolume(bool input, int channel) { return BASS_ASIO_ChannelGetVolume(input, channel); }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelIsActive")]
        public static extern AsioChannelActive ChannelIsActive(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelJoin")]
        public static extern bool ChannelJoin(bool input, int channel, int channel2);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelPause")]
        public static extern bool ChannelPause(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelReset")]
        public static extern bool ChannelReset(bool input, int channel, AsioChannelResetFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetFormat")]
        public static extern bool ChannelSetFormat(bool input, int channel, AsioSampleFormat format);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetRate")]
        public static extern bool ChannelSetRate(bool input, int channel, double rate);

        [DllImport(DllName)]
        static extern bool BASS_ASIO_ChannelSetVolume(bool input, int channel, float volume);

        public static bool ChannelSetVolume(bool input, int channel, double volume)
        {
            return BASS_ASIO_ChannelSetVolume(input, channel, (float)volume);
        }
        #endregion
    }
}
