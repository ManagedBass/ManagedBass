using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXDistortionParameters : IEffectParameter
    {
        public float fGain;
        public float fEdge;
        public float fPostEQCenterFrequency;
        public float fPostEQBandwidth;
        public float fPreLowpassCutoff;

        public EffectType FXType { get { return EffectType.DXDistortion; } }
    }

    public sealed class DXDistortionEffect : Effect<DXDistortionParameters>
    {
        public DXDistortionEffect(int Handle) : base(Handle) { }

        public double Gain
        {
            get { return Parameters.fGain; }
            set
            {
                Parameters.fGain = (float)value;
                Update();
            }
        }

        public double Edge
        {
            get { return Parameters.fEdge; }
            set
            {
                Parameters.fEdge = (float)value;
                Update();
            }
        }

        public double PostEQCenterFrequency
        {
            get { return Parameters.fPostEQCenterFrequency; }
            set
            {
                Parameters.fPostEQCenterFrequency = (float)value;
                Update();
            }
        }

        public double PostEQBandwidth
        {
            get { return Parameters.fPostEQBandwidth; }
            set
            {
                Parameters.fPostEQBandwidth = (float)value;
                Update();
            }
        }

        public double PreLowpassCutoff
        {
            get { return Parameters.fPreLowpassCutoff; }
            set
            {
                Parameters.fPreLowpassCutoff = (float)value;
                Update();
            }
        }
    }
}