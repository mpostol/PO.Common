
using CAS.Lib.DeviceSimulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opc;
using Opc.Da;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.CommServer.DeviceSimulator.UnitTests
{
  [TestClass]
  public class DeviceSimulatorUnitTest
  {
    [TestMethod]
    public void getAddressSpaceTest()
    {
      using (Device _device = new Device())
      {
        IDevice _iDevice = _device as IDevice;
        Assert.IsNotNull(_iDevice);
        string[] _deviceItes = _iDevice.getAddressSpace;
        Assert.AreEqual<int>(UnitTestAssemblyInitialize.NotSortedAddressSpace.Count, _deviceItes.Length);
        CollectionAssert.AreNotEqual(UnitTestAssemblyInitialize.NotSortedAddressSpace.Select<UnitTestAssemblyInitialize.TestTagInDevice, string>(x => x.ToString()).ToArray<string>(),
                                     _deviceItes,
                                     $"{string.Join(",", UnitTestAssemblyInitialize.NotSortedAddressSpace)} # {string.Join(",", _deviceItes)}");
        string[] _expected = new string[] { "TestTagInDevice6", "TestTagInDevice3", "TestTagInDevice5", "TestTagInDevice8", "TestTagInDevice1", "TestTagInDevice0", "TestTagInDevice9", "TestTagInDevice2", "TestTagInDevice7", "TestTagInDevice4" };
        CollectionAssert.AreEqual(_expected, _deviceItes, $"{string.Join(",", UnitTestAssemblyInitialize.NotSortedAddressSpace)} # {string.Join(",", _deviceItes)}");
      }
    }
    [TestMethod]
    public void GetAvailablePropertiesTestMethod()
    {
      using (Device _device = new Device())
      {
        IDevice _iDevice = _device as IDevice;
        Assert.IsFalse(_iDevice.IsKnownItem("bleble"));
        Assert.IsTrue(_iDevice.IsKnownItem("TestTagInDevice6"));

        ItemPropertyCollection _properties = _iDevice.GetAvailableProperties("TestTagInDevice6", true);
        Assert.IsNotNull(_properties);
        Assert.AreEqual<int>(8, _properties.Count);
        Assert.AreEqual<string>(null, _properties.DiagnosticInfo, $"DiagnosticInfo: {_properties.DiagnosticInfo}");
        Assert.AreEqual<string>("TestTagInDevice6", _properties.ItemName, $"ItemName: {_properties.ItemName}");
        Assert.AreEqual<string>(null, _properties.ItemPath, $"ItemPath: {_properties.ItemPath}");
        Assert.AreEqual<ResultID>(ResultID.S_OK, _properties.ResultID, $"ResultID: {_properties.ItemPath}");

        Dictionary<PropertyID, ItemProperty> _sortedProperties = _properties.Cast<ItemProperty>().ToDictionary<ItemProperty, PropertyID>(x => x.ID);
        Assert.IsTrue(_sortedProperties.ContainsKey(Property.VALUE));

        int _valueIndex = _properties.IndexOf(_sortedProperties[Property.VALUE]);
        Assert.IsTrue(_valueIndex >= 0 && _valueIndex < _properties.Count, $"IndexOf: {Property.VALUE} is {_valueIndex}");
        Assert.AreSame(typeof(object), _properties[_valueIndex].DataType, $"DataType: {_properties[1].DataType}");
        Assert.AreEqual<string>("value", (string)_properties[1].Value, $"Current value: {_properties[1].Value}");
        Assert.AreEqual<PropertyID>(Property.VALUE, _properties[_valueIndex].ID, $"ID: {_properties[1].DataType}");

      }
    }
    [TestMethod]
    public void GetIndexedAddressSpaceTestMethod()
    {
      using (Device _device = new Device())
      {
        IDeviceIndexed _iDeviceIndexed = _device as IDeviceIndexed;
        Assert.IsNotNull(_iDeviceIndexed);
        ItemDsc[] _itemsDsc = _iDeviceIndexed.GetIndexedAddressSpace;
        Assert.IsNotNull(_itemsDsc);
        Assert.AreEqual<int>(UnitTestAssemblyInitialize.NotSortedAddressSpace.Count, _itemsDsc.Length);
        string[] _addressSpace = _itemsDsc.Select<ItemDsc, string>(x => x.itemID).ToArray<string>();
        CollectionAssert.AreEqual(UnitTestAssemblyInitialize.NotSortedAddressSpace.Select<UnitTestAssemblyInitialize.TestTagInDevice, string>(x => x.ItemID).ToList<string>(), _addressSpace, 
          $"{string.Join(",", UnitTestAssemblyInitialize.NotSortedAddressSpace.Select<UnitTestAssemblyInitialize.TestTagInDevice, string>(x => x.ItemID))} # {string.Join(",", _addressSpace)}");
      };
    }
    [TestMethod]
    public void IDeviceIndexedGetAvailablePropertiesTest()
    {
      using (Device _device = new Device())
      {
        IDeviceIndexed _iDeviceIndexed = _device as IDeviceIndexed;
        Assert.IsNotNull(_iDeviceIndexed);
        ItemPropertyCollection _properties = _iDeviceIndexed.GetAvailableProperties(0, true);
        Assert.IsNotNull(_properties);
        Assert.AreEqual<int>(8, _properties.Count);
        Assert.AreEqual<string>(null, _properties.DiagnosticInfo, $"DiagnosticInfo: {_properties.DiagnosticInfo}");
        Assert.AreEqual<string>("TestTagInDevice0", _properties.ItemName, $"ItemName: {_properties.ItemName}");
        Assert.AreEqual<string>(null, _properties.ItemPath, $"ItemPath: {_properties.ItemPath}");
        Assert.AreEqual<ResultID>(ResultID.S_OK, _properties.ResultID, $"ResultID: {_properties.ItemPath}");
      };

    }
    [TestMethod]
    public void CreateTagInDeviceTestMethod()
    {
      I4UAServer _addedItem = Device.Find("TestTagInDevice6");
      Assert.IsNotNull(_addedItem);
    }
    [TestMethod]
    public void GetAddressSpaceStaticTest()
    {
      string[] _deviceItes = Device.getAddressSpaceStatic();
      Assert.AreEqual<int>(UnitTestAssemblyInitialize.NotSortedAddressSpace.Count, _deviceItes.Length);
      CollectionAssert.AreEqual(UnitTestAssemblyInitialize.ExpectedAddressSpace, _deviceItes, $"{string.Join(",", UnitTestAssemblyInitialize.NotSortedAddressSpace)} # {string.Join(",", _deviceItes)}");
    }
  }
}
