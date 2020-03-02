//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Opc;
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
    /// <param name="itemID">The item identifier.</param>
    /// <returns><c>true</c> if the item identifier <paramref name="itemID"/> is known; otherwise, <c>false</c>.</returns>
    bool IsKnownItem(string itemID);
    /// <summary>
    /// Returns all available properties for the specified item.
    /// </summary>
    /// <param name="itemID">The item identifier.</param>
    /// <param name="returnValues">if set to <c>true</c> the method returns values.</param>
    /// <returns>An instance of <see cref="ItemPropertyCollection"/>.</returns>
    ItemPropertyCollection GetAvailableProperties(string itemID, bool returnValues);
    /// <summary>
    /// Returns the specified properties for the specified item.
    /// </summary>
    ItemPropertyCollection GetAvailableProperties(string itemID, PropertyID[] propertyIDs, bool returnValues);
    /// <summary>
    /// Reads the value of the specified item property.
    /// </summary>
    /// <param name="itemID">The item identifier.</param>
    /// <param name="propertyID">The property identifier.</param>
    /// <returns>ItemValueResult.</returns>
    ItemValueResult Read(string itemID, PropertyID propertyID);
    /// <summary>
    /// Writes the value of the specified item property.
    /// </summary>
    /// <param name="itemID">The item identifier.</param>
    /// <param name="propertyID">The property identifier.</param>
    /// <param name="value">The value to write.</param>
    /// <returns>IdentifiedResult.</returns>
    IdentifiedResult Write(string itemID, PropertyID propertyID, ItemValue value);
    /// <summary>
    /// Get all Items created in the device
    /// </summary>
    string[] getAddressSpace { get; }
  }
}
