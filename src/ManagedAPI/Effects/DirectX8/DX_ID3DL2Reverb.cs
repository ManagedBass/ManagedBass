using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DX_ID3DL2ReverbParameters : IEffectParameter
    {
        public int lRoom;                  // [-10000, 0]      default: -1000 mB
        public int lRoomHF;                // [-10000, 0]      default: 0 mB
        public float flRoomRolloffFactor;    // [0.0, 10.0]      default: 0.0
        public float flDecayTime;            // [0.1, 20.0]      default: 1.49s
        public float flDecayHFRatio;         // [0.1, 2.0]       default: 0.83
        public int lReflections;           // [-10000, 1000]   default: -2602 mB
        public float flReflectionsDelay;     // [0.0, 0.3]       default: 0.007 s
        public int lReverb;                // [-10000, 2000]   default: 200 mB
        public float flReverbDelay;          // [0.0, 0.1]       default: 0.011 s
        public float flDiffusion;            // [0.0, 100.0]     default: 100.0 %
        public float flDensity;              // [0.0, 100.0]     default: 100.0 %
        public float flHFReference;          // [20.0, 20000.0]  default: 5000.0 Hz

        public EffectType FXType { get { return EffectType.DX_I3DL2Reverb; } }
    }

    public sealed class DX_ID3DL2ReverbEffect : Effect<DX_ID3DL2ReverbParameters>
    {
        public DX_ID3DL2ReverbEffect(int Handle) : base(Handle) { }

        public int Room
        {
            get { return Parameters.lRoom; }
            set
            {
                Parameters.lRoom = value;
                Update();
            }
        }

        public int RoomHF
        {
            get { return Parameters.lRoomHF; }
            set
            {
                Parameters.lRoomHF = value;
                Update();
            }
        }

        public double RoomRolloffFactor
        {
            get { return Parameters.flRoomRolloffFactor; }
            set
            {
                Parameters.flRoomRolloffFactor = (float)value;
                Update();
            }
        }

        public double DecayTime
        {
            get { return Parameters.flDecayTime; }
            set
            {
                Parameters.flDecayTime = (float)value;
                Update();
            }
        }

        public double DecayHFRatio
        {
            get { return Parameters.flDecayHFRatio; }
            set
            {
                Parameters.flDecayHFRatio = (float)value;
                Update();
            }
        }

        public int Reflections
        {
            get { return Parameters.lReflections; }
            set
            {
                Parameters.lReflections = value;
                Update();
            }
        }

        public double ReflectionsDelay
        {
            get { return Parameters.flReflectionsDelay; }
            set
            {
                Parameters.flReflectionsDelay = (float)value;
                Update();
            }
        }

        public int Reverb
        {
            get { return Parameters.lReverb; }
            set
            {
                Parameters.lReverb = value;
                Update();
            }
        }

        public double ReverbDelay
        {
            get { return Parameters.flReverbDelay; }
            set
            {
                Parameters.flReverbDelay = (float)value;
                Update();
            }
        }

        public double Diffusion
        {
            get { return Parameters.flDiffusion; }
            set
            {
                Parameters.flDiffusion = (float)value;
                Update();
            }
        }

        public double Density
        {
            get { return Parameters.flDensity; }
            set
            {
                Parameters.flDensity = (float)value;
                Update();
            }
        }

        public double HFReference
        {
            get { return Parameters.flHFReference; }
            set
            {
                Parameters.flHFReference = (float)value;
                Update();
            }
        }
    }
}