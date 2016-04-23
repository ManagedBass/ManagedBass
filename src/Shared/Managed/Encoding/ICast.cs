namespace ManagedBass.Enc
{
    public interface ICast
    {
        bool IsConnected { get; }

        void Connect();
        void Disconnect();

        long DataSent { get; }
    }
}