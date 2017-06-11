
using CAS.Lib.DeviceSimulator;
using CAS.Lib.RTLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opc.Da;
using System;
using System.Collections.Generic;

namespace CAS.CommServer.DeviceSimulator.UnitTests
{
  [TestClass]
  public class UnitTestAssemblyInitialize
  {

    [AssemblyInitialize]
    public static void TestMethod1(TestContext context)
    {
      List<string> _tags = new List<string>();
      for (int i = 0; i < 10; i++)
      {
        string _tagName = $"TestTagInDevice{i}";
        _tags.Add(_tagName);
        NotSortedAddressSpace.Add(new TestTagInDevice(_tagName, "value", (qualityBits)Int32.MaxValue, ItemAccessRights.ReadOnly, typeof(string)));
      }
    }
    internal static List<TestTagInDevice> NotSortedAddressSpace = new List<TestTagInDevice>();
    internal static string[] ExpectedAddressSpace { get; } =
      new string[] { "TestTagInDevice6", "TestTagInDevice3", "TestTagInDevice5", "TestTagInDevice8", "TestTagInDevice1", "TestTagInDevice0", "TestTagInDevice9", "TestTagInDevice2", "TestTagInDevice7", "TestTagInDevice4" };
    internal class TestTagInDevice : Device.TagInDevice
    {
      public TestTagInDevice(string itemID) : base(itemID) { }
      public TestTagInDevice(string itemID, object value, qualityBits InitialQuality, ItemAccessRights AccessRights, Type tagCanonicalType) : base(itemID, value, InitialQuality, AccessRights, tagCanonicalType){}
      /// <summary>
      /// Read new value directly from device
      /// </summary>
      /// <param name="data">new received value</param>
      /// <returns>true if success</returns>
      protected override bool ReadRemote(out object data)
      {
        throw new NotImplementedException();
      }
      /// <summary>
      /// Send new value to a device
      /// </summary>
      /// <param name="data">new value to be set</param>
      /// <returns>true if success</returns>
      /// <exception cref="System.NotImplementedException"></exception>
      protected override bool UpdateRemote(object data)
      {
        throw new NotImplementedException();
      }
     
    }

  }
}
