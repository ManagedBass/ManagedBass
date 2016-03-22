using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXGargleParameters : IEffectParameter
    {
        public int dwRateHz; // Rate of modulation in hz
        public DXWaveform dwWaveShape;

        public EffectType FXType => EffectType.DXGargle;
    }

    public sealed class DXGargleEffect : Effect<DXGargleParameters>
    {
        public DXGargleEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXGargleEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public int Rate
        {
            get { return Parameters.dwRateHz; }
            set
            {
                Parameters.dwRateHz = value;

                OnPropertyChanged();
                Update();
            }
        }

        public DXWaveform WaveShape
        {
            get { return Parameters.dwWaveShape; }
            set
            {
                Parameters.dwWaveShape = value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}