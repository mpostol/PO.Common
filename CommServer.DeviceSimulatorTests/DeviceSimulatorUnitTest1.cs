
using CAS.Lib.DeviceSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CommServer.DeviceSimulator.UnitTests
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
