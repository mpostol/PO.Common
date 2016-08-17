using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.Lib.CommonBus;
using CAS.Lib.RTLib.Management;

namespace CAS.CommServer.CommonBus.UnitTests
{
  [TestClass]
  public class DataProviderIDUnitTest
  {
    [TestMethod]
    public void DataProviderIDTestMethod()
    {
      DataProviderID _provider = new ConcreteDataProviderID();
      Assert.IsNotNull(_provider);
      Assert.AreEqual<int>(5, _provider.GetDataProviderDescription.Version.Major);
    }

    private class ConcreteDataProviderID : DataProviderID
    {

      protected override void WriteSettings(System.Xml.XmlWriter pSettings)
      {
        throw new NotImplementedException();
      }

      protected override void ReadSettings(System.Xml.XmlReader pSettings)
      {
        throw new NotImplementedException();
      }

      public override Lib.CommonBus.ApplicationLayer.IApplicationLayerMaster GetApplicationLayerMaster(IProtocolParent pStatistic, CommonBusControl pParent)
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
}