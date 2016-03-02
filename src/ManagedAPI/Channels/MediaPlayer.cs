using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class MediaPlayer : Channel
    {
        public bool Load(string FileName)
        {
            try
            {
                if (Handle != 0)
                    Bass.StreamFree(Handle);
            }
            catch { }

            int h = Bass.CreateStream(FileName);

            if (h == 0)
                return false;

            Handle = h;

            return true;
        }
    }

    public class MediaPlayerFX : Channel
    {
        int TempoHandle;

        public bool Load(string FileName)
        {
            try
            {
                if (Handle != 0)
                    Bass.StreamFree(Handle);
            }
            catch { }

            int h = Bass.CreateStream(FileName, Flags: BassFlags.Decode);

            if (h == 0)
                return false;

            h = BassFx.TempoCreate(h, BassFlags.Decode | BassFlags.FxFreeSource);
            
            if (h == 0)
                return false;

            TempoHandle = h;

            h = BassFx.ReverseCreate(h, 2, BassFlags.FxFreeSource);

            if (h == 0)
                return false;

            Handle = h;

            Reverse = false;

            return true;
        }

        public bool Reverse
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        public double Pitch
        {
            get { return Bass.ChannelGetAttribute(TempoHandle, ChannelAttribute.Pitch); }
            set { Bass.ChannelSetAttribute(TempoHandle, ChannelAttribute.Pitch, value); }
        }

        public double Tempo
        {
            get { return Bass.ChannelGetAttribute(TempoHandle, ChannelAttribute.Tempo); }
            set { Bass.ChannelSetAttribute(TempoHandle, ChannelAttribute.Tempo, value); }
        }
    }
}