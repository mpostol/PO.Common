//<summary>
//  Title   : Implementation of BaseStation.ICommunicationLayer for serial port
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  History :
//    MPostol: 15-04-2007: 
//      Used SerialPort from Framework 2.0 instead of unmanaged wrapper.
//    MPostol: 15-02-2007:
//      Dopasowalem do nowej definicji interfejsu ICommunicationLayer, gdzie nie ma ListenReq, PutChar, FrameEndSignal
//      ListenReq ma byc wywolywana w kreatorze zgodnie z konfiguracja - tego nie zaimplementowalem.
//    MPostol 2007-02-09:
//      Dodoalem lock w FlusherThreadRun i Flush - musi byæ wzajemnie wylaczna dla klienta i tego watku, 
//      zatem nie jest ju¿ potrzebny w FlushComm
//    MZbrzezny 2007-01-31
//      usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//    MPOstol - 22-12-2005
//      W DisconnectInd zmieni³em zakres #if zawsze generowal if
//    MZbrzezny - 02-06-2005:
//      Dodano mozliwosc wielokrotnego podlaczania do 
//      portu COM oraz oproznianie bufora gdy COM powinien byc w stanie nie aktywnosci 
//      (ale caly czas jest podlaczony - doadno parametry kompilacji _COM_MultiConnect - 
//      ktore powoduja uruchomienie dzialan przy podlaczaniu i odlaczaniu - gdy zmienna ta
//      jest ustawiona to nastepuje wielokrotne podlaczanie, chyba ze ostawiona jest tez
//      _COM_FlusherThread - ktora powoduje uruchomienie watku czyszczacego bufor gdy com jest nieaktywny 
//      ale roeniez nie odlacza go za kazdym razem, a tylko przestawia flage aktywnosci
//    MPostol - 13-03-04
//      FlushComm, GetChar - czyta³y dane z portu bez uwzglednienia limitu bufora, co powodowa³o b³¹d 
//      przepe³nienia bufora i crash systemu.
//    MPostol - 2003: created
//
//  Copyright (C)2008, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

// ustawienie ponizszej powoduje ze cialo watku flush jest niepuste
//#define _COM_FlusherThreadBody
// ustawienie ponizszej powoduje ze uruchamiany jest watek usuwajacy i nie dochodzi do calkowitego rozlaczania
//#define _COM_FlusherThread
// powoduje podlaczanie i odlaczanie od poirtu COM za kazdym razem
//#define _COM_MultiConnect

using System;
using System.Diagnostics;
using System.IO.Ports;
using System.IO;

