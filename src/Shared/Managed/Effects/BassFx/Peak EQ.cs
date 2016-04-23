using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="PeakEQ"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PeakEQParameters : IEffectParameter
    {
        public int lBand;
        public float fBandwidth = 1f;
        public float fQ;
        public float fCenter = 1000f;
        public float fGain;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.PeakEQ;
    }
    
    public sealed class PeakEQ
    {
        readonly PeakEQParameters _parameters;
        readonly int _handle;
        GCHandle _gch;

        public PeakEQ(int Channel, double Q = 0, double Bandwith = 2.5)
        {
            _handle = Bass.ChannelSetFX(Channel, EffectType.PeakEQ, 0);

            _parameters = new PeakEQParameters
            {
                lBand = -1,
                fQ = (float)Q,
                fBandwidth = (float)Bandwith
            };

            _gch = GCHandle.Alloc(_parameters, GCHandleType.Pinned);
        }

        public int AddBand(double CenterFrequency)
        {
            ++_parameters.lBand;

            _parameters.fCenter = (float)CenterFrequency;
            _parameters.fGain = 0;

            Bass.FXSetParameters(_handle, _gch.AddrOfPinnedObject());

            return _parameters.lBand;
        }

        public void UpdateBand(int Band, double Gain)
        {
            _parameters.lBand = Band;

            Bass.FXGetParameters(_handle, _gch.AddrOfPinnedObject());

            _parameters.fGain = (float)Gain;

            Bass.FXSetParameters(_handle, _gch.AddrOfPinnedObject());
        }
    }
}