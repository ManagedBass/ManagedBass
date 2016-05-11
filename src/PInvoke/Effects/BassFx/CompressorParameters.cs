using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for Compressor Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class CompressorParameters : IEffectParameter
    {
        public float fGain = 5f;
        public float fThreshold = -15f;
        public float fRatio = 3f;
        public float fAttack = 20f;
        public float fRelease = 200f;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Compressor;
    }
}