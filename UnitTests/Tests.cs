using ManagedBass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class DeviceTests
    {
        [TestMethod]
        public void DeviceCount()
        {
            Assert.AreEqual<int>(PlaybackDevice.Count, WasapiPlaybackDevice.DeviceCount, "Playback Device Count by Bass doesn't agree with WASAPI");
            Assert.AreEqual<int>(RecordingDevice.Count, WasapiRecordingDevice.DeviceCount, "Recording Device Count by Bass doesn't agree with WASAPI");
        }
    }

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
