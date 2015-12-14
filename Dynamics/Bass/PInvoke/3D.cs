using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_Apply3D")]
        public static extern void Apply3D();

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
    }
}