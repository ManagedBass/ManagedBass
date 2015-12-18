using System;

namespace ManagedBass
{
    public interface IAudioFileWriter : IDisposable
    {
        void Write(byte[] buffer, int Length);

        void Write(short[] buffer, int Length);

        void Write(float[] buffer, int Length);
    }
}
