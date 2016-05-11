using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for Distortion Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class DistortionParameters : IEffectParameter
    {
        public float fDrive;
        public float fDryMix = 5f;
        public float fWetMix = 0.1f;
        public float fFeedback;
        public float fVolume = 0.3f;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Distortion;
    }
}