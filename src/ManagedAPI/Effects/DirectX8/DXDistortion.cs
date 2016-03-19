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

        public EffectType FXType => EffectType.DXDistortion;
    }

    public sealed class DXDistortionEffect : Effect<DXDistortionParameters>
    {
        public DXDistortionEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXDistortionEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public double Gain
        {
            get { return Parameters.fGain; }
            set
            {
                Parameters.fGain = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Edge
        {
            get { return Parameters.fEdge; }
            set
            {
                Parameters.fEdge = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double PostEQCenterFrequency
        {
            get { return Parameters.fPostEQCenterFrequency; }
            set
            {
                Parameters.fPostEQCenterFrequency = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double PostEQBandwidth
        {
            get { return Parameters.fPostEQBandwidth; }
            set
            {
                Parameters.fPostEQBandwidth = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double PreLowpassCutoff
        {
            get { return Parameters.fPreLowpassCutoff; }
            set
            {
                Parameters.fPreLowpassCutoff = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}