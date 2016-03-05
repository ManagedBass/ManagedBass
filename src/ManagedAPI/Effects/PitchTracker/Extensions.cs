using System;

namespace Pitch
{
    static class Extensions
    {
        /// <summary>
        /// Copy the values from one Buffer to a different or the same Buffer. 
        /// It is safe to copy to the same Buffer, even if the areas overlap
        /// </summary>
        public static void Copy<T>(this T[] from, T[] to, int fromStart, int toStart, int length)
        {
            if (to == null || from.Length == 0 || to.Length == 0)
                return;

            var fromEndIdx = fromStart + length;
            var toEndIdx = toStart + length;

            if (fromStart < 0)
            {
                toStart -= fromStart;
                fromStart = 0;
            }

            if (toStart < 0)
            {
                fromStart -= toStart;
                toStart = 0;
            }

            if (fromEndIdx >= from.Length)
            {
                toEndIdx -= fromEndIdx - from.Length + 1;
                fromEndIdx = from.Length - 1;
            }

            if (toEndIdx >= to.Length)
            {
                fromEndIdx -= toEndIdx - to.Length + 1;
                toEndIdx = from.Length - 1;
            }

            if (fromStart < toStart)
            {
                // Shift right, so start at the right
                for (int fromIdx = fromEndIdx, toIdx = toEndIdx; fromIdx >= fromStart; fromIdx--, toIdx--)
                    to[toIdx] = from[fromIdx];
            }
            else
            {
                // Shift left, so start at the left
                for (int fromIdx = fromStart, toIdx = toStart; fromIdx <= fromEndIdx; fromIdx++, toIdx++)
                    to[toIdx] = from[fromIdx];
            }
        }

        /// <summary>
        /// Fill the Buffer with the specified value
        /// </summary>
        public static void Fill<T>(this T[] buffer, T value)
        {
            for (int idx = 0; idx < buffer.Length; idx++)
                buffer[idx] = value;
        }
    }
}
