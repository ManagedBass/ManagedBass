using System;

namespace ManagedBass.Enc
{
    public abstract class BassEncCast : ICast
    {
        protected BassEncEncoder Encoder { get; }
        readonly EncodeNotifyProcedure _proc;

        protected BassEncCast(BassEncEncoder Encoder)
        {
            this.Encoder = Encoder;
            _proc = Notify;
        }

        public bool IsConnected { get; protected set; }

        public long DataSent => Encoder.GetCount(EncodeCount.Cast);

        public string StationName { get; set; }

        public string ServerAddress { get; set; } = "localhost";

        public int ServerPort { get; set; } = 8000;

        public string Url { get; set; }

        protected abstract string Mime { get; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public bool IsPublic { get; set; } = true;

        #region Abstract
        protected abstract string _server { get; }

        protected virtual string _headers => null;

        protected virtual int _bitrate => Encoder.OutputBitRate;
        #endregion

        public void Connect()
        {
            if (!BassEnc.CastInit(Encoder.Handle,
                 _server,
                 UserName == null ? PassWord : $"{UserName}:{PassWord}",
                 Mime,
                 StationName,
                 Url,
                 Genre,
                 Description,
                 _headers,
                 _bitrate,
                 IsPublic))
                return;

            IsConnected = true;

            BassEnc.EncodeSetNotify(Encoder.Handle, _proc, IntPtr.Zero);
        }

        public void Disconnect()
        {
            if (Encoder.Stop())
                IsConnected = false;
        }

        void Notify(int Handle, EncodeNotifyStatus Notify, IntPtr User)
        {
            switch (Notify)
            {
                case EncodeNotifyStatus.CastTimeout:
                    Disconnect();
                    OnError(CastError.Timeout);
                    break;

                case EncodeNotifyStatus.EncoderDied:
                    IsConnected = false;
                    OnError(CastError.EncoderDied);
                    break;

                case EncodeNotifyStatus.CastServerConnectionDied:
                    IsConnected = false;
                    OnError(CastError.ConnectionDied);
                    break;
            }
        }

        public event Action<CastError> Error;

        protected void OnError(CastError ErrorType) => Error?.Invoke(ErrorType);
    }
}