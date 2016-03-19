using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXParamEQParameters : IEffectParameter
    {
        public float fCenter;
        public float fBandwidth;
        public float fGain;

        public EffectType FXType => EffectType.DXParamEQ;
    }

    public sealed class DXParamEQEffect : Effect<DXParamEQParameters>
    {
        public DXParamEQEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public double Center
        {
            get { return Parameters.fCenter; }
            set
            {
                Parameters.fCenter = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Bandwidth
        {
            get { return Parameters.fBandwidth; }
            set
            {
                Parameters.fBandwidth = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

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
    }
}