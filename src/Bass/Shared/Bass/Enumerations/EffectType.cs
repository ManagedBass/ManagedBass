namespace ManagedBass
{
    /// <summary>
    /// FX effect types, use with <see cref="Bass.ChannelSetFX" />.
    /// </summary>
    public enum EffectType
    {
        #region DirectX
        /// <summary>
        /// DX8 Chorus.
        /// </summary>
        DXChorus,
        
        /// <summary>
        /// DX8 Distortion.
        /// </summary>
        DXDistortion,

        /// <summary>
        /// DX8 Echo.
        /// </summary>
        DXEcho,

        /// <summary>
        /// DX8 Flanger.
        /// </summary>
        DXFlanger,

        /// <summary>
        /// DX8 Compressor (Windows Only).
        /// </summary>
        DXCompressor,

        /// <summary>
        /// DX8 Gargle (Windows Only).
        /// </summary>
        DXGargle,

        /// <summary>
        /// DX8 I3DL2 (Interactive 3D Audio Level 2) reverb (Windows Only).
        /// </summary>
        DX_I3DL2Reverb,

        /// <summary>
        /// DX8 Parametric equalizer.
        /// </summary>
        DXParamEQ,

        /// <summary>
        /// DX8 Reverb.
        /// </summary>
        DXReverb,
        #endregion

        #region BassFx
        /// <summary>
        /// <see cref="Fx.BassFx"/>: Channel Volume Ping-Pong (multi channel).
        /// </summary>
        Rotate = 0x10000,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Volume control (multi channel).
        /// </summary>
        Volume = 0x10003,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Peaking Equalizer (multi channel).
        /// </summary>
        PeakEQ = 0x10004,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Channel Swap/Remap/Downmix (multi channel).
        /// </summary>
        Mix = 0x10007,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Dynamic Amplification (multi channel).
        /// </summary>
        Damp = 0x10008,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Auto WAH (multi channel).
        /// </summary>
        AutoWah = 0x10009,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Phaser (multi channel).
        /// </summary>
        Phaser = 0x1000b,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Chorus (multi channel).
        /// </summary>
        Chorus = 0x1000d,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Distortion (multi channel).
        /// </summary>
        Distortion = 0x10010,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Dynamic Range Compressor (multi channel).
        /// </summary>
        Compressor = 0x10011,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Volume Envelope (multi channel).
        /// </summary>
        VolumeEnvelope = 0x10012,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: BiQuad filters (multi channel).
        /// </summary>
        BQF = 0x10013,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Echo/Reverb 4 (multi channel).
        /// </summary>
        Echo = 0x10014,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Pitch Shift using FFT (multi channel).
        /// </summary>
        PitchShift = 0x10015,

        /// <summary>
        /// <see cref="Fx.BassFx"/>: Pitch Shift using FFT (multi channel).
        /// </summary>
        Freeverb = 0x10016
        #endregion
    }
}
