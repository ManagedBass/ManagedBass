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
        
        public static void Load(string folder = null)
        {
            Extensions.Load(DllName, folder);
        }

        // TODO: BASS_ChannelGetAttributeEx
        // TODO: BASS_ChannelSetAttributeEx
        // TODO: BASS_ChannelGetLevelEx

        [DllImport(DllName, EntryPoint = "BASS_Start")]
        public static extern bool Start();

        [DllImport(DllName, EntryPoint = "BASS_Pause")]
        public static extern bool Pause();

        [DllImport(DllName, EntryPoint = "BASS_Stop")]
        public static extern bool Stop();

        [DllImport(DllName, EntryPoint = "BASS_Update")]
        public static extern bool Update(int Length);

        [DllImport(DllName)]
        static extern int BASS_GetVersion();

        public static int Version { get { return BASS_GetVersion(); } }

        [DllImport(DllName, EntryPoint = "BASS_GetInfo")]
        public static extern bool GetInfo(out BassInfo Info);

        public static BassInfo Info
        {
            get
            {
                BassInfo temp;
                GetInfo(out temp);
                return temp;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_GetDSoundObject")]
        public static extern IntPtr GetDSoundObject(int obj);

        #region Plugin
        [DllImport(DllName, EntryPoint = "BASS_PluginGetInfo")]
        public static extern PluginInfo GetPluginInfo(int Handle);

        [DllImport(DllName)]
        static extern int BASS_PluginLoad([MarshalAs(UnmanagedType.LPWStr)]string FileName, BassFlags Flags = BassFlags.Unicode);

        public static int LoadPlugin(string FileName) { return BASS_PluginLoad(FileName); }

        [DllImport(DllName, EntryPoint = "BASS_PluginFree")]
        public static extern bool FreePlugin(int Handle);

        public static int LoadPluginsFromDirectory(string directory)
        {
            int Count = 0;

            if (Directory.Exists(directory))
            {
                foreach (var lib in Directory.EnumerateFiles(directory, "bass*.dll"))
                    if (BASS_PluginLoad(lib) != 0)
                        Count++;
            }

            return Count;
        }
        #endregion

        #region Devices
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
        #endregion

        #region Streams
        [DllImport(DllName, EntryPoint = "BASS_StreamGetFilePosition")]
        public static extern long StreamGetFilePosition(int Handle, FileStreamPosition Mode = FileStreamPosition.Current);

        [DllImport(DllName)]
        static extern int BASS_StreamCreateFile(bool Memory, [MarshalAs(UnmanagedType.LPWStr)]string File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName)]
        static extern int BASS_StreamCreateFile(bool Memory, IntPtr File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_StreamCreateFileUser")]
        public static extern int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user);

        public static int CreateStream(string File, long Offset, long Length, BassFlags Flags)
        {
            return BASS_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags)
        {
            return BASS_StreamCreateFile(true, Memory, Offset, Length, Flags);
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            return CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);
        }

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags);
        }

        [DllImport(DllName, EntryPoint = "BASS_StreamCreateURL")]
        public static extern int CreateStream([MarshalAs(UnmanagedType.LPWStr)]string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr));

        // TODO: Unicode
        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedureType Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamFree")]
        public static extern bool StreamFree(int Handle);
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

        [DllImport(DllName, EntryPoint = "BASS_FXReset")]
        public static extern bool FXReset(int handle);
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

        [DllImport(DllName, EntryPoint = "BASS_SampleSetInfo")]
        public static extern bool SampleSetInfo(int Handle, SampleInfo info);

        [DllImport(DllName)]
        static extern int BASS_SampleGetChannels(int handle, [MarshalAs(UnmanagedType.LPArray)] int[] channels);

        public static int[] SampleGetChannels(int handle)
        {
            var channels = new int[SampleGetInfo(handle).Max];

            BASS_SampleGetChannels(handle, channels);

            return channels;
        }

        [DllImport(DllName, EntryPoint = "BASS_SampleStop")]
        public static extern bool SampleStop(int handle);

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