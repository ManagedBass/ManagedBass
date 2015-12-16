namespace ManagedBass.Dynamics
{
    public enum EncodeCount
    {
        In, // sent to encoder
        Out, // received from encoder
        Cast, // sent to cast server
        Queue, // queued
        QueueLimit, // queue limit
        QueueFail // failed to queue
    }
}