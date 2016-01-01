using ManagedBass.Dynamics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ManagedBass.Tests
{
    [TestClass]
    public class DeviceTests
    {
        [TestMethod]
        public void PlaybackInit()
        {
            int i;

            do { i = new Random().Next(Bass.DeviceCount); }
            while (!Bass.GetDeviceInfo(i).IsEnabled);

            if (Bass.GetDeviceInfo(i).IsInitialized) Bass.Free(i);

            Assert.IsTrue(Bass.Initialize(i), "Cannot Init a random device - {0}", Bass.LastError);

            Assert.IsTrue(Bass.Free(i), "Cannot free initialized device - {0}", Bass.LastError);
        }

        [TestMethod]
        public void RecordInit()
        {
            int i;

            do { i = new Random().Next(Bass.RecordingDeviceCount); }
            while (!Bass.GetRecordingDeviceInfo(i).IsEnabled);

            if (Bass.GetRecordingDeviceInfo(i).IsInitialized)
            {
                Bass.CurrentRecordingDevice = i;
                Bass.RecordFree();
            }

            Assert.IsTrue(Bass.RecordInit(i), "Cannot init a random recording device - {0}", Bass.LastError);

            Assert.IsTrue(Bass.RecordFree(), "Cannot free initialized recording device - {0}", Bass.LastError);
        }

        [TestMethod]
        public void WasapiInit()
        {
            int i;

            do { i = new Random().Next(BassWasapi.DeviceCount); }
            while (!BassWasapi.GetDeviceInfo(i).IsEnabled);

            if (BassWasapi.GetDeviceInfo(i).IsInitialized)
            {
                BassWasapi.CurrentDevice = i;
                BassWasapi.Free();
            }

            var proc = new WasapiProcedure((b, c, u) => c);

            Assert.IsTrue(BassWasapi.Init(i, 44100, 2, WasapiInitFlags.Shared, 0, 0, proc), "Cannot Init a random WASAPI device - {0}", Bass.LastError);

            Assert.IsTrue(BassWasapi.Free(), "Cannot free initialized WASAPI device - {0}", Bass.LastError);
        }
    }    
}
