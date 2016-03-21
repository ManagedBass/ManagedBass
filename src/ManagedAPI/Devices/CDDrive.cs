using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;

namespace ManagedBass
{
    /// <summary>
    /// Managed Wrapper around BassCd
    /// </summary>
    /// <remarks>Requires basscd.dll</remarks>
    public class CDDrive : IDisposable
    {
        #region Singleton
        static Dictionary<int, CDDrive> Singleton = new Dictionary<int, CDDrive>();

        CDDrive(int Index) { DriveIndex = Index; }

        public static CDDrive Get(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                CDInfo info;
                if (!BassCd.GetInfo(Device, out info))
                    throw new ArgumentException("Invalid CDDrive Index");

                var Dev = new CDDrive(Device);
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }
        #endregion

        /// <summary>
        /// Number of CD Drives available on the System
        /// </summary>
        public static int DriveCount => BassCd.DriveCount;

        /// <summary>
        /// The Drive Index used by Bass to identify a Drive
        /// </summary>
        public int DriveIndex { get; }

        /// <summary>
        /// Gets Information about a Drive
        /// </summary>
        public CDInfo DriveInfo => BassCd.GetInfo(DriveIndex);

        public int Speed
        {
            get { return BassCd.GetSpeed(DriveIndex); }
            set { if (!BassCd.SetSpeed(DriveIndex, value)) throw new InvalidOperationException(); }
        }

        public bool HasDisk => BassCd.IsReady(DriveIndex);

        /// <summary>
        /// Enumerates CDDrives present on the Computer
        /// </summary>
        public static IEnumerable<CDDrive> Drives
        {
            get
            {
                CDInfo info;

                for (int i = 0; BassCd.GetInfo(i, out info); ++i)
                    yield return Get(i);
            }
        }

        /// <summary>
        /// Release the Disk
        /// </summary>
        public void Dispose() => BassCd.Release(DriveIndex);
    }
}