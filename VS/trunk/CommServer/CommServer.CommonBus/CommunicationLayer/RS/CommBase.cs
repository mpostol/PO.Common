//<summary>
//  Title   : Lowest level Com driver handling all Win32 API
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    Mpostol - 19-10-03 
//      created (derived from article John Hind: Use P/Invoke to Develop a .NET Base Class Library for Serial Device 
//      Communications; 
//    Wptasinski - 11 - 05 - 04
//      bytesTransferred counter added - to monitor the amount of bytes transferred over the channel
//      method incBytesTransferred( int datalength ) added - to increment the counter by the amount of data sent
//      methods send (2 versions) and read modified - to ensure the counter increment whenever a byte is transferred over the channel
//      method getBytesTransferred() added - to ensure the access to the bytesTransferred counter from outside the class  
//    <Author> - <date>:
//    <description>
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.com.pl
//  http:\\www.cas.eu
//</summary>
namespace CAS.Lib.CommonBus.CommunicationLayer.RS
{
  using System;
  using Win32API;
  /// <summary>
  /// Lowest level Com driver handling all Win32 API calls and processing send and receive in terms of
  /// individual bytes. Used as a base class for higher level drivers.
  /// </summary>
  internal abstract class CommBase: Medium_to_Serial
  {
    #region PRIVATE
    private IntPtr hPort;
    private IntPtr ptrUWO = IntPtr.Zero;
    private bool online = false;
    private void InternalClose()
    {
      RSWrapper.CloseHandle( hPort );
      online = false;
    }
    //number of bytes sent or read during the communication
    //MPTD t¹ zmien¹ i ca³¹ jej obs³ugê wywaliæ do BaseStation.Management - utorzyæ specjaln¹ klasê!!
    private ulong bytesTransferred = 0;
    #endregion
    #region PUBLIC
    /// <summary>
    /// Check if the port is online.
    /// </summary>
    /// <returns></returns>
    protected bool CheckOnline()
    {
      uint f;
      if ( online )
        if ( RSWrapper.GetHandleInformation( hPort, out f ) )
          return true;
      return false;
    }//CheckOnline
    //The ClearCommError function retrieves information about a communications error and reports the current
    // status of a communications device. 
    /// <summary>
    /// The ClearCommError function retrieves information about a communications error and reports the current 
    /// status of a communications device. 
    /// </summary>
    /// <param name="inRxQueue">Position in rx queue</param>
    /// <param name="inTxQueue">Position in tx queue</param>
    /// <returns></returns>
    protected bool ClearCommErrorSel( out UInt32 inRxQueue, out UInt32 inTxQueue )
    {
      RSWrapper.COMSTAT cs;
      uint er;
      inRxQueue = 0;
      inTxQueue = 0;
      if ( !CheckOnline() )
        return false;
      if ( !RSWrapper.ClearCommError( hPort, out er, out cs ) )
        return false;
      inRxQueue = cs.cbInQue;
      inTxQueue = cs.cbOutQue;
      return true;
    }
    /// <summary>
    /// The GetCommState function retrieves the current control settings for a specified communications device.
    /// </summary>
    /// <param name="BaudRate">Baud rate at which the communications device operates.</param>
    /// <param name="Parity">If param is TRUE, parity checking is performed and errors are reported.</param>
    /// <param name="StopBits">Number of stop bits to be used.
    /// </param>
    /// <param name="ByteSize">Number of bits in the bytes transmitted and received.</param>
    /// <returns></returns>
    protected bool GetComStatSel
      ( ref Int32 BaudRate, ref RSWrapper.ParityBit Parity, ref RSWrapper.StopBits StopBits, ref byte ByteSize )
    {
      RSWrapper.DCB PortDCB = new RSWrapper.DCB();
      if ( !RSWrapper.GetCommState( hPort, ref PortDCB ) )
        return false;
      BaudRate = PortDCB.BaudRate;
      ByteSize = PortDCB.ByteSize;
      Parity = PortDCB.Parity;
      StopBits = PortDCB.stopBits;
      return true;
    }
    /// <summary>
    /// This function configures a communications device
    /// </summary>
    /// <param name="BaudRate">Specifies the baud rate at which the communication device operates.</param>
    /// <param name="Parity">Specifies the parity scheme to be used.</param>
    /// <param name="StopBits">Specifies the number of stop bits to be used.</param>
    /// <param name="ByteSize">Specifies the number of bits in the bytes transmitted and received.</param>
    /// <returns>true indicates success.</returns>
    protected bool SetComStat
      ( RSWrapper.BAUD_RATE BaudRate, RSWrapper.ParityBit Parity, RSWrapper.StopBits StopBits, byte ByteSize )
    {
      RSWrapper.DCB PortDCB = new RSWrapper.DCB();
      if ( !RSWrapper.GetCommState( hPort, ref PortDCB ) )
        return false;
      PortDCB.BaudRate = (int)BaudRate;
      PortDCB.ByteSize = ByteSize;
      PortDCB.Parity = Parity;
      PortDCB.stopBits = StopBits;
      if ( !RSWrapper.SetCommState( hPort, ref PortDCB ) )
        return false;
      return true;
    }
    /// <summary>
    /// Opens the com port and configures it with the required settings
    /// </summary>
    /// <returns>false if the port could not be opened</returns>
    protected bool Open
      ( byte comNum, ushort portSpeed, ushort portParity, ushort portStopBits
      )
    {
      if ( online )
        return false;
      string comtxt = "COM" + comNum.ToString();
      hPort = RSWrapper.CreateFile
        (
        comtxt, RSWrapper.GENERIC_READ | RSWrapper.GENERIC_WRITE, 0, IntPtr.Zero, RSWrapper.OPEN_EXISTING,
        0, IntPtr.Zero
        );
      if ( hPort == (IntPtr)RSWrapper.INVALID_HANDLE_VALUE )
        return false;
      if ( !SetComStat( (RSWrapper.BAUD_RATE)portSpeed, (RSWrapper.ParityBit)portParity, (RSWrapper.StopBits)portStopBits, 8 ) )
        return false;
      if ( !AfterOpen() )
      {
        Close();
        return false;
      }
      online = true;
      return true;
    }
    /// <summary>
    /// Closes the com port.
    /// </summary>
    protected void Close()
    {
      if ( online )
      {
        BeforeClose( false );
        InternalClose();
      }
    }
    /// <summary>
    /// True if online.
    /// </summary>
    protected bool Online { get { if ( !online ) return false; else return CheckOnline(); } }
    /// <summary>
    /// The ReadFile function reads data from a serial port,
    /// </summary>
    /// <param name="toRead">Pointer to the buffer that receives the data read from</param>
    /// <param name="maxDataLength">[in] Number of bytes to be read from the port</param>
    /// <param name="dataLength">Variable that receives the number of bytes read. Read sets this value to zero
    /// before doing any work or error checking. </param>
    /// <returns>If the function succeeds, the return value is true. </returns>
    protected bool Read( IntPtr toRead, uint maxDataLength, out uint dataLength )
    {
      if ( RSWrapper.ReadFile( hPort, toRead, maxDataLength, out dataLength, ptrUWO ) )
      {
        incBytesTransferred( dataLength );
        return true;
      }
      else
        return false;
    }
    /// <summary>
    /// Queues bytes for transmission.
    /// </summary>
    /// <param name="tosend">Array of bytes to be sent</param>
    /// <param name="dataLength">the length of data</param>
    protected bool Send( IntPtr tosend, ushort dataLength )
    {
      uint sent = 0;
      if ( !CheckOnline() )
        return false;
      if ( ( RSWrapper.WriteFile( hPort, tosend, dataLength, out sent, ptrUWO ) ) && ( dataLength == sent ) )
      {
        incBytesTransferred( dataLength );
        return true;
      }
      else
        return false;
    }
    protected bool Send( Byte[] tosend )
    {
      uint sent = 0;
      ushort writeCount = (ushort)tosend.GetLength( 0 );
      CheckOnline();
      if ( ( RSWrapper.WriteFile( hPort, tosend, writeCount, out sent, ptrUWO ) ) && ( writeCount == sent ) )
      {
        incBytesTransferred( writeCount );
        return true;
      }
      return false;
    }
    /// <summary>
    /// Queues a single byte for transmission.
    /// </summary>
    /// <param name="tosend">Byte to be sent</param>
    protected void Send( byte tosend )
    {
      byte[] b = new byte[ 1 ];
      b[ 0 ] = tosend;
      Send( b );
    }
    /// <summary>
    /// Sends a protocol byte immediately ahead of any queued bytes.
    /// </summary>
    /// <param name="tosend">Byte to send</param>
    /// <returns>False if an immediate byte is already scheduled and not yet sent</returns>
    protected bool SendImmediate( byte tosend )
    {
      CheckOnline();
      if ( RSWrapper.TransmitCommChar( hPort, tosend ) )
        return true;
      return false;
    }
    /// <summary>
    /// Override this to provide processing after the port is openned (i.e. to configure remote
    /// device or just check presence).
    /// </summary>
    /// <returns>false to close the port again</returns>
    protected virtual bool AfterOpen() { return true; }
    /// <summary>
    /// Override this to provide processing prior to port closure.
    /// </summary>
    /// <param name="error">True if closing due to an error</param>
    protected virtual void BeforeClose( bool error ) { }
    private void incBytesTransferred( ulong incr )
    {
      bytesTransferred += incr;
    }
    public ulong getBytesTransferred()
    {
      return bytesTransferred;
    }
    #endregion
    #region creator
    protected CommBase( string pName, CommonBusControl pParent )
      : base( pName, pParent )
    { }
    #endregion
    #region IDisposable Members
    // Check to see if Dispose has already been called.
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
      // Release unmanaged resources. If disposing is false, only the following code is executed.
      Close();
      // Note that this is not thread safe.Another thread could start disposing the object
      // after the managed resources are disposed,
      // but before the disposed flag is set to true.
      // If thread safety is necessary, it must be
      // implemented by the client.
      disposed = true;
      base.Dispose( true );
    }
    #endregion
  }
}
