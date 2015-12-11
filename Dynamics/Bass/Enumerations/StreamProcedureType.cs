namespace ManagedBass.Dynamics
{
    public enum StreamProcedureType
    {
        /// <summary>
        /// Flag to signify that the end of the stream is reached.
        /// </summary>
        End = -2147483648,

        /// <summary>
        /// Create a "push" stream.
        /// Instead of BASS pulling data from a StreamProcedure function, data is pushed to
        /// BASS via Bass.StreamPutData().
        /// </summary>
        Push = -1,

        /// <summary>
        /// Create a "dummy" stream.
        /// A dummy stream doesn't have any sample data of its own, but a decoding dummy
        /// stream (with BassFlags.Decode flag) can be used to apply DSP/FX processing
        /// to any sample data, by setting DSP/FX on the stream and feeding the data
        /// through Bass.ChannelGetData().
        /// The dummy stream should have the same sample format as the data being fed
        /// through it.
        /// </summary>
        Dummy = 0,
    }
}