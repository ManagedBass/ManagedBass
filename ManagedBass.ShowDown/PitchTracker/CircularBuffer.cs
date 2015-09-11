using System;

namespace Pitch
{
    public class CircularBuffer<T> : IDisposable
    {
        int m_bufSize, m_begBufOffset, m_availBuf;
        T[] m_buffer;

        public CircularBuffer() { }
        public CircularBuffer(int bufCount) { Size = bufCount; }

        public void Dispose() { Size = 0; }

        /// <summary>
        /// Reset to the beginning of the Buffer
        /// </summary>
        public void Reset() { StartPosition = m_begBufOffset = m_availBuf = 0; }

        /// <summary>
        /// Set the Buffer to the specified size
        /// </summary>
        public int Size
        {
            get { return m_bufSize; }
            set
            {
                Reset();

                if (m_bufSize == value) return;

                if (m_buffer != null) m_buffer = null;

                m_bufSize = value;

                if (m_bufSize > 0) m_buffer = new T[m_bufSize];
            }
        }

        /// <summary>
        /// Clear the Buffer
        /// </summary>
        public void Clear() { m_buffer.Clear(); }

        /// <summary>
        /// Get or set the start position
        /// </summary>
        public long StartPosition { get; set; }

        /// <summary>
        /// Get the end position
        /// </summary>
        public long EndPosition { get { return StartPosition + m_availBuf; } }

        /// <summary>
        /// Get or set the amount of avaliable space
        /// </summary>
        public int Available { get { return m_availBuf; } set { m_availBuf = Math.Min(value, m_bufSize); } }

        /// <summary>
        /// Write data into the Buffer
        /// </summary>
        public int Write(T[] m_pInBuffer, int count)
        {
            count = Math.Min(count, m_bufSize);

            var startPos = m_availBuf != m_bufSize ? m_availBuf : m_begBufOffset;
            var pass1Count = Math.Min(count, m_bufSize - startPos);
            var pass2Count = count - pass1Count;

            Copy(m_pInBuffer, 0, m_buffer, startPos, pass1Count);

            if (pass2Count > 0) Copy(m_pInBuffer, pass1Count, m_buffer, 0, pass2Count);

            if (pass2Count == 0)
            {
                // did not wrap around
                if (m_availBuf != m_bufSize) m_availBuf += count;   // have never wrapped around
                else
                {
                    m_begBufOffset += count;
                    StartPosition += count;
                }
            }
            else
            {
                // wrapped around
                if (m_availBuf != m_bufSize) StartPosition += pass2Count;  // first time wrap-around
                else StartPosition += count;

                m_begBufOffset = pass2Count;
                m_availBuf = m_bufSize;
            }

            return count;
        }

        /// <summary>
        /// Read from the Buffer
        /// </summary>
        public bool Read(T[] outBuffer, long startRead, int readCount)
        {
            var endRead = (int)(startRead + readCount);
            var endAvail = (int)(StartPosition + m_availBuf);

            if (startRead < StartPosition || endRead > endAvail) return false;

            var startReadPos = (int)(((startRead - StartPosition) + m_begBufOffset) % m_bufSize);
            var block1Samples = Math.Min(readCount, m_bufSize - startReadPos);
            var block2Samples = readCount - block1Samples;

            Copy(m_buffer, startReadPos, outBuffer, 0, block1Samples);

            if (block2Samples > 0) Copy(m_buffer, 0, outBuffer, block1Samples, block2Samples);

            return true;
        }

        /// <summary>
        /// Copy the data from the source to the destination Buffer
        /// </summary>
        public static void Copy(T[] source, int srcStart, T[] destination, int dstStart, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length");

            if (source == null || source.Length < srcStart + length) throw new Exception("Source buffer is null or not large enough");

            if (destination == null || destination.Length < dstStart + length) throw new Exception("Destination buffer is null or not large enough");

            var srcIdx = srcStart;
            var dstIdx = dstStart;

            for (int idx = 0; idx < length; idx++) destination[dstIdx++] = source[srcIdx++];
        }
    }
}
