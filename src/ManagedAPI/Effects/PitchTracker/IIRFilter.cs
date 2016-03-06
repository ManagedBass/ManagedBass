using System;

namespace ManagedBass.Effects
{
    enum IIRFilterType { LP, HP }
    
    /// <summary>
    /// Infinite impulse response filter (old style analog filters)
    /// </summary>
    class IIRFilter
    {
        const int kHistMask = 31, kHistSize = 32;

        int m_order;
        IIRFilterType m_filterType;

        float m_fp1, m_fp2, m_fN, m_sampleRate;
        double[] m_real, m_imag, m_z, m_aCoeff, m_bCoeff, m_inHistory, m_outHistory;
        int m_histIdx;
        bool m_invertDenormal;

        public IIRFilter(IIRFilterType Type, int Order, float SampleRate) 
        {
            m_filterType = Type;

            m_order = Math.Min(16, Math.Max(1, Math.Abs(Order)));
            
            m_sampleRate = SampleRate;
            m_fN = 0.5f * m_sampleRate;

            Design();
        }

        /// <summary>
        /// Returns true if all the filter parameters are valid
        /// </summary>
        public bool FilterValid
        {
            get
            {
                if (m_order < 1 || m_order > 16 ||
                    m_sampleRate <= 0 ||
                    m_fN <= 0)
                    return false;

                switch (m_filterType)
                {
                    case IIRFilterType.LP:
                        if (m_fp2 <= 0) return false;
                        break;
                        
                    case IIRFilterType.HP:
                        if (m_fp1 <= 0) return false;
                        break;
                }

                return true;
            }
        }
        
        public float FreqLow
        {
            get { return m_fp1; }
            set
            {
                m_fp1 = value;
                Design();
            }
        }

        public float FreqHigh
        {
            get { return m_fp2; }
            set
            {
                m_fp2 = value;
                Design();
            }
        }
        
        bool IsOdd(int n) => (n & 1) == 1;
        
        double Sqr(double value) => value * value;

