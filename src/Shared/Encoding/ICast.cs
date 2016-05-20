namespace ManagedBass.Enc
{
    public interface ICast
    {
        bool IsConnected { get; }

        void Connect();
        void Disconnect();

        long DataSent { get; }
        
        string Url { get; set; }
        
        string UserName { get; set; }
        
        string PassWord { get; set; }
    }
}