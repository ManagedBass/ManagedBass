namespace ManagedBass.Dynamics
{
    public static class BassAac
    {
        const string DllName = "bassaac.dll";

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

        /// <summary>
        /// Play audio from mp4 (video) files?
        /// playmp4 (bool): If true (default) BassAac will play the audio from mp4 video files. 
        /// If false mp4 video files will not be played.
        /// </summary>
        public static bool PlayAudioFromMp4
        {
            get { return Bass.GetConfigBool(Configuration.PlayAudioFromMp4); }
            set { Bass.Configure(Configuration.PlayAudioFromMp4, value); }
        }

        /// <summary>
        /// BASSaac add-on: Support MP4 in BASS_AAC_StreamCreateXXX functions?
        /// usemp4 (bool): If true BASSaac supports MP4 in the BASS_AAC_StreamCreateXXX functions. 
        /// If false (default) only AAC is supported.
        /// </summary>
        public static bool SupportMp4
        {
            get { return Bass.GetConfigBool(Configuration.AacSupportMp4); }
            set { Bass.Configure(Configuration.AacSupportMp4, value); }
        }
    }
}
