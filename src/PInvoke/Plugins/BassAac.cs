#if WINDOWS || LINUX || __ANDROID__
namespace ManagedBass
{
    public static partial class BassAac
    {
        public static bool PlayAudioFromMp4
        {
            get { return Bass.GetConfigBool(Configuration.PlayAudioFromMp4); }
            set { Bass.Configure(Configuration.PlayAudioFromMp4, value); }
        }

        public static bool SupportMp4
        {
            get { return Bass.GetConfigBool(Configuration.AacSupportMp4); }
            set { Bass.Configure(Configuration.AacSupportMp4, value); }
        }
    }
}
#endif