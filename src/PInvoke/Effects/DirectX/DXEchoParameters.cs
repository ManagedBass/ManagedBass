using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXEchoParameters : IEffectParameter
    {
        public float fWetDryMix;
        public float fFeedback;
        public float fLeftDelay;
        public float fRightDelay;
        public bool lPanDelay;

        public EffectType FXType => EffectType.DXEcho;
    }
}