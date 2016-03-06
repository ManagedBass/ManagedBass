using System;

namespace ManagedBass.Effects
{
    /// <summary>
    /// Tracks pitch
    /// </summary>
    public class PitchTracker
    {
        #region Fields
        const int kOctaveSteps = 96;
        const int kStepOverlap = 4;
        public const float MinimumDetectedFrequency = 50;               // A1, Midi note 33, 55.0Hz
        public const float MaximumDetectedFrequency = 1600;             // A#6. Midi note 92
        const int kStartCircular = 40;		        // how far into the sample Buffer do we start checking (allow for filter settling)
        const float kDetectOverlapSec = 0.005f;
        const float kMaxOctaveSecRate = 10.0f;

        const float kAvgOffset = 0.005f;	        // time offset between pitch averaging values
        const int kAvgCount = 1;			        // number of average pitch samples to take
        const float kCircularBufSaveTime = 1.0f;    // Amount of samples to store in the history Buffer

        PitchProcessor m_dsp;
        CircularBuffer m_circularBufferLo;
        CircularBuffer m_circularBufferHi;
        double m_sampleRate;
        float m_detectLevelThreshold = 0.01f;       // -40dB
        int m_pitchRecordsPerSecond = 50;           // default is 50, or one record every 20ms

        float[] m_pitchBufLo, m_pitchBufHi;
        int m_pitchBufSize, m_samplesPerPitchBlock, m_curPitchIndex;
        long m_curPitchSamplePos;

        int m_detectOverlapSamples;
        float m_maxOverlapDiff;

        IIRFilter m_iirFilterLoLo, m_iirFilterLoHi, m_iirFilterHiLo, m_iirFilterHiHi;
        #endregion

        public PitchTracker(double SampleRate = 44100)
        {
            this.SampleRate = SampleRate;
        }

        #region Properties
        /// <summary>
        /// Set the sample rate
        /// </summary>
        public double SampleRate
        {
            set
            {
                if (m_sampleRate == value)
                    return;

                m_sampleRate = value;
                Setup();
            }
        }

        /// <summary>
        /// Set the detect level threshold, The value must be between 0.0001f and 1.0f (-80 dB to 0 dB)
        /// </summary>
        public float DetectLevelThreshold
        {
            set
            {
                var newValue = Math.Max(0.0001f, Math.Min(1.0f, value));

                if (m_detectLevelThreshold == newValue) 
                    return;

                m_detectLevelThreshold = newValue;
                Setup();
            }
        }
        
        /// <summary>
        /// Return the samples per pitch block
        /// </summary>
        public int SamplesPerPitchBlock => m_samplesPerPitchBlock;

        /// <summary>
        /// Get or set the number of pitch records per second (default is 50, or one record every 20ms)
        /// </summary>
        public int PitchRecordsPerSecond
        {
            get { return m_pitchRecordsPerSecond; }
            set
            {
                m_pitchRecordsPerSecond = Math.Max(1, Math.Min(100, value));
                Setup();
            }
        }

        /// <summary>
        /// Get the current pitch position
        /// </summary>
        public long CurrentPitchSamplePosition => m_curPitchSamplePos;

        /// <summary>
        /// Get the frequency step
        /// </summary>
        public static double FrequencyStep => Math.Pow(2.0, 1.0 / kOctaveSteps);

        /// <summary>
        /// Get the number of samples that the detected pitch is offset from the Input Buffer.
        /// This is just an estimate to sync up the samples and detected pitch
        /// </summary>
        public int DetectSampleOffset => (m_pitchBufSize + m_detectOverlapSamples) / 2;
        #endregion

        #region Methods
        /// <summary>
        /// Reset the pitch tracker. Call this when the sample position is
        /// not consecutive from the previous position
        /// </summary>
        public void Reset()
        {
            m_curPitchSamplePos = m_curPitchIndex = 0;
            m_iirFilterLoLo.Reset();
            m_iirFilterLoHi.Reset();
            m_iirFilterHiLo.Reset();
            m_iirFilterHiHi.Reset();
            m_circularBufferLo.Reset();
            m_circularBufferLo.Clear();
            m_circularBufferHi.Reset();
            m_circularBufferHi.Clear();
            Array.Clear(m_pitchBufLo, 0, m_pitchBufLo.Length);
            Array.Clear(m_pitchBufHi, 0, m_pitchBufHi.Length);

            m_circularBufferLo.StartPosition = -m_detectOverlapSamples;
            m_circularBufferLo.Available = m_detectOverlapSamples;
            m_circularBufferHi.StartPosition = -m_detectOverlapSamples;
            m_circularBufferHi.Available = m_detectOverlapSamples;
        }

