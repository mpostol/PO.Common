//<summary>
//  Title   : IOPCDataAccess - interface used to access divace as DA using remoting.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//  20081217: mzbrzezny: set value functionality is added
//  2005 - created
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// IOPCDataAccess - interface used to access divace as DA using remoting.
  /// </summary>
  public interface IOPCDataAccess
  {
    /// <summary>
    /// Gets the OPC properties.
    /// </summary>
    /// <returns>list of properties</returns>
    Opc.Da.PropertyID[] GetOPCProperties();
    /// <summary>
    /// Gets the availiable items.
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
    Opc.Da.ItemPropertyCollection GetAvailableProperties( string itemID, Opc.Da.PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection GetAvailableProperties( string itemID, Opc.Da.PropertyID[] propertyIDs );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection GetAvailableProperties( string itemID, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection GetAvailableProperties( string itemID );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( string[] itemID, Opc.Da.PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( string[] itemID, Opc.Da.PropertyID[] propertyIDs );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( string[] itemID, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="itemID">The item ID.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( string[] itemID );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( Opc.Da.PropertyID[] propertyIDs, bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="propertyIDs">The property identifiers.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( Opc.Da.PropertyID[] propertyIDs );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <param name="returnValues">if set to <c>true</c> values are returned.</param>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties( bool returnValues );
    /// <summary>
    /// Gets the available properties.
    /// </summary>
    /// <returns>Property collection accordingly to passed arguments</returns>
    Opc.Da.ItemPropertyCollection[] GetAvailableProperties();
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="ItemName">Name of the item.</param>
    /// <returns>ItemValueResult (value, timestamp, quality, diagnostics)</returns>
    Opc.Da.ItemValueResult GetValue( string ItemName );
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="ItemName">Name of the item.</param>
    /// <returns>list of ItemValueResult (value, timestamp, quality, diagnostics)</returns>
    Opc.Da.ItemValueResult[] GetValue( string[] ItemName );
    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="ItemName">Name of the item.</param>
    /// <param name="val">The value.</param>
    /// <returns>true if succeded</returns>
    bool SetValue( string ItemName, object val );
    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <returns>version of interface</returns>
    string GetVersion();
  }
  /// <summary>
  /// Interfac e used to access buffered queues (it extend the DA interface)
  /// </summary>
  public interface IOPCBufferedDataAccess: IOPCDataAccess
  {
    /// <summary>
    /// Gets the availiable queues.
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
    /// <returns>true if succeded</returns>
    Opc.Da.ItemValueResult[] StartTransaction( string Hash );
    /// <summary>
    /// Ends the transaction.
    /// </summary>
    /// <param name="Hash">The hash - Identifier of the queue.</param>
    /// <returns>true if succeded</returns>
    bool EndTransaction( string Hash );
  }
  /// <summary>
  /// Interfac e used to access buffered queues 
  /// </summary>
  public interface IOPCBufferedDataAccessQueue
  {
    /// <summary>
    /// Starts the transaction.
    /// </summary>
    /// <returns>values returned from buffered queue</returns>
    Opc.Da.ItemValueResult[] StartTransaction();
    /// <summary>
    /// Ends the transaction.
    /// </summary>
    /// <returns>true if succeded</returns>
    bool EndTransaction();
  }
}
