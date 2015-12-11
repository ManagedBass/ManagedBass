namespace ManagedBass.Dynamics
{
    public enum FileStreamPosition
    {
        /// <summary>
        /// Position that is to be decoded for playback next. 
        /// This will be a bit ahead of the position actually being heard due to buffering.
        /// </summary>
        Current = 0,

        /// <summary>
        /// Download progress of an internet file stream or "buffered" user file stream.
        /// </summary>
        Download = 1,

        /// <summary>
        /// End of the file, in other words the file length. 
        /// When streaming in blocks, the file length is unknown, so the download buffer length is returned instead.
        /// </summary>
        End = 2,

        /// <summary>
        /// Start of stream data in the file.
        /// </summary>
        Start = 3,

        /// <summary>
        /// Internet file stream or "buffered" user file stream is still connected? 0 = no, 1 = yes.
        /// </summary>
        Connected = 4,

        /// <summary>
        /// The amount of data in the buffer of an internet file stream or "buffered" user file stream.
        /// Unless streaming in blocks, this is the same as FileStreamPosition.Download.
        /// </summary>
        Buffer = 5,

        /// <summary>
        /// Returns the socket hanlde used for streaming.
        /// </summary>
        Socket = 6,

        /// <summary>
        /// The amount of data in the asynchronous file reading buffer. 
        /// This requires that the BASS_ASYNCFILE flag was used at the stream's creation.
        /// </summary>
        AsyncBuffer = 7,

        /// <summary>
        /// WMA add-on: internet buffering progress (0-100%)
        /// </summary>
        WmaBuffer = 1000,
    }
}