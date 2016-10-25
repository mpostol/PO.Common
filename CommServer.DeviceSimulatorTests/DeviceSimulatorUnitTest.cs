
using CAS.Lib.DeviceSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.CommServer.DeviceSimulator.UnitTests
{
  [TestClass]
  public class DeviceSimulatorUnitTest
  {
    [TestMethod]
    public void _iDeviceTest()
    {
      using (Device _device = new Device())
      {
        IDevice _iDevice = _device as IDevice;
        Assert.IsNotNull(_iDevice);
        string[] _addresSpace = _iDevice.getAddressSpace;
        Assert.IsNotNull(_addresSpace);
        Assert.IsTrue(_addresSpace.Length == 0);
        Assert.IsFalse(_iDevice.IsKnownItem("bleble"));
        IDeviceIndexed _iDeviceIndexed = _device as IDeviceIndexed;
        Assert.IsNotNull(_iDeviceIndexed);
        ItemDsc[] _itemsDsc = _iDeviceIndexed.GetIndexedAddressSpace;
        Assert.IsNotNull(_itemsDsc);
        Assert.IsTrue(_itemsDsc.Length == 0);
      }
    }
  }
}
