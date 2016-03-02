using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class ReverbParameters : IEffectParameter
    {
        public float fDryMix = 0;
        public float fWetMix = 1f;
        public float fRoomSize = 0.5f;
        public float fDamp = 0.5f;
        public float fWidth = 1f;
        public int lMode = 0;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Freeverb;
    }

    public sealed class ReverbEffect : Effect<ReverbParameters>
    {
        public ReverbEffect(int Handle) : base(Handle) { }

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