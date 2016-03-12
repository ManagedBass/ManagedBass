using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    /// <summary>
    /// Used with <see cref="ReverbEffect"/>.
    /// </summary>
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

        /// <summary>
		/// Damping factor (0.0...1.0, def. 0.5).
		/// </summary>
        public double Damp
        {
            get { return Parameters.fDamp; }
            set
            {
                Parameters.fDamp = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Dry (unaffected) signal mix (0.0...1.0, def. 0).
		/// </summary>
        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Room size (0.0...1.0, def. 0.5).
		/// </summary>
        public double RoomSize
        {
            get { return Parameters.fRoomSize; }
            set
            {
                Parameters.fRoomSize = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Wet (affected) signal mix (0.0...3.0, def. 1.0).
		/// </summary>
        public double WetMix
        {
            get { return Parameters.fWetMix; }
            set
            {
                Parameters.fWetMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Stereo width (0.0...1.0, def. 1.0).
		/// </summary>
		/// <remarks>It should at least be 4 for moderate scaling ratios. A value of 32 is recommended for best quality (better quality = higher CPU usage).</remarks>
        public double Width
        {
            get { return Parameters.fWidth; }
            set
            {
                Parameters.fWidth = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}