//<summary>
//  Title   : Facade implementation of ICommunicationLayer.
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 10-04-2007: created
//
//  Copyright (C)2007, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.e
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.CommunicationLayer.NULL
{
  /// <summary>
  /// Simulated implementation of ICommunicationLayer
  /// </summary>
  internal class NULL_to_Serial: Medium_to_Serial, ICommunicationLayer
  {
    #region private
    private bool m_connected = false;
    #endregion
    #region ICommunicationLayer Members
    /// <summary>
    /// Check if there is data awaiting in the buffer. 
    /// </summary>
    /// <returns>
    /// NoDataAvailable:
    ///   No data available.
    /// </returns>
    /// <remarks>Not implemented in this provider.</remarks>
    TCheckCharRes ICommunicationLayer.CheckChar()
    {
      return TCheckCharRes.NoDataAvailable;
    }
    /// <summary>
    /// Get the next character from the receiving stream. Blocks until next character is available.
    /// </summary>
    /// <param name="lastChr">The character</param>
    /// <returns>Never returns.</returns>
    /// <remarks>Not implemented in this provider. Never returns – blocks the calling thread in the false assertion.</remarks>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr )
    {
      lastChr = 0;
      Manager.Assert( false );
      return TGetCharRes.Timeout;
    }
    /// <summary>
    /// Get the next character from the receiving stream.
    /// </summary>
    /// <param name="lastChr">Always lastChr = 0</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the character.</param>
    /// <returns>
    /// Timeout:
    ///   Data is unavailable because of a communication error – loss of communication with a station
    ///</returns>
    ///<remarks>Not implemented in this provider. </remarks>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr, int millisecondsTimeout )
    {
      lastChr = 0;
      System.Threading.Thread.Sleep( TimeSpan.FromMilliseconds( millisecondsTimeout ) );
      return TGetCharRes.Timeout;
    }
    /// <summary>
    /// Transmits the data contained in the frame.
    /// </summary>
    /// <param name="frame">Frame with the data to be transmitted.</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// </returns>
    /// <remarks>Not implemented in this provider.</remarks>
    TFrameEndSignalRes ICommunicationLayer.FrameEndSignal( UMessage frame )
    {
      return TFrameEndSignalRes.Success;
    }
    /// <summary>
    /// Flushes the buffer - Clean the inbound buffer.
    /// </summary>
    /// <remarks>Do nothing in this implementataion.</remarks>
    void ICommunicationLayer.Flush() { }
    #endregion
    #region IConnectionManagement Members
    /// <summary>
    /// Connect Request, this fuction is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// </returns>
    TConnectReqRes IConnectionManagement.ConnectReq( IAddress remoteAddress )
    {
      TraceEvent( TraceEventType.Verbose, 98, "ConnectReq: Attempt to connect to: " + remoteAddress.address.ToString() );
      m_connected = true;
      TraceEvent( TraceEventType.Verbose, 100, "ConnectReq: connected to: " + remoteAddress.address.ToString() );
      return TConnectReqRes.Success;
    }
    /// <summary>
    /// Connect indication – Check if there is a connection accepted to the remote address. 
    /// </summary>
    /// <param name="pRemoteAddress">
    /// The address of the remote unit we are waiting for connection from. Null if we are waiting for any connection.
    /// </param>
    /// <param name="pTimeOutInMilliseconds">
    /// How long the client is willing to wait for an incoming connection in ms.
    /// </param>
    /// <returns>
    /// NoConnection:
    ///   There is no incoming connection awaiting.
    /// </returns>
    /// <remarks>Not implemented in this provider.</remarks>
    TConnIndRes IConnectionManagement.ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds )
    {
      return TConnIndRes.NoConnection;
    }
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    void IConnectionManagement.DisReq()
    {
      m_connected = false;
    }
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication 
    /// for connectionless communication.
    /// </summary>
    bool IConnectionManagement.Connected { get { return m_connected; } }
    #endregion
    #region IDisposable Members
    private bool disposed = false;
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// Dispose(bool disposing) executes in two distinct scenarios. If disposing equals true, the method has been called directly
    /// or indirectly by a user's code. Managed and unmanaged resources can be disposed. If disposing equals false, 
    /// the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed.
    /// </remarks>
    /// <param name="disposing">
    /// If disposing equals true, the method has been called directly or indirectly by a user's code.
    /// </param>
    protected override void Dispose( bool disposing )
    {
      // Check to see if Dispose has already been called.
      if ( this.disposed )
        return;
      disposed = true;
    }
    #endregion
    #region creatoor
    internal NULL_to_Serial( object[] param )
      : this( (string)param[ 0 ], (CommonBusControl)param[ 1 ] )
    {
    }
    internal NULL_to_Serial( string pTraceName, CommonBusControl pParent )
      : base( pTraceName, pParent )
    { }
    #endregion
  }
}
