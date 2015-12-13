using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Managed Wrapper around BassCd
    /// </summary>
    /// <remarks>Requires basscd.dll</remarks>
    public class CDDrive : IDisposable
    {
        /// <summary>
        /// Number of CD Drives available on the System
        /// </summary>
        public static int DriveCount { get { return BassCd.DriveCount; } }

        /// <summary>
        /// The Drive Index used by Bass to identify a Drive
        /// </summary>
        int DriveIndex = -1;

        /// <summary>
        /// Gets Information about a Drive
        /// </summary>
        CDInfo DriveInfo { get { return BassCd.GetDriveInfo(DriveIndex); } }

        /// <summary>
        /// Private Constructor to Init a CD Drive
        /// </summary>
        /// <param name="Index">The Index of the CD Drive (0 ... DriveCount-1)</param>
        /// <remarks>No need to check IndexOutOfRange since this constructor is handled internally</remarks>
        CDDrive(int Index) { DriveIndex = Index; }

        /// <summary>
        /// Gets the Product Name of the Drive
        /// </summary>
        public string Name { get { return DriveInfo.Name; } }

        /// <summary>
        /// Gets the Drive Manufacturer's Name
        /// </summary>
        public string Manufacturer { get { return DriveInfo.Manufacturer; } }

        public int SpeedMultiplier { get { return DriveInfo.SpeedMultiplier; } }

        public char DriveLetter { get { return DriveInfo.DriveLetter; } }

        public int Speed
        {
            get { return BassCd.GetSpeed(DriveIndex); }
            set { if (!BassCd.SetSpeed(DriveIndex, value)) throw new InvalidOperationException(); }
        }

        public bool HasDisk { get { return BassCd.IsReady(DriveIndex); } }

        public static Decoder DecoderFromFile(string FileName, BufferKind BufferKind = BufferKind.Short)
        {
            return new Decoder(BassCd.CreateStream(FileName, BassFlags.Decode | BufferKind.ToBassFlag()), BufferKind);
        }

        public Decoder DecodeTrack(int Track, BufferKind BufferKind = BufferKind.Short)
        {
            return new Decoder(BassCd.CreateStream(DriveIndex, Track, BassFlags.Decode | BufferKind.ToBassFlag()), BufferKind);
        }

        public static CDDrive[] Drives
        {
            get
            {
                CDDrive[] Drives = new CDDrive[DriveCount];

                for (int i = 0; i < DriveCount; ++i) Drives[i] = new CDDrive(i);

                return Drives;
            }
        }

        public void Dispose() { BassCd.Release(DriveIndex); }
    }
}