using ManagedBass.Dynamics;
using System;

namespace ManagedBass.Effects
{
    public class PitchDSP : DSP
    {
        PitchTracker pTracker;
        float[] _buffer;

        public PitchDSP(int Channel, int Priority = 0)
            : base(Channel, Priority)
        {
            pTracker = new PitchTracker(Bass.ChannelGetInfo(Channel).Frequency);
            pTracker.PitchDetected += R => PitchDetected?.Invoke(R);
        }

        public PitchDSP(MediaPlayer player, int Priority = 0)
            : base(player, Priority)
        {
            pTracker = new PitchTracker(Bass.ChannelGetInfo(Channel).Frequency);
            pTracker.PitchDetected += R => PitchDetected?.Invoke(R);
        }

        public event Action<PitchRecord> PitchDetected;

        protected override void Callback(BufferProvider buffer)
        {
            if (_buffer == null || _buffer.Length < buffer.FloatLength)
                _buffer = new float[buffer.FloatLength];

            buffer.Read(_buffer);

            pTracker.ProcessBuffer(_buffer, buffer.FloatLength);
        }
    }
}