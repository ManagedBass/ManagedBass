namespace ManagedBass
{
    public class PCMFormat
    {
        public PCMFormat(int Frequency = 44100, int Channels = 2, Resolution Resolution = Resolution.Short)
        {
            this.Frequency = Frequency;
            this.Channels = Channels;
            this.Resolution = Resolution;
        }

        public int Frequency { get; set; }
        public int Channels { get; set; }
        public Resolution Resolution { get; set; }

        public WaveFormat ToWaveFormat()
        {
            switch (Resolution)
            {
                case Resolution.Float:
                    return WaveFormat.CreateIeeeFloat(Frequency, Channels);

                case Resolution.Byte:
                    return new WaveFormat(Frequency, 8, Channels);

                default:
                    return new WaveFormat(Frequency, 16, Channels);
            }
        }
    }
}