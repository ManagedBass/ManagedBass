using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    [StructLayout(LayoutKind.Sequential)]
    public class ApeBinaryTag
    {
        IntPtr key, data;

        int length;

        /// <summary>
        /// The binary tag data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                if (data == IntPtr.Zero || length == 0)
                    return null;

                byte[] arr = new byte[length];
                Marshal.Copy(data, arr, 0, length);
                return arr;
            }
        }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Key
        {
            get
            {
                if (key == IntPtr.Zero)
                    return null;

                return Marshal.PtrToStringAnsi(key);
            }
        }

        /// <summary>
        /// The size of data in bytes.
        /// </summary>
        public int Length => length;

        public static ApeBinaryTag Read(int handle, int index)
        {
            return (ApeBinaryTag)Marshal.PtrToStructure(Bass.ChannelGetTags(handle, TagType.ApeBinary + index), typeof(ApeBinaryTag));
        }

        /// <summary>
        /// Read all APE binary tags
        /// </summary>
        public static IEnumerable<ApeBinaryTag> ReadAll(int Handle)
        {
            ApeBinaryTag tag;

            for (int i = 0; (tag = Read(Handle, i)) != null; ++i)
                yield return tag;
        }

        /// <summary>
        /// Returns the Key of the binary tag.
        /// </summary>
        public override string ToString() => Key;
    }
}