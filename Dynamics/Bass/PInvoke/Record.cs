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
        public static extern bool GetRecordingDeviceInfo(int Device, out DeviceInfo info);

        public static DeviceInfo GetRecordingDeviceInfo(int Device)
        {
            DeviceInfo temp;
            GetRecordingDeviceInfo(Device, out temp);
            return temp;
        }
        #endregion

        #region Record Get Info
        [DllImport(DllName, EntryPoint = "BASS_RecordGetInfo")]
        public static extern bool GetRecordingInfo(out RecordInfo info);

        public static RecordInfo RecordingInfo
        {
            get
            {
                RecordInfo temp;
                GetRecordingInfo(out temp);
                return temp;
            }
        }
        #endregion

        /// <summary>
        /// The buffer length for recording channels.
        /// length (int): The buffer length in milliseconds... 1000 (min) - 5000 (max).
        /// If the length specified is outside this range, it is automatically capped.
        /// Unlike a playback buffer, where the aim is to keep the buffer full, a recording
        /// buffer is kept as empty as possible and so this setting has no effect on latency.
        /// The default recording buffer length is 2000 milliseconds. 
        /// Unless processing of the recorded data could cause significant delays, or you want to 
        /// use a large recording period with Bass.StartRecording(), there should be no need to increase this.
        /// Using this config option only affects the recording channels that are created afterwards, 
        /// not any that have already been created. 
        /// So you can have channels with differing buffer lengths by using this config option each time before creating them.
        /// </summary>
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

                for (int i = 0; GetRecordingDeviceInfo(i, out info); i++)
                    if (info.IsEnabled) ++Count;

                return Count;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_RecordGetInput")]
        public extern static int RecordGetInput(int input, ref float volume);

        [DllImport(DllName, EntryPoint = "BASS_RecordGetInputName")]
        public extern static string RecordGetInputName(int input);

        [DllImport(DllName, EntryPoint = "BASS_RecordSetInput")]
        public extern static bool RecordSetInput(int input, InputFlags setting, float volume);
    }
}