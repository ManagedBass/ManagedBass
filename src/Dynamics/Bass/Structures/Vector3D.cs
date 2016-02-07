using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Structure used by the 3D functions to describe positions, velocities, and orientations.
    /// </summary>
    /// <remarks>
    /// The left-handed coordinate system is used.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3D
    {
        /// <summary>
        /// +values=right, -values=left (default=0)
        /// </summary>
        public float X;

        /// <summary>
        /// +values=up, -values=down (default=0)
        /// </summary>
        public float Y;

        /// <summary>
        /// +values=front, -values=behind (default=0)
        /// </summary>
        public float Z;

        public Vector3D(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
}