namespace ManagedBass
{
    public abstract class AudioFileWriter
    {
        public string FileName { get; protected set; }
        
        public abstract void Write(float[] buffer, int Length);

        public abstract void Close();
    }
}
