using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXReverbParameters : IEffectParameter
    {
        public float fInGain;                // [-96.0,0.0]            default: 0.0 dB
        public float fReverbMix;             // [-96.0,0.0]            default: 0.0 db
        public float fReverbTime;            // [0.001,3000.0]         default: 1000.0 ms
        public float fHighFreqRTRatio;       // [0.001,0.999]          default: 0.001

        public EffectType FXType { get { return EffectType.DXReverb; } }
    }

    public sealed class DXReverbEffect : Effect<DXReverbParameters>
    {
        public DXReverbEffect(int Handle) : base(Handle) { }

        public double InGain
        {
            get { return Parameters.fInGain; }
            set
            {
                Parameters.fInGain = (float)value;
                Update();
            }
        }

        public double ReverbMix
        {
            get { return Parameters.fReverbMix; }
            set
            {
                Parameters.fReverbMix = (float)value;
                Update();
            }
        }

        public double ReverbTime
        {
            get { return Parameters.fReverbTime; }
            set
            {
                Parameters.fReverbTime = (float)value;
                Update();
            }
        }

        public double HighFreqRTRatio
        {
            get { return Parameters.fHighFreqRTRatio; }
            set
            {
                Parameters.fHighFreqRTRatio = (float)value;
                Update();
            }
        }
    }
}