using System;

namespace ManagedBass.Pitch
{
    class CircularBuffer : IDisposable
    {
        int m_bufSize, m_begBufOffset, m_availBuf;
        float[] m_buffer;
        
        public CircularBuffer(int BufferCount) 
        {            
            m_bufSize = BufferCount;
            
            if (m_bufSize > 0)
                m_buffer = new float[m_bufSize];
        }

        public void Dispose() => m_buffer = null;

        /// <summary>
        /// Reset to the beginning of the Buffer
        /// </summary>
        public void Reset() => StartPosition = m_begBufOffset = m_availBuf = 0;
        
        /// <summary>
        /// Clear the Buffer
        /// </summary>
        public void Clear() => Array.Clear(m_buffer, 0, m_buffer.Length);

        /// <summary>
        /// Get or set the start position
        /// </summary>
        public long StartPosition { get; set; }

        /// <summary>
        /// Get or set the amount of avaliable space
        /// </summary>
        public int Available 
        {
            get { return m_availBuf; } 
            set { m_availBuf = Math.Min(value, m_bufSize); }
        }

        /// <summary>
        /// Write data into the Buffer
        /// </summary>
        public int Write(float[] m_pInBuffer, int count)
        {
            count = Math.Min(count, m_bufSize);

            var startPos = m_availBuf != m_bufSize ? m_availBuf : m_begBufOffset;
            var pass1Count = Math.Min(count, m_bufSize - startPos);
            var pass2Count = count - pass1Count;

            Array.Copy(m_pInBuffer, 0, m_buffer, startPos, pass1Count);

            if (pass2Count > 0) 
                Array.Copy(m_pInBuffer, pass1Count, m_buffer, 0, pass2Count);

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
                if (m_availBuf != m_bufSize)
                    StartPosition += pass2Count;  // first time wrap-around
                else StartPosition += count;

                m_begBufOffset = pass2Count;
                m_availBuf = m_bufSize;
            }

            return count;
        }

        /// <summary>
        /// Read from the Buffer
        /// </summary>
        public bool Read(float[] outBuffer, long startRead, int readCount)
        {
            var endRead = (int)(startRead + readCount);
            var endAvail = (int)(StartPosition + m_availBuf);

            if (startRead < StartPosition || endRead > endAvail) return false;

            var startReadPos = (int)((startRead - StartPosition + m_begBufOffset) % m_bufSize);
            var block1Samples = Math.Min(readCount, m_bufSize - startReadPos);
            var block2Samples = readCount - block1Samples;

            Array.Copy(m_buffer, startReadPos, outBuffer, 0, block1Samples);
            
            if (block2Samples > 0)
                Array.Copy(m_buffer, 0, outBuffer, block1Samples, block2Samples);

            return true;
        }
    }
}