        /// <summary>
        /// Determines poles and zeros of IIR filter
        /// based on bilinear transform method
        /// </summary>
        void LocatePolesAndZeros()
        {
            m_real = new double[m_order + 1];
            m_imag = new double[m_order + 1];
            m_z = new double[m_order + 1];
            double ln10 = Math.Log(10.0);

            // Butterworth, Chebyshev parameters
            int n = m_order;

            int ir = n % 2;
            int n1 = n + ir;
            int n2 = (3 * n + ir) / 2 - 1;
            double f1;

            switch (m_filterType)
            {
                case IIRFilterType.LP:
                    f1 = m_fp2;
                    break;

                case IIRFilterType.HP:
                    f1 = m_fN - m_fp1;
                    break;
                    
                default:
                    f1 = 0.0;
                    break;
            }

            double tanw1 = Math.Tan(0.5 * Math.PI * f1 / m_fN);
            double tansqw1 = Sqr(tanw1);

            // Real and Imaginary parts of low-pass poles
            double t, r = 1.0, i = 1.0;

            for (int k = n1; k <= n2; k++)
            {
                t = 0.5 * (2 * k + 1 - ir) * Math.PI / (double)n;

                double b3 = 1.0 - 2.0 * tanw1 * Math.Cos(t) + tansqw1;
                r = (1.0 - tansqw1) / b3;
                i = 2.0 * tanw1 * Math.Sin(t) / b3;

                int m = 2 * (n2 - k) + 1;
                m_real[m + ir] = r;
                m_imag[m + ir] = Math.Abs(i);
                m_real[m + ir + 1] = r;
                m_imag[m + ir + 1] = -Math.Abs(i);
            }

            if (IsOdd(n))
            {
                r = (1.0 - tansqw1) / (1.0 + 2.0 * tanw1 + tansqw1);
                
                m_real[1] = r;
                m_imag[1] = 0.0;
            }

            switch (m_filterType)
            {
                case IIRFilterType.LP:
                    for (int m = 1; m <= n; m++)
                        m_z[m] = -1.0;
                    break;

                case IIRFilterType.HP:
                    // Low-pass to high-pass transformation
                    for (int m = 1; m <= n; m++)
                    {
                        m_real[m] = -m_real[m];
                        m_z[m] = 1.0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Calculate all the values
        /// </summary>
        public void Design()
        {
            if (!this.FilterValid)
                return;

            m_aCoeff = new double[m_order + 1];
            m_bCoeff = new double[m_order + 1];
            m_inHistory = new double[kHistSize];
            m_outHistory = new double[kHistSize];

            double[] newA = new double[m_order + 1];
            double[] newB = new double[m_order + 1];

            // Find filter poles and zeros
            LocatePolesAndZeros();

            // Compute filter coefficients from pole/zero values
            m_aCoeff[0] = 1.0;
            m_bCoeff[0] = 1.0;

            for (int i = 1; i <= m_order; i++)
            {
                m_aCoeff[i] = 0.0;
                m_bCoeff[i] = 0.0;
            }

            int k = 0;
            int n = m_order;
            int pairs = n / 2;

            if (IsOdd(m_order))
            {
                // First subfilter is first order
                m_aCoeff[1] = -m_z[1];
                m_bCoeff[1] = -m_real[1];
                k = 1;
            }

            for (int p = 1; p <= pairs; p++)
            {
                int m = 2 * p - 1 + k;
                double alpha1 = -(m_z[m] + m_z[m + 1]);
                double alpha2 = m_z[m] * m_z[m + 1];
                double beta1 = -2.0 * m_real[m];
                double beta2 = Sqr(m_real[m]) + Sqr(m_imag[m]);

                newA[1] = m_aCoeff[1] + alpha1 * m_aCoeff[0];
                newB[1] = m_bCoeff[1] + beta1 * m_bCoeff[0];

                for (int i = 2; i <= n; i++)
                {
                    newA[i] = m_aCoeff[i] + alpha1 * m_aCoeff[i - 1] + alpha2 * m_aCoeff[i - 2];
                    newB[i] = m_bCoeff[i] + beta1 * m_bCoeff[i - 1] + beta2 * m_bCoeff[i - 2];
                }

                for (int i = 1; i <= n; i++)
                {
                    m_aCoeff[i] = newA[i];
                    m_bCoeff[i] = newB[i];
                }
            }

            // Ensure the filter is normalized
            FilterGain(1000);
        }

        /// <summary>
        /// Reset the history buffers
        /// </summary>
        public void Reset()
        {
            if (m_inHistory != null) 
                Array.Clear(m_inHistory, 0, m_inHistory.Length);

            if (m_outHistory != null) 
                Array.Clear(m_outHistory, 0, m_outHistory.Length);

            m_histIdx = 0;
        }
        
        /// <summary>
        /// Apply the filter to the Buffer
        /// </summary>
        public void FilterBuffer(float[] srcBuf, long srcPos, float[] dstBuf, long dstPos, long nLen)
        {
            const double kDenormal = 0.000000000000001;
            double denormal = m_invertDenormal ? -kDenormal : kDenormal;
            m_invertDenormal = !m_invertDenormal;

            for (int sampleIdx = 0; sampleIdx < nLen; sampleIdx++)
            {
                double sum = 0;

                m_inHistory[m_histIdx] = srcBuf[srcPos + sampleIdx] + denormal;

                for (int idx = 0; idx < m_aCoeff.Length; idx++) sum += m_aCoeff[idx] * m_inHistory[(m_histIdx - idx) & kHistMask];

                for (int idx = 1; idx < m_bCoeff.Length; idx++) sum -= m_bCoeff[idx] * m_outHistory[(m_histIdx - idx) & kHistMask];

                m_outHistory[m_histIdx] = sum;
                m_histIdx = (m_histIdx + 1) & kHistMask;
                dstBuf[dstPos + sampleIdx] = (float)sum;
            }
        }

        /// <summary>
        /// Get the gain at the specified number of frequency points
        /// </summary>
        /// <param name="freqPoints"></param>
        /// <returns></returns>
        public float[] FilterGain(int freqPoints)
        {
            // Filter gain at uniform frequency intervals
            float[] g = new float[freqPoints];
            double theta, s, c, sac, sas, sbc, sbs;
            float gMax = -100;
            float sc = 10 / (float)Math.Log(10);
            double t = Math.PI / (freqPoints - 1);

            for (int i = 0; i < freqPoints; i++)
            {
                theta = i * t;

                if (i == 0) theta = Math.PI * 0.0001;

                if (i == freqPoints - 1) theta = Math.PI * 0.9999;

                sac = sas = sbc = sbs = 0;

                for (int k = 0; k <= m_order; k++)
                {
                    c = Math.Cos(k * theta);
                    s = Math.Sin(k * theta);
                    sac += c * m_aCoeff[k];
                    sas += s * m_aCoeff[k];
                    sbc += c * m_bCoeff[k];
                    sbs += s * m_bCoeff[k];
                }

                g[i] = sc * (float)Math.Log((Sqr(sac) + Sqr(sas)) / (Sqr(sbc) + Sqr(sbs)));
                gMax = Math.Max(gMax, g[i]);
            }

            // Normalize to 0 dB maximum gain
            for (int i = 0; i < freqPoints; i++) g[i] -= gMax;

            // Normalize numerator (a) coefficients
            float normFactor = (float)Math.Pow(10.0, -0.05 * gMax);

            for (int i = 0; i <= m_order; i++) m_aCoeff[i] *= normFactor;

            return g;
        }
    }
}
