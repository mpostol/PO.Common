//<summary>
//  Title   : IDevice base on OPC Foundation Resources
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

//========================================================================
// TITLE: IDevice.cs
//
// CONTENTS:
// 
// An interface for an abstract device that contains a set of data points.
//
// (c) Copyright 2003 The OPC Foundation
// ALL RIGHTS RESERVED.
//
// DISCLAIMER:
//  This code is provided by the OPC Foundation solely to assist in 
//  understanding and use of the appropriate OPC Specification(s) and may be 
//  used as set forth in the License Grant section of the OPC Specification.
//  This code is provided as-is and without warranty or support of any sort
//  and is subject to the Warranty and Liability Disclaimers which appear
//  in the printed OPC Specification.
//
// MODIFICATION LOG:
//
// Date       By    Notes
// ---------- ---   -----
// 2004/03/26 RSA   Initial implementation.
// 2005/07/05 MP    Initialize, BuildAddressSpace were removed from the interface

using Opc.Da;

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// An interface for an abstract device that contains a set of data points.
  /// </summary>
  public interface IDevice
  {
    /// <summary>
    /// Returns whether item id belongs to the device's address space.
    /// </summary>
    bool IsKnownItem( string itemID );

    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection GetAvailableProperties(
      string itemID,
      bool returnValues );

    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    Opc.Da.ItemPropertyCollection GetAvailableProperties(
      string itemID,
      PropertyID[] propertyIDs,
      bool returnValues );

    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    Opc.Da.ItemValueResult Read( string itemID, PropertyID propertyID );

    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    Opc.IdentifiedResult Write(
      string itemID,
      PropertyID propertyID,
      Opc.Da.ItemValue value );
    /// <summary>
    /// Get all Items created in the device
    /// </summary>
    string[] getAddressSpace
    { get; }
  }
}
