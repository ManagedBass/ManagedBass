using System;

namespace ManagedBass
{
    /// <summary>
    /// Streams an audio file from a Url
    /// </summary>
    public sealed class NetworkChannel : Channel
    {
        public string Url { get; }
        
        public NetworkChannel(string Url, bool IsDecoder = false, Resolution Resolution = Resolution.Short, int Offset = 0)
        {
            this.Url = Url;

            var flags = Resolution.ToBassFlag() | BassFlags.Unicode;
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = Bass.CreateStream(Url, Offset, flags, OnCallback, IntPtr.Zero);

            Bass.ChannelSetSync(Handle, SyncFlags.Downloaded, 0, OnDownloadCompleted, IntPtr.Zero);
        }

        void OnCallback(IntPtr Buffer, int Length, IntPtr User) => Callback?.Invoke(new BufferProvider(Buffer, Length));

        public event Action<BufferProvider> Callback;

        public long DownloadProgress => Bass.StreamGetFilePosition(Handle, FileStreamPosition.Download);

        public bool IsStalled => Bass.ChannelIsActive(Handle) == PlaybackState.Stalled;

        public bool IsConnected => Bass.StreamGetFilePosition(Handle, FileStreamPosition.Connected) == 1;

        #region Download Completed
        void OnDownloadCompleted(int handle, int channel, int data, IntPtr User) => DownloadComplete?.Invoke();

        public event Action DownloadComplete;
        #endregion
    }
}