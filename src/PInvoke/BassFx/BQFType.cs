namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx BiQuad filter type. Defines within the <see cref="BQFParameters" /> structure which BiQuad filter should be used.
    /// </summary>
    public enum BQFType
    {
        /// <summary>
        /// BiQuad Lowpass filter.
        /// </summary>
        LowPass = 0,
        
        /// <summary>
        /// BiQuad Highpass filter.
        /// </summary>
        HighPass = 1,
        
        /// <summary>
        /// BiQuad Bandpass filter (constant 0 dB peak gain).
        /// </summary>
        BandPass = 2,
        
        /// <summary>
        /// BiQuad Bandpass Q filter (constant skirt gain, peak gain = Q).
        /// </summary>
        BandPassQ = 3,
        
        /// <summary>
        /// BiQuad Notch filter.
        /// </summary>
        Notch = 4,
        
        /// <summary>
        /// BiQuad All-Pass filter.
        /// </summary>
        AllPass = 5,
        
        /// <summary>
        /// BiQuad Peaking EQ filter.
        /// </summary>
        PeakingEQ = 6,
        
        /// <summary>
        /// BiQuad Low-Shelf filter.
        /// </summary>
        LowShelf = 7,
        
        /// <summary>
        /// BiQuad High-Shelf filter.
        /// </summary>
        HighShelf = 8
    }
}