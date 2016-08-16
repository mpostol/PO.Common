using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAS.Lib.CommonBus.Tests
{
  [TestClass]
  public class CommonBusUnitTest1
  {
    [TestMethod]
    public void CommonBusUnitTestMethod1()
    {
      DataProviderID _provider = new ConcreteDataProviderID();
      Assert.IsNotNull(_provider);
      Assert.AreEqual<int>(5, _provider.GetDataProviderDescription.Version.Major);
    }
  }
  internal class ConcreteDataProviderID : DataProviderID
  {

    protected override void WriteSettings(System.Xml.XmlWriter pSettings)
    {
      throw new NotImplementedException();
    }

    protected override void ReadSettings(System.Xml.XmlReader pSettings)
    {
      throw new NotImplementedException();
    }

    public override ApplicationLayer.IApplicationLayerMaster GetApplicationLayerMaster(CAS.Lib.RTLib.Management.IProtocolParent pStatistic, CommonBusControl pParent)
    {
      throw new NotImplementedException();
    }

    public override IAddressSpaceDescriptor[] GetAvailiableAddressspaces()
    {
      throw new NotImplementedException();
    }

    public override IItemDefaultSettings GetItemDefaultSettings(short AddressSpaceIdentifier, ulong AddressInTheAddressSpace)
    {
      throw new NotImplementedException();
    }
  }
}
