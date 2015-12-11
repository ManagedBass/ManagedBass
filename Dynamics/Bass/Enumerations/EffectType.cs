namespace ManagedBass.Dynamics
{
    public enum EffectType
    {
        // Summary:
        //     DX8 Chorus. Use Un4seen.Bass.BASS_DX8_CHORUS structure to set/get parameters.
        DXChorus = 0,
        //
        // Summary:
        //     DX8 Compressor. Use Un4seen.Bass.BASS_DX8_COMPRESSOR structure to set/get
        //     parameters.
        DXCompressor = 1,
        //
        // Summary:
        //     DX8 Distortion. Use Un4seen.Bass.BASS_DX8_DISTORTION structure to set/get
        //     parameters.
        DXDistortion = 2,
        //
        // Summary:
        //     DX8 Echo. Use Un4seen.Bass.BASS_DX8_ECHO structure to set/get parameters.
        DXEcho = 3,
        //
        // Summary:
        //     DX8 Flanger. Use Un4seen.Bass.BASS_DX8_FLANGER structure to set/get parameters.
        DXFlanger = 4,
        //
        // Summary:
        //     DX8 Gargle. Use Un4seen.Bass.BASS_DX8_GARGLE structure to set/get parameters.
        DXGargle = 5,
        //
        // Summary:
        //     DX8 I3DL2 (Interactive 3D Audio Level 2) reverb. Use Un4seen.Bass.BASS_DX8_I3DL2REVERB
        //     structure to set/get parameters.
        DX_I3DL2Reverb = 6,
        //
        // Summary:
        //     DX8 Parametric equalizer. Use Un4seen.Bass.BASS_DX8_PARAMEQ structure to
        //     set/get parameters.
        DXParamEQ = 7,
        //
        // Summary:
        //     DX8 Reverb. Use Un4seen.Bass.BASS_DX8_REVERB structure to set/get parameters.
        DXReverb = 8,
        //
        // Summary:
        //     BASS_FX Channel Volume Ping-Pong (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_ROTATE
        //     structure to set/get parameters.
        Rotate = 65536,
        //
        // Summary:
        //     BASS_FX Volume control (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_VOLUME
        //     structure to set/get parameters.
        Volume = 65539,
        //
        // Summary:
        //     BASS_FX Peaking Equalizer (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_PEAKEQ
        //     structure to set/get parameters.
        PeakEQ = 65540,
        //
        // Summary:
        //     BASS_FX Channel Swap/Remap/Downmix (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_MIX
        //     structure to set/get parameters.
        Mix = 65543,
        //
        // Summary:
        //     BASS_FX Dynamic Amplification (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_DAMP
        //     structure to set/get parameters.
        Damp = 65544,
        //
        // Summary:
        //     BASS_FX Auto WAH (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_AUTOWAH
        //     structure to set/get parameters.
        AutoWah = 65545,
        //
        // Summary:
        //     BASS_FX Phaser (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_PHASER
        //     structure to set/get parameters.
        Phaser = 65547,
        //
        // Summary:
        //     BASS_FX Chorus (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_CHORUS
        //     structure to set/get parameters.
        Chorus = 65549,
        //
        // Summary:
        //     BASS_FX Distortion (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_DISTORTION
        //     structure to set/get parameters.
        Distortion = 65552,
        //
        // Summary:
        //     BASS_FX Dynamic Range Compressor (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_COMPRESSOR2
        //     structure to set/get parameters.
        Compressor = 65553,
        //
        // Summary:
        //     BASS_FX Volume Envelope (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_VOLUME_ENV
        //     structure to set/get parameters.
        VolumeEnvelope = 65554,
        //
        // Summary:
        //     BASS_FX BiQuad filters (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_BQF
        //     structure to set/get parameters.
        BQF = 65555,
        //
        // Summary:
        //     BASS_FX Echo/Reverb 4 (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_ECHO4
        //     structure to set/get parameters.
        Echo = 65556,
        //
        // Summary:
        //     BASS_FX Pitch Shift using FFT (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_PITCHSHIFT
        //     structure to set/get parameters.
        PitchShift = 65557,
        //
        // Summary:
        //     BASS_FX Pitch Shift using FFT (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_FREEVERB
        //     structure to set/get parameters.
        Freeverb = 65558,
    }
}