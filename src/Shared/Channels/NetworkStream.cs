using System;

namespace ManagedBass
{
    public class NetworkStream : Channel
    {
        /// <summary>
        /// Gets the stream Url.
        /// </summary>
        public string Url { get; }

        public NetworkStream(string Url, int Offset = 0, BassFlags Flags = BassFlags.Default)
        {
            this.Url = Url;
            
            Handle = Bass.CreateStream(Url, Offset, Flags, OnCallback, IntPtr.Zero);

            Bass.ChannelSetSync(Handle, SyncFlags.Downloaded, 0, OnDownloadCompleted, IntPtr.Zero);
        }

        void OnCallback(IntPtr Buffer, int Length, IntPtr User) => Callback?.Invoke(new BufferProvider(Buffer, Length));

        public event Action<BufferProvider> Callback;

        /// <summary>
        /// Gets the Download Progress.
        /// </summary>
        public long DownloadProgress => Bass.StreamGetFilePosition(Handle, FileStreamPosition.Download);

        /// <summary>
        /// Gets whether the stream is Stalled.
        /// </summary>
        public bool IsStalled => Bass.ChannelIsActive(Handle) == PlaybackState.Stalled;

        /// <summary>
        /// Gets whether the stream is connected to network source.
        /// </summary>
        public bool IsConnected => Bass.StreamGetFilePosition(Handle, FileStreamPosition.Connected) == 1;

        #region Download Completed
        void OnDownloadCompleted(int handle, int channel, int data, IntPtr User) => DownloadComplete?.Invoke();

        /// <summary>
        /// Fired when the Download Completes.
        /// </summary>
        public event Action DownloadComplete;
        #endregion
    }
}