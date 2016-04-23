using System;
using ManagedBass.Enc;

namespace ManagedBass.Wma
{
    public class WmaCast : ICast
    {
        int Handle;

        public bool IsConnected => Handle != 0;

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public long DataSent { get; }
    }
}