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
        public Record() : this(RecordDevice.Default, new PCMFormat()) { }

        /// <summary>
        /// Creates a new instance of <see cref="Record"/> with the Default Format.
        /// </summary>
        /// <param name="Device">The <see cref="RecordDevice"/> to use.</param>
        public Record(RecordDevice Device) : this(Device, new PCMFormat()) { }
        
        /// <summary>
        /// Creates a new instance of <see cref="Record"/>.
        /// </summary>
        /// <param name="Device">The <see cref="RecordDevice"/> to use.</param>
        /// <param name="Format">Channels, SampleRate, Resolution.</param>
        public Record(RecordDevice Device, PCMFormat Format)
        {
            this.Format = Format;

            Device.Init();
            var deviceIndex = Device.Index;

            Bass.CurrentRecordingDevice = deviceIndex;
            
            Handle = Bass.RecordStart(Format.Frequency, Format.Channels, BassFlags.RecordPause | Format.Resolution.ToBassFlag(), Processing);
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

        /// <summary>
        /// The Format of Recorded Data.
        /// </summary>
        public PCMFormat Format { get; }

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