namespace CAS.Lib.CommonBus.CommunicationLayer.RS
{
  /// <summary>
  ///  Title   : Implementation of the ICommunicationLayer for serial port 
  /// </summary>
  internal class RS_to_Serial: Medium_to_Serial, ICommunicationLayer
  {
    #region PRIVATE
    private const int c_WriteTimeoutDefault_ms = 500;
    private SerialPort m_SerialPort = new SerialPort();
    #endregion
    #region ICommunicationLayer Members
    /// <summary>
    /// Get the next character from the receiving stream.
    /// </summary>
    /// <param name="lastChr">The character or 0 if timeout</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait for the character.</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// Timeout:
    ///   Data is unavailable because of a communication error – loss of communication with a station
    /// DisInd:
    ///   Disconnect indication – connection has been shut down remotely or lost because of communication error.
    ///   Data is unavailable
    ///</returns>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr, int millisecondsTimeout )
    {
      lastChr = 0;
      TGetCharRes cRes = TGetCharRes.Success;
      try
      {
        if ( !m_SerialPort.IsOpen )
        {
          TraceEvent( TraceProgError, 88, "GetChar is called while in {0} state.", CurrentLayerState );
          Debug.Assert( CurrentLayerState != LayerState.Connected );
          return TGetCharRes.DisInd;
        }
        m_SerialPort.ReadTimeout = millisecondsTimeout;
        lastChr = Convert.ToByte( m_SerialPort.ReadByte() );
      }
      catch ( TimeoutException )
      {
        TraceEvent( TraceFlow, 97, "GetChar: Timeout while reading next byte from serial port." );
        cRes = TGetCharRes.Timeout;
      }
      catch ( Exception ex )
      {
        if ( m_SerialPort.IsOpen )
        {
          TraceEvent( TraceCommError, 104, "GetChar: Exception cought from the underling layer: " + ex.Message );
          cRes = TGetCharRes.Timeout;
        }
        else
        {
          MarkException( "GetChar: Exception cought from the underling layer", TraceCommError, 109, ex );
          cRes = TGetCharRes.DisInd;
        }
      }
      return cRes;
    }
    /// <summary>
    /// Get the next character from the receiving stream. Blocks until next character is available.
    /// </summary>
    /// <param name="lastChr">The byte that was read.</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// Timeout:
    ///   Data is unavailable because of a communication error – loss of communication with a station
    /// DisInd:
    ///   Disconnect indication – connection has been shut down remotely or lost because of communication error.
    ///   Data is unavailable
    /// </returns>
    TGetCharRes ICommunicationLayer.GetChar( out byte lastChr )
    {
      return ( (ICommunicationLayer)this ).GetChar( out lastChr, SerialPort.InfiniteTimeout );
    }//GetChar
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
      TCheckCharRes cRes = TCheckCharRes.DataInd;
      try
      {
        if ( !m_SerialPort.IsOpen )
        {
          TraceEvent( TraceProgError, 174, "CheckChar is called while in {0} state.", CurrentLayerState );
          Debug.Assert( CurrentLayerState != LayerState.Connected );
          return TCheckCharRes.DisInd;
        }
        if ( m_SerialPort.BytesToRead == 0 )
          return TCheckCharRes.NoDataAvailable;
      }
      catch ( Exception ex )
      {
        if ( m_SerialPort.IsOpen )
        {
          TraceEvent( TraceCommError, 167, "CheckChar: Exception cought from the underling layer: " + ex.Message );
          cRes = TCheckCharRes.NoDataAvailable;
        }
        else
        {
          MarkException( "CheckChar: Exception cought from the underling layer", TraceCommError, 188, ex );
          cRes = TCheckCharRes.DisInd;
        }
      }
      return cRes;
    }
    /// <summary>
    /// Transmits the data contained in the frame.
    /// </summary>
    /// <param name="frame">Frame with the data to be transmitted.</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// DisInd:
    ///   Disconnect indication – connection has been shut down remotely or lost because of communication error.
    ///   Data is unavailable
    ///  </returns>
    TFrameEndSignalRes ICommunicationLayer.FrameEndSignal( CommunicationLayer.UMessage frame )
    {
      TFrameEndSignalRes cRes = TFrameEndSignalRes.Success;
      try
      {
        if ( !m_SerialPort.IsOpen )
        {
          TraceEvent( TraceProgError, 190, "FrameEndSignal is called while in {0} state.", CurrentLayerState );
          Debug.Assert( CurrentLayerState != LayerState.Connected );
          return TFrameEndSignalRes.DisInd;
        }
        byte[] cDataOut = frame.GetManagedBuffer();
        m_SerialPort.Write( cDataOut, 0, cDataOut.Length );
        Console.WriteLine( "FrameEndSignal:" + this.ToString()+"; "+frame.ToString()  );
      }
      catch (TimeoutException timeoutex )
      {
        TraceEvent( TraceCommError, 199, "FrameEndSignal: TimeoutException cought from the underling layer: " + timeoutex.Message );
        // chyba zawsze powinnismy poinformowac warstwe wyzsza - bo przeciez skoro zapisujemy - to myslelismy ze jest polaczenie
        //(moze i port jest otworzony, lub zamkniety - ale stan naszego communication layer moze byc zupelnie inny)
        MarkException( "FrameEndSignal: TimeoutException cought from the underling layer", TraceCommError, 205, timeoutex );
        cRes = TFrameEndSignalRes.DisInd;
      }
      catch ( Exception ex )
      {
        if ( m_SerialPort.IsOpen )
        {
          TraceEvent( TraceCommError, 205, "FrameEndSignal: Exception cought from the underling layer: " + ex.Message );
        }
        else
        {
          MarkException( "FrameEndSignal: Exception cought from the underling layer", TraceCommError, 205, ex );
          cRes = TFrameEndSignalRes.DisInd;
        }
      }
      return cRes;
    }
    /// <summary>
    /// Flushes the buffer - clean the inbound buffer.
    /// </summary>
    void ICommunicationLayer.Flush()
    {
      if ( !m_SerialPort.IsOpen )
      {
        TraceEvent( TraceProgError, 218, "Flush is called while in {0} state.", CurrentLayerState );
        Debug.Assert( CurrentLayerState != LayerState.Connected );
        return;
      }
      try
      {
        m_SerialPort.DiscardInBuffer();
      }
      catch ( Exception ex )
      {
        if ( m_SerialPort.IsOpen )
        {
          TraceEvent( TraceCommError, 230, "Flush: Exception cought from the underling layer: " + ex.Message );
        }
        else
        {
          MarkException( "Flush: Exception cought from the underling layer", TraceCommError, 234, ex );
        }
      }
    }
    #endregion
    #region IConnectionManagement
    /// <summary>
    /// Connect Request, this fuction is used for establishing the connection
    /// </summary>
    /// <param name="remoteAddress">address of the remote unit</param>
    /// <returns>
    /// Result of the operation
    /// </returns>
    TConnectReqRes IConnectionManagement.ConnectReq( IAddress remoteAddress )
    {
      TraceEvent( TraceEventType.Verbose, 249, "ConnectReq: Attempt to connect to: " + remoteAddress.address.ToString() );
      try
      {
        if ( m_SerialPort.IsOpen )
        {
          TraceEvent( TraceProgError, 254, "ConnectReq is called while in {0} state.", CurrentLayerState );
          m_SerialPort.Close();
          CurrentLayerState = LayerState.Disconnected;
        }
        m_SerialPort.Open();
      }
      catch ( Exception ex )
      {
        if ( m_SerialPort.IsOpen )
        {
          TraceEvent( TraceCommError, 264, "ConnectReq: Exception cought from the underling layer: " + ex.Message );
        }
        else
        {
          MarkException( "ConnectReq: Exception cought from the underling layer", TraceCommError, 268, ex );
          return TConnectReqRes.NoConnection;
        }
      }
      CurrentLayerState = LayerState.Connected;
      return TConnectReqRes.Success;
    }
    /// <summary>
    /// Connect indication – Check if there is a connection waiting to be accepted from the remote unit. 
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
    /// <remarks>not applicable for serial connection – returns always <see cref="TConnIndRes.NoConnection "/></remarks>
    TConnIndRes IConnectionManagement.ConnectInd( IAddress pRemoteAddress, int pTimeOutInMilliseconds )
    {
      if ( !m_SerialPort.IsOpen)
        m_SerialPort.Open();
      Stopwatch sw = new Stopwatch();
      sw.Start();
      while (sw.ElapsedMilliseconds<pTimeOutInMilliseconds)
      {
      if ( m_SerialPort.BytesToRead > 0 )
       return TConnIndRes.ConnectInd;
      }
      return TConnIndRes.NoConnection;
    }
    /// <summary>
    /// Disconnect Request - Unconditionally disconnect the connection if any.
    /// </summary>
    void IConnectionManagement.DisReq()
    {
      if ( CurrentLayerState != LayerState.Connected )
      {
        TraceEvent( TraceProgError, 299, "DisReq is called while in {0} state.", CurrentLayerState );
        Debug.Assert( CurrentLayerState != LayerState.Connected );
        return;
      }
      try
      {
        m_SerialPort.Close();
        CurrentLayerState = LayerState.Disconnected;
      }
      catch ( Exception ex )
      {
        MarkException( "DisReq: Exception cought from the underling layer", TraceCommError, 310, ex );
      }
    }
    /// <summary>
    /// true if the layer is connected for connection oriented communication or ready for communication 
    /// for connectionless communication.
    /// </summary>
    bool IConnectionManagement.Connected
    {
      get { return m_SerialPort.IsOpen; }
    }
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
      try { m_SerialPort.Close(); }
      catch { }
      disposed = true;
      base.Dispose( disposing );
    }
    #endregion
    #region PUBLIC
    /// <summary>
    /// Gets the RS_to_Serial description as a string. 
    /// </summary>
    /// <returns>Description in form: COM[serialNum], [baudRate], [parityBit], [stopBits]</returns>
    public override string ToString()
    {
      return m_SerialPort.PortName + ", " + m_SerialPort.BaudRate.ToString() + ", " + m_SerialPort.Parity.ToString()
              + ", " + m_SerialPort.StopBits.ToString();
    }
    /// <summary>
    /// Creator for RS_to_Serial it sets the inistial setting for serial port
    /// </summary>
    /// <param name="pSettings">Setting for serial port</param>
    /// <param name="pParent">Parent component class <seealso cref="CommonBusControl"/>providing needed resources.</param>
    internal RS_to_Serial( SerialPortSettings pSettings, CommonBusControl pParent )
      : base( "COM" + pSettings.PortName.ToString(), pParent )
    {
      m_SerialPort.PortName = "COM" + pSettings.PortName.ToString();
      m_SerialPort.BaudRate = (int)pSettings.BaudRate;
      m_SerialPort.Parity = pSettings.Parity;
      m_SerialPort.StopBits = pSettings.StopBits;
      m_SerialPort.DataBits = pSettings.DataBits;
      m_SerialPort.WriteTimeout = c_WriteTimeoutDefault_ms;
      //m_SerialPort.Handshake = Handshake.None;
      //m_SerialPort.ReadTimeout = 500;
      m_SerialPort.Encoding = System.Text.Encoding.UTF8;
    }
    internal RS_to_Serial( object[] param ):base (param)
    {
      m_SerialPort.PortName = "COM" + (string)param[2];
      m_SerialPort.BaudRate = (int)param[3];
      m_SerialPort.Parity = (Parity)param[4];
      m_SerialPort.StopBits = (StopBits)param[ 5 ];
      m_SerialPort.DataBits = (int)param[ 6 ];
      m_SerialPort.WriteTimeout = c_WriteTimeoutDefault_ms;
      //m_SerialPort.Handshake = Handshake.None;
      //m_SerialPort.ReadTimeout = 500;
      m_SerialPort.Encoding = System.Text.Encoding.UTF8;
    }
    #endregion
  }
}
