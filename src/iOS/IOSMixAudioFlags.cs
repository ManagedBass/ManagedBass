using System;

namespace ManagedBass
{
    /// <summary>
    /// Flags to be used with <see cref="Bass.IOSMixAudio"/>.
    /// </summary>
    [Flags]
    public enum IOSMixAudioFlags
    {
        /// <summary>
        /// Allow other audio to mix with app's audio (default).
        /// Enables kAudioSessionProperty_OverrideCategoryMixWithOthers.
        /// </summary>
        MixWithOthers = 1,

        /// <summary>
        /// Also 'duck' the other audio.
        /// Enables kAudioSessionProperty_OtherMixableAudioShouldDuck.
        /// </summary>
        OtherMixableAudioShouldDuck = 2,

        /// <summary>
        /// Use the 'ambient' category.
        /// Enables kAudioSessionCategory_SoloAmbientSound/AmbientSound instead of kAudioSessionCategory_MediaPlayback.
        /// </summary>
        AmbientSound = 4
    }
}