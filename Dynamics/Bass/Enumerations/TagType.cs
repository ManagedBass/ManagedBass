namespace ManagedBass.Dynamics
{
    public enum TagType
    {
        /// <summary>
        /// Unknown tags : not supported tags
        /// </summary>
        Unknown = -1,
        
        /// <summary>
        /// ID3v1 tags : A pointer to a 128 byte block is returned (see BASSTag.BASS_TAG_ID3).
        /// See www.id3.org for details of the block's structure.
        /// </summary>
        ID3 = 0,
        
        /// <summary>
        /// ID3v2 tags : A pointer to a variable length block is returned.
        /// See www.id3.org for details of the block's structure.
        /// </summary>
        ID3v2 = 1,
        
        /// <summary>
        /// OGG comments : Only available when streaming an OGG file.
        /// A pointer to a series of null-terminated UTF-8 strings is returned, the final string ending with a double null.
        /// </summary>
        OGG = 2,
        
        /// <summary>
        /// HTTP headers : Only available when streaming from a HTTP server. 
        /// A pointer to a series of null-terminated ANSI strings is returned, the final string ending with a double null.
        /// </summary>
        HTTP = 3,
        
        /// <summary>
        /// ICY headers : A pointer to a series of null-terminated ANSI strings is returned,
        /// the final string ending with a double null.
        /// </summary>
        ICY = 4,
        
        /// <summary>
        /// ICY (Shoutcast) metadata : A single null-terminated ANSI string containing
        /// the current stream title and url (usually omitted).
        /// The format of the string is: StreamTitle='xxx';StreamUrl='xxx';
        /// </summary>
        META = 5,
        
        /// <summary>
        /// APE (v1 or v2) tags : Only available when streaming an APE file. 
        /// A pointer to a series of null-terminated UTF-8 strings is returned,
        /// the final string ending with a double null.
        /// </summary>
        APE = 6,
        
        /// <summary>
        /// iTunes/MP4 metadata : Only available when streaming a MP4 file. 
        /// A pointer to a series of null-terminated UTF-8 strings is returned, the final string ending with a double null.
        /// </summary>
        MP4 = 7,
        
        /// <summary>
        /// WMA header tags: WMA tags : Only available when streaming a WMA file. 
        /// A pointer to a series of null-terminated UTF-8 strings is returned, the final string ending with a double null.
        /// </summary>
        WMA = 8,

        /// <summary>
        /// OGG encoder : A single null-terminated UTF-8 string.
        /// </summary>
        Vendor = 9,
        
        /// <summary>
        /// Lyric3v2 tag : A single ANSI string is returned, containing the Lyrics3v2 information. 
        /// See www.id3.org/Lyrics3v2 for details of its format.
        /// </summary>
        Lyrics3v2 = 10,

        /// <summary>
        /// WMA mid-stream tag: a single UTF-8 string.
        /// </summary>
        WmaMeta = 11,

        /// <summary>
        /// Apple CoreAudio codec info : Un4seen.Bass.BASS_TAG_CACODEC structure.
        /// </summary>
        CoreAudioCodec = 11,
        
        /// <summary>
        /// WMA codec: A description of the codec used by the file. 
        /// 2 null-terminated UTF-8 strings are returned, with the 1st string being the name of the codec,
        /// and the 2nd containing additional information like what VBR setting was used.
        /// </summary>
        WmaCodec = 12,
        
        /// <summary>
        /// FLAC cuesheet : BASSTag.FLAC_CUE structure (which includes BASS_TAG_FLAC_CUE_TRACK and BASS_TAG_FLAC_CUE_TRACK_INDEX).
        /// </summary>
        FlacCue = 12,
        
        /// <summary>
        /// Media Foundation tags : A pointer to a series of null-terminated UTF-8 strings
        /// is returned, the final string ending with a double null.
        /// </summary>
        MF = 13,

        /// <summary>
        /// WAVE format : A pointer to a Un4seen.Bass.WAVEFORMATEXT structure is returned.
        /// </summary>
        WaveFormat = 14,

        /// <summary>
        /// RIFF/WAVE tags : array of null-terminated ANSI strings.
        /// </summary>
        RiffInfo = 256,
        
        /// <summary>
        /// BWF/RF64 tags (Broadcast Audio Extension) : A pointer to a variable length block is returned (see BASS_TAG_BEXT).
        /// See the EBU specification for details of the block's structure.
        /// When reading BWF tags into a Tags.TAG_INFO structure e.g. via BassTags.BASS_TAG_GetFromFile()
        /// the following mapping is performed if no RIFF_INFO tags are present:
        /// Description = Title (max. 256 chars)
        /// Originator = Artist (max. 32 chars)
        /// OriginatorReference = EncodedBy (max. 32 chars)
        /// OriginationDate = Year (in format 'yyyy-mm-dd hh:mm:ss')
        /// TimeReference = Track
        /// UMID = Copyright (max. 64 chars)
        /// CodingHistory = Comment
        /// However, if RIFF_INFO tags are present the BWF tags are present in the NativeTags.
        /// </summary>
        RiffBext = 257,
        
        /// <summary>
        /// RIFF/BWF Radio Traffic Extension tags : A pointer to a variable length block is returned (see BASS_TAG_CART).
        /// See the EBU specifications for details of the block's structure.
        /// When reading BWF tags into a TAG_INFO structure e.g. via BassTags.BASS_TAG_GetFromFile()
        /// the following mapping is performed if no RIFF_INFO tags are present:
        /// Title = Title (max. 64 chars)
        /// Artist = Artist (max. 64 chars)
        /// Category = Grouping (max. 64 chars)
        /// Classification = Mood (max. 64 chars)
        /// ProducerAppID = Publisher (max. 64 chars)
        /// ProducerAppVersion = EncodedBy (max. 64 chars)
        /// TagText = Comment
        /// However, if RIFF_INFO tags are present the CART tags are present in the NativeTags.
        /// </summary>
        RiffCart = 258,

        /// <summary>
        /// RIFF DISP text chunk: a single ANSI string.
        /// </summary>
        RiffDISP = 259,

        /// <summary>
        /// + index# : Un4seen.Bass.BASSTag.BASS_TAG_APE_BINARY structure.
        /// </summary>
        ApeBinary = 4096,

        /// <summary>
        /// MOD music name : a single ANSI string.
        /// </summary>
        MusicName = 65536,

        /// <summary>
        /// MOD message : a single ANSI string.
        /// </summary>
        MusicMessage = 65537,
        
        /// <summary>
        /// MOD music order list: BYTE array of pattern numbers played at that order position.
        /// Pattern number 254 is "+++" (skip order) and 255 is "---" (end song). 
        /// You can use Bass.ChannelGetLength() with BASS_POS_MUSIC_ORDER to get the length of the array.
        /// </summary>
        MusicOrders = 65538,

        /// <summary>
        /// + instrument#, MOD instrument name : ANSI string
        /// </summary>
        MusicInstrument = 65792,

        /// <summary>
        /// + sample#, MOD sample name : ANSI string
        /// </summary>
        MusicSample = 66304,

        /// <summary>
        /// + track#, track text : array of null-terminated ANSI strings
        /// </summary>
        MidiTrack = 69632,

        /// <summary>
        /// + index# : Un4seen.Bass.BASSTag.BASS_TAG_FLAC_PICTURE structure.
        /// </summary>
        FlacPicture = 73728,

        /// <summary>
        /// ADX tags: A pointer to the ADX loop structure (see Un4seen.Bass.AddOn.Adx.BASS_ADX_TAG_LOOP).
        /// </summary>
        AdxLoop = 73728
    }
}