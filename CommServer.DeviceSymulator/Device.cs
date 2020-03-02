//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using CAS.Lib.RTLib;
using Opc;
using Opc.Da;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// Item address coupled with it's internal index
  /// </summary>
  [Serializable]
  public struct ItemDsc
  {

    /// <summary>
    /// Item identifier
    /// </summary>
    public string itemID;
    /// <summary>
    /// Item index
    /// </summary>
    public ushort itemIdx;
  }
  /// <summary>
  /// An interface for an abstract device extended to allow indexing items.
  /// </summary>
  public interface IDeviceIndexed
  {
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    /// <param name="itemIdx">The item index.</param>
    /// <param name="returnValues">if set to <c>true</c> the function is returning values of the properties.</param>
    /// <returns></returns>
    ItemPropertyCollection GetAvailableProperties(ushort itemIdx, bool returnValues);
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    ItemPropertyCollection GetAvailableProperties(ushort itemIdx, PropertyID[] propertyIDs, bool returnValues);
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    ItemValueResult Read(ushort itemIdx, PropertyID propertyID);
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    IdentifiedResult Write(ushort itemIdx, PropertyID propertyID, ItemValue value);
    /// <summary>
    /// Get all Items created in the device with indexes
    /// </summary>
    ItemDsc[] GetIndexedAddressSpace { get; }
  }
  /// <summary>
  /// A class that represents a device with a set of simulated data points.
  /// </summary>
  [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
  [ComVisible(false)]
  [Serializable]
  //TODO CommServer.DeviceSimulator - remove dependency on Remoting #15
  public class Device : MarshalByRefObject, IDeviceIndexed, IDevice, IDisposable
  {
    #region public members
    /// <summary>
    /// Exception for the Device class
    /// </summary>
    public class DeviceException : Exception
    {
      internal DeviceException(string msg) : base(msg) { }
    }
    #endregion

    #region IDisposable Members
    /// <summary>
    /// Stops all threads and releases all resources.
    /// </summary>
    public void Dispose() { }
    #endregion

    #region IDevice Members
    /// <summary>
    /// Returns whether item id belongs to the device's address space.
    /// </summary>
    bool IDevice.IsKnownItem(string itemID)
    {
      lock (m_items)
        return m_items.Contains(itemID);
    }
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    ItemPropertyCollection IDevice.GetAvailableProperties(string itemID, bool returnValues)
    {
      // initialize result.
      ItemPropertyCollection properties = new ItemPropertyCollection
      {
        ItemName = itemID,
        ItemPath = null,
        ResultID = Opc.ResultID.S_OK,
        DiagnosticInfo = null
      };
      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID(itemID);
      if (item == null)
      {
        properties.ResultID = ResultID.Da.E_UNKNOWN_ITEM_NAME;
        return properties;
      }
      // fetch properties.
      return item.GetAvailableProperties(returnValues);
    }
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    ItemPropertyCollection IDevice.GetAvailableProperties(string itemID, PropertyID[] propertyIDs, bool returnValues)
    {
      // initialize result.
      ItemPropertyCollection properties = new ItemPropertyCollection
      {
        ItemName = itemID,
        ItemPath = null,
        ResultID = Opc.ResultID.S_OK,
        DiagnosticInfo = null
      };
      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID(itemID);
      if (item == null)
      {
        properties.ResultID = ResultID.Da.E_UNKNOWN_ITEM_NAME;
        return properties;
      }
      // fetch properties.
      return item.GetAvailableProperties(propertyIDs, returnValues);
    }
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    ItemValueResult IDevice.Read(string itemID, PropertyID propertyID)
    {
      // initialize result.
      ItemValueResult result = new ItemValueResult
      {
        ItemName = itemID,
        ItemPath = null,
        ResultID = ResultID.S_OK,
        DiagnosticInfo = null
      };
      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID(itemID);
      if (item == null)
      {
        result.ResultID = ResultID.Da.E_UNKNOWN_ITEM_NAME;
        return result;
      }
      // read value.
      return item.Read(propertyID);
    }
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    IdentifiedResult IDevice.Write(string itemID, PropertyID propertyID, ItemValue value)
    {
      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID(itemID);
      if (item == null)
        return new IdentifiedResult(itemID, ResultID.Da.E_UNKNOWN_ITEM_NAME);
      // write value.
      return item.Write(propertyID, value);
    }
    string[] IDevice.getAddressSpace
    {
      get
      {
        lock (m_items)
        {
          string[] addSpce = new string[m_items.Count];
          short ix = 0;
          foreach (DeviceItem elId in m_items.Values)
            addSpce[ix++] = elId.ItemID;
          return addSpce;
        }
      }
    }//getAddressSpace
    #endregion

    #region IDeviceIndexed
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    ItemPropertyCollection IDeviceIndexed.GetAvailableProperties(ushort itemIdx, bool returnValues)
    {
      if (itemIdx >= m_tagIdx)
        throw new DeviceException("Item index out of range");
      // fetch properties.
      return GetItemFromItemsByItemIDx(itemIdx).GetAvailableProperties(returnValues);
    }
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    ItemPropertyCollection IDeviceIndexed.GetAvailableProperties(ushort itemIdx, PropertyID[] propertyIDs, bool returnValues)
    {
      if (itemIdx >= m_tagIdx)
        throw new DeviceException("Item index out of range");
      // fetch properties.
      return GetItemFromItemsByItemIDx(itemIdx).GetAvailableProperties(propertyIDs, returnValues);
    }
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    ItemValueResult IDeviceIndexed.Read(ushort itemIdx, PropertyID propertyID)
    {
      if (itemIdx >= m_tagIdx)
        throw new DeviceException("Item index out of range");
      // read value.
      return GetItemFromItemsByItemIDx(itemIdx).Read(propertyID);
    }
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    IdentifiedResult IDeviceIndexed.Write(ushort itemIdx, PropertyID propertyID, ItemValue value)
    {
      if (itemIdx >= m_tagIdx)
        throw new DeviceException("Item index out of range");
      // write value.
      return GetItemFromItemsByItemIDx(itemIdx).Write(propertyID, value);
    }
    /// <summary>
    /// Get all Items created in the device with indexes
    /// </summary>
    ItemDsc[] IDeviceIndexed.GetIndexedAddressSpace
    {
      get
      {
        lock (m_items)
        {
          ItemDsc[] addSpce = new ItemDsc[m_tagIdx];
          for (ushort ix = 0; ix < m_tagIdx; ix++)
          {
            addSpce[ix].itemID = m_itemsIndexTab[ix].ItemID;
            addSpce[ix].itemIdx = ix;
          }
          return addSpce;
        }
      }
    }
    #endregion IDeviceIndexed

    #region public
    /// <summary>
    /// class that represent TAG in Device - 
    /// </summary>
    [Serializable]
    public abstract class TagInDevice : DeviceItem
    {
      /// <summary>
      /// Initializes the object from a data value. It creates new TAG in the device address space.
      /// </summary>
      /// <param name="itemID">Item identifier</param>
      public TagInDevice(string itemID)
        : base(itemID)
      {
        Add(itemID, this);
      }
      /// <summary>
      /// Initializes the object from a data value. It creates new TAG in the device address space.
      /// </summary>
      /// <param name="itemID">Item identifier</param>
      /// <param name="value">Current value</param>
      /// <param name="InitialQuality">Initial quality</param>
      /// <param name="AccessRights">Initial access rights</param>
      /// <param name="tagCanonicalType">Canonical Type for the tag</param>
      public TagInDevice(string itemID, object value, qualityBits InitialQuality, ItemAccessRights AccessRights, System.Type tagCanonicalType)
        : base(itemID, value, InitialQuality, AccessRights, tagCanonicalType)
      {
        Add(itemID, this);
      }
    }
    #endregion

    #region Static

    #region Private Members
    private static Hashtable m_items = new Hashtable();
    private static readonly TagInDevice[] m_itemsIndexTab = new TagInDevice[ushort.MaxValue];
    private static ushort m_tagIdx = 0;
    private static void Add(string idx, TagInDevice Item)
    {
      lock (m_items)
      {
        m_items[idx] = Item;
        m_itemsIndexTab[m_tagIdx++] = Item;
      }
    }
    private static DeviceItem GetItemFromItemsByItemID(string itemID)
    {
      DeviceItem item = null;
      lock (m_items)
      {
        item = (DeviceItem)m_items[itemID];
      }
      return item;
    }
    private static TagInDevice GetItemFromItemsByItemIDx(ushort itemIdx)
    {
      TagInDevice indexedTagInDevice = null;
      lock (m_items)
      {
        indexedTagInDevice = m_itemsIndexTab[itemIdx];
      }
      return indexedTagInDevice;
    }
    #endregion

    /// <summary>
    /// Browse the address space of the device
    /// </summary>
    /// <returns>array of all created tags in the device</returns>
    public static string[] getAddressSpaceStatic()
    {
      lock (m_items)
      {
        string[] addSpce = new string[m_items.Count];
        short ix = 0;
        foreach (DeviceItem elId in m_items.Values)
          addSpce[ix++] = elId.ItemID;
        return addSpce;
      }
    }//getAddressSpace
    /// <summary>
    /// Finds and returns the specified item by the ID (element name).
    /// </summary>
    /// <param name="itemID">The item ID(element name).</param>
    /// <returns>the specified item by the ID as I4UAServer interface</returns>
    public static I4UAServer Find(string itemID)
    {
      return GetItemFromItemsByItemID(itemID);
    }
    #endregion

  }
}
