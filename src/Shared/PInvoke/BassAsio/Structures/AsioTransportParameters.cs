#if WINDOWS
using System.Runtime.InteropServices;

namespace ManagedBass.Asio
{
    /// <summary>
    /// Used with <see cref="BassAsio.Future" /> and the Transport selector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AsioTransportParameters
    {
        /// <summary>
        /// One of the <see cref="AsioTransportCommand"/> values (other might be optinally available)
        /// </summary>
        public AsioTransportCommand command;

        /// <summary>
        /// number of samples data Type is 64 bit integer
        /// </summary>
        public long SamplePosition;

        /// <summary>
        /// track index
        /// </summary>
        public int Track;

        /// <summary>
        /// 512 tracks on/off
        /// </summary>
        public int[] TrackSwitches = new int[16];

        /// <summary>
        /// up to 64 chars
        /// </summary>
        public string Future = string.Empty;
    }

    public enum AsioTransportCommand
    {
        Start = 1,
        Stop = 2,
        Locate = 3,
        PunchIn = 4,
        PunchOut = 5,
        ArmOn = 6,
        ArmOff = 7,
        MonitorOn = 8,
        MonitorOff = 9,
        Arm = 10,
        Monitor = 11
    }
}
#endif