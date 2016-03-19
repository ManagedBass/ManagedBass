using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
    /// <summary>
    /// Used with <see cref="PitchShiftEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PitchShiftParameters : IEffectParameter
    {
        public float fPitchShift;
        public float fSemitones;
        public long lFFTsize;
        public long lOsamp;
        
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.PitchShift;
    }

    /// <summary>
	/// Used with <see cref="Bass.ChannelSetFX" />, <see cref="Bass.FXSetParameters" /> and <see cref="Bass.FXGetParameters" /> to retrieve and set the parameters of the DSP effect Pitch Shift using FFT.
	/// </summary>
	/// <remarks>
	/// <para>This effect uses FFT for its pitch shifting while maintaining duration.</para>
	/// </remarks>
    public sealed class PitchShiftEffect : Effect<PitchShiftParameters>
    {
        public PitchShiftEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }
        
		/// <summary>
		/// A factor value which is between 0.5 (one octave down) and 2 (one octave up) (1 won't change the pitch, default).
		/// </summary>
        public double PitchShift
        {
            get { return Parameters.fPitchShift; }
            set
            {
                Parameters.fPitchShift = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Semitones (0 won't change the pitch). Default = 0.
		/// </summary>
        public double Semitones
        {
            get { return Parameters.fSemitones; }
            set
            {
                Parameters.fSemitones = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Defines the FFT frame size used for the processing. Typical values are 1024, 2048 (default) and 4096, max is 8192.
		/// </summary>
		/// <remarks>It may be any value up to 8192 but it MUST be a power of 2.</remarks>
        public long FFTFrameSize
        {
            get { return Parameters.lFFTsize; }
            set
            {
                Parameters.lFFTsize = value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Is the STFT oversampling factor which also determines the overlap between adjacent STFT frames. Default = 8.
		/// </summary>
		/// <remarks>It should at least be 4 for moderate scaling ratios. A value of 32 is recommended for best quality (better quality = higher CPU usage).</remarks>
        public long OversamplingFactor
        {
            get { return Parameters.lOsamp; }
            set
            {
                Parameters.lOsamp = value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}