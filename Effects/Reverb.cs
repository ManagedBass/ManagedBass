using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ReverbParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix;
        public float fRoomSize;
        public float fDamp;
        public float fWidth;
        public int lMode;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Freeverb; } }
    }

    public sealed class ReverbEffect : Effect<ReverbParameters>
    {
        public ReverbEffect(IEffectAssignable Stream) : base(Stream) { }

        public double Damp
        {
            get { return Parameters.fDamp; }
            set
            {
                Parameters.fDamp = (float)value;
                Update();
            }
        }

        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;
                Update();
            }
        }

        public double RoomSize
        {
            get { return Parameters.fRoomSize; }
            set
            {
                Parameters.fRoomSize = (float)value;
                Update();
            }
        }

        public double WetMix
        {
            get { return Parameters.fWetMix; }
            set
            {
                Parameters.fWetMix = (float)value;
                Update();
            }
        }

        public double Width
        {
            get { return Parameters.fWidth; }
            set
            {
                Parameters.fWidth = (float)value;
                Update();
            }
        }
    }
}