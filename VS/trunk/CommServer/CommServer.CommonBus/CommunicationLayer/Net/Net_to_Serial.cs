//<summary>
//  Title   : Implementation of ICommunicationLayer for TCP/UDP
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    20080612: mzbrzezny: Net_to_Serial priority of some errors that goes to log are changed
//    MPostol
//      TCP protocol implemented;
//      Dodoano obsluge exceptions w operacjach na sket'cie
//      wywalilem NotMyAnswer, dla UDP tez jest robiony connect, co eleminuje przychodzenie ramek z dowolnego zrodla.
//    MZbrzezny 2007-01-31
//   -  usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//	  - wprowadzono zmiany dot.odswiezania buforow, jest to zwi¹zane 
//	  z wyjatkami, ktore moga wystapic
//
//    Maciej Zbrzezny - 12-04-2006
//    aby usunac warningi wykomentowano return po throw
//    MPostol - 23-12-2005
//      zmodyfikowano i przetestowano
//    MZbrzezny - 2005-12-16:
//	    created
//    
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CAS.Lib.RTLib.Processes;

namespace CAS.Lib.CommonBus.CommunicationLayer.Net
{
  /// <summary>
  /// Implementation of the <see cref="ICommunicationLayer"/> for the UDP and TCP protocols
  /// </summary>
  internal class Net_to_Serial: Medium_to_Serial, ICommunicationLayer
  {
    #region private
    private class MySocket: Socket
    {
      public MySocket( AddressFamily addressFamily, SocketType socketType, System.Net.Sockets.ProtocolType protocolType ) :
        base( addressFamily, socketType, protocolType )
      {
        Disposed = false;
        LastException = null;
      }
      public Exception LastException { get; set; }
      public bool Disposed { get; private set; }
      protected override void Dispose( bool disposing )
      {
        if ( Disposed )
          return;
        Disposed = true;
        base.Dispose( disposing );
      }
    }
    private const string m_ExceptionMesage = "Unexpected exception has been cached: {0}";
    private const ushort c_mySocketSendTimeout = 500;
    private const ushort c_SocektDisconnectTimeOut = 10000;
    private const ushort c_buffCapacity = 1024;
    private readonly ProtocolType m_ProtocolType;
    private byte[] m_Buffer = new byte[ c_buffCapacity ];
    private int m_DataLength = 0;
    private int m_Position = int.MaxValue;
    private EventWaitHandleTimeOut m_ConnectWaitAsnychResponse = new EventWaitHandleTimeOut( true );
    private EventWaitHandleTimeOut m_ReceiveNotStarted = new EventWaitHandleTimeOut( true );
    private MySocket m_Socket = null;
    private short m_SocketNum = 0;
    private IPEndPoint m_RemoteEndPoint = new IPEndPoint( IPAddress.Any, 0 );
    private bool BufferEmpty { get { return m_DataLength <= m_Position; } }
    [Conditional( "TRACE" )]
    protected new void TraceEvent( TraceEventType pEventType, int pId, string pMessage )
    {
      if ( m_RemoteEndPoint != null )
      {
        pMessage = String.Format( "{0} ({1})", pMessage, m_RemoteEndPoint.ToString() );
      }
      base.TraceEvent( pEventType, pId, pMessage );
    }
    /// <summary>
    /// Gets or sets the value that indicates whether a this instance is logicaly connected.
    /// </summary>
    /// <value><c>true</c> if this instance is logicaly connected</value>
    private bool IsConnected { get { return m_Socket != null; } }
    private bool EndPointsAreEqual( IPEndPoint loc, EndPoint rem )
    {
      IPEndPoint IPRem = rem as IPEndPoint;
      if ( IPRem == null )
      {
        Debug.Fail( "Remote end point is empty - it should has been never happen. " );
        return false;
      }
      return ( loc.Address == IPRem.Address ) && ( loc.Port == IPRem.Port );
    }
    #region comment
    /// <summary>
    /// Resolves DNS address
    /// </summary>
    ///<remarks>
    ///Dns.GetHostEntry inside may trows Exceptions:
    ///   System.ArgumentNullException:
    ///     The hostNameOrAddress parameter is null.
    ///
    ///   System.ArgumentOutOfRangeException:
    ///     The length of hostNameOrAddress parameter is greater than 126 characters.
    ///
    ///   System.Net.Sockets.SocketException:
    ///     An error was encountered when resolving the hostNameOrAddress parameter.
    ///
    ///   System.ArgumentException:
    ///     The hostNameOrAddress parameter is an invalid IP address.
    ///</remarks>
    /// <param name="address">
    /// Text string representing the address <see cref="IAddress"/>. 
    /// address.address must has syntax host:port, othervise the ArgumentException exception is thrown. 
    /// </param>
    /// <returns>
    /// Represents a network endpoint as an IP address and a port number.
    /// </returns>
    /// <exception cref="ArgumentException">ArgumentException</exception>
    #endregion
    private static IPEndPoint DNSResolve( IAddress address )
    {
      string[] addr = ( (string)address.address ).Split( ':' );
      if ( addr.Length != 2 )
        throw new ArgumentException( "Wrong address syntax: " + address.address );
      IPHostEntry host = Dns.GetHostEntry( addr[ 0 ] );
      IPAddress m_addr = null;
      foreach ( IPAddress ip in host.AddressList )
        if ( ip.AddressFamily == AddressFamily.InterNetwork )
        {
          m_addr = ip;
          break;
        }
      if ( m_addr == null )
        throw ( new ArgumentException( "Cannot resolve DNS address: " + address.address ) );
      return new IPEndPoint( m_addr, System.Convert.ToInt32( addr[ 1 ] ) );
    }
    /// <summary>
    /// Ends a pending asynchronous connection request. 
    /// </summary>
    /// <param name="ar">
    /// An IAsyncResult that stores state information for this asynchronous operation 
    /// as well as any user defined data.
    /// </param>
    private void ConnectCallBack( IAsyncResult ar )
    {
      MySocket m_s = null;
      try
      {
        m_s = (MySocket)ar.AsyncState;
        m_s.EndConnect( ar );
      }
      catch ( Exception ex )
      {
        m_s.LastException = ex;
        TraceEvent( TraceEventType.Verbose, 183, "An exception has been thrown by the ConnCallBack" );
      }
      finally { m_ConnectWaitAsnychResponse.Set(); }
    }
    /// <summary>
    /// Ends a pending asynchronous read.
    /// </summary>
    /// <param name="ar">An IAsyncResult that stores state information for this asynchronous operation
    /// as well as any user defined data.
    /// </param>
    private void ReciveCallBack( IAsyncResult ar )
    {
      MySocket m_s = null;
      try
      {
        m_s = (MySocket)ar.AsyncState;
        EndPoint endPoint = m_RemoteEndPoint;
        int lngt = m_s.EndReceiveFrom( ar, ref endPoint );
        if ( lngt > 0 && !EndPointsAreEqual( m_RemoteEndPoint, endPoint ) )
        {
          TraceEvent( TraceEventType.Error, 207, "We have got frame from unepected address" );
          Debug.Fail( "We have got frame from unepected address" );
          ReenterReciveCallBack( m_s );
          return;
        }
        if ( lngt == 0 )
        {
          // according to: http://msdn.microsoft.com/en-us/library/system.net.sockets.socket.endreceivefrom%28v=VS.80%29.aspx
          // Return Value - If successful, the number of bytes received. If unsuccessful, returns 0. 
          // The EndReceiveFrom method will block until data is available. If you are using a connectionless protocol, 
          // EndReceiveFrom will read the first enqueued datagram available in the incoming network buffer. 
          // If you are using a connection-oriented protocol, the EndReceiveFrom method will read as much data as is available 
          // up to the number of bytes you specified in the size parameter of the BeginReceiveFrom method. 
          // If the remote host shuts down the Socket connection with the Shutdown method, and all available data has been received, 
          // the EndReceiveFrom method will complete immediately and return zero bytes. 
          TraceEvent( TraceEventType.Information, 226,
            "The unexpected empty frame (0 length) was received. This would happen if the remote host shuts down the connection and all available data has been received previously." );
          if ( this.m_ProtocolType == ProtocolType.Udp )
          {
            //UDP:
            ReenterReciveCallBack( m_s );
          }
          else
          {
            //TCP:
            // We do not need to reenter callback (ReenterReciveCallBack( m_s );) because the connection might be closed 
            // We have to set that the buffer is empty:
            m_Position = int.MaxValue;
            m_DataLength = 0;
          }
          return;
        }
        m_DataLength = lngt;
        m_Position = 0;
      }
      catch ( Exception ex )
      {
        m_s.LastException = ex;
      }
      finally
      {
        m_ReceiveNotStarted.Set();
      }
    }
    private void ReenterReciveCallBack( MySocket m_s )
    {
      EndPoint endPoint = m_RemoteEndPoint;
      m_s.BeginReceiveFrom
         ( m_Buffer, 0, c_buffCapacity, SocketFlags.None, ref endPoint, new AsyncCallback( ReciveCallBack ), m_s );
      return;
    }
    private void Receive()
    {
      //Activating the call back we must be sure the buffer is empty;
      m_Position = int.MaxValue;
      m_DataLength = 0;
      bool exceptionHasOccured = false;
      Debug.Assert( m_ReceiveNotStarted.State,
        "CAS.Lib.CommonBus.CommunicationLayer.Net.Net_to_Serial.Receive(): Reentered Receive method" );
      Debug.Assert( IsConnected, "Reveive may be called only in Connected state" );
      try
      {
        // Reset must be called befor BeginReceiveFrom
        // based on experiments we have discovered that sometimes 
        // callback is executed on the same thread as this method!!
        // note: that also in Exception catched we have to Set m_receiveNotStarted
        m_ReceiveNotStarted.Reset();
        EndPoint endPoint = m_RemoteEndPoint;
        m_Socket.BeginReceiveFrom
           ( m_Buffer, 0, c_buffCapacity, SocketFlags.None, ref endPoint, new AsyncCallback( ReciveCallBack ), m_Socket );
        Debug.Assert( this.EndPointsAreEqual( m_RemoteEndPoint, endPoint ), "Provider has changed the remote endpoint." );
      }
      catch ( SocketException ex )
      {
        exceptionHasOccured = true;
        MarkSocketException( ex, 266 );
      }
      catch ( ObjectDisposedException ex )
      {
        exceptionHasOccured = true;
        MarkSocketException( ex, 269 );
      }
      catch ( Exception ex )
      {
        exceptionHasOccured = true;
        MarkException( String.Format( m_ExceptionMesage, ex.Message ), TraceEventType.Error, 273, ex );
      }
      finally
      {
        if ( exceptionHasOccured )
        {
          // the exception comes from m_Socket.BeginReceiveFrom only and it indicates that 
          // callback and m_ReceiveNotStarted.Set() never occur, so we have to call it here:
          m_ReceiveNotStarted.Set();
        }
      }
    }
    private void MarkSocketException( ObjectDisposedException ex, int position )
    {
      string msg = "The socket has been unexpectedly closed: {0}";
      MarkException( string.Format( msg, ex.Message ), TraceEventType.Information, position, ex );
    }
    private void MarkSocketException( SocketException ex, int position )
    {
      string socketErrorMesage = String.Format( " with the socket error: {0}", ex.SocketErrorCode.ToString() );
      string msg = "An error occurred when attempting to access the socket: {0}" + socketErrorMesage;
      MarkException( string.Format( msg, ex.Message ), TraceEventType.Information, position, ex );
    }
    #endregion private
    #region creators
    /// <summary>
    /// Initializes a new instance of the <see cref="Net_to_Serial"/> class.
    /// </summary>
    /// <param name="param">The paramites array.</param>
    internal Net_to_Serial( object[] param )
      : this( (short)param[ 0 ], (ProtocolType)param[ 1 ], (string)param[ 2 ], (CommonBusControl)param[ 3 ] )
    { }
    /// <summary>
    /// Initializes a new instance of the <see cref="Net_to_Serial"/> class that implements
    /// of the <see cref="ICommunicationLayer"/> for the UDP and TCP protocols.
    /// </summary>
    /// <param name="sockedNum">
    /// The socked num used by the UDP protocol to send and receive packets. 
    /// If 0 the underling service allocates dynamically appropriate number – 
    /// it is preferred approach. For TCP this parameter is ignored.
    /// </param>
    /// <param name="protocol">The protocol type one of the <see cref="ProtocolType"/> values.</param>
    /// <param name="pTraceName">Name of the trace.</param>
    /// <param name="pParent">The parent component.</param>
    internal Net_to_Serial( short sockedNum, ProtocolType protocol, string pTraceName, CommonBusControl pParent )
      : base( pTraceName, pParent )
    {
      m_ProtocolType = protocol;
      m_SocketNum = sockedNum;
      if ( m_SocketNum < 0 )
        m_SocketNum = 0;
      switch ( protocol )
      {
        case ProtocolType.Tcp:
          TraceEvent( TraceEventType.Information, 361, "Creating TCP serial communication layer protocol " );
          break;
        case ProtocolType.Udp:
          TraceEvent( TraceEventType.Information, 364, "Creating UDP serial communication layer protocol " );
          break;
        default:
          throw ( new ArgumentException( "Net_to_Serial: ProtocolType not supported" ) );
      }
    }
    #endregion
    #region IDisposable
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
      if ( this.disposed )
        return;
      if ( disposing )
        if ( m_Socket != null )
          m_Socket.Close();
      m_Socket = null;
      // Release unmanaged resources.
      // Set large fields to null.
      // Call Dispose on your base class.
      disposed = true;
      base.Dispose( true );
    }
    #endregion
    #region IConnectionManagement Members
    /// <summary>
    /// Connect Request, this fuction is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>
    /// Result of the operation
    /// </returns>
    TConnectReqRes IConnectionManagement.ConnectReq( IAddress remoteAddress )
    {
      if ( remoteAddress == null || remoteAddress.address == null )
      {
        TraceEvent( TraceEventType.Warning, 432, "ConnectReq: Invalid or empty remote address is passed to Connect Request" );
        return TConnectReqRes.NoConnection;
      }
      Debug.Assert( !IsConnected, "ConnectReq may be called only while disconnected" );
      if ( IsConnected )
      {
        TraceEvent( TraceEventType.Error, 443, "ConnectReq: Attempt to connect while connected" );
        ( (IConnectionManagement)this ).DisReq();
      }
      TraceEvent( TraceEventType.Verbose, 439, "ConnectReq: Attempt to connect to: " + remoteAddress.address.ToString() );
      try
      {
        m_RemoteEndPoint = DNSResolve( remoteAddress );
      }
      catch ( Exception ex )
      {
        #region Comments
        // DNSResolve may trows:
        // Exceptions:
        //   System.ArgumentNullException:
        //     The hostNameOrAddress parameter is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The length of hostNameOrAddress parameter is greater than 126 characters.
        //
        //   System.Net.Sockets.SocketException:
        //     An error was encountered when resolving the hostNameOrAddress parameter.
        //
        //   System.ArgumentException:
        //     The hostNameOrAddress parameter is an invalid IP address.

        #endregion
        string msg = "ConnectReq: It is impossible to resolve the address: {0}, correct the configuration file. More detais: {1}";
        TraceEvent( TraceEventType.Information, 469, msg, remoteAddress.address, ex, ex.Message );
        return TConnectReqRes.NoConnection;
      }
      try
      {
        if ( m_ProtocolType == ProtocolType.Tcp )
          m_Socket = new MySocket( AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp );
        else
        {
          m_Socket = new MySocket( AddressFamily.InterNetwork, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp );
          m_Socket.Bind( new IPEndPoint( IPAddress.Any, m_SocketNum ) );
        }
        m_Socket.SendTimeout = c_mySocketSendTimeout;
        //m_socket.SetSocketOption( SocketOptionLevel.Udp, SocketOptionName.Broadcast, true );
        //m_socket.SetSocketOption( SocketOptionLevel.IP, SocketOptionName.PacketInformation, true );
        m_ConnectWaitAsnychResponse.Reset();
        m_Socket.BeginConnect( m_RemoteEndPoint, new AsyncCallback( ConnectCallBack ), m_Socket );
        m_ConnectWaitAsnychResponse.WaitOne( Timeout.Infinite );
        if ( m_Socket.LastException != null )
          throw m_Socket.LastException;
        if ( m_ProtocolType == ProtocolType.Tcp && !m_Socket.Connected )
        {
          m_Socket.Close();
          m_Socket = null;
          return TConnectReqRes.NoConnection;
        }
        TraceEvent( TraceEventType.Verbose, 489, "ConnectReq: connected to: " + remoteAddress.address.ToString() );
        Receive();
        return TConnectReqRes.Success;
      }
      catch ( SocketException ex )
      {
        MarkSocketException( ex, 496 );
      }
      catch ( ObjectDisposedException ex )
      {
        MarkSocketException( ex, 500 );
      }
      catch ( Exception ex )
      {
        MarkException( String.Format( m_ExceptionMesage, ex.Message ), TraceEventType.Error, 504, ex );
      }
      Debug.Assert( m_Socket != null, "Here I should have an active socket" );
      if ( m_Socket != null )
        m_Socket.Close();
      m_Socket = null;
      return TConnectReqRes.NoConnection;
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
    /// Result of the operation
    /// </returns>
    TConnIndRes IConnectionManagement.ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds )
    {
      return TConnIndRes.NoConnection;
    }
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    void IConnectionManagement.DisReq()
    {
      if ( !IsConnected )
      {
        string cMsg = "DisReq: Client called disconnect request while it is not allowed in the state: : " + IsConnected.ToString();
        TraceEvent( TraceEventType.Error, 537, cMsg );
      }
      TraceEvent( TraceEventType.Verbose, 636, "DisReq: Start unconditionally disconnect the connection if any." );
      try
      {
        Debug.Assert( m_Socket != null, "To disconnect the socket it must not be null" );
        if ( m_Socket.ProtocolType == System.Net.Sockets.ProtocolType.Tcp )
        {
          m_Socket.Shutdown( SocketShutdown.Both );
          m_Socket.Disconnect( false );
          Debug.Assert( !m_Socket.Connected, "After disconnecting the socked must be disconnected to release the used endpoint" );
        }
      }
      catch ( SocketException ex )
      {
        MarkSocketException( ex, 514 );
      }
      catch ( ObjectDisposedException ex )
      {
        MarkSocketException( ex, 519 );
      }
      catch ( Exception ex )
      {
        MarkException( String.Format( m_ExceptionMesage, ex.Message ), TraceEventType.Error, 304, ex );
      }
      finally
      {
        m_Socket.Close();
        m_ReceiveNotStarted.WaitOne( Timeout.Infinite );
        if ( m_Socket.LastException != null )
          if ( m_Socket.LastException is ObjectDisposedException )
            TraceEvent( TraceEventType.Verbose, 516, "Finish of ReciveCallBack because of Disconnecting" );
          else
            TraceEvent( TraceEventType.Warning, 518, String.Format( m_ExceptionMesage, m_Socket.LastException.Message ) );
        m_Socket = null;
      }
    }
    #endregion
    #region ICommunicationLayer
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr )
    {
      return ( (ICommunicationLayer)this ).GetChar( out lastChr, Timeout.Infinite );
    }//GetChar
    /// <summary>
    /// Get the next character from the receiving stream.
    /// </summary>
    /// <param name="lastChr">The character or 0 if timeout</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the character.</param>
    /// <returns></returns>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr, int millisecondsTimeout )
    {
      lastChr = 0;
      if ( !IsConnected )
        return TGetCharRes.DisInd;
      if ( !m_ReceiveNotStarted.WaitOne( millisecondsTimeout ) )
        return TGetCharRes.Timeout;
      if ( BufferEmpty )
        return TGetCharRes.DisInd;
      lastChr = m_Buffer[ m_Position++ ];
      if ( BufferEmpty )
        Receive();
      return TGetCharRes.Success;
    }
    /// <summary>
    /// Check if there is data awaiting in the buffer. 
    /// </summary>
    /// <returns>
    /// DataInd:
    ///   Data indication – there is data awaiting in the buffer.
    /// NoDataAvailable:
    ///   No data available.
    /// DisInd:
    ///   Disconnect indication – connection has been shut down remotely or lost because of communication error. Data is unavailable
    /// </returns>
    TCheckCharRes ICommunicationLayer.CheckChar()
    {
      if ( !IsConnected )
        return TCheckCharRes.DisInd;
      if ( !BufferEmpty )
        return TCheckCharRes.DataInd;
      if ( m_ReceiveNotStarted.State )
        return TCheckCharRes.DisInd;
      return TCheckCharRes.NoDataAvailable;
    }
    /// <summary>
    /// Flushes the buffer - Clean the inbound buffer.
    /// </summary>
    void ICommunicationLayer.Flush()
    {
      if ( !IsConnected )
        return;
      if ( !m_ReceiveNotStarted.State )
        return;
      try
      {
        while ( m_Socket.Available > 0 )
          m_Socket.Receive( m_Buffer );
        Receive();
      }
      catch ( SocketException ex )
      {
        MarkSocketException( ex, 631 );
      }
      catch ( ObjectDisposedException ex )
      {
        MarkSocketException( ex, 633 );
      }
      catch ( Exception ex )
      {
        MarkException( String.Format( m_ExceptionMesage, ex.Message ), TraceEventType.Error, 665, ex );
      }
    }
    /// <summary>
    /// Transmits the data contained in the frame.
    /// </summary>
    /// <param name="frame">Frame with the data to be transmitted.</param>
    /// <returns>
    /// Success:
    /// Operation accomplished successfully
    /// DisInd:
    /// Disconnect indication – connection has been shut down remotely or lost because of communication error.
    /// Data is unavailable. To reestablisch the communication the <see cref="IConnectionManagement.ConnectReq"/> 
    /// must be called first.
    /// </returns>
    TFrameEndSignalRes ICommunicationLayer.FrameEndSignal( CommunicationLayer.UMessage frame )
    {
      if ( !IsConnected )
        return TFrameEndSignalRes.DisInd;
      try
      {
        Debug.Assert( m_Socket != null, "We can send a frame only after creating the socket, it can be disposed but must exist" );
        if ( m_Socket.Connected )
        {
          m_Socket.Send( frame.GetManagedBuffer() );
          return TFrameEndSignalRes.Success;
        }
        this.TraceEvent( TraceEventType.Information, 712, "FrameEndSignal: Disconnected by the remote station." );
      }
      catch ( SocketException ex )
      {
        MarkSocketException( ex, 718 );
      }
      catch ( ObjectDisposedException ex )
      {
        MarkSocketException( ex, 723 );
      }
      catch ( Exception ex )
      {
        MarkException( String.Format( m_ExceptionMesage, ex.Message ), TraceEventType.Error, 728, ex );
      }
      return TFrameEndSignalRes.DisInd;
    }
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication 
    /// for connectionless communication.
    /// </summary>
    bool IConnectionManagement.Connected { get { return this.IsConnected; } }
    #endregion
    #region PUBLIC
    /// <summary>
    /// ToString representation
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      if ( m_Socket == null )
        return "Not connected";
      string rep = m_Socket.RemoteEndPoint.ToString();
      string lep = m_Socket.LocalEndPoint.ToString();
      return
        m_Socket.ProtocolType.ToString() + String.Format( " State = {0} sockets L = {1} R = {2}", IsConnected.ToString(), lep, rep );
    }
    #endregion
  }
}
