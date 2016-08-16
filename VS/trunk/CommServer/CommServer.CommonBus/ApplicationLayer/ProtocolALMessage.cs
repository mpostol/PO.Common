//<summary>
//  Title   : COMMUNICATIONS LIBRARY - Protocol Application Layer message class
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//      all registers address are relative to the block begining,
//      new overloaded version of SetBlockDescription 
//      ReturnEmptyEnvelope resets the message content
//      ProtocolALMessage - new parameter stating if big-Endian representatation 
//      (the most significant byte first) is to use. 
//      This property cannot be changed later.
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Lib.RTLib.Processes;
using CAS.Lib.CommonBus.CommunicationLayer;
using CAS.Lib.CommonBus.CommunicationLayer.Generic;

namespace CAS.Lib.CommonBus.ApplicationLayer
{
  /// <summary>
  /// COMMUNICATIONS LIBRARY - Protocol Application Layer message class
  /// </summary>
  public abstract class ProtocolALMessage: UMessage, IReadValue, IWriteValue, IResponseValue, IReadCMDValue, ISesDBuffer, IBlockAddress
  {
    #region IBlockDescription
    int IBlockAddress.station { get { return currStation; } }
    int IBlockDescription.length { get { return currBlockLength; } }
    int IBlockDescription.startAddress { get { return currBlockStartAddress; } }
    short IBlockDescription.dataType { get { return currDataType; } }
    #endregion
    #region IReadValue
    /// <summary>
    /// Check if address belongs to the block
    /// </summary>
    /// <param name="address">address to be checked</param>
    /// <param name="station">station ro be checked</param>
    /// <param name="myDataType">data type</param>
    /// <returns>true if address belongs to the block</returns>
    bool IReadValue.IsInBlock( uint station, ushort address, short myDataType )
    {
      return ( IsInBlockTest( station, address, myDataType ) );
    }
    /// <summary>
    /// Read the selected contend (value) from the message in the requested type. 
    /// If the address space cannot contain values in this type no conversion is done. 
    /// </summary>
    /// <param name="regAddress">Address</param>
    /// <param name="pCanonicalType">Requested canonical type.</param>
    /// <returns>Converted value.</returns>
    public abstract object ReadValue( int regAddress, System.Type pCanonicalType );
    #endregion
    #region IWriteValue
    /// <summary>
    /// Writes the value to the message in the requested type.
    /// If the address space cannot contain values in the type of pValue no conversion is done.
    /// </summary>
    /// <param name="pValue">Value to write.</param>
    /// <param name="pRegAddress">Address</param>
    public abstract void WriteValue( object pValue, int pRegAddress );
    #endregion
    #region IResponseValue
    /// <summary>
    /// Sets the <see cref="System.Object"/> with the specified register value.
    /// </summary>
    /// <value>The specified register value</value>
    object IResponseValue.this[ int regAddressOffset ]
    {
      set { SetValue( value, regAddressOffset ); }
    }
    #endregion
    #region IReadCMDValue
    bool IReadCMDValue.IsInBlock( uint station, ushort address, short myDataType )
    {
      return ( IsInBlockTest( station, address, myDataType ) );
    }
    /// <summary>
    /// array of registers values
    /// </summary>
    object IReadCMDValue.this[ int regAddressOffset ]
    {
      get { return ReadCMD( regAddressOffset ); }
    }
    /// <summary>
    /// A command a master station request to be processed by this station – slave station on the field bus network. 
    /// </summary>
    public abstract int GetCommand { get;}
    #endregion
    #region Processes.IEnvelope
    /// <summary>
    /// Checks if the buffer is in the pool or otherwise is alone and used by a user. 
    /// Used to the state by the governing pool.
    /// </summary>
    bool IEnvelope.InPool
    {
      set { this.inPoolFlag = value; }
      get { return this.inPoolFlag; }
    }
    /// <summary>
    /// Used by a user to return an empty envelope to the common pool. It also resets the message content.
    /// </summary>
    public void ReturnEmptyEnvelope()
    {
      ISesDBuffer var = (ISesDBuffer)this;
      ResetContent();
      this.pool.ReturnEmptyISesDBuffer( ref var );
    }
    #endregion
    #region abstract – protocol dependent.
    ///// <summary>
    ///// Gets a value from the message according. Must be implemented by the inheritor according to the implemented protocol rules.
    ///// </summary>
    ///// <param name="regAddressOffset">Register address in the address space the message was created for.</param>
    ///// <returns>The value received in the message.</returns>
    //protected abstract object GetValueFromMess( int regAddressOffset );
    /// <summary>
    /// Read a value from the Command message at the specified position. Used by the slave stations.
    /// </summary>
    /// <param name="regAddressOffset">Position of the value.</param>
    /// <returns>Value from the message.</returns>
    protected abstract object ReadCMD( int regAddressOffset );
    ///// <summary>
    ///// Wrtite a value to the message at the specified position. Used by the master stations.
    ///// </summary>
    ///// <param name="regValue">Value to write.</param>
    ///// <param name="regAddressOffset">Position of the value.</param>
    ///// <returns>Value to write.</returns>
    //protected abstract void WriteValue( object regValue, int regAddressOffset );
    /// <summary>
    /// Wrtite a value to the message at the specified position. Used by the slave stations to respond.
    /// </summary>
    /// <param name="regValue">Value to write.</param>
    /// <param name="regAddressOffset">Position of the value.</param>
    /// <returns>Value to write.</returns>
    protected abstract void SetValue( object regValue, int regAddressOffset );
    /// <summary>
    /// Result of the check if the frame is exactly what we are looking for.
    /// </summary>
    public enum CheckResponseResult
    {
      /// <summary>OK</summary>
      CR_OK,
      /// <summary>CRC Error</summary>
      CR_CRCError,
      /// <summary>Negative acknowledge.</summary>
      CR_NAK,
      /// <summary>Synchronization error.</summary>
      CR_SynchError,
      /// <summary>Incomplete frame.</summary>
      CR_Incomplete,
      /// <summary>Invalid frame</summary>
      CR_Invalid,
      /// <summary>Answer is from unexpected station.</summary>
      CR_OtherStation
    };
    /// <summary>
    /// Checks if the frame is exactly what we are looking for.
    /// </summary>
    /// <param name="txmsg">Sent frame as request – we expect answer to this frame.</param>
    /// <returns>
    /// CR_OK: OK
    /// CR_CRCError: CRC Error
    /// CR_NAK: Negative acknowledge.
    /// CR_SynchError: Synchronization error.
    /// CR_Incomplete: Incomplete frame.
    /// CR_Invalid: Invalid frame
    /// CR_OtherStation: Answer is from unexpected station.
    /// </returns>
    public abstract CheckResponseResult CheckResponseFrame( ProtocolALMessage txmsg );
    /// <summary>
    /// Prepares before sending it to the data provider of the remote unit by the WriteData.
    /// </summary>
    /// <param name="block">Description of the data block.</param>
    /// <param name="station">Address of the destination station.</param>
    internal protected abstract void PrepareReqWriteValue( IBlockDescription block, int station );
    /// <summary>
    /// Prepare a request to obtain a requested data block. 
    /// </summary>
    /// <param name="station">Address of the remote station.</param>
    /// <param name="block">Description of the requested data block.</param>
    internal protected abstract void PrepareRequest( int station, IBlockDescription block );
    #endregion
    #region public
    /// <summary>
    /// Assigns description of the block of data to the message.
    /// </summary>
    /// <param name="station">Address of the remote station.</param>
    /// <param name="address">Data block starting address</param>
    /// <param name="myDataType">Determines the remote unit address space (resource) the data block belongs to.
    /// It could also be used to define data type if it is determined by address space. </param>
    /// <param name="length">The length of the data in bytes.</param>
    public virtual void SetBlockDescription( int station, int address, short myDataType, int length )
    {
      currStation = station;
      currBlockStartAddress = address;
      currDataType = myDataType;
      currBlockLength = length;
    }
    /// <summary>
    /// Assigns description of the block of data to the message.
    /// </summary>
    /// <param name="station"></param>
    /// <param name="block"></param>
    public virtual void SetBlockDescription( int station, IBlockDescription block )
    {
      currStation = station;
      currBlockStartAddress = block.startAddress;
      currDataType = block.dataType;
      currBlockLength = block.length;
    }
    /// <summary>
    /// Creator
    /// </summary>
    /// <param name="length">Length of the buffer to keep contend.</param>
    /// <param name="homePool">Home pool the message belongs to.</param>
    /// <param name="bigEndian">If true, numerical quantity larger then a single byte is stored in the buffer 
    /// with the most significant byte first.
    /// </param>
    internal protected ProtocolALMessage( ushort length, IBufferLink homePool, bool bigEndian )
      : base( length, bigEndian )
    {
      this.pool = homePool;
    }
    /// <summary>
    /// Retrieves a string representation of the object.
    /// </summary>
    /// <returns>Description of the message:[station][address][myDataType][length]</returns>
    public override string ToString()
    {
      return " | station: " + currStation.ToString() +
        " | address: " + currBlockStartAddress.ToString() +
        " | myDataType: " + currDataType.ToString() +
        " | length: " + currBlockLength.ToString() + " | "
        + base.ToString();
      ;
    }
    #endregion
    #region PRIVAT
    bool inPoolFlag;
    private int currStation = -1;
    private int currBlockStartAddress = -1;
    private short currDataType = -1;
    private int currBlockLength = -1;
    private IBufferLink pool = null;
    private bool IsInBlockTest( uint station, ushort address, short myType )
    {
      return
        ( address >= currBlockStartAddress ) &&
        ( address < ( currBlockStartAddress + currBlockLength ) ) &&
        ( myType == currDataType );
    }
    #endregion
  } //class ProtocolALMessage
}