using System;
using System.IO;
using System.Runtime.InteropServices;
using ManagedBass.Effects;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        public const int NoSoundDevice = 0, DefaultDevice = -1;

        const string DllName = "bass.dll";

        static Bass() { BassManager.LoadBass(); }

        /* To Wrap:
         * 
         * 3D * 
         * BASS_Apply3D
         * BASS_ChannelGet3DAttributes
         * BASS_ChannelGet3DPosition
         * BASS_ChannelSet3DAttributes
         * BASS_ChannelSet3DPosition
         * BASS_Get3DFactors
         * BASS_Get3DPosition
         * 
         * BASS_ChannelUpdate
         * BASS_FXReset
         * BASS_GetEAXParameters
         * BASS_GetInfo
         * BASS_GetVersion
         * BASS_Pause
         * BASS_PluginFree
         * BASS_PluginGetInfo
         * BASS_PluginLoad
         * BASS_PluginLoadDirectory
         * BASS_RecordGetInfo
         * BASS_SampleSetInfo
         * BASS_SetEAXParameters
         * BASS_Start
         * BASS_Stop
         * BASS_Update
         */

        #region Devices
        [DllImport(DllName)]
        static extern bool BASS_Init(int Device, int Frequency, DeviceInitFlags Flags, IntPtr hParent = default(IntPtr), IntPtr ClsID = default(IntPtr));

        public static Return<bool> Initialize(int Device, int Frequency = 44100, DeviceInitFlags Flags = DeviceInitFlags.Default)
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
                int Count = 0;
                DeviceInfo info;

                for (int i = 0; DeviceInfo(i, out info); i++)
                    if (info.IsEnabled) ++Count;

                return Count;
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
        public static extern bool DeviceInfo(int Device, out DeviceInfo Info);

        public static DeviceInfo DeviceInfo(int Device)
        {
            DeviceInfo temp;
            DeviceInfo(Device, out temp);
            return temp;
        }
        #endregion
        #endregion

        #region Streams
        [DllImport(DllName, EntryPoint = "BASS_StreamGetFilePosition")]
        public static extern long StreamGetFilePosition(int Handle, FileStreamPosition Mode = FileStreamPosition.Current);

        [DllImport(DllName)]
        static extern int BASS_StreamCreateFile(bool Memory, [MarshalAs(UnmanagedType.LPWStr)]string File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName)]
        static extern int BASS_StreamCreateFile(bool Memory, IntPtr File, long Offset, long Length, BassFlags Flags);

        public static int CreateStream(string File, long Offset, long Length, BassFlags Flags)
        {
            return BASS_StreamCreateFile(false, File, Offset, Length, Flags);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags)
        {
            return BASS_StreamCreateFile(true, Memory, Offset, Length, Flags);
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            return Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);
        }

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags);
        }

        [DllImport(DllName, EntryPoint = "BASS_StreamCreateURL")]
        public static extern int CreateStream([MarshalAs(UnmanagedType.LPWStr)]string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedureType Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, IntPtr Buffer, int Length);
        
        [DllImport(DllName, EntryPoint = "BASS_StreamFree")]
        public static extern bool StreamFree(int Handle);
        #endregion

        #region Channels
        [DllImport(DllName, EntryPoint = "BASS_ChannelGetInfo")]
        public static extern bool ChannelInfo(int Device, out ChannelInfo Info);

        public static ChannelInfo ChannelInfo(int Device)
        {
            ChannelInfo temp;
            ChannelInfo(Device, out temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetDSP")]
        public static extern int ChannelSetDSP(int Handle, DSPProcedure Procedure, IntPtr User = default(IntPtr), int Priority = 0);

        [DllImport(DllName, EntryPoint = "BASS_ChannelPlay")]
        public static extern bool PlayChannel(int Handle, bool Restart = false);

        [DllImport(DllName, EntryPoint = "BASS_ChannelPause")]
        public static extern bool PauseChannel(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelStop")]
        public static extern bool StopChannel(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelLock")]
        public extern static bool LockChannel(int Handle, bool Lock);

        [DllImport(DllName, EntryPoint = "BASS_ChannelIsActive")]
        public extern static PlaybackState IsChannelActive(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetLink")]
        public extern static bool LinkChannels(int Handle, int Channel);

        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveLink")]
        public extern static bool ChannelRemoveLink(int Handle, int Channel);

        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveDSP")]
        public extern static bool ChannelRemoveDSP(int Handle, int DSP);

        #region Channel Flags
        [DllImport(DllName)]
        extern static BassFlags BASS_ChannelFlags(int Handle, BassFlags Flags, BassFlags Mask);

        public static bool ChannelHasFlag(int Handle, BassFlags Flag) { return BASS_ChannelFlags(Handle, 0, 0).HasFlag(Flag); }

        public static bool ChannelAddFlag(int Handle, BassFlags Flag) { return BASS_ChannelFlags(Handle, Flag, Flag).HasFlag(Flag); }

        public static bool ChannelRemoveFlag(int Handle, BassFlags Flag) { return !(BASS_ChannelFlags(Handle, 0, Flag).HasFlag(Flag)); }
        #endregion

        #region Channel Attributes
        [DllImport(DllName)]
        extern static bool BASS_ChannelGetAttribute(int Handle, ChannelAttribute attrib, ref float value);

        public static double GetChannelAttribute(int Handle, ChannelAttribute attrib)
        {
            float temp = 0;
            BASS_ChannelGetAttribute(Handle, attrib, ref temp);
            return temp;
        }

        [DllImport(DllName)]
        extern static bool BASS_ChannelSetAttribute(int Handle, ChannelAttribute attrib, float value);

        public static bool SetChannelAttribute(int Handle, ChannelAttribute attrib, double value)
        {
            return BASS_ChannelSetAttribute(Handle, attrib, (float)value);
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetTags")]
        public extern static IntPtr GetChannelTags(int Handle, TagType Tags);

        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveFX")]
        public extern static bool ChannelRemoveFX(int Handle, int FX);

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetLength")]
        public extern static long ChannelGetLength(int Handle, PositionFlags Mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetFX")]
        public extern static int ChannelSetFX(int Handle, EffectType Type, int Priority);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetSync")]
        public static extern int ChannelSetSync(int Handle, SyncFlags Type, long Param, SyncProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ChannelBytes2Seconds")]
        public extern static double ChannelBytes2Seconds(int Handle, long Position);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSeconds2Bytes")]
        public extern static long ChannelSeconds2Bytes(int Handle, double Position);

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetPosition")]
        public extern static long GetChannelPosition(int Handle, PositionFlags mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetPosition")]
        public extern static bool SetChannelPosition(int Handle, long pos, PositionFlags mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_ChannelIsSliding")]
        public extern static bool IsChannelSliding(int Handle, ChannelAttribute attrib);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSlideAttribute")]
        public extern static bool SlideChannelAttribute(int Handle, ChannelAttribute attrib, float value, int time);

        #region Channel Get Level
        [DllImport(DllName)]
        static extern int BASS_ChannelGetLevel(int Handle);

        public static double GetChannelLevel(int Channel)
        {
            int Temp = BASS_ChannelGetLevel(Channel);
            return (Temp.LoWord() + Temp.HiWord()) / (2.0 * 32768);
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, IntPtr Buffer, int Length);
        #endregion

        #region FX Parameters
        [DllImport(DllName)]
        static extern bool BASS_FXSetParameters(int Handle, [MarshalAs(UnmanagedType.Struct)] IEffectParameter param);

        public static bool SetFXParameters(int Handle, IEffectParameter Parameters)
        {
            try { return Bass.BASS_FXSetParameters(Handle, Parameters); }
            catch (MarshalDirectiveException) { return true; }
        }

        [DllImport(DllName)]
        static extern bool BASS_FXGetParameters(int Handle, [MarshalAs(UnmanagedType.Struct)] IEffectParameter param);

        public static bool GetFXParameters(int Handle, IEffectParameter Param)
        {
            try { return BASS_FXGetParameters(Handle, Param); }
            catch { return true; }
        }
        #endregion

        #region Config
        [DllImport(DllName, EntryPoint = "BASS_SetConfig")]
        public extern static bool Configure(Configuration option, bool newvalue);

        [DllImport(DllName, EntryPoint = "BASS_SetConfig")]
        public extern static bool Configure(Configuration option, int newvalue);

        [DllImport(DllName, EntryPoint = "BASS_GetConfig")]
        public extern static int GetConfig(Configuration option);

        public static bool GetConfigBool(Configuration option) { return GetConfig(option) == 1; }

        /// <summary>
        /// The buffer length in milliseconds. The minimum length is 1ms
        /// above the update period (See <see cref="UpdatePeriod"/>),
        /// the maximum is 5000 milliseconds. If the length specified is outside this
        /// range, it is automatically capped.
        /// The default buffer length is 500 milliseconds. Increasing the length, decreases
        /// the chance of the sound possibly breaking-up on slower computers, but also
        /// increases the latency for DSP/FX.
        /// Small buffer lengths are only required if the sound is going to be changing
        /// in real-time, for example, in a soft-synth. If you need to use a small buffer,
        /// then the minbuf member of BASS_INFO should be used to get the recommended
        /// minimum buffer length supported by the device and it's drivers. Even at this
        /// default length, it's still possible that the sound could break up on some
        /// systems, it's also possible that smaller buffers may be fine. So when using
        /// small buffers, you should have an option in your software for the user to
        /// finetune the length used, for optimal performance.
        /// Using this config option only affects the HMUSIC/HSTREAM channels that you
        /// create afterwards, not the ones that have already been created. So you can
        /// have channels with differing buffer lengths by using this config option each
        /// time before creating them.
        /// If automatic updating is disabled, make sure you call Bass.BASS_Update(System.Int32)
        /// frequently enough to keep the buffers updated.
        /// </summary>
        public static int PlaybackBufferLength
        {
            get { return GetConfig(Configuration.PlaybackBufferLength); }
            set { Configure(Configuration.PlaybackBufferLength, value); }
        }

        /// <summary>
        /// The update period of HSTREAM and HMUSIC channel playback buffers in milliseconds.
        /// 0 = disable automatic updating. The minimum period is 5ms, the maximum is 100ms. If the period
        /// specified is outside this range, it is automatically capped.
        /// The update period is the amount of time between updates of the playback buffers
        /// of HSTREAM/HMUSIC channels. Shorter update periods allow smaller buffers
        /// to be set with the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER option, but
        /// as the rate of updates increases, so the overhead of setting up the updates
        /// becomes a greater part of the CPU usage. The update period only affects HSTREAM
        /// and HMUSIC channels, it does not affect samples. Nor does it have any effect
        /// on decoding channels, as they are not played.
        /// BASS creates one or more threads (determined by Un4seen.Bass.BASSConfig.BASS_CONFIG_UPDATETHREADS)
        /// specifically to perform the updating, except when automatic updating is disabled
        /// (period=0) - then you must regularly call Un4seen.Bass.Bass.BASS_Update(System.Int32)
        /// or Un4seen.Bass.Bass.BASS_ChannelUpdate(System.Int32,System.Int32)instead.
        /// This allows you to synchronize BASS's CPU usage with your program's. For
        /// example, in a game loop you could call Un4seen.Bass.Bass.BASS_Update(System.Int32)
        /// once per frame, which keeps all the processing in sync so that the frame
        /// rate is as smooth as possible. BASS_Update should be called at least around
        /// 8 times per second, even more often if the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        /// option is used to set smaller buffers.
        /// The update period can be altered at any time, including during playback.
        /// The default period is 100ms.
        /// </summary>
        public static int UpdatePeriod
        {
            get { return GetConfig(Configuration.UpdatePeriod); }
            set { Configure(Configuration.UpdatePeriod, value); }
        }

        public static int GlobalSampleVolume
        {
            get { return GetConfig(Configuration.GlobalSampleVolume); }
            set { Configure(Configuration.GlobalSampleVolume, value); }
        }

        public static int GlobalStreamVolume
        {
            get { return GetConfig(Configuration.GlobalStreamVolume); }
            set { Configure(Configuration.GlobalStreamVolume, value); }
        }

        public static int GlobalMusicVolume
        {
            get { return GetConfig(Configuration.GlobalMusicVolume); }
            set { Configure(Configuration.GlobalMusicVolume, value); }
        }

        public static bool LogarithmicVolumeCurve
        {
            get { return GetConfigBool(Configuration.VolumeCurve); }
            set { Configure(Configuration.VolumeCurve, value); }
        }

        public static bool LogarithmicPanningCurve
        {
            get { return GetConfigBool(Configuration.PanCurve); }
            set { Configure(Configuration.PanCurve, value); }
        }

        public static bool FloatingPointDSP
        {
            get { return GetConfigBool(Configuration.FloatDSP); }
            set { Configure(Configuration.FloatDSP, value); }
        }

        public static int UpdateThreads
        {
            get { return GetConfig(Configuration.UpdateThreads); }
            set { Configure(Configuration.UpdateThreads, value); }
        }

        public static int AsyncFileBufferLength
        {
            get { return GetConfig(Configuration.AsyncFileBufferLength); }
            set { Configure(Configuration.AsyncFileBufferLength, value); }
        }

        public static int HandleCount { get { return GetConfig(Configuration.HandleCount); } }
        #endregion

        #region Error Code
        [DllImport(DllName)]
        extern static Errors BASS_ErrorGetCode();

        public static Errors LastError { get { return BASS_ErrorGetCode(); } }
        #endregion

        #region Sample
        [DllImport(DllName, EntryPoint = "BASS_SampleGetChannel")]
        public static extern int SampleGetChannel(int sample, bool onlynew);

        [DllImport(DllName, EntryPoint = "BASS_SampleFree")]
        public static extern bool SampleFree(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, IntPtr Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleCreate")]
        public static extern int CreateSample(int Length, int freq, int chans, int max, BassFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, IntPtr Buffer);
        
        [DllImport(DllName, EntryPoint = "BASS_SampleGetInfo")]
        public static extern bool SampleGetInfo(int Handle, ref SampleInfo info);

        public static SampleInfo SampleGetInfo(int Handle)
        {
            SampleInfo temp = new SampleInfo();
            SampleGetInfo(Handle, ref temp);
            return temp;
        }

        #region Sample Load
        [DllImport(DllName)]
        static extern int BASS_SampleLoad(bool mem, string file, long offset, int Length, int max, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_SampleLoad(bool mem, IntPtr file, long offset, int Length, int max, BassFlags flags);

        public static int LoadSample(string File, long Offset, int Length, int MaxNoOfPlaybacks, BassFlags Flags)
        {
            return BASS_SampleLoad(false, File, Offset, Length, MaxNoOfPlaybacks, Flags);
        }

        public static int LoadSample(IntPtr Memory, long Offset, int Length, int MaxNoOfPlaybacks, BassFlags Flags)
        {
            return BASS_SampleLoad(true, Memory, Offset, Length, MaxNoOfPlaybacks, Flags);
        }
        #endregion
        #endregion

        #region Music
        [DllImport(DllName, EntryPoint = "BASS_MusicFree")]
        public extern static bool MusicFree(int Handle);

        #region Music Load
        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, string file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, IntPtr file, long offset, int Length, BassFlags flags, int freq);

        public static int LoadMusic(string File, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return BASS_MusicLoad(false, File, Offset, Length, Flags, Frequency);
        }

        public static int LoadMusic(IntPtr Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency);
        }
        #endregion
        #endregion
    }
}