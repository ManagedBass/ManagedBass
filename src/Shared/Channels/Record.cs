using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture audio from Microphone. The <see cref="Handle"/> can be used with BASS functions.
    /// </summary>
    public sealed class Record : IAudioRecorder
    {
        public int Handle { get; }
              
        /// <summary>
        /// Creates a new instance of <see cref="Record"/> with the Default Format and Device.
        /// </summary>
        public Record() : this(RecordDevice.Default) { }

        /// <summary>
        /// Creates a new instance of <see cref="Record"/> with the Default Format.
        /// </summary>
        /// <param name="Device">The <see cref="RecordDevice"/> to use.</param>
        public Record(RecordDevice Device) : this(Device, 44100, 2, BassFlags.Default) { }
        
        /// <summary>
        /// Creates a new instance of <see cref="Record"/>.
        /// </summary>
        public Record(RecordDevice Device, int Frequency, int Channels, BassFlags Flags)
        {
            Device.Init();
            var deviceIndex = Device.Index;

            Bass.CurrentRecordingDevice = deviceIndex;
            
            Handle = Bass.RecordStart(Frequency, Channels, BassFlags.RecordPause | Flags, Processing);

            AudioFormat = WaveFormat.FromChannel(Handle);
        }

        /// <summary>
        /// Gets if Capturing is in progress.
        /// </summary>
        public bool IsRecording => Bass.ChannelIsActive(Handle) == PlaybackState.Playing;

        /// <summary>
        /// Start Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public bool Start() => Bass.ChannelPlay(Handle);

        /// <summary>
        /// Stop Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public bool Stop() => Bass.ChannelPause(Handle);

        public WaveFormat AudioFormat { get; }

        public void Dispose() => Bass.StreamFree(Handle);

        #region Callback
        /// <summary>
        /// Provides the captured data.
        /// </summary>
        public event Action<IntPtr, int> DataAvailable;

        bool Processing(int HRecord, IntPtr Buffer, int Length, IntPtr User)
        {
            DataAvailable?.Invoke(Buffer, Length);

            return true;
        }
        #endregion
    }
}