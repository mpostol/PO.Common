//___________________________________________________________________________________
//
//  Copyright (C) 2020, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using Opc.Da;

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// IOPCDataAccess - interface used to access device as DA using remoting.
  /// </summary>
  public interface IOPCDataAccess
  {
    /// <summary>
    /// Gets the OPC properties.
    /// </summary>
    /// <returns>list of properties</returns>
    PropertyID[] GetOPCProperties();
    /// <summary>
    /// Gets the available items.
    /// </summary>
    /// <returns>list of items</returns>
    string[] GetAvailiableItems();
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection GetAvailableProperties( string itemID, PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection GetAvailableProperties( string itemID, PropertyID[] propertyIDs );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection GetAvailableProperties( string itemID, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection GetAvailableProperties( string itemID );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties( string[] itemID, PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties( string[] itemID, PropertyID[] propertyIDs );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties( string[] itemID, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties( string[] itemID );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties(PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties(PropertyID[] propertyIDs );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties( bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <returns>Property collection accordingly to passed arguments</returns>
    ItemPropertyCollection[] GetAvailableProperties();
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="ItemName">Name of the item.</param>
    /// <returns>ItemValueResult (value, time-stamp, quality, diagnostics)</returns>
    ItemValueResult GetValue( string ItemName );
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="ItemName">Name of the item.</param>
    /// <returns>list of ItemValueResult (value, time-stamp, quality, diagnostics)</returns>
    Opc.Da.ItemValueResult[] GetValue( string[] ItemName );
    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="ItemName">Name of the item.</param>
    /// <param name="val">The value.</param>
    /// <returns>true if succeeded</returns>
    bool SetValue( string ItemName, object val );
    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns>version of interface</returns>
    string GetVersion();
  }
  /// <summary>
  /// Interface used to access buffered queues (it extend the DA interface)
  /// </summary>
  public interface IOPCBufferedDataAccess: IOPCDataAccess
  {
    /// <summary>
    /// Gets the available queues.
    /// </summary>
    /// <returns></returns>
    string[] GetAvailiableQueues();
    /// <summary>
    /// Connects to queue.
    /// </summary>
    /// <param name="queueID">The queue ID.</param>
    /// <returns>queue list</returns>
    string ConnectToQueue( string queueID );
    /// <summary>
    /// Starts the transaction.
    /// </summary>
    /// <param name="Hash">The hash - Identifier of the queue.</param>
    /// <returns>true if succeeded</returns>
    ItemValueResult[] StartTransaction( string Hash );
    /// <summary>
    /// Ends the transaction.
    /// </summary>
    /// <param name="Hash">The hash - Identifier of the queue.</param>
    /// <returns>true if succeeded</returns>
    bool EndTransaction( string Hash );
  }
  /// <summary>
  /// Interface e used to access buffered queues 
  /// </summary>
  public interface IOPCBufferedDataAccessQueue
  {
    /// <summary>
    /// Starts the transaction.
    /// </summary>
    /// <returns>values returned from buffered queue</returns>
    ItemValueResult[] StartTransaction();
    /// <summary>
    /// Ends the transaction.
    /// </summary>
    /// <returns>true if succored</returns>
    bool EndTransaction();
  }
}
