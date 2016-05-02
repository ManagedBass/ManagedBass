using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="ChorusEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class ChorusParameters : IEffectParameter
    {
        public float fDryMix = 0.9f;
        public float fWetMix = 0.35f;
        public float fFeedback = 0.5f;
        public float fMinSweep = 1;
        public float fMaxSweep = 400;
        public float fRate = 200;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Chorus;
    }
}