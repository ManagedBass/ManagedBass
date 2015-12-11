using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleInfo
    {
        public int Frequency;
        public float Volume;
        public float Pan;
        public BassFlags Flags;
        public int Length;
        public int Max;
        public int OriginalResolution;
        public int Channels;
        public int MinGap;
        public BASS3DMode Mode3D;
        public float MinDistance;
        public float MaxDistance;
        public int iAngle;
        public int oAngle;
        public float OutVolume;
        public BASSVam VAM;
        public int Priority;
    }
}