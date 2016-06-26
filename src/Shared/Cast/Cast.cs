using System;

namespace ManagedBass.Enc
{
    public abstract class Cast
    {
        readonly int _handle;

        readonly EncodeNotifyProcedure _proc;
        readonly int _bitrate;
        readonly string _mime;

        const string MimeMp3 = "audio/mpeg";
        const string MimeOgg = "application/ogg";
        const string MimeAac = "audio/aacp";

        internal Cast(int Handle, ChannelType OutputType, int Bitrate = 0)
        {
            _handle = Handle;
            _proc = Notify;
            _bitrate = Bitrate;

            switch (OutputType)
            {
                case ChannelType.AAC:
                    _mime = MimeAac;
                    break;

                case ChannelType.MP3:
                    _mime = MimeMp3;
                    break;

                case ChannelType.OGG:
                    _mime = MimeOgg;
                    break;

                default:
                    throw new ArgumentException("Output type should be Mp3, Ogg or Aac", nameof(OutputType));
            }
        }

        public bool IsConnected { get; protected set; }

        public long DataSent => BassEnc.EncodeGetCount(_handle, EncodeCount.Cast);

        public string StationName { get; set; }

        public string ServerAddress { get; set; } = "localhost";

        public int ServerPort { get; set; } = 8000;

        public string Url { get; set; }
        
        public string Genre { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public bool IsPublic { get; set; } = true;

        #region Abstract
        protected abstract string _server { get; }

        protected virtual string _headers => null;
        #endregion

        public void Connect()
        {
            if (!BassEnc.CastInit(_handle,
                 _server,
                 UserName == null ? PassWord : $"{UserName}:{PassWord}",
                 _mime,
                 StationName,
                 Url,
                 Genre,
                 Description,
                 _headers,
                 _bitrate,
                 IsPublic))
                return;

            IsConnected = true;

            BassEnc.EncodeSetNotify(_handle, _proc, IntPtr.Zero);
        }

        public void Disconnect()
        {
            if (BassEnc.EncodeStop(_handle))
                IsConnected = false;
        }

        void Notify(int Handle, EncodeNotifyStatus Notify, IntPtr User)
        {
            switch (Notify)
            {
                case EncodeNotifyStatus.CastTimeout:
                    Disconnect();
                    break;

                case EncodeNotifyStatus.EncoderDied:
                case EncodeNotifyStatus.CastServerConnectionDied:
                    IsConnected = false;
                    break;
            }
            
            OnNotify(Notify);
        }

        public event Action<EncodeNotifyStatus> Notification;

        void OnNotify(EncodeNotifyStatus Status) => Notification?.Invoke(Status);
    }
}