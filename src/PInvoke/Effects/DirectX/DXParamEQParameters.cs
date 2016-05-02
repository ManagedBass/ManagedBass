using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXParamEQParameters : IEffectParameter
    {
        public float fCenter;
        public float fBandwidth;
        public float fGain;

        public EffectType FXType => EffectType.DXParamEQ;
    }
}