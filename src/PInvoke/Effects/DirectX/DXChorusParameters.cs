using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXChorusParameters : IEffectParameter
    {
        public float fWetDryMix = 50f;
        public float fDepth = 25f;
        public float fFeedback;
        public float fFrequency;
        public DXWaveform lWaveform = DXWaveform.Triangle;
        public float fDelay;
        public DXPhase lPhase = DXPhase.Zero;

        public EffectType FXType => EffectType.DXChorus;
    }
}