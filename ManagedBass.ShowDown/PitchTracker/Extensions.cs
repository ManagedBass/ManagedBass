using System;

namespace Pitch
{
    static class Extensions
    {
        public static void Clear<T>(this T[] arr) { Array.Clear(arr, 0, arr.Length); }

        public static void Clear<T>(this T[] buffer, int startIdx, int endIdx) { Array.Clear(buffer, startIdx, endIdx - startIdx + 1); }

        /// <summary>
        /// Copy the values from one Buffer to a different or the same Buffer. 
        /// It is safe to copy to the same Buffer, even if the areas overlap
        /// </summary>
        public static void Copy<T>(this T[] from, T[] to, int fromStart, int toStart, int length)
        {
            if (to == null || from.Length == 0 || to.Length == 0)
                return;

            var fromBegIdx = fromStart;
            var fromEndIdx = fromStart + length;
            var toBegIdx = toStart;
            var toEndIdx = toStart + length;

            if (fromBegIdx < 0)
            {
                toBegIdx -= fromBegIdx;
                fromBegIdx = 0;
            }

            if (toBegIdx < 0)
            {
                fromBegIdx -= toBegIdx;
                toBegIdx = 0;
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

            if (fromBegIdx < toBegIdx)
            {
                // Shift right, so start at the right
                for (int fromIdx = fromEndIdx, toIdx = toEndIdx; fromIdx >= fromBegIdx; fromIdx--, toIdx--)
                    to[toIdx] = from[fromIdx];
            }
            else
            {
                // Shift left, so start at the left
                for (int fromIdx = fromBegIdx, toIdx = toBegIdx; fromIdx <= fromEndIdx; fromIdx++, toIdx++)
                    to[toIdx] = from[fromIdx];
            }
        }

        /// <summary>
        /// Fill the Buffer with the specified value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Buffer"></param>
        /// <param name="?"></param>
        public static void Fill<T>(this T[] buffer, T value)
        {
            for (int idx = 0; idx < buffer.Length; idx++)
                buffer[idx] = value;
        }
    }
}
