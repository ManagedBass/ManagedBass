using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="PhaserEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PhaserParameters : IEffectParameter
    {
        public float fDryMix = 0.999f;
        public float fWetMix = 0.999f;
        public float fFeedback;
        public float fRate = 1;
        public float fRange = 4.3f;
        public float fFreq = 50;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Phaser;
    }
}