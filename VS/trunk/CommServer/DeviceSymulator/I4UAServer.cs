//<summary>
//  Title   : It is the base class for classes containing process data.
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C)2009, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.Lib.DeviceSimulator
{
  /// <summary>
  /// It is the base class for classes containing process data.
  /// </summary>
  public class ItemValueArgs: EventArgs
  {
    /// <summary>
    /// Gets or sets the value of the process data .
    /// </summary>
    /// <value>The results of an operation on a uniquely identifiable item value.</value>
    public Opc.Da.ItemValue Value { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemValueArgs"/> class.
    /// </summary>
    /// <param name="value">The value of the process data to be be provided to all subscribers of the <see cref="I4UAServer.OnValueChanged"/>.</param>
    public ItemValueArgs( Opc.Da.ItemValueResult value )
    {
      Value = value;
    }
  }
  /// <summary>
  /// Interface for the Unified Architectur Serve
  /// </summary>
  public interface I4UAServer
  {
    /// <summary>
    /// Occurs when on value of the process data is changed.
    /// </summary>
    event EventHandler<ItemValueArgs> OnValueChanged;
    /// <summary>
    /// Gets the canonical type of the item.
    /// </summary>
    /// <value>An instance of the <see cref="Type"/> representing the canonical type of the item.</value>
    Type ItemCanonicalType { get; }
    /// <summary>
    /// Gets the last known value.
    /// </summary>
    /// <value>The last known value.</value>
    Opc.Da.ItemValue LastKnownValue { get; }
    /// <summary>
    /// Sets the value to write.
    /// </summary>
    /// <value>The value to write.</value>
    object ValueToWrite { set; }
    /// <summary>
    /// Submits outstanding write operation for the last values set by the <see cref="ValueToWrite"/> to the remote OPC Da server.
    /// </summary>
    /// <returns><c>true</c> if sucess.</returns>
    bool Flush();
  }
}
