using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class AudioSample : Channel
    {
        int Sample;
        
        public AudioSample(int Length, Resolution BufferKind = Resolution.Short)
            : base(false, BufferKind)
        {
            Sample = Bass.CreateSample(Length, 44100, 2, 1, BufferKind.ToBassFlag());
            Handle = Bass.SampleGetChannel(Sample, true);

            Player = new BassPlayer(Handle, this);
        }

        public AudioSample(string FilePath, Resolution BufferKind = Resolution.Short)
            : base(false, BufferKind)
        {
            Sample = Bass.LoadSample(FilePath, 0, 0, 1, BufferKind.ToBassFlag());
            Handle = Bass.SampleGetChannel(Sample, true);

            Player = new BassPlayer(Handle, this);
        }

        public AudioSample(byte[] Memory, int Length, Resolution BufferKind = Resolution.Short)
            : base(false, BufferKind)
        {
            // Pin
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Sample = Bass.LoadSample(GCPin.AddrOfPinnedObject(), 0, Length, 1, BufferKind.ToBassFlag());
            Handle = Bass.SampleGetChannel(Sample, true);

            Player = new BassPlayer(Handle, this);

            GCPin.Free();
        }

        public long Length { get { return Bass.SampleGetInfo(Sample).Length; } }

        public void Read(byte[] Buffer)
        {
            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            Bass.SampleGetData(Handle, gch.AddrOfPinnedObject());

            gch.Free();
        }

        public byte[] ReadByte()
        {
            var Buffer = new byte[Bass.SampleGetInfo(Sample).Length];

            Read(Buffer);

            return Buffer;
        }

        public void Read(float[] Buffer)
        {
            GCHandle gch = GCHandle.Alloc(Buffer, GCHandleType.Pinned);

            Bass.SampleGetData(Handle, gch.AddrOfPinnedObject());

            gch.Free();
        }

        public float[] ReadFloat()
        {
            var Buffer = new float[Bass.SampleGetInfo(Sample).Length / 4];

            Read(Buffer);

            return Buffer;
        }

        public bool Write(byte[] buffer) { return WriteObj(buffer); }

        public bool Write(float[] buffer) { return WriteObj(buffer); }

        bool WriteObj(object buffer)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            bool Result = Bass.SampleSetData(Handle, gch.AddrOfPinnedObject());

            gch.Free();

            return Result;
        }

        public override void Dispose() { try { Bass.SampleFree(Sample); } catch { } }
    }
}
