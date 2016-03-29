namespace ManagedBass.Enc
{
    /// <summary>
    /// To be used with <see cref="BassEnc.CastSendMeta" /> to define the type of metadata to send.
    /// </summary>
    public enum EncodeMetaDataType
    {
        /// <summary>
        /// SHOUTcast v2 Cacheable Metadata (Content Info Metadata).
        /// </summary>
        XmlContent = 12288,

        /// <summary>
        /// SHOUTcast v2 Cacheable Metadata (Url Metadata).
        /// </summary>
        XmlUrl = 12289,

        /// <summary>
        /// SHOUTcast v2 Cacheable Metadata (Aol Radio format).
        /// </summary>
        XmlAol = 14593,

        /// <summary>
        /// SHOUTcast v2 Cacheable Metadata (SHOUTcast 2.0 format).
        /// </summary>
        XmlShoutcast = 14594,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Station logo image/jpeg).
        /// </summary>
        BinStationLogoJpg = 16384,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Station logo image/png).
        /// </summary>
        BinStationLogoPng = 16385,
        
        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Station logo image/bmp).
        /// </summary>
        BinStationLogoBmp = 16386,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Station logo image/gif).
        /// </summary>
        BinStationLogoGif = 16387,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Album art image/jpeg).
        /// </summary>
        BinAlbumArtJpg = 16640,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Album art image/png).
        /// </summary>
        BinAlbumArtPng = 16641,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Album art image/bmp).
        /// </summary>
        BinAlbumArtBmp = 16642,

        /// <summary>
        /// SHOUTcast v2 Cacheable Binary Metadata (Album art image/gif).
        /// </summary>
        BinAlbumArtGif = 16643,

        /// <summary>
        /// SHOUTcast v2 Pass-through Metadata (Time Remaining).
        /// </summary>
        XmlTimeRemaining = 20481
    }
}