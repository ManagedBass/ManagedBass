using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class MediaPlayer : Channel
    {
        public bool Load(string FileName)
        {
            if (!IsDisposed)
                Dispose();

            int h = Bass.CreateStream(FileName);

            if (h == 0)
                return false;

            IsDisposed = false;

            Handle = h;

            return true;
        }
    }

    public class MediaPlayerFX : Channel
    {
        public bool Load(string FileName)
        {
            if (!IsDisposed)
                Dispose();

            int h = Bass.CreateStream(FileName, Flags: BassFlags.Decode);

            if (h == 0)
                return false;

            try { h = BassFx.TempoCreate(h, BassFlags.Decode | BassFlags.FxFreeSource); }
            catch { return false; }

            h = BassFx.ReverseCreate(h, Bass.ChannelSeconds2Bytes(h, 1), BassFlags.FxFreeSource);

            Reverse = false;

            IsDisposed = false;

            Handle = h;

            return true;
        }

        public bool Reverse
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        public double Pitch
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Pitch); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pitch, value); }
        }

        public double Tempo
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Tempo); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Tempo, value); }
        }
    }
}