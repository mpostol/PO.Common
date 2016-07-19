//<summary>
//  Title   : Device
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History 
//  20090910: mzbrzezny: changes connected to implementation of I4UAServer
//  20081006: mzbrzezny: implementation of ItemAccessRights
//    Maciej Zbrzezny - 12-04-2006
//    OZNACZONO PEWNE KLASY JAKO SERIALIZOWALNE
//    MPOstol: 17-12-2005
//      adapted from OPC Foundation Sample code:
//    RSA: 2004/03/26
//      Initial implementation.
//
//  Copyright (C)2006-2009, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using CAS.Lib.RTLib;
using Opc.Da;

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
    /// <param name="itemIdx">The item idx.</param>
    /// <param name="returnValues">if set to <c>true</c> function is returning values of the properties.</param>
    /// <returns></returns>
    Opc.Da.ItemPropertyCollection GetAvailableProperties( ushort itemIdx, bool returnValues );
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection GetAvailableProperties( ushort itemIdx, PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    Opc.Da.ItemValueResult Read( ushort itemIdx, PropertyID propertyID );
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    Opc.IdentifiedResult Write( ushort itemIdx, PropertyID propertyID, Opc.Da.ItemValue value );
    /// <summary>
    /// Get all Items created in the device with indexes
    /// </summary>
    ItemDsc[] GetIndexedAddressSpace { get; }
  }
  /// <summary>
  /// A class that represents a device with a set of simulated data points.
  /// </summary>
  [SecurityPermissionAttribute( SecurityAction.Demand, SerializationFormatter = true )]
  [ComVisible( false )]
  [Serializable]
  public class Device: MarshalByRefObject, IDeviceIndexed, IDevice, IDisposable
  {
    #region public members
    /// <summary>
    /// Exception for the Device class
    /// </summary>
    public class DeviceException: Exception
    {
      internal DeviceException( string msg ) : base( msg ) { }
    }
    /// <summary>
    /// Browse the address space of the device
    /// </summary>
    /// <returns>array of all created tags in the device</returns>
    public static string[] getAddressSpaceStatic()
    {
      lock ( m_items )
      {
        string[] addSpce = new string[ m_items.Count ];
        short ix = 0;
        foreach ( DeviceItem elId in m_items.Values )
          addSpce[ ix++ ] = elId.ItemID;
        return addSpce;
      }
    }//getAddressSpace
    /// <summary>
    /// Initializes the object with default values.
    /// </summary>
    public Device() { }
    #endregion
    #region IDisposable Members
    /// <summary>
    /// Stops all threads and releases all resources.
    /// </summary>
    public void Dispose()
    {
      //        m_disposed = true;
    }
    #endregion
    #region IDevice Members
    /// <summary>
    /// Returns whether item id belongs to the device's address space.
    /// </summary>
    bool IDevice.IsKnownItem( string itemID )
    {
      lock ( m_items )
        return m_items.Contains( itemID );
    }
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection IDevice.GetAvailableProperties( string itemID, bool returnValues )
    {
      // initialize result.
      ItemPropertyCollection properties = new ItemPropertyCollection();

      properties.ItemName = itemID;
      properties.ItemPath = null;
      properties.ResultID = Opc.ResultID.S_OK;
      properties.DiagnosticInfo = null;

      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID( itemID );

      if ( item == null )
      {
        properties.ResultID = Opc.ResultID.Da.E_UNKNOWN_ITEM_NAME;
        return properties;
      }
      // fetch properties.
      return item.GetAvailableProperties( returnValues );
    }
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection IDevice.GetAvailableProperties
      ( string itemID, PropertyID[] propertyIDs, bool returnValues )
    {
      // initialize result.
      ItemPropertyCollection properties = new ItemPropertyCollection();

      properties.ItemName = itemID;
      properties.ItemPath = null;
      properties.ResultID = Opc.ResultID.S_OK;
      properties.DiagnosticInfo = null;

      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID( itemID );

      if ( item == null )
      {
        properties.ResultID = Opc.ResultID.Da.E_UNKNOWN_ITEM_NAME;
        return properties;
      }
      // fetch properties.
      return item.GetAvailableProperties( propertyIDs, returnValues );
    }
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    Opc.Da.ItemValueResult IDevice.Read( string itemID, PropertyID propertyID )
    {
      // initialize result.
      ItemValueResult result = new ItemValueResult();

      result.ItemName = itemID;
      result.ItemPath = null;
      result.ResultID = Opc.ResultID.S_OK;
      result.DiagnosticInfo = null;

      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID( itemID );

      if ( item == null )
      {
        result.ResultID = Opc.ResultID.Da.E_UNKNOWN_ITEM_NAME;
        return result;
      }
      // read value.
      return item.Read( propertyID );
    }
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    Opc.IdentifiedResult IDevice.Write( string itemID, PropertyID propertyID, Opc.Da.ItemValue value )
    {
      // lookup item.
      DeviceItem item = GetItemFromItemsByItemID( itemID );

      if ( item == null )
        return new Opc.IdentifiedResult( itemID, Opc.ResultID.Da.E_UNKNOWN_ITEM_NAME );
      // write value.
      return item.Write( propertyID, value );
    }
    string[] IDevice.getAddressSpace
    {
      get
      {
        lock ( m_items )
        {
          string[] addSpce = new string[ m_items.Count ];
          short ix = 0;
          foreach ( DeviceItem elId in m_items.Values )
            addSpce[ ix++ ] = elId.ItemID;
          return addSpce;
        }
      }
    }//getAddressSpace
    #endregion
    #region IDeviceIndexed
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection IDeviceIndexed.GetAvailableProperties( ushort itemIdx, bool returnValues )
    {
      if ( itemIdx >= m_tagIdx )
        new DeviceException( "Item index out of range" );
      // fetch properties.
      return GetItemFromItemsByItemIDx( itemIdx ).GetAvailableProperties( returnValues );
    }
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection IDeviceIndexed.GetAvailableProperties
      ( ushort itemIdx, PropertyID[] propertyIDs, bool returnValues )
    {
      if ( itemIdx >= m_tagIdx )
        new DeviceException( "Item index out of range" );
      // fetch properties.
      return GetItemFromItemsByItemIDx( itemIdx ).GetAvailableProperties( propertyIDs, returnValues );
    }
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    Opc.Da.ItemValueResult IDeviceIndexed.Read( ushort itemIdx, PropertyID propertyID )
    {
      if ( itemIdx >= m_tagIdx )
        new DeviceException( "Item index out of range" );
      // read value.
      return GetItemFromItemsByItemIDx( itemIdx ).Read( propertyID );
    }
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    Opc.IdentifiedResult IDeviceIndexed.Write( ushort itemIdx, PropertyID propertyID, Opc.Da.ItemValue value )
    {
      if ( itemIdx >= m_tagIdx )
        new DeviceException( "Item index out of range" );
      // write value.
      return GetItemFromItemsByItemIDx( itemIdx ).Write( propertyID, value );
    }
    /// <summary>
    /// Get all Items created in the device with indexes
    /// </summary>
    ItemDsc[] IDeviceIndexed.GetIndexedAddressSpace
    {
      get
      {
        lock ( m_items )
        {
          ItemDsc[] addSpce = new ItemDsc[ m_tagIdx ];
          for ( ushort ix = 0; ix < m_tagIdx; ix++ )
          {
            addSpce[ ix ].itemID = m_itemsIndexTab[ ix ].ItemID;
            addSpce[ ix ].itemIdx = ix;
          }
          return addSpce;
        }
      }
    }
    #endregion IDeviceIndexed
    #region protected members
    //    /// <summary>
    //    /// Removes the address space for the device to the cache.
    //    /// </summary>
    //    private void ClearAddressSpace(Cache cache)
    //    {
    //      lock (this)
    //      {
    //        foreach (DeviceItem item in m_items.Values)
    //        {
    //          cache.RemoveItemAndLink(item.ItemID);
    //        }
    //      }
    //    }
    /// <summary>
    /// class that represent TAG in Device
    /// </summary>
    [Serializable]
    public abstract class TagInDevice: DeviceItem
    {
      /// <summary>
      /// Initializes the object from a data value.
      /// </summary>
      /// <param name="itemID">Item identyfier</param>
      public TagInDevice( string itemID )
        : base( itemID )
      {
        Add( itemID, this );
      }
      /// <summary>
      /// Initializes the object from a data value.
      /// </summary>
      /// <param name="itemID">Item identyfier</param>
      /// <param name="value">Current value</param>
      /// <param name="InitialQuality">Initial quality</param>
      /// <param name="AccessRights">Initial access rights</param>
      /// <param name="tagCanonicalType">Cannonical Type for the tag</param>
      public TagInDevice
        ( string itemID, object value, Opc.Da.qualityBits InitialQuality, ItemAccessRights AccessRights, System.Type tagCanonicalType )
        : base
          ( itemID, value, InitialQuality, AccessRights, tagCanonicalType )
      {
        Add( itemID, this );
      }
    }
    #endregion
    #region Static
    #region Private Members
    //    private bool m_disposed = false;
    private static Hashtable m_items = new Hashtable();
    private static TagInDevice[] m_itemsIndexTab = new TagInDevice[ ushort.MaxValue ];
    private static ushort m_tagIdx = 0;
    private static void Add( string idx, TagInDevice Item )
    {
      lock ( m_items )
      {
        m_items[ idx ] = Item;
        m_itemsIndexTab[ m_tagIdx++ ] = Item;
      }
    }
    private static DeviceItem GetItemFromItemsByItemID( string itemID )
    {
      DeviceItem item = null;
      lock ( m_items )
      {
        item = (DeviceItem)m_items[ itemID ];
      }
      return item;
    }
    private static TagInDevice GetItemFromItemsByItemIDx( ushort itemIdx )
    {
      TagInDevice indexedTagInDevice = null;
      lock ( m_items )
      {
        indexedTagInDevice = m_itemsIndexTab[ itemIdx ];
      }
      return indexedTagInDevice;
    }
    #endregion
    /// <summary>
    /// Finds and returns the specified item by the ID (element name).
    /// </summary>
    /// <param name="itemID">The item ID(element name).</param>
    /// <returns>the specified item by the ID as I4UAServer interface</returns>
    public static I4UAServer Find( string itemID )
    {
      return GetItemFromItemsByItemID( itemID );
    }
    #endregion
  }
}
