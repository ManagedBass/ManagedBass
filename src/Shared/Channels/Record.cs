using System;

namespace ManagedBass
{
    /// <summary>
    /// Capture audio from Microphone. This class inherits from <see cref="Channel"/> and can have Effects/DSP applied on it.
    /// </summary>
    public sealed class Record : Channel, IAudioRecorder
    {        
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

            var res = Resolution.Short;

            if (Flags.HasFlag(BassFlags.Byte))
                res = Resolution.Byte;
            else if (Flags.HasFlag(BassFlags.Float))
                res = Resolution.Float;

            AudioFormat = WaveFormat.FromParams(Frequency, Channels, res);
            
            Handle = Bass.RecordStart(Frequency, Channels, BassFlags.RecordPause | Flags, Processing);
        }

        /// <summary>
        /// Gets if Capturing is in progress.
        /// </summary>
        public bool IsRecording => IsActive == PlaybackState.Playing;

        /// <summary>
        /// Start Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public bool Start() => Play();

        /// <summary>
        /// Stop Audio Capture.
        /// </summary>
        /// <returns><see langword="true"/> on success, else <see langword="false"/>.</returns>
        public override bool Stop() => Bass.ChannelPause(Handle);

        public WaveFormat AudioFormat { get; }

        #region Callback
        /// <summary>
        /// Provides the captured data.
        /// </summary>
        public event Action<BufferProvider> DataAvailable;

        bool Processing(int HRecord, IntPtr Buffer, int Length, IntPtr User)
        {
            DataAvailable?.Invoke(new BufferProvider(Buffer, Length));

            return true;
        }
        #endregion
    }
}