        /// <summary>
        /// Process the passed in Buffer of data. During this call, the PitchDetected event will
        /// be fired zero or more times, depending how many pitch records will fit in the new
        /// and previously cached Buffer.
        ///
        /// This means that there is no size restriction on the Buffer that is passed into ProcessBuffer.
        /// For instance, ProcessBuffer can be called with one very large Buffer that contains all of the
        /// audio to be processed (many PitchDetected events will be fired), or just a small Buffer at
        /// a time which is more typical for realtime applications. In the latter case, the PitchDetected
        /// event might not be fired at all since additional calls must first be made to accumulate enough
        /// data do another pitch detect operation.
        /// </summary>
        /// <param name="inBuffer">Input Buffer. Samples must be in the range -1.0 to 1.0</param>
        /// <param name="sampleCount">Number of samples to process. Zero means all samples in the Buffer</param>
        public void ProcessBuffer(float[] inBuffer, int sampleCount = 0)
        {
            if (inBuffer == null) throw new ArgumentNullException("inBuffer", "Input buffer cannot be null");

            var samplesProcessed = 0;
            var srcLength = sampleCount == 0 ? inBuffer.Length : Math.Min(sampleCount, inBuffer.Length);

            while (samplesProcessed < srcLength)
            {
                int frameCount = Math.Min(srcLength - samplesProcessed, m_pitchBufSize + m_detectOverlapSamples);

                m_iirFilterLoLo.FilterBuffer(inBuffer, samplesProcessed, m_pitchBufLo, 0, frameCount);
                m_iirFilterLoHi.FilterBuffer(m_pitchBufLo, 0, m_pitchBufLo, 0, frameCount);

                m_iirFilterHiLo.FilterBuffer(inBuffer, samplesProcessed, m_pitchBufHi, 0, frameCount);
                m_iirFilterHiHi.FilterBuffer(m_pitchBufHi, 0, m_pitchBufHi, 0, frameCount);

                m_circularBufferLo.Write(m_pitchBufLo, frameCount);
                m_circularBufferHi.Write(m_pitchBufHi, frameCount);

                // Loop while there is enough samples in the circular Buffer
                while (m_circularBufferLo.Read(m_pitchBufLo, m_curPitchSamplePos, m_pitchBufSize + m_detectOverlapSamples))
                {
                    float pitch1, pitch2 = 0, detectedPitch = 0;

                    m_circularBufferHi.Read(m_pitchBufHi, m_curPitchSamplePos, m_pitchBufSize + m_detectOverlapSamples);

                    pitch1 = m_dsp.DetectPitch(m_pitchBufLo, m_pitchBufHi, m_pitchBufSize);

                    if (pitch1 > 0.0f)
                    {
                        // Shift the buffers left by the overlapping amount
                        SafeCopy(m_pitchBufLo, m_pitchBufLo, m_detectOverlapSamples, 0, m_pitchBufSize);
                        SafeCopy(m_pitchBufHi, m_pitchBufHi, m_detectOverlapSamples, 0, m_pitchBufSize);

                        pitch2 = m_dsp.DetectPitch(m_pitchBufLo, m_pitchBufHi, m_pitchBufSize);

                        if (pitch2 > 0.0f)
                        {
                            float fDiff = Math.Max(pitch1, pitch2) / Math.Min(pitch1, pitch2) - 1.0f;

                            if (fDiff < m_maxOverlapDiff) detectedPitch = (pitch1 + pitch2) * 0.5f;
                        }
                    }

                    // Log the pitch record
                    PitchDetected?.Invoke(new PitchRecord(detectedPitch));

                    m_curPitchSamplePos += m_samplesPerPitchBlock;
                    m_curPitchIndex++;
                }

                samplesProcessed += frameCount;
            }
        }

        /// <summary>
        /// Copy the values from one Buffer to a different or the same Buffer. 
        /// It is safe to copy to the same Buffer, even if the areas overlap
        /// </summary>
        static void SafeCopy<T>(T[] from, T[] to, int fromStart, int toStart, int length)
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
        /// Setup
        /// </summary>
        void Setup()
        {
            if (m_sampleRate < 1) return;

            m_dsp = new PitchProcessor(m_sampleRate, MinimumDetectedFrequency, MaximumDetectedFrequency, m_detectLevelThreshold);

            m_iirFilterLoLo = new IIRFilter(IIRFilterType.HP, 5, (float)m_sampleRate);
            m_iirFilterLoLo.FreqLow = 45.0f;

            m_iirFilterLoHi = new IIRFilter(IIRFilterType.LP, 5, (float)m_sampleRate);
            m_iirFilterLoHi.FreqHigh = 280.0f;

            m_iirFilterHiLo = new IIRFilter(IIRFilterType.HP, 5, (float)m_sampleRate);
            m_iirFilterHiLo.FreqLow = 45.0f;

            m_iirFilterHiHi = new IIRFilter(IIRFilterType.LP, 5, (float)m_sampleRate);
            m_iirFilterHiHi.FreqHigh = 1500.0f;

            m_detectOverlapSamples = (int)(kDetectOverlapSec * m_sampleRate);
            m_maxOverlapDiff = kMaxOctaveSecRate * kDetectOverlapSec;

            m_pitchBufSize = (int)(((1.0f / (float)MinimumDetectedFrequency) * 2.0f + ((kAvgCount - 1) * kAvgOffset)) * m_sampleRate) + 16;
            m_pitchBufLo = new float[m_pitchBufSize + m_detectOverlapSamples];
            m_pitchBufHi = new float[m_pitchBufSize + m_detectOverlapSamples];
            m_samplesPerPitchBlock = (int)Math.Round(m_sampleRate / m_pitchRecordsPerSecond);

            m_circularBufferLo = new CircularBuffer((int)(kCircularBufSaveTime * m_sampleRate + 0.5f) + 10000);
            m_circularBufferHi = new CircularBuffer((int)(kCircularBufSaveTime * m_sampleRate + 0.5f) + 10000);
        }
        #endregion
        
        public event Action<PitchRecord> PitchDetected;
    }
}
