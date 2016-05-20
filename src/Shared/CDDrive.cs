#if WINDOWS || LINUX
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ManagedBass.Cd
{
    /// <summary>
    /// Managed Wrapper around BassCd
    /// </summary>
    /// <remarks>Requires basscd.dll</remarks>
    public class CDDrive : IDisposable
    {
        #region Singleton
        static readonly Dictionary<int, CDDrive> Singleton = new Dictionary<int, CDDrive>();

        CDDrive(int Index) { this.Index = Index; }

        public static CDDrive GetByIndex(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device];

            CDInfo info;
            if (!BassCd.GetInfo(Device, out info))
                throw new ArgumentException("Invalid CDDrive Index");

            var dev = new CDDrive(Device);
            Singleton.Add(Device, dev);

            return dev;
        }
        #endregion

        /// <summary>
        /// Number of CD Drives available on the System
        /// </summary>
        public static int Count => BassCd.DriveCount;

        /// <summary>
        /// The Drive Index used by Bass to identify a Drive
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets Information about a Drive
        /// </summary>
        public CDInfo Info => BassCd.GetInfo(Index);

        public int Speed
        {
            get { return BassCd.GetSpeed(Index); }
            set
            {
                if (!BassCd.SetSpeed(Index, value))
                    throw new InvalidOperationException();
            }
        }

        public double SpeedMultiplier
        {
            get { return BassCd.GetSpeedMultiplier(Index); }
            set
            {
                if (!BassCd.SetSpeed(Index, (int)(value * 176.4)))
                    throw new InvalidOperationException();
            }
        }

        public bool IsReady => BassCd.IsReady(Index);

        public bool IsDoorLocked
        {
            get { return BassCd.DoorIsLocked(Index); }
            set { BassCd.Door(Index, value ? CDDoorAction.Lock : CDDoorAction.Unlock); }
        }

        public bool IsDoorOpen
        {
            get { return BassCd.DoorIsOpen(Index); }
            set { BassCd.Door(Index, value ? CDDoorAction.Open : CDDoorAction.Close); }
        }

        public bool SetOffset(int Offset) => BassCd.SetOffset(Index, Offset);

        public int Tracks => BassCd.GetTracks(Index);

        public int GetTrackLength(int Track) => BassCd.GetTrackLength(Index, Track);

        public int GetTrackPregap(int Track) => BassCd.GetTrackPregap(Index, Track);

        #region Analog
        public int AnalogPosition => BassCd.AnalogGetPosition(Index);

        public bool AnalogIsActive => BassCd.AnalogIsActive(Index);

        public bool AnalogPlay(int Track, int Position) => BassCd.AnalogPlay(Index, Track, Position);

        public static bool AnalogPlay(string FileName, int Position) => BassCd.AnalogPlay(FileName, Position);

        public bool AnalogStop() => BassCd.AnalogStop(Index);
        #endregion

        /// <summary>
        /// Enumerates CDDrives present on the Computer
        /// </summary>
        public static IEnumerable<CDDrive> Drives
        {
            get
            {
                CDInfo info;

                for (var i = 0; BassCd.GetInfo(i, out info); ++i)
                    yield return GetByIndex(i);
            }
        }

        /// <summary>
        /// Release the Disk
        /// </summary>
        public void Dispose() => BassCd.Release(Index);
    }
}
#endif