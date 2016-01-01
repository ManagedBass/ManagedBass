using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManagedBass.Tests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void SampleAndBytes()
        {
            long bytes = new Random().Next(1, int.MaxValue);
            
            Assert.AreEqual<long>(bytes, Extensions.SamplesToBytes(Extensions.BytesToSamples(bytes, 16, 2), 16, 2));
        }
    }
}
