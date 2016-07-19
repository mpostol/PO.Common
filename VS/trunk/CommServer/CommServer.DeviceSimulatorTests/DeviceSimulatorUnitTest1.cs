using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.Lib.DeviceSimulator.Tests
{
  [TestClass]
  public class DeviceSimulatorUnitTest1
  {
    [TestMethod]
    public void DeviceSimulatorTestMethod1()
    {
      Device _device = new Device();
      Assert.IsNotNull(_device);
    }
  }
}
