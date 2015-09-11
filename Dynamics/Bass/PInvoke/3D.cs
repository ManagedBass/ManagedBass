using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        /// <summary>
        /// Applies changes made to the 3D system.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This must be called to apply any changes made with Set3D methods. 
        /// It improves performance to have DirectSound do all the required recalculating at the same time like this, rather than recalculating after every little change is made.
        /// </para>
        /// <para>This function applies 3D changes on all the initialized devices - there's no need to re-call it for each individual device when using multiple devices.</para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_Apply3D")]
        public static extern void Apply3D();

        [DllImport(DllName, EntryPoint = "BASS_Get3DFactors")]
        public static extern bool Get3DFactors(ref float Distance, ref float RollOff, ref float Doppler);

        [DllImport(DllName, EntryPoint = "BASS_Set3DFactors")]
        public static extern bool Set3DFactors(float Distance, float RollOff, float Doppler);

        [DllImport(DllName, EntryPoint = "BASS_GetEAXParameters")]
        public static extern bool GetEAXParameters(ref EAXEnvironment Environment, ref float Volume, ref float Decay, ref float Damp);

        [DllImport(DllName, EntryPoint = "BASS_SetEAXParameters")]
        public static extern bool SetEAXParameters(EAXEnvironment Environment, float Volume, float Decay, float Damp);
        
        [DllImport(DllName, EntryPoint = "BASS_Get3DPosition")]
        public static extern bool Get3DPosition(ref Vector3D Position, ref Vector3D Velocity, ref Vector3D Front, ref Vector3D Top);

        [DllImport(DllName, EntryPoint = "BASS_Set3DPosition")]
        public static extern bool Set3DPosition(Vector3D Position, Vector3D Velocity, Vector3D Front, Vector3D Top);

        /// <summary>
        /// The 3D algorithm for software mixed 3D channels.
        /// </summary>
        /// <remarks>
        /// These algorithms only affect 3D channels that are being mixed in software.
        /// <see cref="Bass.ChannelGetInfo(int,out ChannelInfo)"/> can be used to check whether a channel is being software mixed.
        /// Changing the algorithm only affects subsequently created or loaded samples, musics, or streams; 
        /// it does not affect any that already exist.
        /// On Windows, DirectX 7 or above is required for this option to have effect.
        /// On other platforms, only the <see cref="ManagedBass.Dynamics.Algorithm3D.Default"/> and <see cref="ManagedBass.Dynamics.Algorithm3D.Off"/> options are available.
        /// </remarks>
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

        [DllImport(DllName, EntryPoint = "BASS_ChannelSet3DPosition")]
        public static extern bool ChannelSet3DPosition(int handle, Vector3D pos, Vector3D orient, Vector3D vel);
    }
}