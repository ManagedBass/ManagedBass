using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXFlangerParameters : IEffectParameter
    {
        public float fWetDryMix;
        public float fDepth;
        public float fFeedback;
        public float fFrequency;
        public DXWaveform lWaveform;
        public float fDelay;
        public DXPhase lPhase;

        public EffectType FXType => EffectType.DXFlanger;
    }
}