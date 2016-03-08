using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
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

    public sealed class DXEchoEffect : Effect<DXEchoParameters>
    {
        public DXEchoEffect(int Handle) : base(Handle) { }

        public double WetDryMix
        {
            get { return Parameters.fWetDryMix; }
            set
            {
                Parameters.fWetDryMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double LeftDelay
        {
            get { return Parameters.fLeftDelay; }
            set
            {
                Parameters.fLeftDelay = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double RightDelay
        {
            get { return Parameters.fRightDelay; }
            set
            {
                Parameters.fRightDelay = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public bool PanDelay
        {
            get { return Parameters.lPanDelay; }
            set
            {
                Parameters.lPanDelay = value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}