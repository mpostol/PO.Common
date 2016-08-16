//<summary>
//  Title   : Interface responsible for providing the communication with the remote unit –service access point 
//  System  : Microsoft Visual C# .NET 2005
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//  History :
//    MPostol 14-02-2007:
//      Zmienilem komentarz dla ListenReq
//      wywali³em ListenReq - ma byc wywolywane w kretaorze w zaleznosci od parametru adresu lokalnego podanego w konfiguracji
//      Dodalem parametr do ConnectInd - adres stacji zdalnej
//      Wywalilem PutChar, FrameEndSignal nie sa uzywane i zaimplementowane
//      Dodalem GetChar z TimeOut 
//    MZbrzezny 2007-01-31
//    usuwanie mechanizmu bazujacego na porcie 
//    w application layer i communication layer
//
//  Copyright (C)2006, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto:techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.Lib.CommonBus.CommunicationLayer
{
  /// <summary>
  /// Results returned by the CheckChar
  /// </summary>
  public enum TCheckCharRes
  {
    /// <summary>
    /// Data indication – there is data awaiting in the buffer.
    /// </summary>
    DataInd,
    /// <summary>
    /// No data available.
    /// </summary>
    NoDataAvailable,
    /// <summary>
    /// Disconnect indication – connection has been shut down remotely or lost because of communication error.
    /// Data is unavailable
    /// </summary>
    DisInd
  }
  /// <summary>
  /// Results returned by the GetChar
  /// </summary>
  public enum TGetCharRes
  {
    /// <summary>
    /// Operation accomplished successfully 
    /// </summary>
    Success,
    /// <summary>
    /// Data is unavailable because of a communication error – loss of communication with a station
    /// </summary>
    Timeout,
    /// <summary>
    /// Disconnect indication – connection has been shut down remotely or lost because of communication error.
    /// Data is unavailable
    /// </summary>
    DisInd
  }
  /// <summary>
  /// Results returned by the FrameEndSignal .
  /// </summary>
  public enum TFrameEndSignalRes
  {
    /// <summary>
    /// Operation accomplished successfully 
    /// </summary>
    Success,
    /// <summary>
    /// Disconnect indication – connection has been shut down remotely or lost because of communication error.
    /// Data is unavailable
    /// </summary>
    DisInd
  }
  /// <summary>
  /// Interface responsible for providing the communication with the remote unit – service access point 
  /// </summary>
  public interface ICommunicationLayer: IConnectionManagement, IDisposable
  {
    #region DATA TRABSFER
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
    TCheckCharRes CheckChar();
    /// <summary>
    /// Get the next character from the receiving stream. Blocks until next character is available.
    /// </summary>
    /// <param name="lastChr">The character</param>
    /// <returns>
    /// Success:
    ///   Operation accomplished successfully 
    /// Timeout:
    ///   Data is unavailable because of a communication error – loss of communication with a station
    /// DisInd:
    ///   Disconnect indication – connection has been shut down remotely or lost because of communication error.
    ///   Data is unavailable
    /// </returns>
    TGetCharRes GetChar( out byte lastChr );
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
    TGetCharRes GetChar( out byte lastChr, int millisecondsTimeout );
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
    TFrameEndSignalRes FrameEndSignal( CommunicationLayer.UMessage frame );
    /// <summary>
    /// Flushes the buffer - Clean the inbound buffer.
    /// </summary>
    void Flush();
    #endregion
  }
}