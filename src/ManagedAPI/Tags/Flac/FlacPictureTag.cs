using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    [StructLayout(LayoutKind.Sequential)]
    public class FlacPictureTag
    {
        int Reserved;

        IntPtr mime;

        /// <summary>
        /// The MIME type. This may be "--&gt;" to signify that data contains a URL of the picture rather than the picture data itself.
        /// </summary>
        public string Mime => Marshal.PtrToStringAnsi(mime);
                
        IntPtr description;

        /// <summary>
		/// A description of the picture.
		/// </summary>
		public string Description => Extensions.PtrToStringUtf8(description);
        
        /// <summary>
		/// The width in pixels.
		/// </summary>
        public int Width;

        /// <summary>
		/// The height in pixels.
		/// </summary>
        public int Height;

        /// <summary>
		/// The colour depth in bits-per-pixel.
		/// </summary>
        public int Depth;

        /// <summary>
		/// The number of colours used for indexed-colour pictures (eg. GIF).
		/// </summary>
        public int Colors;

        /// <summary>
		/// The size of <see cref="Data" /> in bytes.
		/// </summary>
        public int Length;

        IntPtr data;

        /// <summary>
		/// The picture data.
		/// </summary>
		public byte[] Data
		{
			get
			{
				if (Length == 0)
                    return null;
				
				byte[] arr = new byte[Length];
				Marshal.Copy(data, arr, 0, Length);
				return arr;
			}
		}

        /// <summary>
		/// Returns the image URL, if the <see cref="Mime" /> type is "--&gt;" - else <see langword="null" />.
		/// </summary>
        public string URL
        {
            get
            {
                if (Mime == "-->")
                    return null;

                return Marshal.PtrToStringAnsi(data);
            }
        }

        public static FlacPictureTag Read(int Channel, int Index)
        {
            return (FlacPictureTag)Marshal.PtrToStructure(Bass.ChannelGetTags(Channel, TagType.FlacPicture + Index), typeof(FlacPictureTag));
        }
    }
}