namespace ManagedBass.Dynamics
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
        DXChorus = 0,

        /// <summary>
        /// DX8 Compressor
        /// </summary>
        DXCompressor = 1,

        /// <summary>
        /// DX8 Distortion.
        /// </summary>
        DXDistortion = 2,

        /// <summary>
        /// DX8 Echo.
        /// </summary>
        DXEcho = 3,

        /// <summary>
        /// DX8 Flanger.
        /// </summary>
        DXFlanger = 4,

        /// <summary>
        /// DX8 Gargle.
        /// </summary>
        DXGargle = 5,

        /// <summary>
        /// DX8 I3DL2 (Interactive 3D Audio Level 2) reverb.
        /// </summary>
        DX_I3DL2Reverb = 6,

        /// <summary>
        /// DX8 Parametric equalizer.
        /// </summary>
        DXParamEQ = 7,

        /// <summary>
        /// DX8 Reverb.
        /// </summary>
        DXReverb = 8,
        #endregion

        #region BassFx
        /// <summary>
        /// <see cref="BassFx"/>: Channel Volume Ping-Pong (multi channel).
        /// </summary>
        Rotate = 65536,

        /// <summary>
        /// <see cref="BassFx"/>: Volume control (multi channel).
        /// </summary>
        Volume = 65539,

        /// <summary>
        /// <see cref="BassFx"/>: Peaking Equalizer (multi channel).
        /// </summary>
        PeakEQ = 65540,

        /// <summary>
        /// <see cref="BassFx"/>: Channel Swap/Remap/Downmix (multi channel).
        /// </summary>
        Mix = 65543,

        /// <summary>
        /// <see cref="BassFx"/>: Dynamic Amplification (multi channel).
        /// </summary>
        Damp = 65544,

        /// <summary>
        /// <see cref="BassFx"/>: Auto WAH (multi channel).
        /// </summary>
        AutoWah = 65545,

        /// <summary>
        /// <see cref="BassFx"/>: Phaser (multi channel).
        /// </summary>
        Phaser = 65547,

        /// <summary>
        /// <see cref="BassFx"/>: Chorus (multi channel).
        /// </summary>
        Chorus = 65549,

        /// <summary>
        /// <see cref="BassFx"/>: Distortion (multi channel).
        /// </summary>
        Distortion = 65552,

        /// <summary>
        /// <see cref="BassFx"/>: Dynamic Range Compressor (multi channel).
        /// </summary>
        Compressor = 65553,

        /// <summary>
        /// <see cref="BassFx"/>: Volume Envelope (multi channel).
        /// </summary>
        VolumeEnvelope = 65554,

        /// <summary>
        /// <see cref="BassFx"/>: BiQuad filters (multi channel).
        /// </summary>
        BQF = 65555,

        /// <summary>
        /// <see cref="BassFx"/>: Echo/Reverb 4 (multi channel).
        /// </summary>
        Echo = 65556,

        /// <summary>
        /// <see cref="BassFx"/>: Pitch Shift using FFT (multi channel).
        /// </summary>
        PitchShift = 65557,

        /// <summary>
        /// <see cref="BassFx"/>: Pitch Shift using FFT (multi channel).
        /// </summary>
        Freeverb = 65558,
        #endregion
    }
}