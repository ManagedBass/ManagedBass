using System;

namespace ManagedBass
{
    public class Converter
    {
        public static double BpmToSeconds(double bpm) => 60 / bpm;

        public static int DbToLevel(double dB, int maxLevel) => (int)Math.Round(maxLevel * Math.Pow(10, dB / 20));

        public static double DbToLevel(double dB, double maxLevel) => maxLevel * Math.Pow(10, dB / 20);

        // Converters from https://github.com/aybe/audioObjects/
        // ms = samples * 1000 / samplerate. 
        // bytes = samples * bits * channels / 8. 

        public static long BytesToSamples(long Bytes, int bits, int channels) => Bytes * 8 / bits / channels;

        public static long MilisecondsToSamples(int samplerate, long ms) => ms * samplerate / 1000;

        public static long SamplesToBytes(long samples, int bits, int channels) => samples * bits * channels / 8;

        public static long SamplesToMiliseconds(long samples, int samplerate) => samples * 1000 / samplerate;

        public static long SamplesToSamplerate(long samples, long ms) => samples * 1000 / ms;

        public static double SamplesToSeconds(long samples, int samplerate) => samples / (double)samplerate;

        public static long SecondsToSamples(double seconds, int samplerate) => (long)(seconds * samplerate);

        public static double BytesToSeconds(long bytes, int bits, int channels, int samplerate)
        {
            return SamplesToSeconds(BytesToSamples(bytes, bits, channels), samplerate);
        }

        public static long SecondsToBytes(double seconds, int samplerate, int bits, int channels)
        {
            return SamplesToBytes(SecondsToSamples(seconds, samplerate), bits, channels);
        }
    }
}