using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for BQF Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class BQFParameters : IEffectParameter
    {
        public BQFType lFilter = BQFType.AllPass;
        public float fCenter = 200f;
        public float fGain;
        public float fBandwidth = 1f;
        public float fQ;
        public float fS;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.BQF;
    }
}