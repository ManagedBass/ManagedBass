using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXDistortionParameters : IEffectParameter
    {
        public float fGain;
        public float fEdge;
        public float fPostEQCenterFrequency;
        public float fPostEQBandwidth;
        public float fPreLowpassCutoff;

        public EffectType FXType => EffectType.DXDistortion;
    }
}