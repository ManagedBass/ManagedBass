using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_Apply3D")]
        public static extern void Apply3D();

        [DllImport(DllName, EntryPoint = "BASS_Get3DFactors")]
        public static extern bool Get3DFactors(ref float distance, ref float rolloff, ref float doppler);

        [DllImport(DllName, EntryPoint = "BASS_Set3DFactors")]
        public static extern bool Set3DFactors(float distance, float rolloff, float doppler);

        [DllImport(DllName, EntryPoint = "BASS_GetEAXParameters")]
        public static extern bool GetEAXParameters(ref int environment, ref float volume, ref float decay, ref float damp);

        [DllImport(DllName, EntryPoint = "BASS_SetEAXParameters")]
        public static extern bool SetEAXParameters(int environment, float volume, float decay, float damp);

        [DllImport(DllName, EntryPoint = "BASS_Get3DPosition")]
        public static extern bool Get3DPosition(ref Vector3D position, ref Vector3D velocity, ref Vector3D front, ref Vector3D top);

        [DllImport(DllName, EntryPoint = "BASS_Set3DPosition")]
        public static extern bool Set3DPosition(Vector3D position, Vector3D velocity, Vector3D front, Vector3D top);

        /// <summary>
        /// The 3D algorithm for software mixed 3D channels.
        /// algo (int): Use one of the Algorithm3D flags.
        /// These algorithms only affect 3D channels that are being mixed in software.
        /// Bass.ChannelGetInfo() can be used to check whether a channel is being software mixed.
        /// Changing the algorithm only affects subsequently created or loaded samples, musics, or streams; 
        /// it does not affect any that already exist.
        /// On Windows, DirectX 7 or above is required for this option to have effect.
        /// On other platforms, only the BASS_3DALG_DEFAULT and BASS_3DALG_OFF options are available.
        /// </summary>
        public static Algorithm3D Algorithm3D
        {
            get { return (Algorithm3D)GetConfig(Configuration.Algorithm3D); }
            set { Configure(Configuration.Algorithm3D, (int)value); }
        }

        [DllImport(DllName, EntryPoint = "BASS_ChannelGet3DAttributes")]
        public static extern bool ChannelGet3DAttributes(int handle, ref Mode3D mode, ref float min, ref float max, ref int iangle, ref int oangle, ref float outvol);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSet3DAttributes")]
        public static extern bool ChannelSet3DAttributes(int handle, Mode3D mode, float min, float max, int iangle, int oangle, float outvol);

        [DllImport(DllName, EntryPoint = "BASS_ChannelGet3DPosition")]
        public static extern bool ChannelGet3DPosition(int handle, ref Vector3D pos, ref Vector3D orient, ref Vector3D vel);

        [DllImport(DllName, EntryPoint = "BASS_ChannelGet3DPosition")]
        public static extern bool ChannelGet3DPosition(int handle, Vector3D pos, Vector3D orient, Vector3D vel);
    }
}