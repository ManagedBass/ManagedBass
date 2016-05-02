using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="AutoWahEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AutoWahParameters : IEffectParameter
    {
        public float fDryMix = 0.5f;
        public float fWetMix = 1.5f;
        public float fFeedback = 0.5f;
        public float fRate = 2;
        public float fRange = 4.3f;
        public float fFreq = 50;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.AutoWah;
    }
}