using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="PeakEQ"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PeakEQParameters : IEffectParameter
    {
        public int lBand;
        public float fBandwidth = 1f;
        public float fQ;
        public float fCenter = 1000f;
        public float fGain;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.PeakEQ;
    }
}