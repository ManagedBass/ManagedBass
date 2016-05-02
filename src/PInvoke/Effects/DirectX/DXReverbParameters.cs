using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXReverbParameters : IEffectParameter
    {
        public float fInGain;                // [-96.0,0.0]            default: 0.0 dB
        public float fReverbMix;             // [-96.0,0.0]            default: 0.0 db
        public float fReverbTime;            // [0.001,3000.0]         default: 1000.0 ms
        public float fHighFreqRTRatio;       // [0.001,0.999]          default: 0.001

        public EffectType FXType => EffectType.DXReverb;
    }
}