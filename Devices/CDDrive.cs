using System;
using ManagedBass.Dynamics;
using System.Collections.Generic;

namespace ManagedBass
{
    public class CDChannel : Channel
    {
        public CDChannel(string FileName, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassCd.CreateStream(FileName, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public CDChannel(CDDrive Drive, int Track, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassCd.CreateStream(Drive.DriveIndex, Track, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }
    }

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
        public int DriveIndex { get; private set; }

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

        public static IEnumerable<CDDrive> Drives
        {
            get
            {
                CDInfo info;

                for (int i = 0; BassCd.GetDriveInfo(i, out info); ++i)
                    yield return new CDDrive(i);
            }
        }

        public void Dispose() { BassCd.Release(DriveIndex); }
    }
}