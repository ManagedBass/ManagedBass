using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_RecordInit")]
        public extern static bool RecordInit(int Device);

        [DllImport(DllName, EntryPoint = "BASS_RecordFree")]
        public extern static bool RecordFree();

        [DllImport(DllName, EntryPoint = "BASS_RecordStart")]
        public extern static int StartRecording(int freq, int chans, BassFlags flags, RecordProcedure proc, IntPtr User = default(IntPtr));

        #region Current Recording Device
        [DllImport(DllName)]
        extern static int BASS_RecordGetDevice();

        [DllImport(DllName)]
        extern static bool BASS_RecordSetDevice(int Device);

        public static int CurrentRecordingDevice
        {
            get { return BASS_RecordGetDevice(); }
            set { BASS_RecordSetDevice(value); }
        }
        #endregion

        #region Record Get Device Info
        [DllImport(DllName, EntryPoint = "BASS_RecordGetDeviceInfo")]
        public static extern bool RecordingDeviceInfo(int Device, out DeviceInfo info);

        public static DeviceInfo RecordingDeviceInfo(int Device)
        {
            DeviceInfo temp;
            RecordingDeviceInfo(Device, out temp);
            return temp;
        }
        #endregion

        public static int RecordingBufferLength
        {
            get { return GetConfig(Configuration.RecordingBufferLength); }
            set { Configure(Configuration.RecordingBufferLength, value); }
        }

        public static int RecordingDeviceCount
        {
            get
            {
                int Count = 0;
                DeviceInfo info;

                for (int i = 0; RecordingDeviceInfo(i, out info); i++)
                    if (info.IsEnabled) ++Count;

                return Count;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_RecordGetInput")]
        public extern static int RecordGetInput(int input, ref float volume);

        [DllImport(DllName, EntryPoint = "BASS_RecordSetInput")]
        public extern static bool RecordSetInput(int input, InputFlags setting, float volume);
    }
}