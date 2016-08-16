//<summary>
//  Title   : Connection Management Interface – must be implemented by all layers making together a communication stack  – 
//            Communication and Application layers
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPOstol 03-04-2007: created from ICommunicationLayer
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>
 
using System;

namespace CAS.Lib.CommonBus
{
  /// <summary>
  /// Interface to represent an address
  /// </summary>
  public interface IAddress
  {
    /// <summary>
    /// Gets or sets the address
    /// </summary>
    object address
    {
      get;
      set;
    }
  }
  /// <summary>
  /// Base implementation of IAddress - class that describe the address based on string
  /// </summary>
  public class StringAddress: IAddress
  {
    private string m_address = string.Empty;
    /// <summary>
    /// Gets and sets the address.
    /// </summary>
    object IAddress.address
    {
      get { return m_address; }
      set { m_address = (string)value; }
    }
    /// <summary>
    /// default constructor
    /// </summary>
    public StringAddress()
    {
    }
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="_address">the address as <see cref="string"/></param>
    public StringAddress( string _address )
    {
      m_address = _address;
    }
    /// <summary>
    /// ToString implementation
    /// </summary>
    /// <returns>address</returns>
    public override string ToString()
    {
      return m_address;
    }
  }
  /// <summary>
  /// Result of the ConnectReq 
  /// </summary>
  public enum TConnectReqRes
  {
    /// <summary>
    /// Operation accomplished successfully 
    /// </summary>
    Success,
    /// <summary>
    /// Connection cannot be established.
    /// </summary>
    NoConnection
  }
  /// <summary>
  /// Result for the ConnectInd
  /// </summary>
  public enum TConnIndRes
  {
    /// <summary>
    /// Indicates need connection
    /// </summary>
    ConnectInd,
    /// <summary>
    /// There is no incoming connection awaiting.
    /// </summary>
    NoConnection
  }
  /// <summary>
  /// Interface used to manage connection.
  /// </summary>
  public interface IConnectionManagement: IDisposable
  {
    /// <summary>
    /// Connect Request, this fuction is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// NoConnection:
    ///   Connection cannot be established.
    /// </returns>
    TConnectReqRes ConnectReq( IAddress remoteAddress );
    /// <summary>
    /// Connect indication – Check if there is a connection accepted to the remote address. 
    /// </summary>
    /// <param name="pRemoteAddress">
    /// The remote address we are waiting for connection from. Null if we are waiting for any connection.
    /// </param>
    /// <param name="pTimeOutInMilliseconds">
    /// How long the client is willing to wait for an incoming connection in ms.
    /// </param>
    /// <returns>
    /// ConnectInd:
    ///   Connection initiated by a remote unit has been established.
    /// NoConnection:
    ///   There is no incoming connection awaiting.
    /// </returns>
    TConnIndRes ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds );
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    void DisReq();
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication 
    /// for connectionless communication.
    /// </summary>
    bool Connected { get;}
  }
}