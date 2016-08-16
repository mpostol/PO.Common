//<summary>
//  Title   : COMMUNICATIONS LIBRARY - Interfaces used to read data from frames.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//   20081003: mzbrzezny: ItemDefaultSettings implementation
//    MPostol - 13-06-2003:
//    Created (start point (SB_SBAPP.DEF )
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// Interface used to read data from a frame received by the master station from a remote unit.
  /// </summary>
  public interface IReadValue: IBlockDescription,IEnvelope
  {
    /// <summary>
    /// Check if address belongs to the block
    /// </summary>
    /// <param name="station">station ro be checked</param>
    /// <param name="address">address to be checked</param>
    /// <param name="type">data type</param>
    /// <returns>true if address belongs to the block</returns>
    bool IsInBlock( uint station, ushort address, short type );
    /// <summary>
    /// Read the selected contend (value) from the message in the requested type.
    /// If the address space cannot contain values in this type no conversion is done.
    /// </summary>
    /// <param name="regAddress">Register address in the defined address space  the message was created for.</param>
    /// <param name="pCanonicalType">Requested canonical type.</param>
    /// <returns>
    /// Value retrieved from the message received from remote unit.
    /// </returns>
    object ReadValue( int regAddress, System.Type pCanonicalType );
  }
  /// <summary>
  /// Interface used to write data to the frame to be sent by the master station to a remote unit.
  /// </summary>
  public interface IWriteValue: IBlockDescription, IEnvelope
  {
    /// <summary>
    /// Writes the value to the message in the requested type.
    /// If the address space cannot contain values in the type of pValue no conversion is done.
    /// </summary>
    /// <param name="pValue">Value to write.</param>
    /// <param name="pRegAddress">The address</param>
    void WriteValue( object pValue, int pRegAddress );
  }
  /// <summary>
  /// Interface used to write data to frame to be sent by the slave station to a remote unit.
  /// </summary>
  public interface IResponseValue: IBlockDescription, IEnvelope
  {
    /// <summary>
    /// array of registers values
    /// </summary>
    /// <value>The response value</value>
    object this[ int regAddress ] { set;}
  }
  /// <summary>
  /// Interface used to get command from the frame by received by the slave station from a remote unit.
  /// </summary>
  public interface IReadCMDValue: IBlockDescription, IEnvelope
  {
    /// <summary>
    /// Check if address belongs to the block
    /// </summary>
    /// <param name="station">station to be checked</param>
    /// <param name="address">address to be checked</param>
    /// <param name="type">type of data</param>
    /// <returns>true if address belongs to the block</returns>
    bool IsInBlock( uint station, ushort address, short type );
    /// <summary>
    /// array of registers values
    /// </summary>
    /// <value>The register value</value>
    object this[ int regAddress ] { get;}
    /// <summary>
    /// A command a master station request to be processed by this station – slave station on the field bus network.
    /// </summary>
    /// <value>The command.</value>
    int GetCommand { get;}
  }
  /// <summary>
  /// Description of the data block.
  /// </summary>
  public interface IBlockDescription
  {
    /// <summary>
    /// Gets the data block starting address.
    /// </summary>
    /// <value>The start address.</value>
    int startAddress { get;}
    /// <summary>
    /// Gets the length of the data in bytes.
    /// </summary>
    /// <value>The length.</value>
    int length { get;}
    /// <summary>
    /// Determines the remote unit address space (resource) the data block belongs to. It could also be used to define
    /// data type if it is determined by address space.
    /// </summary>
    /// <value>The type of the data.</value>
    short dataType { get;}
  }
  /// <summary>
  /// Address of a data block on the local field bus including a station address and block description.
  /// </summary>
  public interface IBlockAddress: IBlockDescription
  {
    /// <summary>
    /// Gets the station address.
    /// </summary>
    /// <value>The station address.</value>
    int station { get;}
  }
}
